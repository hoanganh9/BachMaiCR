// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ConfigDirectController
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
      return this.View();
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
      PagedList<CONFIG_DIRECT> all = this.unitOfWork.ConfigDirect.GetAll(searchEntity, pageIndex, pagination.PageSize);
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
          lmDepartmentList = this.unitOfWork.Departments.GetChildDepartment(0).ToList();
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
        selectListItemList = source.Where((d =>
        {
          bool? isdelete = d.ISDELETE;
          return !isdelete.GetValueOrDefault() && isdelete.HasValue;
        })).OrderBy<DOCTOR, string>((d => d.DOCTOR_NAME)).Select<DOCTOR, SelectListItem>((d =>
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
        })).ToList();
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
      List<SelectListItem> list = this.unitOfWork.Feasts.GetAll().OrderBy<FEAST, string>((o => o.FEAST_TITLE)).Select<FEAST, SelectListItem>((t => new SelectListItem()
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
ViewBag.ListHoliday = this.GetFeast(0);
        this.OnList(0);
ViewBag.ActionUpdate = this.CheckPermistion("CONFIG_DIRECT_UPDATE");
        return this.PartialView("_Search", configHolidaySearch);
      }
      catch (Exception ex)
      {
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CONFIG_DIRECT_UPDATE", ActionName = "Cập nhật thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
    public ActionResult OnInsert(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
ViewBag.ListEducation = this.GetFeast(1);
        this.OnList(1);
        return this.PartialView("_Insert", new ConfigDirect());
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CONFIG_DIRECT_UPDATE", ActionName = "Cập nhật thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        CONFIG_DIRECT byId = this.unitOfWork.ConfigDirect.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        ConfigDirect configDirect = new ConfigDirect(byId);
ViewBag.ListEducation = this.GetFeast(1);
        this.OnList(1);
        return this.PartialView("_Insert", configDirect);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_DIRECT_DELETE", ActionName = "Xóa thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
    [HttpPost]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        CONFIG_DIRECT byId = this.unitOfWork.ConfigDirect.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.ISDELETE = true;
        this.unitOfWork.ConfigDirect.Update(byId);
        this.unitOfWork.ConfigDirect.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ValidateInput(false)]
    [ActionDescription(ActionCode = "CONFIG_DIRECT_UPDATE", ActionName = "Cập nhật thông tin cán bộ trực", GroupCode = "CONFIG_DIRECT", GroupName = "Danh sách cán bộ đi trực")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult SubmitChange(ConfigDirect entity)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        CONFIG_DIRECT configDirect = entity.GetConfigDirect();
        if (configDirect.CONFIG_DIRECT_ID.Equals(0))
        {
          configDirect.ISDELETE = new bool?(false);
          configDirect.DATE_CREATE = new DateTime?(DateTime.Now);
          configDirect.USER_CREATE_ID = new int?(this.UserX.ADMIN_USER_ID);
          this.unitOfWork.ConfigDirect.Add(configDirect);
          this.unitOfWork.ConfigDirect.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, "", Localization.MsgAddSuccess, "N/A", configDirect.CONFIG_DIRECT_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        CONFIG_DIRECT byId = this.unitOfWork.ConfigDirect.GetById(configDirect.CONFIG_DIRECT_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.DOCTORS_ID = configDirect.DOCTORS_ID;
        byId.LM_DEPARTMENT_ID = configDirect.LM_DEPARTMENT_ID;
        byId.DATE_BEGIN = configDirect.DATE_BEGIN;
        byId.DATE_END = configDirect.DATE_END;
        byId.FEAST_ID = configDirect.FEAST_ID;
        this.unitOfWork.ConfigDirect.Update(byId);
        this.unitOfWork.ConfigDirect.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, byId.DOCTOR.DOCTOR_NAME, Localization.MsgEditSuccess, "N/A", configDirect.CONFIG_DIRECT_ID, "", "");
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
