// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ReportOfHolidayController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
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
      this.WriteLog(enLogType.NomalLog, enActionType.View, "Truy cập chức năng Thống kê số ca trực ngày nghỉ lễ", "Truy cập thành công!", "N/A", 0, "", "");
      SelectListItem selectListItem = new SelectListItem()
      {
        Value = "0",
        Text = Localization.LabelSearchAll
      };
      List<SelectListItem> listItemBase = this.unitOfWork.Categories.GetListItemBase(3);
      listItemBase.Insert(0, selectListItem);
ViewBag.ListPosition = listItemBase;
      List<SelectListItem> list = this.unitOfWork.Feasts.GetAll().OrderBy<FEAST, string>((Func<FEAST, string>) (o => o.FEAST_TITLE)).Select<FEAST, SelectListItem>((Func<FEAST, SelectListItem>) (t => new SelectListItem()
      {
        Text = t.FEAST_TITLE.ToString(),
        Value = t.FEAST_ID.ToString()
      })).ToList<SelectListItem>();
      list.Insert(0, selectListItem);
ViewBag.ListFeast = list;
      return this.View();
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
ViewBag.ListDepartment = this.unitOfWork.Departments.GetListItemBase();
      PagedList<DoctorCalendarLeader> doctorCalendarHoliday = this.unitOfWork.CalendarDuty.GetDoctorCalendarHoliday(searchEntity, pageIndex, pagination.PageSize);
      pagination.TotalRows = doctorCalendarHoliday.TotalItemCount;
      pagination.CurrentRow = doctorCalendarHoliday.Count;
ViewBag.DoctorCalendarHoliday = doctorCalendarHoliday;
ViewBag.Pagination = pagination;
      return this.PartialView("_AddReportOfHolidayData");
    }

    [ActionDescription(ActionCode = "REPORTOFHOLIDAY_EXPORT", ActionName = "Xuất Excel", GroupCode = "REPORTOFHOLIDAY", GroupName = "Thống kê cán bộ trực theo ngày nghỉ lễ")]
    [CustomAuthorize]
    public ActionResult ExportReportOfHoliday(string SearchName, int SearchFeastId, int SearchDeprtId, int SearchPositionId)
    {
      FileInfo fileInfo = new FileInfo(this.Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfHoliday.xlsx"));
      if (fileInfo.Exists.Equals(false))
        throw new Exception("Export");
      PagedList<DoctorCalendarLeader> doctorCalendarHoliday = this.unitOfWork.CalendarDuty.GetDoctorCalendarHoliday(new DoctorSearch()
      {
        SearchName = SearchName,
        SearchFeastId = new int?(SearchFeastId),
        SearchDeprtId = new int?(SearchDeprtId),
        SearchPositionId = new int?(SearchPositionId)
      }, 0, int.MaxValue);
      byte[] asByteArray;
      using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
      {
        ExcelWorksheet worldSheet = excelPackage.Workbook.Worksheets[1];
        FReport freport = new FReport();
        freport.HeaderCommon.Title = "Thống kê cán bộ trực theo ngày nghỉ lễ".ToUpper();
        ExportReport.DrawHeaderCommonInfo(worldSheet, 7, freport.HeaderCommon);
        freport.HeaderItem = new TreeItem("Báo cáo", 0, "", new List<TreeItem>()
        {
          new TreeItem(Localization.LabelIndex, 1, "", (List<TreeItem>) null),
          new TreeItem(Localization.LabelDoctorName, 1, "", (List<TreeItem>) null),
          new TreeItem(Localization.LabelPosition, 1, "", (List<TreeItem>) null),
          new TreeItem(Localization.LabelHolidayContent, 1, "", (List<TreeItem>) null),
          new TreeItem(Localization.LabelHolidayPlace, 1, "", (List<TreeItem>) null),
          new TreeItem(Localization.LabelHolidayTime, 1, "", (List<TreeItem>) null),
          new TreeItem(Localization.LabelNote, 1, "", (List<TreeItem>) null)
        });
        List<FField> ffield = FUtils.ConvertToFField(freport.HeaderItem);
        if (ffield.Any<FField>())
        {
          int num = ffield.Select<FField, int>((Func<FField, int>) (t => t.Level)).Max() - 1;
          ExportReport.DrawHeaderTabel(ffield, 8, 0, worldSheet);
        }
        if (doctorCalendarHoliday.Any<DoctorCalendarLeader>())
        {
          List<SelectListItem> listItemBase = this.unitOfWork.Departments.GetListItemBase();
          ExcelRange excelRange = worldSheet.Cells[8, 1, 8 + doctorCalendarHoliday.Count, 7];
          int num = 0;
          foreach (DoctorCalendarLeader doctorCalendarLeader in (List<DoctorCalendarLeader>) doctorCalendarHoliday)
          {
            DoctorCalendarLeader item = doctorCalendarLeader;
            ++num;
            ExportReport.ApplyStyle(excelRange[num + 8, 1], num.ToString(), null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 2), new ExcelVerticalAlignment?(), null, null, null);
            ExportReport.ApplyStyle(excelRange[num + 8, 2], item.DOCTOR_NAME, null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 1), new ExcelVerticalAlignment?(), null, null, null);
            ExportReport.ApplyStyle(excelRange[num + 8, 3], item.POSITION_NAMEs, null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 1), new ExcelVerticalAlignment?(), null, null, null);
            ExportReport.ApplyStyle(excelRange[num + 8, 4], item.CALENDAR_DUTY_NAME, null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 1), new ExcelVerticalAlignment?(), null, null, null);
            SelectListItem selectListItem = listItemBase.FirstOrDefault<SelectListItem>((Func<SelectListItem, bool>) (t => t.Value.Equals(item.LM_DEPARTMENT_ID.ToString())));
            if (selectListItem != null)
              ExportReport.ApplyStyle(excelRange[num + 8, 5], selectListItem.Text, null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 1), new ExcelVerticalAlignment?(), null, null, null);
            DateTime? nullable = item.DATE_START;
            DateTime dateTime;
            string str1;
            if (!nullable.HasValue)
            {
              str1 = "";
            }
            else
            {
              nullable = item.DATE_START;
              dateTime = nullable.Value;
              str1 = dateTime.ToString("dd/MM/yyyy");
            }
            nullable = item.DATE_END;
            string str2;
            if (!nullable.HasValue)
            {
              str2 = "";
            }
            else
            {
              string str3 = " - ";
              nullable = item.DATE_END;
              dateTime = nullable.Value;
              string str4 = dateTime.ToString("dd/MM/yyyy");
              str2 = str3 + str4;
            }
            string str5 = str1 + str2;
            ExportReport.ApplyStyle(excelRange[num + 8, 6], str5, null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 2), new ExcelVerticalAlignment?(), null, null, null);
            ExportReport.ApplyStyle(excelRange[num + 8, 7], "", null, new ExcelHorizontalAlignment?((ExcelHorizontalAlignment) 1), new ExcelVerticalAlignment?(), null, null, null);
          }
        }
        asByteArray = excelPackage.GetAsByteArray();
      }
      return new DownloadResult(asByteArray, "ReportOfHoliday.xlsx");
    }
  }
}
