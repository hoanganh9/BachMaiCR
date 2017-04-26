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
using Resources;
using System;
using System.Web.Mvc;

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
            return View();
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
                int adminUserId = UserX.ADMIN_USER_ID;
                int? doctorsId = UserX.DOCTORS_ID;
                PagedList<REPORT> all = unitOfWork.ReportOfDays.GetAll(searchEntity, pageIndex, pagination.PageSize, adminUserId, doctorsId);
                pagination.TotalRows = all.TotalItemCount;
                pagination.CurrentRow = all.Count;
                ViewBag.Category = all;
                ViewBag.Pagination = pagination;
                ViewBag.UserId = adminUserId;
                return PartialView("_Lists");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return null;
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "REPORTOFDAY_VIEW", ActionName = "Xem báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        public ActionResult OnSearch()
        {
            try
            {
                return PartialView("_Search", new ItemSearch());
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Other, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        public ActionResult OnInsert(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                ReportOfDay reportOfDay = new ReportOfDay();
                reportOfDay.DateCreate = DateTime.Now;
                reportOfDay.UserCreateId = UserX.ADMIN_USER_ID;
                reportOfDay.UserCreateName = UserX.FULLNAME;
                string departmentName = UserX.LM_DEPARTMENT.DEPARTMENT_NAME;
                reportOfDay.Name = "Báo cáo nhân lực " + departmentName + " ngày " + DateTime.Now.ToString("dd-MM-yyyy");
                ViewBag.ListDoctors = unitOfWork.Doctors.GetListItemBase();
                WriteLog(enLogType.NomalLog, enActionType.Update, "Thêm mới báo cáo hàng ngày", Localization.MsgActionSuccess, "N/A", 0, string.Empty, string.Empty);
                return PartialView("_Insert", reportOfDay);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        [CustomAuthorize]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                REPORT byId = unitOfWork.ReportOfDays.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                ReportOfDay reportOfDay = new ReportOfDay(byId);
                ViewBag.ListDoctors = unitOfWork.Doctors.GetListItemBase();
                WriteLog(enLogType.NomalLog, enActionType.Update, "Sửa báo cáo hàng ngày", Localization.MsgActionSuccess, "N/A", 0, string.Empty, string.Empty);
                return PartialView("_Insert", reportOfDay);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "Sửa báo cáo hàng ngày", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "REPORTOFDAY_VIEW", ActionName = "Xem báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        [CustomAuthorize]
        public ActionResult OnDetail(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                REPORT byId = unitOfWork.ReportOfDays.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                ReportOfDay reportOfDay = new ReportOfDay(byId);
                ViewBag.ListDoctors = unitOfWork.Doctors.GetListItemBase();
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem báo cáo chi tiết hàng ngày", Localization.MsgActionSuccess, "N/A", id, string.Empty, string.Empty);
                return PartialView("_Detail", reportOfDay);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem báo cáo chi tiết hàng ngày", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [HttpPost]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        public ActionResult OnSent(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                REPORT byId = unitOfWork.ReportOfDays.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.STATUS = 1;
                byId.DATE_SENDED = DateTime.Now;
                unitOfWork.ReportOfDays.Update(byId);
                unitOfWork.ReportOfDays.Save();
                WriteLog(enLogType.NomalLog, enActionType.Update, "Gửi báo cáo hàng ngày", "Gửi báo cáo thành công", "N/A", id, string.Empty, string.Empty);
                return ReturnValue("Gửi báo cáo thành công!", true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "Gửi báo cáo hàng ngày", "Gửi báo cáo thất bại", ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "REPORTOFDAY_DELETE", ActionName = "Xóa báo cáo hàng ngày", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        [CustomAuthorize]
        [HttpPost]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                REPORT byId = unitOfWork.ReportOfDays.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                if (byId.STATUS.Equals(1))
                    throw new Exception("Không được phép xóa báo cáo đã gửi!");
                byId.ISDELETE = true;
                unitOfWork.ReportOfDays.Update(byId);
                unitOfWork.ReportOfDays.Save();
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "REPORTOFDAY_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "REPORTOFDAY", GroupName = "Báo cáo hàng ngày")]
        public ActionResult SubmitChange(ReportOfDay entity)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                string mutilNameDoctors = unitOfWork.Doctors.GetMutilNameDoctors(entity.UserRecipientId);
                entity.UserRecipientName = mutilNameDoctors;
                REPORT getModelMap = entity.GetModelMap;
                if (getModelMap.REPORT_ID.Equals(0))
                {
                    getModelMap.DATE_CREATE = DateTime.Now;
                    if (getModelMap.STATUS.Equals(1))
                    {
                        getModelMap.DATE_SENDED = getModelMap.DATE_CREATE;
                        getModelMap.STATUS = 1;
                    }
                    else
                        getModelMap.STATUS = 0;
                    getModelMap.LM_DEPARTMENT_NAME = UserX.LM_DEPARTMENT.DEPARTMENT_NAME;
                    getModelMap.LM_DEPARTMENT_ID = UserX.LM_DEPARTMENT_ID.Value;
                    if (unitOfWork.ReportOfDays.CheckExistDateCreate(getModelMap.DATE_CREATE, getModelMap.LM_DEPARTMENT_ID))
                        throw new Exception("Báo cáo ngày hôm nay đã được lập, vui lòng kiểm tra lại!");
                    getModelMap.USER_CREATE_ID = UserX.ADMIN_USER_ID;
                    getModelMap.USER_CREATE_NAME = UserX.FULLNAME;
                    getModelMap.ISDELETE = false;
                    unitOfWork.ReportOfDays.Add(getModelMap);
                    unitOfWork.ReportOfDays.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgAddSuccess, "N/A", getModelMap.REPORT_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                REPORT byId = unitOfWork.ReportOfDays.GetById(getModelMap.REPORT_ID);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                if (byId.STATUS.Equals(1))
                    throw new Exception("Không được phép sửa báo cáo đã gửi!");
                if (getModelMap.STATUS.Equals(1))
                {
                    byId.DATE_SENDED = DateTime.Now;
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
                unitOfWork.ReportOfDays.Update(byId);
                unitOfWork.ReportOfDays.Save();
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", getModelMap.REPORT_ID, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }
    }
}