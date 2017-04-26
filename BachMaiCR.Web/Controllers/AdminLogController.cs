using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Utils;
using Resources;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class AdminLogController : BaseController
    {
        public AdminLogController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "ADMINLOG_VIEW", ActionName = "Xem nhật ký người dùng", GroupCode = "ADMINLOG", GroupName = "Nhật ký người dùng")]
        public ActionResult Index()
        {
            ViewBag.Title = "Nhật ký người dùng";
            return View();
        }

        [CustomAuthorize]
        [ValidateInput(false)]
        [ActionDescription(ActionCode = "ADMINLOG_VIEW", ActionName = "Xem nhật ký người dùng", GroupCode = "ADMINLOG", GroupName = "Nhật ký người dùng")]
        public PartialViewResult GetList(LogSearch searchEntity, int pageIndex = 0)
        {
            try
            {
                if (searchEntity == null)
                    searchEntity = new LogSearch();
                pageIndex = pageIndex <= 0 ? 0 : pageIndex;
                Pagination pagination = new Pagination()
                {
                    ActionName = "GetList",
                    ModelName = "AdminLog",
                    MaxPages = 7,
                    PageSize = 10,
                    CurrentPage = pageIndex,
                    InputSearchId = "txt_search_form",
                    TagetReLoadId = "cat_list_render"
                };
                PagedList<ADMIN_LOG> all = unitOfWork.AdminLogs.GetAll(searchEntity, pageIndex, pagination.PageSize);
                pagination.TotalRows = all.TotalItemCount;
                pagination.CurrentRow = all.Count;
                ViewBag.Category = all;
                ViewBag.Pagination = pagination;
                return PartialView("_Lists");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return null;
            }
        }

        [ActionDescription(ActionCode = "ADMINLOG_VIEW", ActionName = "Xem nhật ký người dùng", GroupCode = "ADMINLOG", GroupName = "Nhật ký người dùng")]
        [CustomAuthorize]
        public ActionResult OnSearch()
        {
            try
            {
                LogSearch logSearch = new LogSearch();
                List<SelectListItem> listMenuName = unitOfWork.AdminLogs.GetListMenuName();
                listMenuName.Insert(0, new SelectListItem()
                {
                    Value = string.Empty,
                    Text = Localization.LabelSearchAll
                });
                ViewBag.ListMenuName = listMenuName;
                ViewBag.ListStatus = DataTemplate.ListStatusSent;
                ViewBag.ListLogAction = EnumHelper<enActionType>.ConvertToSelectListItem(true, "-- Tất cả --");
                ViewBag.ListLogType = EnumHelper<enLogType>.ConvertToSelectListItem(true, "-- Tất cả --");
                return PartialView("_Search", logSearch);
            }
            catch (Exception ex)
            {
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "ADMINLOG_VIEW", ActionName = "Xem nhật ký người dùng", GroupCode = "ADMINLOG", GroupName = "Nhật ký người dùng")]
        [CustomAuthorize]
        public ActionResult OnDetail(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                ADMIN_LOG byId = unitOfWork.AdminLogs.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem nhật ký người dùng", Localization.MsgActionSuccess, "N/A", id, string.Empty, string.Empty);
                return PartialView("_Detail", byId);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem nhật ký người dùng", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [HttpPost]
        [ActionDescription(ActionCode = "ADMINLOG_DELETE", ActionName = "Xóa nhật ký người dùng", GroupCode = "ADMINLOG", GroupName = "Nhật ký người dùng")]
        public ActionResult OnDelete(string id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception(Localization.MsgItemInvalid);
                if (unitOfWork.AdminLogs.OnDeleteListId(id))
                {
                    WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa nhật ký người dùng", Localization.MsgDelSuccess, "N/A", 0, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgDelSuccess, true, "Index");
                }
                WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa nhật ký người dùng", Localization.MsgDelFailed, "N/A", 0, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelFailed, false, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa nhật ký người dùng", Localization.MsgDelFailed, ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }
    }
}