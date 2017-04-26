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
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CATEGORY_SAVE", ActionName = "Cập nhật danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        public ActionResult OnInsert(int type)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                Category category = new Category();
                category.Type = type;
                if (Request.IsAjaxRequest())
                    return PartialView("~/Views/Category/AddCategory.cshtml", category);
                return null;
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CATEGORY_SAVE", ActionName = "Cập nhật danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemNotExist);
                LM_CATEGORY byId = unitOfWork.Categories.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                return PartialView("~/Views/Category/AddCategory.cshtml", new Category(byId));
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
            }
        }

        [ActionDescription(ActionCode = "CATEGORY_DELETE", ActionName = "Xóa danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        [HttpPost]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                LM_CATEGORY byId = unitOfWork.Categories.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                if (unitOfWork.Doctors.ExistReferenceCategory(byId.LM_CATEGORY_ID))
                    throw new Exception(Localization.MsgItemReference);
                byId.ISDELETE = true;
                unitOfWork.Categories.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionDescription(ActionCode = "CATEGORY_SAVE", ActionName = "Cập nhật danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        public ActionResult SubmitChange(Category entity)
        {
            try
            {
                LM_CATEGORY categoryModel = entity.GetCategoryModel();
                if (unitOfWork.Categories.ExistCode(categoryModel))
                    throw new Exception(Localization.MsgCodeExist);
                entity.IsDel = false;
                if (entity.Id.Equals(0))
                {
                    categoryModel.ISDELETE = false;
                    unitOfWork.Categories.Add(categoryModel);
                    unitOfWork.Categories.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", Localization.MsgAddSuccess, "N/A", categoryModel.LM_CATEGORY_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                LM_CATEGORY byId = unitOfWork.Categories.GetById(categoryModel.LM_CATEGORY_ID);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.CATEGORY_NAME = categoryModel.CATEGORY_NAME;
                byId.CODE = categoryModel.CODE;
                byId.CATEGORY_DESCRIPTION = categoryModel.CATEGORY_DESCRIPTION;
                unitOfWork.Categories.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", categoryModel.LM_CATEGORY_ID, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
            }
        }

        [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        [CustomAuthorize]
        public ActionResult ProvinceIndex()
        {
            ViewBag.Type = enCategoryType.Province;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionUpdate = list.Any() && list.Contains("CATEGORY_SAVE");
            return View("Index");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        public ActionResult PositionIndex()
        {
            ViewBag.Type = enCategoryType.Position;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionUpdate = list.Any() && list.Contains("CATEGORY_SAVE");
            return View("Index");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        public ActionResult EducationIndex()
        {
            ViewBag.Type = enCategoryType.EducationIndex;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionUpdate = list.Any() && list.Contains("CATEGORY_SAVE");
            return View("Index");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
        public ActionResult HolidaysIndex()
        {
            ViewBag.Type = enCategoryType.TypeOfHolidays;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionUpdate = list.Any() && list.Contains("CATEGORY_SAVE");
            return View("Index");
        }

        [ValidateInput(false)]
        [CustomAuthorize]
        public PartialViewResult GetCategoryList(int type, string name, string sortFiled, string sortDir, int pageIndex = 0)
        {
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "GetCategoryList",
                ModelName = "Category",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render",
                CtgType = type
            };
            name = string.IsNullOrEmpty(name) ? string.Empty : name.Trim();
            sortFiled = string.IsNullOrEmpty(sortFiled) ? "CODE" : sortFiled;
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            PagedList<LM_CATEGORY> all = unitOfWork.Categories.GetAll(type, name, pageIndex, pagination.PageSize, sortFiled, sortDir);
            pagination.TotalRows = all.TotalItemCount;
            pagination.CurrentRow = all.Count;
            ViewBag.Category = all;
            ViewBag.Pagination = pagination;
            ViewBag.ActionUpdate = CheckPermistion("CATEGORY_SAVE");
            ViewBag.ActionDelete = CheckPermistion("CATEGORY_DELETE");
            return PartialView("_PartialCategoryList");
        }
    }
}