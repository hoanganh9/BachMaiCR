using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class FeastController : BaseController
    {
        public FeastController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [ActionDescription(ActionCode = "FEAST_VIEW", ActionName = "Xem thông tin danh mục ngày nghỉ lễ", GroupCode = "FEAST", GroupName = "Danh mục ngày nghỉ lễ")]
        [CustomAuthorize]
        public ActionResult Index()
        {
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionUpdate = list.Any() && list.Contains("FEAST_SAVE");
            return View("Index");
        }

        [ActionDescription(ActionCode = "FEAST_SAVE", ActionName = "Cập nhật danh mục ngày nghỉ lễ", GroupCode = "FEAST", GroupName = "Danh mục ngày nghỉ lễ")]
        [CustomAuthorize]
        public ActionResult OnInsert()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                Feast feast = new Feast();
                if (Request.IsAjaxRequest())
                    return PartialView("~/Views/Feast/Add.cshtml", feast);
                return null;
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Feast/Index.cshtml");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "FEAST_SAVE", ActionName = "Cập nhật danh mục ngày nghỉ lễ", GroupCode = "FEAST", GroupName = "Danh mục ngày nghỉ lễ")]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemNotExist);
                FEAST byId = unitOfWork.Feasts.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                return PartialView("~/Views/Feast/Add.cshtml", new Feast(byId));
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Feast/Index.cshtml");
            }
        }

        [ActionDescription(ActionCode = "FEAST_DELETE", ActionName = "Xóa danh mục ngày nghỉ lễ", GroupCode = "FEAST", GroupName = "Danh mục ngày nghỉ lễ")]
        [HttpPost]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                FEAST byId = unitOfWork.Feasts.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.ISDELETE = true;
                unitOfWork.Feasts.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Feast/Index.cshtml");
            }
        }

        [ValidateInput(false)]
        [ActionDescription(ActionCode = "FEAST_SAVE", ActionName = "Cập nhật danh mục ngày nghỉ lễ", GroupCode = "FEAST", GroupName = "Danh mục ngày nghỉ lễ")]
        [HttpPost]
        public ActionResult SubmitChange(Feast entity)
        {
            try
            {
                FEAST categoryModel = entity.GetCategoryModel();
                entity.IsDeleted = false;
                if (entity.FeastId.Equals(0))
                {
                    categoryModel.ISDELETE = false;
                    categoryModel.FEAST_YEAR = new int?(Utils.Utils.GetDateTime().Year);
                    unitOfWork.Feasts.Add(categoryModel);
                    unitOfWork.Feasts.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", Localization.MsgAddSuccess, "N/A", categoryModel.FEAST_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                FEAST byId = unitOfWork.Feasts.GetById(categoryModel.FEAST_ID);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.FEAST_TITLE = categoryModel.FEAST_TITLE.Trim();
                unitOfWork.Feasts.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", categoryModel.FEAST_ID, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Feast/Index.cshtml");
            }
        }

        [CustomAuthorize]
        [ValidateInput(false)]
        public PartialViewResult GetCategoryList(string name, string sortFiled, string sortDir, int pageIndex = 0)
        {
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "GetCategoryList",
                ModelName = "Feast",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render"
            };
            name = string.IsNullOrEmpty(name) ? string.Empty : name.Trim();
            sortFiled = string.IsNullOrEmpty(sortFiled) ? "FEAST_TITLE" : sortFiled;
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            int year = DateTime.Now.Year;
            PagedList<FEAST> allList = unitOfWork.Feasts.GetAllList(name, year, pageIndex, pagination.PageSize, sortFiled, sortDir);
            pagination.TotalRows = allList.TotalItemCount;
            pagination.CurrentRow = allList.Count;
            ViewBag.Category = allList;
            ViewBag.Pagination = pagination;
            ViewBag.ActionUpdate = CheckPermistion("FEAST_SAVE");
            ViewBag.ActionDelete = CheckPermistion("FEAST_DELETE");
            return PartialView("_PartialList");
        }

        [ActionDescription(ActionCode = "CONFIG_DIRECT_VIEW", ActionName = "Xem danh sách cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực", IsMenu = false)]
        [HttpGet]
        [CustomAuthorize]
        public ActionResult GetFeastInfo(int feastId)
        {
            FEAST byId = unitOfWork.Feasts.GetById(feastId);
            var data = new
            {
                dateBegin = string.Format("{0:dd/MM/yyyy}", byId.DATE_BEGIN),
                dateEnd = string.Format("{0:dd/MM/yyyy}", byId.DATE_END)
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}