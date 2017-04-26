using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
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
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class ConfigHolidaysController : BaseController
    {
        public ConfigHolidaysController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_VIEW", ActionName = "Xem danh sách cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        [CustomAuthorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách cán bộ nghỉ";
            return View();
        }

        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_VIEW", ActionName = "Xem danh sách cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        [CustomAuthorize]
        [ValidateInput(false)]
        public PartialViewResult GetList(ConfigHolidaySearch searchEntity, int pageIndex = 0)
        {
            if (searchEntity == null)
                searchEntity = new ConfigHolidaySearch();
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "GetList",
                ModelName = "ConfigHolidays",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render"
            };
            PagedList<CONFIG_HOLIDAYS> all = unitOfWork.ConfigHolidays.GetAll(searchEntity, pageIndex, pagination.PageSize);
            pagination.TotalRows = all.TotalItemCount;
            pagination.CurrentRow = all.Count;
            ViewBag.Category = all;
            ViewBag.Pagination = pagination;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_GetList");
        }

        private void OnList(int isAdd = 0)
        {
            ADMIN_USER currentUser = unitOfWork.Users.GetByUserName(User.Identity.Name);
            List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            if (currentUser != null)
            {
                List<int> intList1 = new List<int>();
                List<DOCTOR> source = new List<DOCTOR>();
                if (currentUser.LM_DEPARTMENT_ID.HasValue)
                {
                    bool? isdelete = unitOfWork.Departments.GetById(currentUser.LM_DEPARTMENT_ID.Value).ISDELETE;
                    if (!isdelete.GetValueOrDefault())
                    {
                        source = unitOfWork.Doctors.GetAllByDepartmentId(currentUser.LM_DEPARTMENT_ID.Value);
                    }
                }
                else if (currentUser.USERNAME == "admin")
                {
                    lmDepartmentList = unitOfWork.Departments.GetChildDepartment(0).ToList();
                }
                else
                {
                    if (currentUser.LM_DEPARTMENT_ID.HasValue && currentUser.LM_DEPARTMENT_ID.Value > 0)
                    {
                        LM_DEPARTMENT byId = unitOfWork.Departments.GetById(currentUser.LM_DEPARTMENT_ID.Value);
                        bool? isdelete = byId.ISDELETE;
                        if (!isdelete.GetValueOrDefault())
                        {
                            List<int> intList2 = intList1;
                            intList2.Add(currentUser.LM_DEPARTMENT_ID.Value);
                            lmDepartmentList.Add(byId);
                        }
                    }
                }
                selectListItemList = source.Where(d => !d.ISDELETE.GetValueOrDefault()).OrderBy(d => d.DOCTOR_NAME).Select(d => new SelectListItem()
                {
                    Text = d.DOCTOR_NAME,
                    Value = d.DOCTORS_ID.ToString(),
                    Selected = d.DOCTORS_ID == currentUser.DOCTORS_ID
                }).ToList();
            }
            string str = isAdd != 0 ? Localization.LabelSelect : Localization.LabelSearchAll;
            SelectListItem selectListItem = new SelectListItem()
            {
                Text = str,
                Value = "0"
            };
            selectListItemList.Insert(0, selectListItem);
            ViewBag.ListDoctor = selectListItemList;
            ViewBag.RootDepartment = lmDepartmentList;
        }

        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_VIEW", ActionName = "Xem danh sách cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        [CustomAuthorize]
        public ActionResult OnSearch()
        {
            try
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Value = "0",
                    Text = Localization.LabelSearchAll
                };
                ConfigHolidaySearch configHolidaySearch = new ConfigHolidaySearch();
                List<SelectListItem> listItemBase = unitOfWork.Categories.GetListItemBase(5);
                listItemBase.Insert(0, selectListItem);
                ViewBag.ListHoliday = listItemBase;
                OnList(0);
                ViewBag.ActionUpdate = CheckPermistion("CONFIG_HOLIDAYS_UPDATE");
                return PartialView("_Search", configHolidaySearch);
            }
            catch (Exception ex)
            {
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_UPDATE", ActionName = "Cập nhật thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        public ActionResult OnInsert(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                ViewBag.ListEducation = unitOfWork.Categories.GetListItemBase(5);
                OnList(1);
                return PartialView("_Insert", new ConfigHoliday());
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_UPDATE", ActionName = "Cập nhật thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        [CustomAuthorize]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                CONFIG_HOLIDAYS byId = unitOfWork.ConfigHolidays.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                ConfigHoliday configHoliday = new ConfigHoliday(byId);
                ViewBag.ListEducation = unitOfWork.Categories.GetListItemBase(5);
                OnList(1);
                return PartialView("_Insert", configHoliday);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_DELETE", ActionName = "Xóa thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        [HttpPost]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                CONFIG_HOLIDAYS byId = unitOfWork.ConfigHolidays.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.ISDELETE = true;
                unitOfWork.ConfigHolidays.Update(byId);
                unitOfWork.ConfigHolidays.Save();
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_UPDATE", ActionName = "Cập nhật thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitChange(ConfigHoliday entity)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                CONFIG_HOLIDAYS configHoliday = entity.GetConfigHoliday();
                if (configHoliday.CONFIG_HOLIDAY_ID.Equals(0))
                {
                    configHoliday.ISDELETE = false;
                    configHoliday.DATE_CREATE = DateTime.Now;
                    configHoliday.USER_CREATE_ID = UserX.ADMIN_USER_ID;
                    unitOfWork.ConfigHolidays.Add(configHoliday);
                    unitOfWork.ConfigHolidays.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, string.Empty, Localization.MsgAddSuccess, "N/A", configHoliday.CONFIG_HOLIDAY_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                CONFIG_HOLIDAYS byId = unitOfWork.ConfigHolidays.GetById(configHoliday.CONFIG_HOLIDAY_ID);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.DOCTORS_ID = configHoliday.DOCTORS_ID;
                byId.LM_DEPARTMENT_ID = configHoliday.LM_DEPARTMENT_ID;
                byId.DATE_BEGIN = configHoliday.DATE_BEGIN;
                byId.DATE_END = configHoliday.DATE_END;
                byId.HOLIDAYS_ID = configHoliday.HOLIDAYS_ID;
                unitOfWork.ConfigHolidays.Update(byId);
                unitOfWork.ConfigHolidays.Save();
                WriteLog(enLogType.NomalLog, enActionType.Update, byId.DOCTOR.DOCTOR_NAME, Localization.MsgEditSuccess, "N/A", configHoliday.CONFIG_HOLIDAY_ID, string.Empty, string.Empty);
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