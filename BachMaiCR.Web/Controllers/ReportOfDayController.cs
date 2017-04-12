// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ReportOfDayController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using Resources;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class ReportOfDayController : BaseController
  {
    public ReportOfDayController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [ActionDescription(ActionCode = "REPORTOFDAY_VIEW", ActionName = "Xem báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    [CustomAuthorize]
    public ActionResult Index()
    {
ViewBag.Title = "Báo cáo hàng ngày";
      return (ActionResult) this.View();
    }

    [CustomAuthorize]
    [ValidateInput(false)]
    [ActionDescription(ActionCode = "REPORTOFDAY_VIEW", ActionName = "Xem báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    public PartialViewResult GetList(ItemSearch searchEntity, int pageIndex = 0)
    {
      try
      {
        if (searchEntity == null)
          searchEntity = new ItemSearch();
        pageIndex = pageIndex <= 0 ? 0 : pageIndex;
        Pagination pagination = new Pagination()
        {
          ActionName = "GetList",
          ModelName = "ReportOfDay",
          MaxPages = 7,
          PageSize = 10,
          CurrentPage = pageIndex,
          InputSearchId = "txt_search_form",
          TagetReLoadId = "cat_list_render"
        };
        int adminUserId = this.UserX.ADMIN_USER_ID;
        int? doctorsId = this.UserX.DOCTORS_ID;
        PagedList<REPORT> all = this.unitOfWork.ReportOfDays.GetAll(searchEntity, pageIndex, pagination.PageSize, adminUserId, doctorsId);
        pagination.TotalRows = all.TotalItemCount;
        pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
ViewBag.UserId = adminUserId;
        return this.PartialView("_Lists");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return (PartialViewResult) null;
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "REPORTOFDAY_VIEW", ActionName = "Xem báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    public ActionResult OnSearch()
    {
      try
      {
        return (ActionResult) this.PartialView("_Search", (object) new ItemSearch());
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Other, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    public ActionResult OnInsert(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        ReportOfDay reportOfDay = new ReportOfDay();
        reportOfDay.DateCreate = DateTime.Now;
        reportOfDay.UserCreateId = this.UserX.ADMIN_USER_ID;
        reportOfDay.UserCreateName = this.UserX.FULLNAME;
        string departmentName = this.UserX.LM_DEPARTMENT.DEPARTMENT_NAME;
        reportOfDay.Name = "Báo cáo nhân lực " + departmentName + " ngày " + DateTime.Now.ToString("dd-MM-yyyy");
ViewBag.ListDoctors = this.unitOfWork.Doctors.GetListItemBase();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "Thêm mới báo cáo hàng ngày", Localization.MsgActionSuccess, "N/A", 0, "", "");
        return (ActionResult) this.PartialView("_Insert", (object) reportOfDay);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    [CustomAuthorize]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        REPORT byId = this.unitOfWork.ReportOfDays.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        ReportOfDay reportOfDay = new ReportOfDay(byId);
ViewBag.ListDoctors = this.unitOfWork.Doctors.GetListItemBase();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "Sửa báo cáo hàng ngày", Localization.MsgActionSuccess, "N/A", 0, "", "");
        return (ActionResult) this.PartialView("_Insert", (object) reportOfDay);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "Sửa báo cáo hàng ngày", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "REPORTOFDAY_VIEW", ActionName = "Xem báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    [CustomAuthorize]
    public ActionResult OnDetail(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        REPORT byId = this.unitOfWork.ReportOfDays.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        ReportOfDay reportOfDay = new ReportOfDay(byId);
ViewBag.ListDoctors = this.unitOfWork.Doctors.GetListItemBase();
        this.WriteLog(enLogType.NomalLog, enActionType.View, "Xem báo cáo chi tiết hàng ngày", Localization.MsgActionSuccess, "N/A", id, "", "");
        return (ActionResult) this.PartialView("_Detail", (object) reportOfDay);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.View, "Xem báo cáo chi tiết hàng ngày", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [HttpPost]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    public ActionResult OnSent(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        REPORT byId = this.unitOfWork.ReportOfDays.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.STATUS = 1;
        byId.DATE_SENDED = new DateTime?(DateTime.Now);
        this.unitOfWork.ReportOfDays.Update(byId);
        this.unitOfWork.ReportOfDays.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "Gửi báo cáo hàng ngày", "Gửi báo cáo thành công", "N/A", id, "", "");
        return this.ReturnValue("Gửi báo cáo thành công!", true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "Gửi báo cáo hàng ngày", "Gửi báo cáo thất bại", ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "REPORTOFDAY_DELETE", ActionName = "Xóa báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    [CustomAuthorize]
    [HttpPost]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        REPORT byId = this.unitOfWork.ReportOfDays.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        if (byId.STATUS.Equals(1))
          throw new Exception("Không được phép xóa báo cáo đã gửi!");
        byId.ISDELETE = true;
        this.unitOfWork.ReportOfDays.Update(byId);
        this.unitOfWork.ReportOfDays.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ValidateAntiForgeryToken]
    [ValidateInput(false)]
    [HttpPost]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
    public ActionResult SubmitChange(ReportOfDay entity)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        string mutilNameDoctors = this.unitOfWork.Doctors.GetMutilNameDoctors(entity.UserRecipientId);
        entity.UserRecipientName = mutilNameDoctors;
        REPORT getModelMap = entity.GetModelMap;
        if (getModelMap.REPORT_ID.Equals(0))
        {
          getModelMap.DATE_CREATE = DateTime.Now;
          if (getModelMap.STATUS.Equals(1))
          {
            getModelMap.DATE_SENDED = new DateTime?(getModelMap.DATE_CREATE);
            getModelMap.STATUS = 1;
          }
          else
            getModelMap.STATUS = 0;
          getModelMap.LM_DEPARTMENT_NAME = this.UserX.LM_DEPARTMENT.DEPARTMENT_NAME;
          getModelMap.LM_DEPARTMENT_ID = this.UserX.LM_DEPARTMENT_ID.Value;
          if (this.unitOfWork.ReportOfDays.CheckExistDateCreate(getModelMap.DATE_CREATE, getModelMap.LM_DEPARTMENT_ID))
            throw new Exception("Báo cáo ngày hôm nay đã được lập, vui lòng kiểm tra lại!");
          getModelMap.USER_CREATE_ID = this.UserX.ADMIN_USER_ID;
          getModelMap.USER_CREATE_NAME = this.UserX.FULLNAME;
          getModelMap.ISDELETE = new bool?(false);
          this.unitOfWork.ReportOfDays.Add(getModelMap);
          this.unitOfWork.ReportOfDays.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgAddSuccess, "N/A", getModelMap.REPORT_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        REPORT byId = this.unitOfWork.ReportOfDays.GetById(getModelMap.REPORT_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        if (byId.STATUS.Equals(1))
          throw new Exception("Không được phép sửa báo cáo đã gửi!");
        if (getModelMap.STATUS.Equals(1))
        {
          byId.DATE_SENDED = new DateTime?(DateTime.Now);
          byId.STATUS = 1;
        }
        byId.REPORT_NAME = getModelMap.REPORT_NAME;
        byId.USER_RECIPIENTS_NAMEs = getModelMap.USER_RECIPIENTS_NAMEs;
        byId.USER_RECIPIENTS_IDs = getModelMap.USER_RECIPIENTS_IDs;
        byId.NUMBER_STAFF_BUSINESS_TRIP = getModelMap.NUMBER_STAFF_BUSINESS_TRIP;
        byId.NUMBER_STAFF_DIRECT = getModelMap.NUMBER_STAFF_DIRECT;
        byId.NUMBER_STAFF_LEAVE = getModelMap.NUMBER_STAFF_LEAVE;
        byId.NUMBER_STAFF_MATERNITY = getModelMap.NUMBER_STAFF_MATERNITY;
        byId.NUMBER_STAFF_SICK = getModelMap.NUMBER_STAFF_SICK;
        byId.NUMBER_STAFF_UNPAID = getModelMap.NUMBER_STAFF_UNPAID;
        byId.NUMBER_STAFF_VACATION = getModelMap.NUMBER_STAFF_VACATION;
        this.unitOfWork.ReportOfDays.Update(byId);
        this.unitOfWork.ReportOfDays.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", getModelMap.REPORT_ID, "", "");
        return this.ReturnValue(Localization.MsgEditSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }
  }
}
