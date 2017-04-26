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
    public class ConfigDirectController : BaseController
    {
        public ConfigDirectController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [ActionDescription(ActionCode = "CONFIG_DIRECT_VIEW", ActionName = "Xem danh sách cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
        [CustomAuthorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách cán bộ đi trực";
            return View();
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CONFIG_DIRECT_VIEW", ActionName = "Xem danh sách cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
        [ValidateInput(false)]
        public PartialViewResult GetList(ConfigHolidaySearch searchEntity, int pageIndex = 0)
        {
            if (searchEntity == null)
                searchEntity = new ConfigHolidaySearch();
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "GetList",
                ModelName = "ConfigDirect",
                MaxPages = 7,
                PageSize = 10,
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render"
            };
            PagedList<CONFIG_DIRECT> all = unitOfWork.ConfigDirect.GetAll(searchEntity, pageIndex, pagination.PageSize);
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
                int? lmDepartmentId;
                int num1;
                if (currentUser.LM_DEPARTMENT_ID.HasValue)
                {
                    IDepartmentRepository departments = unitOfWork.Departments;
                    bool? isdelete = departments.GetById(currentUser.LM_DEPARTMENT_ID.Value).ISDELETE;
                    if (!isdelete.GetValueOrDefault())
                    {
                        IDoctorRepository doctors = unitOfWork.Doctors;
                        lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
                        int deptId = lmDepartmentId.Value;
                        source = doctors.GetAllByDepartmentId(deptId);
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
                            intList1.Add(currentUser.LM_DEPARTMENT_ID.Value);
                            lmDepartmentList.Add(byId);
                        }
                    }
                }
                selectListItemList = source.Where(d => !d.ISDELETE.GetValueOrDefault()).OrderBy(d => d.DOCTOR_NAME).Select(d => new SelectListItem()
                {
                    Text = d.DOCTOR_NAME,
                    Value = d.DOCTORS_ID.ToString(),
                    Selected = currentUser.DOCTORS_ID == d.DOCTORS_ID
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

        private List<SelectListItem> GetFeast(int isAdd)
        {
            string str = isAdd != 0 ? Localization.LabelSelect : Localization.LabelSearchAll;
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = "0",
                Text = str
            };
            List<SelectListItem> list = unitOfWork.Feasts.GetAll().OrderBy((o => o.FEAST_TITLE)).Select((t => new SelectListItem()
            {
                Text = t.FEAST_TITLE.ToString(),
                Value = t.FEAST_ID.ToString()
            })).ToList();
            list.Insert(0, selectListItem);
            return list;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CONFIG_DIRECT_VIEW", ActionName = "Xem danh sách cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
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
                ViewBag.ListHoliday = GetFeast(0);
                OnList(0);
                ViewBag.ActionUpdate = CheckPermistion("CONFIG_DIRECT_UPDATE");
                return PartialView("_Search", configHolidaySearch);
            }
            catch (Exception ex)
            {
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CONFIG_DIRECT_UPDATE", ActionName = "Cập nhật thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
        public ActionResult OnInsert(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                ViewBag.ListEducation = GetFeast(1);
                OnList(1);
                return PartialView("_Insert", new ConfigDirect());
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CONFIG_DIRECT_UPDATE", ActionName = "Cập nhật thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                CONFIG_DIRECT byId = unitOfWork.ConfigDirect.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                ConfigDirect configDirect = new ConfigDirect(byId);
                ViewBag.ListEducation = GetFeast(1);
                OnList(1);
                return PartialView("_Insert", configDirect);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "CONFIG_DIRECT_DELETE", ActionName = "Xóa thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
        [HttpPost]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                CONFIG_DIRECT byId = unitOfWork.ConfigDirect.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.ISDELETE = true;
                unitOfWork.ConfigDirect.Update(byId);
                unitOfWork.ConfigDirect.Save();
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ValidateInput(false)]
        [ActionDescription(ActionCode = "CONFIG_DIRECT_UPDATE", ActionName = "Cập nhật thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SubmitChange(ConfigDirect entity)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                CONFIG_DIRECT configDirect = entity.GetConfigDirect();
                if (configDirect.CONFIG_DIRECT_ID.Equals(0))
                {
                    configDirect.ISDELETE = false;
                    configDirect.DATE_CREATE = DateTime.Now;
                    configDirect.USER_CREATE_ID = UserX.ADMIN_USER_ID;
                    unitOfWork.ConfigDirect.Add(configDirect);
                    unitOfWork.ConfigDirect.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, string.Empty, Localization.MsgAddSuccess, "N/A", configDirect.CONFIG_DIRECT_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                CONFIG_DIRECT byId = unitOfWork.ConfigDirect.GetById(configDirect.CONFIG_DIRECT_ID);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.DOCTORS_ID = configDirect.DOCTORS_ID;
                byId.LM_DEPARTMENT_ID = configDirect.LM_DEPARTMENT_ID;
                byId.DATE_BEGIN = configDirect.DATE_BEGIN;
                byId.DATE_END = configDirect.DATE_END;
                byId.FEAST_ID = configDirect.FEAST_ID;
                unitOfWork.ConfigDirect.Update(byId);
                unitOfWork.ConfigDirect.Save();
                WriteLog(enLogType.NomalLog, enActionType.Update, byId.DOCTOR.DOCTOR_NAME, Localization.MsgEditSuccess, "N/A", configDirect.CONFIG_DIRECT_ID, string.Empty, string.Empty);
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