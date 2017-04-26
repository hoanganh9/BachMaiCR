using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Utilities.ReportForm;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Common.CustomActionResult;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class ReportOfHolidayController : BaseController
    {
        public ReportOfHolidayController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "REPORTOFHOLIDAY_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOLIDAY", GroupName = "Thống kê cán bộ trực theo ngày nghỉ lễ")]
        public ActionResult Index()
        {
            ViewBag.Title = "Thống kê cán bộ trực theo ngày nghỉ lễ";
            WriteLog(enLogType.NomalLog, enActionType.View, "Truy cập chức năng Thống kê số ca trực ngày nghỉ lễ", "Truy cập thành công!", "N/A", 0, string.Empty, string.Empty);
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = "0",
                Text = Localization.LabelSearchAll
            };
            List<SelectListItem> listItemBase = unitOfWork.Categories.GetListItemBase(3);
            listItemBase.Insert(0, selectListItem);
            ViewBag.ListPosition = listItemBase;
            List<SelectListItem> list = unitOfWork.Feasts.GetAll().OrderBy((o => o.FEAST_TITLE)).Select((t => new SelectListItem()
            {
                Text = t.FEAST_TITLE.ToString(),
                Value = t.FEAST_ID.ToString()
            })).ToList();
            list.Insert(0, selectListItem);
            ViewBag.ListFeast = list;
            return View();
        }

        [ActionDescription(ActionCode = "REPORTOFHOLIDAY_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOLIDAY", GroupName = "Thống kê cán bộ trực theo ngày nghỉ lễ")]
        [HttpGet]
        [CustomAuthorize]
        public PartialViewResult LoadReportOfHoliday(DoctorSearch searchEntity, int pageIndex = 0)
        {
            if (searchEntity == null)
                searchEntity = new DoctorSearch();
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "LoadReportOfHoliday",
                ModelName = "ReportOfHoliday",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render"
            };
            ViewBag.ListDepartment = unitOfWork.Departments.GetListItemBase();
            PagedList<DoctorCalendarLeader> doctorCalendarHoliday = unitOfWork.CalendarDuty.GetDoctorCalendarHoliday(searchEntity, pageIndex, pagination.PageSize);
            pagination.TotalRows = doctorCalendarHoliday.TotalItemCount;
            pagination.CurrentRow = doctorCalendarHoliday.Count;
            ViewBag.DoctorCalendarHoliday = doctorCalendarHoliday;
            ViewBag.Pagination = pagination;
            return PartialView("_AddReportOfHolidayData");
        }

        [ActionDescription(ActionCode = "REPORTOFHOLIDAY_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOLIDAY", GroupName = "Thống kê cán bộ trực theo ngày nghỉ lễ")]
        [CustomAuthorize]
        public ActionResult ExportReportOfHoliday(string SearchName, int SearchFeastId, int SearchDeprtId, int SearchPositionId)
        {
            FileInfo fileInfo = new FileInfo(Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfHoliday.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Export");
            PagedList<DoctorCalendarLeader> doctorCalendarHoliday = unitOfWork.CalendarDuty.GetDoctorCalendarHoliday(new DoctorSearch()
            {
                SearchName = SearchName,
                SearchFeastId = SearchFeastId,
                SearchDeprtId = SearchDeprtId,
                SearchPositionId = SearchPositionId
            }, 0, int.MaxValue);
            byte[] asByteArray;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet worldSheet = excelPackage.Workbook.Worksheets[1];
                FReport freport = new FReport();
                freport.HeaderCommon.Title = "Thống kê cán bộ trực theo ngày nghỉ lễ".ToUpper();
                ExportReport.DrawHeaderCommonInfo(worldSheet, 7, freport.HeaderCommon);
                freport.HeaderItem = new TreeItem("Báo cáo", 0, string.Empty, new List<TreeItem>()
                    {
                      new TreeItem(Localization.LabelIndex, 1, string.Empty, null),
                      new TreeItem(Localization.LabelDoctorName, 1, string.Empty, null),
                      new TreeItem(Localization.LabelPosition, 1, string.Empty, null),
                      new TreeItem(Localization.LabelHolidayContent, 1, string.Empty, null),
                      new TreeItem(Localization.LabelHolidayPlace, 1, string.Empty, null),
                      new TreeItem(Localization.LabelHolidayTime, 1, string.Empty, null),
                      new TreeItem(Localization.LabelNote, 1, string.Empty, null)
                    });
                List<FField> ffield = FUtils.ConvertToFField(freport.HeaderItem);
                if (ffield.Any())
                {
                    int num = ffield.Select((t => t.Level)).Max() - 1;
                    ExportReport.DrawHeaderTabel(ffield, 8, 0, worldSheet);
                }
                if (doctorCalendarHoliday.Any())
                {
                    List<SelectListItem> listItemBase = unitOfWork.Departments.GetListItemBase();
                    ExcelRange excelRange = worldSheet.Cells[8, 1, 8 + doctorCalendarHoliday.Count, 7];
                    int num = 0;
                    foreach (DoctorCalendarLeader item in doctorCalendarHoliday)
                    {
                        ++num;
                        ExportReport.ApplyStyle(excelRange[num + 8, 1], num.ToString(), null, ExcelHorizontalAlignment.Center, null, null, null, null);
                        ExportReport.ApplyStyle(excelRange[num + 8, 2], item.DOCTOR_NAME, null, ExcelHorizontalAlignment.Left, null, null, null, null);
                        ExportReport.ApplyStyle(excelRange[num + 8, 3], item.POSITION_NAMEs, null, ExcelHorizontalAlignment.Left, null, null, null, null);
                        ExportReport.ApplyStyle(excelRange[num + 8, 4], item.CALENDAR_DUTY_NAME, null, ExcelHorizontalAlignment.Left, null, null, null, null);
                        SelectListItem selectListItem = listItemBase.FirstOrDefault<SelectListItem>((t => t.Value.Equals(item.LM_DEPARTMENT_ID.ToString())));
                        if (selectListItem != null)
                            ExportReport.ApplyStyle(excelRange[num + 8, 5], selectListItem.Text, null, ExcelHorizontalAlignment.Left, null, null, null, null);
                        string strStart = string.Empty;
                        if (item.DATE_START.HasValue)
                        {
                            strStart = item.DATE_START.Value.ToString("dd/MM/yyyy");
                        }
                        string strEnd = string.Empty;
                        if (item.DATE_END.HasValue)
                        {
                            strEnd = " - " + item.DATE_END.Value.ToString("dd/MM/yyyy");
                        }
                        string str5 = strStart + strEnd;
                        ExportReport.ApplyStyle(excelRange[num + 8, 6], str5, null, ExcelHorizontalAlignment.Center, null, null, null, null);
                        ExportReport.ApplyStyle(excelRange[num + 8, 7], string.Empty, null, ExcelHorizontalAlignment.Left, null, null, null, null);
                    }
                }
                asByteArray = excelPackage.GetAsByteArray();
            }
            return new DownloadResult(asByteArray, "ReportOfHoliday.xlsx");
        }
    }
}