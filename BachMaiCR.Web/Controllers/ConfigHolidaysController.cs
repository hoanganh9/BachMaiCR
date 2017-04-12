﻿// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ConfigHolidaysController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
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
      return (ActionResult) this.View();
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
      PagedList<CONFIG_HOLIDAYS> all = this.unitOfWork.ConfigHolidays.GetAll(searchEntity, pageIndex, pagination.PageSize);
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
      List<string> actionCodesByUserName = this.GetActionCodesByUserName();
ViewBag.Actions = actionCodesByUserName;
      return this.PartialView("_GetList");
    }

    private void OnList(int isAdd = 0)
    {
      ADMIN_USER currentUser = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      if (currentUser != null)
      {
        List<int> intList1 = new List<int>();
        List<DOCTOR> source = new List<DOCTOR>();
        int? lmDepartmentId;
        int num1;
        if (currentUser != null)
        {
          lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
          num1 = !lmDepartmentId.HasValue ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 == 0)
        {
          IDepartmentRepository departments = this.unitOfWork.Departments;
          lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
          int id = lmDepartmentId.Value;
          bool? isdelete = departments.GetById(id).ISDELETE;
          if ((isdelete.GetValueOrDefault() ? 0 : (isdelete.HasValue ? 1 : 0)) != 0)
          {
            IDoctorRepository doctors = this.unitOfWork.Doctors;
            lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
            int deptId = lmDepartmentId.Value;
            source = doctors.GetAllByDepartmentId(deptId);
          }
        }
        else if (currentUser.USERNAME == "admin")
        {
          lmDepartmentList = this.unitOfWork.Departments.GetChildDepartment(0).ToList<LM_DEPARTMENT>();
        }
        else
        {
          lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
          int num2;
          if (lmDepartmentId.HasValue)
          {
            lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
            num2 = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          }
          else
            num2 = 1;
          if (num2 == 0)
          {
            IDepartmentRepository departments = this.unitOfWork.Departments;
            lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
            int id = lmDepartmentId.Value;
            LM_DEPARTMENT byId = departments.GetById(id);
            bool? isdelete = byId.ISDELETE;
            if ((isdelete.GetValueOrDefault() ? 0 : (isdelete.HasValue ? 1 : 0)) != 0)
            {
              List<int> intList2 = intList1;
              lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
              int num3 = lmDepartmentId.Value;
              intList2.Add(num3);
              lmDepartmentList.Add(byId);
            }
          }
        }
        selectListItemList = source.Where<DOCTOR>((Func<DOCTOR, bool>) (d =>
        {
          bool? isdelete = d.ISDELETE;
          return !isdelete.GetValueOrDefault() && isdelete.HasValue;
        })).OrderBy<DOCTOR, string>((Func<DOCTOR, string>) (d => d.DOCTOR_NAME)).Select<DOCTOR, SelectListItem>((Func<DOCTOR, SelectListItem>) (d =>
        {
          SelectListItem selectListItem1 = new SelectListItem();
          selectListItem1.Text = d.DOCTOR_NAME;
          selectListItem1.Value = d.DOCTORS_ID.ToString();
          SelectListItem selectListItem2 = selectListItem1;
          int num;
          if (currentUser.DOCTORS_ID.HasValue)
          {
            int doctorsId1 = d.DOCTORS_ID;
            int? doctorsId2 = currentUser.DOCTORS_ID;
            if ((doctorsId1 != doctorsId2.GetValueOrDefault() ? 0 : (doctorsId2.HasValue ? 1 : 0)) == 0)
            {
              num = 0;
              goto label_4;
            }
          }
          num = 1;
label_4:
          selectListItem2.Selected = num != 0;
          return selectListItem1;
        })).ToList<SelectListItem>();
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
        List<SelectListItem> listItemBase = this.unitOfWork.Categories.GetListItemBase(5);
        listItemBase.Insert(0, selectListItem);
ViewBag.ListHoliday = listItemBase;
        this.OnList(0);
ViewBag.ActionUpdate = this.CheckPermistion("CONFIG_HOLIDAYS_UPDATE");
        return (ActionResult) this.PartialView("_Search", (object) configHolidaySearch);
      }
      catch (Exception ex)
      {
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_UPDATE", ActionName = "Cập nhật thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
    public ActionResult OnInsert(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
ViewBag.ListEducation = this.unitOfWork.Categories.GetListItemBase(5);
        this.OnList(1);
        return (ActionResult) this.PartialView("_Insert", (object) new ConfigHoliday());
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_UPDATE", ActionName = "Cập nhật thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
    [CustomAuthorize]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        CONFIG_HOLIDAYS byId = this.unitOfWork.ConfigHolidays.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        ConfigHoliday configHoliday = new ConfigHoliday(byId);
ViewBag.ListEducation = this.unitOfWork.Categories.GetListItemBase(5);
        this.OnList(1);
        return (ActionResult) this.PartialView("_Insert", (object) configHoliday);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_DELETE", ActionName = "Xóa thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
    [HttpPost]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        CONFIG_HOLIDAYS byId = this.unitOfWork.ConfigHolidays.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.ISDELETE = true;
        this.unitOfWork.ConfigHolidays.Update(byId);
        this.unitOfWork.ConfigHolidays.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_HOLIDAYS_UPDATE", ActionName = "Cập nhật thông tin cán bộ nghỉ", GroupCode = "CONFIG_HOLIDAYS", GroupName = "Danh sách cán bộ nghỉ")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    [ValidateInput(false)]
    public ActionResult SubmitChange(ConfigHoliday entity)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        CONFIG_HOLIDAYS configHoliday = entity.GetConfigHoliday();
        if (configHoliday.CONFIG_HOLIDAY_ID.Equals(0))
        {
          configHoliday.ISDELETE = new bool?(false);
          configHoliday.DATE_CREATE = new DateTime?(DateTime.Now);
          configHoliday.USER_CREATE_ID = new int?(this.UserX.ADMIN_USER_ID);
          this.unitOfWork.ConfigHolidays.Add(configHoliday);
          this.unitOfWork.ConfigHolidays.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, "", Localization.MsgAddSuccess, "N/A", configHoliday.CONFIG_HOLIDAY_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        CONFIG_HOLIDAYS byId = this.unitOfWork.ConfigHolidays.GetById(configHoliday.CONFIG_HOLIDAY_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.DOCTORS_ID = configHoliday.DOCTORS_ID;
        byId.LM_DEPARTMENT_ID = configHoliday.LM_DEPARTMENT_ID;
        byId.DATE_BEGIN = configHoliday.DATE_BEGIN;
        byId.DATE_END = configHoliday.DATE_END;
        byId.HOLIDAYS_ID = configHoliday.HOLIDAYS_ID;
        this.unitOfWork.ConfigHolidays.Update(byId);
        this.unitOfWork.ConfigHolidays.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, byId.DOCTOR.DOCTOR_NAME, Localization.MsgEditSuccess, "N/A", configHoliday.CONFIG_HOLIDAY_ID, "", "");
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
