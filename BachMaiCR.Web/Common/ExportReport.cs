using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BachMaiCR.Utilities.ReportForm;

namespace BachMaiCR.Web.Common
{
    /// <summary>
    /// Summary description for ExportReport
    /// <para>
    /// Creator: GiangPN
    /// </para>
    /// </summary>
    public class ExportReport
    {
        public ExportReport()
        {
            //
            // TODO: Add constructor logic here
            //        

        }

        /// <summary>
        /// Hàm set lại độ rộng cho cột
        /// </summary>
        /// <param name="worldSheet"></param>
        /// <param name="column"></param>
        public static void SetCellWidth(ExcelWorksheet worldSheet, int column)
        {
            int totalCol = 1;
            double maxSize = 124, colSize = 0;
            totalCol = column + 2;
            colSize = maxSize / (double)(totalCol);
            // Column size Index
            worldSheet.Column(1).Width = colSize;
            // Column size Name
            worldSheet.Column(2).Width = colSize * 2;
            // Column size Total
            worldSheet.Column(3).Width = colSize;
            // Column size Item
            for (int i = 4; i < column; i++)
                worldSheet.Column(i).Width = colSize;
        }

        public static void SetStyleSumCells(ExcelRange ex)
        {
            ex.Style.Font.Bold = true;
            ex.Style.Font.Color.SetColor(Color.Maroon);
        }

        /// <summary>
        /// Thiết lập Style cho ExcelRange
        /// </summary>
        /// <param name="er"></param>
        /// <param name="isBold"></param>
        /// <param name="isMerge"></param>
        public static void SetCellInfo(ExcelRange er, bool? isBold = null, bool? isMerge = null)
        {
            er.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            er.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            er.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            er.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            er.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            er.Style.WrapText = true;
            //er.AutoFitColumns();
            er.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            if (isMerge.HasValue)
            {
                if (isMerge.Value)
                {
                    er.Merge = true;
                }
            }
            if (isBold.HasValue)
            {
                if (isBold.Value)
                {
                    er.Style.Font.Bold = true;
                }
            }
        }

        /// <summary>
        /// Hàm vẽ thông tin chung của báo cáo(Header CommonInfo)
        /// </summary>
        /// <param name="worldSheet">Đối tượng ExcelWorksheet</param>
        /// <param name="colCount">Tổng số cột của Sheet</param>
        /// <param name="headerContent">Thông tin Header kiểu FHeader</param>
        public static void DrawHeaderCommonInfo(ExcelWorksheet worldSheet, int colCount, FHeader headerContent)
        {
            var infoLeft = (int)colCount / 2;
            // Company Name
            worldSheet.Cells[1, 2, 1, infoLeft].Merge = true;
            worldSheet.Cells[2, 2, 2, infoLeft].Merge = true;
            var d = colCount % 2;
            var colRightBegin = d == 0 ? colCount - infoLeft + 1 : colCount - infoLeft;
            worldSheet.Cells[1, colRightBegin, 1, colCount].Merge = true;
            worldSheet.Cells[2, colRightBegin, 2, colCount].Merge = true;
            worldSheet.Cells[3, colRightBegin, 3, colCount].Merge = true;

            worldSheet.Cells[1, colRightBegin].Value = headerContent.NationalTitle;
            worldSheet.Cells[2, colRightBegin].Value = headerContent.NationalSlogan;
            worldSheet.Cells[3, colRightBegin].Value = headerContent.SubTime;

            worldSheet.Cells[1, colRightBegin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worldSheet.Cells[2, colRightBegin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worldSheet.Cells[3, colRightBegin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worldSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worldSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            worldSheet.Cells[1, 2].Value = headerContent.CompanyName;
            worldSheet.Cells[2, 2].Value = headerContent.CompanySub;
            // Curent Date
            // Title
            worldSheet.Cells[5, 1, 5, colCount].Merge = true;
            worldSheet.Cells["A5"].Value = headerContent.Title;
            worldSheet.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Date From To
            worldSheet.Cells[6, 1, 6, colCount].Merge = true;
            worldSheet.Cells["A6"].Value = headerContent.DateFromTo;
            worldSheet.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worldSheet.Cells["A6"].Style.Font.Italic = true;
        }

        /// <summary>
        /// Hàm vẽ cell phần header trong table trên Worksheet Excel
        /// </summary>
        /// <param name="ff">Danh sách FFiedl được chuyển từ dạng TreeItem sang</param>
        /// <param name="rowStart">Dòng bắt đầu</param>
        /// <param name="columnStart">Cột bắt đầu</param>
        /// <param name="worldSheet">Đối tượng ExcelWorksheet</param>
        public static void DrawHeaderTabel(List<FField> ff, int rowStart, int columnStart, ExcelWorksheet worldSheet)
        {
            var cols = ff.Where(t => t.ColumnSpan == null || t.ColumnSpan == 0);
            if (cols.Any())
            {
                var row = ff.Where(t => t.Level == 1);
                if (row.Any())
                {
                    DrawCell(rowStart, columnStart, row.ToList(), ff, worldSheet);
                }
            }
        }

        /// <summary>
        /// Hàm vẽ Cell trên ExcelWorksheet
        /// </summary>
        /// <param name="rowStart">Hàng bắt đầu</param>
        /// <param name="columnStart">Cột bắt đầu</param>
        /// <param name="ffieldsChild"></param>
        /// <param name="ffields"></param>
        /// <param name="worldSheet"></param>
        public static void DrawCell(int rowStart, int columnStart, List<FField> ffieldsChild, List<FField> ffields, ExcelWorksheet worldSheet)
        {
            var colUse = 0;
            for (var j = 1; j <= ffieldsChild.Count; j++)
            {
                colUse++;
                var cell = ffieldsChild[j - 1];
                if (cell.ColumnSpan.HasValue && cell.RowSpan.HasValue)
                {
                    worldSheet.Cells[rowStart, columnStart + colUse, rowStart + cell.RowSpan.Value - 1, cell.ColumnSpan.Value + colUse + columnStart].Merge = true;
                    worldSheet.Cells[rowStart, columnStart + colUse, rowStart, columnStart + colUse + cell.ColumnSpan.Value - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                else
                {
                    if (!cell.ColumnSpan.HasValue && !cell.RowSpan.HasValue)
                    {
                        SetCellInfo(worldSheet.Cells[rowStart, colUse + columnStart], true);
                        worldSheet.Cells[rowStart, colUse + columnStart].Value = cell.FieldName;
                    }
                    else
                    {
                        if (cell.ColumnSpan.HasValue)
                        {
                            SetCellInfo(worldSheet.Cells[rowStart, colUse + columnStart, rowStart, columnStart + colUse + cell.ColumnSpan.Value - 1], true, true);
                            worldSheet.Cells[rowStart, colUse + columnStart].Value = cell.FieldName;
                            DrawCell(rowStart + 1, colUse + columnStart - 1, ffields.Where(t => t.ParentValue == cell.FieldName).ToList(), ffields, worldSheet);
                            colUse = colUse + cell.ColumnSpan.Value - 1;
                        }
                        if (cell.RowSpan.HasValue)
                        {
                            SetCellInfo(worldSheet.Cells[rowStart, colUse + columnStart, rowStart + cell.RowSpan.Value - 1, colUse + columnStart], true, true);
                            worldSheet.Cells[rowStart, colUse + columnStart].Value = cell.FieldName;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hàm set Style chung cho từng cell
        /// </summary>
        /// <param name="range"></param>
        /// <param name="value"></param>
        /// <param name="merge"></param>
        /// <param name="horizon"></param>
        /// <param name="vertical"></param>
        /// <param name="bold"></param>
        /// <param name="italic"></param>
        /// <param name="fontSize"></param>
        public static void ApplyStyle(ExcelRange range =null, string value=null, bool? merge = null
           , ExcelHorizontalAlignment? horizon = null, ExcelVerticalAlignment? vertical = null
           , bool? bold = null, bool? italic = null, float? fontSize = null)
        {
            //range = worldSheet.Cells[1, 4, 1, totalCol - 5];
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.WrapText = true;
            range.Style.ShrinkToFit = true;
            if (merge.HasValue)
                range.Merge = merge.Value;
            if (horizon.HasValue)
                range.Style.HorizontalAlignment = horizon.Value;
            if (vertical.HasValue)
                range.Style.VerticalAlignment = vertical.Value;
            if (bold.HasValue)
                range.Style.Font.Bold = bold.Value;
            if (italic.HasValue)
                range.Style.Font.Italic = italic.Value;
            if (fontSize.HasValue)
                range.Style.Font.Size = fontSize.Value;
            if (value != null)
            {
                range.Value = value;
            }
        }

        /// <summary>
        /// Hàm gen table html cho 1 tháng
        /// </summary>
        /// <param name="dayType">null hoặc 1 thì: dùng lịch có các ngày: thứ hai,thứ ba,...Bằng 2 thì lịch có dạng: thứ 2, thứ 3,...</param>
        /// <param name="isLandcape">null hoặc true: lịch hiển thị theo chiều ngang, false: lịch theo cột dọc</param>
        /// <param name="hasDayNight">null hoặc false: ko có thêm cột ngày tháng vào t7, cn, true: có cột ngày tháng</param>
        /// <param name="date">ngày tháng tạo lịch</param>
        /// <returns></returns>
        public static string GenTableCalendar(int? dayType=null, bool? isLandcape=null, bool? hasDayNight=null,DateTime? date=null)
        {
            var lstDay = new List<string>();
            string[] arrDaysDefault = { "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy", "Chủ nhật" };
            string[] arrDays = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
            var row = 6;
            var col = 7;
           
            lstDay = arrDaysDefault.ToList();
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }

            var daysInMonth = DateTime.DaysInMonth(date.Value.Year, date.Value.Month);

            if (dayType.HasValue)
            {
                if (dayType.Value == 2)
                {
                    lstDay.Clear();
                    lstDay = arrDays.ToList();
                }
            }

            if (isLandcape == false)
            {
                row = 7;
                col = 7;
            }
            var firstDate = FirstDate(new DateTime(date.Value.Year, date.Value.Month,1));
            var prevMonth = date.Value.AddMonths(-1);
            var nextMonth = date.Value.AddMonths(1);
            var daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
            var daysInNextMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);

            var result = "<table id='selectable' class='tbl ui-selectable' cellspacing='0' style='width:100%'>";
            var count = 0;
            var dayNext = 1;
            if (isLandcape != false || isLandcape == null)
            {
                result += "<thead>";
                var rowHeader = new FRow { IsHeader = true };
                for (var i =0 ;i<lstDay.Count;i++)
                {
                    var field = new FField { FieldName = lstDay[i] };
                    if (hasDayNight == true)
                    {
                        if (i == 5 || i == 6)
                        {
                            field.ColumnSpan = 2;
                        }
                        else
                        {
                            field.RowSpan = 2;
                        }
                    }
                    rowHeader.Cells.Add(field);
                }
                
                result += rowHeader.ToTrTag();
                if (hasDayNight == true)
                {
                    var tr2 = new FRow();
                    tr2.IsHeader = true;
                    tr2.Cells.Add(new FField { FieldName = "Ngày" });
                    tr2.Cells.Add(new FField { FieldName = "Đêm" });
                    tr2.Cells.Add(new FField { FieldName = "Ngày" });
                    tr2.Cells.Add(new FField { FieldName = "Đêm" });
                    result += tr2.ToTrTag();
                    col = col + 2;
                }
                result += "</thead><tbody>";

                for (var i = 0; i < row; i++)
                {
                    var rowBody = new FRow();
                    for (var j = 0; j < col; j++)
                    {
                        var ffield = new FField();
                        if (j < firstDate && i == 0)
                        {
                            var temp = daysInPrevMonth - firstDate + j + 1;
                            if (j == 5 && hasDayNight == true)
                            {
                                firstDate = firstDate + 1;
                            }
                            var temp1 = daysInPrevMonth == temp ? temp.ToString() + "/" + prevMonth.Month.ToString() : temp.ToString();
                            ffield.FieldName = "<span class ='itemspan day-of-prevmonth' id='" +  temp1+ "'>" + temp1 + "</span>";
                            ffield.ClassName = "item-scheduleOld";
                        }
                        else
                        {
                            if ((firstDate == 5 || firstDate == 6) && hasDayNight == true)
                            {
                                firstDate = firstDate + 1;
                            }
                            if (j == firstDate && i == 0)
                            {
                                count = 1;
                            }
                            if ((j > firstDate && i == 0) || (i > 0))
                            {
                                if (hasDayNight == true)
                                {
                                    if (j != 6 && j != 8)
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    count++;
                                }
                            }

                            if (count > daysInMonth)
                            {
                                var tt = count - daysInMonth;
                                var strNext = tt == 1 ? tt.ToString() + "/" + nextMonth.Month.ToString() : tt.ToString();
                                ffield.FieldName = "<span class ='itemspan day-of-prevmonth' id='" + strNext + "'>" + strNext + "</span>";
                                ffield.ClassName = "item-scheduleOld";
                            }
                            else
                            {
                                ffield.FieldName = count == 0 ? "" : "<span class ='itemspan' id='" + count.ToString() + "'>" + count.ToString() + "</span>";
                                ffield.ClassName = "item-schedule";
                            }

                            if (j > 4)
                            {
                                ffield.ClassName = ffield.ClassName + " sundayCalendar";
                            }
                        }
                        rowBody.Cells.Add(ffield);
                    }
                    result += rowBody.ToTrTag();
                }
            }
            else
            {
                result += "</thead><tbody>";
                var rowBody = new FRow();
                for (var j = 0; j < col; j++)
                {
                    for (var i = 0; i < row; i++)
                    {
                        var ffield = new FField();
                        if (j == 0)
                        {
                            ffield.FieldName = lstDay[i];  
                        }
                        else
                        {
                            if (i < firstDate && j == 1)
                            {
                                var temp = daysInPrevMonth - firstDate + i + 1;
                                var temp1 = daysInPrevMonth == temp ? temp.ToString() + "/" + prevMonth.Month.ToString() : temp.ToString();
                                ffield.FieldName = "<span class ='itemspan day-of-prevmonth' id='" + temp1 + "'>" + temp1 + "</span>";
                                ffield.ClassName = "item-scheduleOld";
                            }
                            else
                            {
                                if (i == firstDate && j == 1)
                                {
                                    count = 1;
                                }
                                if ((i > firstDate && j == 1) || (j > 1))
                                {
                                    count++;
                                }
                                if (count > daysInMonth)
                                {
                                    var strNext = dayNext == 1 ? dayNext.ToString() + "/" + nextMonth.Month.ToString() : dayNext.ToString();
                                    ffield.FieldName = "<span class ='itemspan day-of-prevmonth' id='" + strNext + "'>" + strNext + "</span>";
                                    ffield.ClassName = "item-scheduleOld";
                                    dayNext++;
                                }
                                else
                                {
                                    ffield.FieldName = count == 0  ? "" : "<span class ='itemspan' id='" + count.ToString() + "'>" + count.ToString() + "</span>";
                                    ffield.ClassName = "item-schedule";
                                }
                            }
                        }
                        ffield.ColPosition = j;
                        ffield.RowPosition = i;
                        rowBody.Cells.Add(ffield);
                    }
                }

                for (var i = 0; i < row; i++)
                {
                    var tr = new FRow();
                    tr.Cells =  rowBody.Cells.Where(t => t.RowPosition.Equals(i)).ToList();
                    result += tr.ToTrTag();
                }
            }

            result += "</tbody></table>";

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date1"></param>
        /// <returns></returns>
        public static int FirstDate(DateTime date1)
        {
            switch (date1.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 7;
            }
        }
    }
}