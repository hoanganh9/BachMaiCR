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
    public class DoctorLevelController : BaseController
    {
        public DoctorLevelController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [ActionDescription(ActionCode = "DOCTORLEVEL_VIEW", ActionName = "Xem thông tin vị trí", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
        [CustomAuthorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Danh mục vị trí cán bộ";
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionUpdate = list.Any() && list.Contains("DOCTORLEVEL_SAVE");
            return View();
        }

        [ActionDescription(ActionCode = "DOCTORLEVEL_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
        [CustomAuthorize]
        public ActionResult OnInsert(int type)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                DoctorLevel doctorLevel = new DoctorLevel();
                if (Request.IsAjaxRequest())
                    return PartialView("_AddDoctorLevel", doctorLevel);
                return null;
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "DOCTORLEVEL_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                DOCTOR_LEVEL byId = unitOfWork.DoctorLevels.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                DoctorLevel doctorLevel = new DoctorLevel(byId);
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", id, string.Empty, string.Empty);
                return PartialView("_AddDoctorLevel", doctorLevel);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "DOCTORLEVEL_DELETE", ActionName = "Xóa thông tin vị trí", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
        [CustomAuthorize]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                DOCTOR_LEVEL byId = unitOfWork.DoctorLevels.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.ISDELETE = true;
                unitOfWork.DoctorLevels.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Delete, byId.CODE + "-" + byId.LEVEL_NAME, Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "DOCTORLEVEL_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitChange(DoctorLevel entity)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                DOCTOR_LEVEL categoryModel = entity.GetCategoryModel();
                if (unitOfWork.DoctorLevels.ExistCode(categoryModel))
                    throw new Exception(string.Format("Mã kí hiệu đã tồn tại, hãy chọn mã kí hiệu khác", entity.Code));
                entity.IsDel = false;
                if (entity.Id.Equals(0))
                {
                    categoryModel.ISDELETE = false;
                    unitOfWork.DoctorLevels.Add(categoryModel);
                    unitOfWork.DoctorLevels.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, categoryModel.CODE + "-" + categoryModel.LEVEL_NAME, Localization.MsgAddSuccess, "N/A", categoryModel.DOCTOR_LEVEL_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                DOCTOR_LEVEL byId = unitOfWork.DoctorLevels.GetById(categoryModel.DOCTOR_LEVEL_ID);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.LEVEL_NAME = categoryModel.LEVEL_NAME;
                byId.CODE = categoryModel.CODE;
                byId.LEVEL_NUMBER = categoryModel.LEVEL_NUMBER;
                unitOfWork.DoctorLevels.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Update, categoryModel.CODE + "-" + categoryModel.LEVEL_NAME, Localization.MsgEditSuccess, "N/A", categoryModel.DOCTOR_LEVEL_ID, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ValidateInput(false)]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "DOCTORLEVEL_VIEW", ActionName = "Xem thông tin vị trí", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
        public PartialViewResult DoctorLevelList(int type, string name, string sortFiled, string sortDir, int pageIndex = 0)
        {
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "DoctorLevelList",
                ModelName = "DoctorLevel",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render",
                CtgType = type
            };
            name = string.IsNullOrEmpty(name) ? string.Empty : name.Trim();
            sortFiled = string.IsNullOrEmpty(sortFiled) ? "LEVEL_NUMBER" : sortFiled;
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            PagedList<DOCTOR_LEVEL> all = unitOfWork.DoctorLevels.GetAll(type, name, pageIndex, pagination.PageSize, sortFiled, sortDir);
            pagination.TotalRows = all.TotalItemCount;
            pagination.CurrentRow = all.Count;
            ViewBag.Category = all;
            ViewBag.Pagination = pagination;
            ViewBag.ActionUpdate = CheckPermistion("DOCTORLEVEL_SAVE");
            ViewBag.ActionDelete = CheckPermistion("DOCTORLEVEL_DELETE");
            return PartialView("_DoctorLevelList");
        }
    }
}