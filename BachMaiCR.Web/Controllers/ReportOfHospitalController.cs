using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Utilities.ReportForm;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Common.CustomActionResult;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class ReportOfHospitalController : BaseController
    {
        public ReportOfHospitalController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "REPORTOFHOSPITAL_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOSPITAL", GroupName = "Thống kê số ca trực toàn viện")]
        public ActionResult Index()
        {
            ViewBag.Title = "Thống kê số ca trực toàn viện";
            WriteLog(enLogType.NomalLog, enActionType.View, "Truy cập chức năng Thống kê số ca trực toàn viện", "Truy cập thành công!", "N/A", 0, string.Empty, string.Empty);
            return View();
        }

        [CustomAuthorize]
        [HttpGet]
        [ActionDescription(ActionCode = "REPORTOFHOSPITAL_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOSPITAL", GroupName = "Thống kê số ca trực toàn viện")]
        public PartialViewResult LoadReportOfHospital(string strDate)
        {
            if (!Request.IsAjaxRequest())
                return null;
            StringBuilder stringBuilder1 = new StringBuilder("<thead>");
            List<SelectListItem> listItemBase = unitOfWork.Categories.GetListItemBase(3);
            List<DEPARTMENTLIST> departmentByLevel = unitOfWork.Departments.GetAllDepartmentByLevel(1);
            ViewBag.objDepartment = departmentByLevel;
            ViewBag.lstPositions = listItemBase;
            List<int> intList = departmentByLevel.Any() ? departmentByLevel.Select((t => t.LM_DEPARTMENT_ID)).ToList() : new List<int>();
            List<DoctorCalendarLeader> hospitalBySearch = GetDoctorCalendarHospitalBySearch(strDate);
            int num1 = 0;
            FRow frow1 = new FRow();
            FRow frow2 = new FRow();
            frow2.Cells.Add(new FField()
            {
                FieldName = "Tổng",
                ColumnSpan = 2
            });
            frow1.IsHeader = true;
            frow1.Cells.Add(new FField()
            {
                FieldName = "STT",
                FieldData = "STT",
                Width = 40.0
            });
            frow1.Cells.Add(new FField()
            {
                FieldName = "Đơn vị",
                FieldData = "Đơn vị",
                Width = 280.0
            });
            int num2 = 0;
            foreach (SelectListItem selectListItem in listItemBase)
            {
                SelectListItem position = selectListItem;
                frow1.Cells.Add(new FField()
                {
                    FieldName = position.Text
                });
                int num3 = hospitalBySearch.Where((t => t.POSITION_IDs.Contains(position.Value))).Count<DoctorCalendarLeader>();
                num2 += num3;
                frow2.Cells.Add(new FField()
                {
                    FieldName = num3.ToString()
                });
            }
            frow1.Cells.Add(new FField() { FieldName = "Tổng" });
            stringBuilder1.Append(frow1.ToTrTag() + "</thead><tbody>");
            foreach (DEPARTMENTLIST departmentlist in departmentByLevel)
            {
                DEPARTMENTLIST dept = departmentlist;
                ++num1;
                FRow frow3 = new FRow();
                frow3.Cells.Add(new FField()
                {
                    FieldName = num1.ToString()
                });
                frow3.Cells.Add(new FField()
                {
                    FieldName = dept.DEPARTMENT_NAME
                });
                foreach (SelectListItem selectListItem in listItemBase)
                {
                    SelectListItem position = selectListItem;
                    int num3 = hospitalBySearch.Where((t => t.LM_DEPARTMENT_IDs.Contains("," + dept.LM_DEPARTMENT_ID.ToString() + ",") && t.POSITION_IDs.Contains(position.Value))).Count<DoctorCalendarLeader>();
                    frow3.Cells.Add(new FField()
                    {
                        FieldName = num3.ToString()
                    });
                }
                int num4 = hospitalBySearch == null || hospitalBySearch.Count == 0 ? 0 : hospitalBySearch.Where((t => t.LM_DEPARTMENT_IDs.Contains("," + dept.LM_DEPARTMENT_ID.ToString() + ","))).Count<DoctorCalendarLeader>();
                frow3.Cells.Add(new FField()
                {
                    FieldName = num4.ToString()
                });
                stringBuilder1.Append(frow3.ToTrTag());
            }
            frow2.Cells.Add(new FField()
            {
                FieldName = num2.ToString()
            });
            stringBuilder1.Append(frow2.ToTrTag());
            StringBuilder stringBuilder2 = stringBuilder1.Replace("<script>", "&lt;script&gt;").Replace("</script>", "&lt;&#47;script&gt;");
            ViewBag.tbodyStr = stringBuilder2.Replace("\"", "'") + "</tbody>";
            return PartialView("_AddReportOfHospitalData");
        }

        [ActionDescription(ActionCode = "REPORTOFHOSPITAL_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOSPITAL", GroupName = "Thống kê số ca trực toàn viện")]
        [CustomAuthorize]
        public ActionResult ExportReportOfHospital(string strData)
        {
            FileInfo fileInfo = new FileInfo(Server.MapPath("~/Views/Shared/ExcelTemplate/FormLandscapeA4.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Export");
            byte[] asByteArray;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet worldSheet = excelPackage.Workbook.Worksheets[1];
                FReport freport = new FReport();
                List<DEPARTMENTLIST> departmentByLevel = unitOfWork.Departments.GetAllDepartmentByLevel(1);
                if (departmentByLevel.Any())
                {
                    List<SelectListItem> lstPositions = unitOfWork.Categories.GetListItemBase(3);
                    if (lstPositions.Any())
                    {
                        List<DoctorCalendarLeader> hospitalBySearch = GetDoctorCalendarHospitalBySearch(strData);
                        List<TreeItem> children = new List<TreeItem>();
                        children.Add(new TreeItem("STT", 1, string.Empty, null));
                        children.Add(new TreeItem("Đơn vị", 1, string.Empty, null));
                        foreach (SelectListItem selectListItem in lstPositions)
                            children.Add(new TreeItem(selectListItem.Text, 1, string.Empty, null));
                        children.Add(new TreeItem("Tổng", 1, string.Empty, null));
                        freport.HeaderItem = new TreeItem("Báo cáo", 0, string.Empty, children);
                        List<FField> ffield = FUtils.ConvertToFField(freport.HeaderItem);
                        if (ffield.Any())
                        {
                            int num1 = ffield.Select((t => t.Level)).Max() - 1;
                            ExportReport.DrawHeaderTabel(ffield, 8, 0, worldSheet);
                            ExportReport.SetCellWidth(worldSheet, lstPositions.Count + 3);
                            ExcelRange er = worldSheet.Cells[num1 + 8, 1, 8 + num1 + departmentByLevel.Count + 1, lstPositions.Count + 3];
                            ExportReport.SetCellInfo(er, null, null);
                            int num2 = 0;
                            foreach (DEPARTMENTLIST departmentlist in departmentByLevel)
                            {
                                DEPARTMENTLIST dept = departmentlist;
                                ++num2;
                                (er[num2 + 8, 1]).Value = num2;
                                (er[num2 + 8, 2]).Value = dept.DEPARTMENT_NAME;
                                for (int i = 0; i < lstPositions.Count; ++i)
                                    (er[num2 + 8, i + 3]).Value = (hospitalBySearch.Where((t => t.LM_DEPARTMENT_ID.Equals(dept.LM_DEPARTMENT_ID) && t.POSITION_IDs.Contains(lstPositions[i].Value))).Count<DoctorCalendarLeader>());
                                (er[num2 + 8, lstPositions.Count + 3]).Value = ((hospitalBySearch == null || hospitalBySearch.Count == 0 ? 0 : hospitalBySearch.Where((t => t.LM_DEPARTMENT_ID.Equals(dept.LM_DEPARTMENT_ID))).Count<DoctorCalendarLeader>()));
                                ExportReport.SetStyleSumCells(er[num2 + 8, lstPositions.Count + 3]);
                            }
                            int num3 = 0;
                            ExcelRange ex = er[9 + num1 + departmentByLevel.Count, 1, 9 + num1 + departmentByLevel.Count, 2];
                            (ex).Merge = true;
                            (ex).Value = "Tổng";
                            ExportReport.SetStyleSumCells(ex);
                            for (int j = 0; j < lstPositions.Count; ++j)
                            {
                                if (hospitalBySearch != null && hospitalBySearch.Count > 0)
                                {
                                    int num4 = hospitalBySearch.Where((t => t.POSITION_IDs.Contains(lstPositions[j].Value))).Count<DoctorCalendarLeader>();
                                    (er[9 + num1 + departmentByLevel.Count, j + 3]).Value = num4;
                                    ExportReport.SetStyleSumCells(er[9 + num1 + departmentByLevel.Count, j + 3]);
                                    num3 += num4;
                                }
                            }
                          (er[9 + num1 + departmentByLevel.Count, lstPositions.Count + 3]).Value = num3;
                            ExportReport.SetStyleSumCells(er[9 + num1 + departmentByLevel.Count, lstPositions.Count + 3]);
                            freport.HeaderCommon.Title = "Thống kê số ca trực toàn viện".ToUpper();
                            ExportReport.DrawHeaderCommonInfo(worldSheet, lstPositions.Count + 3, freport.HeaderCommon);
                        }
                    }
                }
                asByteArray = excelPackage.GetAsByteArray();
            }
            return new DownloadResult(asByteArray, "ReportOfHospital.xlsx");
        }

        private List<DoctorCalendarLeader> GetDoctorCalendarHospitalBySearch(string strDate)
        {
            DateTime dateTime1 = Utils.Utils.GetDateTime();
            Utils.Utils.GetDateTime();
            Utils.Utils.GetDateTime();
            int result1 = dateTime1.Month;
            int result2 = dateTime1.Year;
            int result3 = -1;
            DutyType.Hospital.GetHashCode();
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                int.TryParse(strArray[0].ToString(), out result3);
                if (int.TryParse(strArray[1].ToString(), out result1) && int.TryParse(strArray[2].ToString(), out result2))
                    dateTime1 = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result1), Convert.ToString(result2));
            }
            else
            {
                int day = dateTime1.Day;
                result3 = day > 7 ? (day <= 7 || day > 14 ? (day <= 14 || day > 21 ? 3 : 2) : 1) : 0;
            }
            DateTime dateTime2;
            DateTime dateTime3;
            if (result3 < 3)
            {
                int num = result3 * 7;
                dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays((result3 * 7));
                dateTime3 = dateTime2.AddDays(6.0);
            }
            else
            {
                int num1 = DateTime.DaysInMonth(result2, result1) - 28;
                int num2 = result3 * 7;
                dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                dateTime3 = dateTime2.AddDays((6 + num1));
            }
            return unitOfWork.CalendarDuty.GetDoctorCalendarHospital(result1, result2, dateTime2, dateTime3);
        }
    }
}