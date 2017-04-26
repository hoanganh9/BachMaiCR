using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
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
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class DepartmentController : BaseController
    {
        public DepartmentController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [ActionDescription(ActionCode = "DEPARTMENT_VIEW", ActionName = "Xem phòng ban", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý phòng ban";
            return View();
        }

        [ActionDescription(ActionCode = "DEPARTMENT_VIEW", ActionName = "Xem phòng ban", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban", IsMenu = false)]
        [ValidateInput(false)]
        [CustomAuthorize]
        public PartialViewResult getList(int type, string name, string sortFiled, string sortDir, int pageIndex = 0)
        {
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "getList",
                ModelName = "Department",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render",
                CtgType = type
            };
            name = string.IsNullOrEmpty(name) ? string.Empty : name.Trim();
            sortFiled = string.IsNullOrEmpty(sortFiled) ? "DEPARTMENT_PATH" : sortFiled;
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            PagedList<LM_DEPARTMENT> all = unitOfWork.Departments.GetAll(type, name, pageIndex, pagination.PageSize, sortFiled, sortDir);
            pagination.TotalRows = all.TotalItemCount;
            pagination.CurrentRow = all.Count;
            ViewBag.Category = all;
            ViewBag.Pagination = pagination;
            ViewBag.ActionUpdate = CheckPermistion("DEPARTMENT_SAVE");
            ViewBag.ActionDelete = CheckPermistion("DEPARTMENT_DELETE");
            return PartialView("_GetList");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "DEPARTMENT_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban", IsMenu = false)]
        public ActionResult OnInsert(int parentId = 0)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            Department department = new Department();
            department.Parent = parentId;
            WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", 0, string.Empty, string.Empty);
            if (Request.IsAjaxRequest())
                return PartialView("_Insert", department);
            return null;
        }

        [ActionDescription(ActionCode = "DEPARTMENT_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban")]
        [CustomAuthorize]
        public ActionResult OnUpdate(int id = 0)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemNotExist);
                LM_DEPARTMENT byId = unitOfWork.Departments.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                return PartialView("_Insert", new Department(byId));
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [HttpPost]
        [ActionDescription(ActionCode = "DEPARTMENT_DELETE", ActionName = "Xóa phòng ban", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban")]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                LM_DEPARTMENT byId = unitOfWork.Departments.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                if (byId.LM_DEPARTMENT_ID.Equals(1))
                    throw new Exception("Bạn không thể xóa thông tin này!");
                if (unitOfWork.Departments.ExistChild(byId.LM_DEPARTMENT_ID) || unitOfWork.Users.ExistReferenceDepartment(id) || (unitOfWork.Templates.ExistReferenceDepartment(id) || unitOfWork.CalendarDuty.ExistReferenceDepartment(id)) || unitOfWork.Roles.ExistReferenceDepartment(id) || unitOfWork.Doctors.ExistReferenceDepartment(id))
                    throw new Exception(Localization.MsgItemReference);
                byId.ISDELETE = true;
                unitOfWork.Departments.Update(byId);
                unitOfWork.Departments.Save();
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [HttpPost]
        [ActionDescription(ActionCode = "DEPARTMENT_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban")]
        [ValidateInput(false)]
        public ActionResult SubmitChange(Department entity)
        {
            try
            {
                LM_DEPARTMENT categoryModel = entity.GetCategoryModel();
                if (unitOfWork.Departments.ExistCode(categoryModel))
                    throw new Exception(Localization.MsgCodeExist);
                if (categoryModel.LM_DEPARTMENT_ID.Equals(0))
                {
                    string str1 = ",";
                    int? departmentParentId = categoryModel.DEPARTMENT_PARENT_ID;
                    if (departmentParentId.HasValue && departmentParentId.Value >0)
                    {
                        IDepartmentRepository departments = unitOfWork.Departments;
                        LM_DEPARTMENT byId = departments.GetById(departmentParentId.Value);
                        if (byId == null)
                            return Json(JsonResponse.Json500("Thông tin không hợp lệ!"));
                        str1 = byId.DEPARTMENT_PATH;
                        LM_DEPARTMENT lmDepartment = categoryModel;
                        int level;
                        if (!byId.LEVELS.HasValue)
                        {
                            level = 0;
                        }
                        else
                        {
                            level = byId.LEVELS.Value + 1;
                        }
                        lmDepartment.LEVELS = level;
                        categoryModel.DEPARTMENT_PARENT_ID = byId.LM_DEPARTMENT_ID;
                    }
                    else
                        entity.Level = 0;
                    categoryModel.ISDELETE = false;
                    unitOfWork.Departments.Add(categoryModel);
                    unitOfWork.Departments.Save();
                    string str2 = str1 + categoryModel.LM_DEPARTMENT_ID + ",";
                    categoryModel.DEPARTMENT_PATH = str2;
                    unitOfWork.Departments.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgAddSuccess, "N/A", categoryModel.LM_DEPARTMENT_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                LM_DEPARTMENT byId1 = unitOfWork.Departments.GetById(categoryModel.LM_DEPARTMENT_ID);
                byId1.DEPARTMENT_NAME = categoryModel.DEPARTMENT_NAME;
                byId1.DEPARTMENT_CODE = categoryModel.DEPARTMENT_CODE;
                byId1.DEPARTMENT_PHONE = categoryModel.DEPARTMENT_PHONE;
                byId1.DEPARTMENT_FAX = categoryModel.DEPARTMENT_FAX;
                byId1.DEPARTMENT_ADDRESS = categoryModel.DEPARTMENT_ADDRESS;
                byId1.DESCRIPTION = categoryModel.DESCRIPTION;
                unitOfWork.Departments.Update(byId1);
                unitOfWork.Departments.Save();
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", categoryModel.LM_DEPARTMENT_ID, string.Empty, string.Empty);
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