// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.DepartmentController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using Resources;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
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
      return (ActionResult) this.View();
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
      name = string.IsNullOrEmpty(name) ? "" : name.Trim();
      sortFiled = string.IsNullOrEmpty(sortFiled) ? "DEPARTMENT_PATH" : sortFiled;
      sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
      PagedList<LM_DEPARTMENT> all = this.unitOfWork.Departments.GetAll(type, name, pageIndex, pagination.PageSize, sortFiled, sortDir);
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
ViewBag.ActionUpdate = this.CheckPermistion("DEPARTMENT_SAVE");
ViewBag.ActionDelete = this.CheckPermistion("DEPARTMENT_DELETE");
      return this.PartialView("_GetList");
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "DEPARTMENT_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban", IsMenu = false)]
    public ActionResult OnInsert(int parentId = 0)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      Department department = new Department();
      department.Parent = new int?(parentId);
      this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", 0, "", "");
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("_Insert", (object) department);
      return (ActionResult) null;
    }

    [ActionDescription(ActionCode = "DEPARTMENT_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban")]
    [CustomAuthorize]
    public ActionResult OnUpdate(int id = 0)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemNotExist);
        LM_DEPARTMENT byId = this.unitOfWork.Departments.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        return (ActionResult) this.PartialView("_Insert", (object) new Department(byId));
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [HttpPost]
    [ActionDescription(ActionCode = "DEPARTMENT_DELETE", ActionName = "Xóa phòng ban", GroupCode = "DEPARTMENT", GroupName = "Quản lý phòng ban")]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        LM_DEPARTMENT byId = this.unitOfWork.Departments.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        if (byId.LM_DEPARTMENT_ID.Equals(1))
          throw new Exception("Bạn không thể xóa thông tin này!");
        if (this.unitOfWork.Departments.ExistChild(byId.LM_DEPARTMENT_ID) || this.unitOfWork.Users.ExistReferenceDepartment(id) || (this.unitOfWork.Templates.ExistReferenceDepartment(id) || this.unitOfWork.CalendarDuty.ExistReferenceDepartment(id)) || this.unitOfWork.Roles.ExistReferenceDepartment(id) || this.unitOfWork.Doctors.ExistReferenceDepartment(id))
          throw new Exception(Localization.MsgItemReference);
        byId.ISDELETE = true;
        this.unitOfWork.Departments.Update(byId);
        this.unitOfWork.Departments.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
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
        if (this.unitOfWork.Departments.ExistCode(categoryModel))
          throw new Exception(Localization.MsgCodeExist);
        if (categoryModel.LM_DEPARTMENT_ID.Equals(0))
        {
          string str1 = ",";
          int? nullable1 = categoryModel.DEPARTMENT_PARENT_ID;
          int num1;
          if (nullable1.HasValue)
          {
            nullable1 = categoryModel.DEPARTMENT_PARENT_ID;
            num1 = nullable1.Value <= 0 ? 1 : 0;
          }
          else
            num1 = 1;
          if (num1 == 0)
          {
            IDepartmentRepository departments = this.unitOfWork.Departments;
            nullable1 = categoryModel.DEPARTMENT_PARENT_ID;
            int id = nullable1.Value;
            LM_DEPARTMENT byId = departments.GetById(id);
            if (byId == null)
              return (ActionResult) this.Json(JsonResponse.Json500((object) "Thông tin không hợp lệ!"));
            str1 = byId.DEPARTMENT_PATH;
            LM_DEPARTMENT lmDepartment = categoryModel;
            nullable1 = byId.LEVELS;
            int num2;
            if (!nullable1.HasValue)
            {
              num2 = 0;
            }
            else
            {
              nullable1 = byId.LEVELS;
              num2 = nullable1.Value + 1;
            }
            int? nullable2 = new int?(num2);
            lmDepartment.LEVELS = nullable2;
            categoryModel.DEPARTMENT_PARENT_ID = new int?(byId.LM_DEPARTMENT_ID);
          }
          else
            entity.Level = new int?(0);
          categoryModel.ISDELETE = new bool?(false);
          this.unitOfWork.Departments.Add(categoryModel);
          this.unitOfWork.Departments.Save();
          string str2 = str1 + categoryModel.LM_DEPARTMENT_ID.ToString() + ",";
          categoryModel.DEPARTMENT_PATH = str2;
          this.unitOfWork.Departments.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgAddSuccess, "N/A", categoryModel.LM_DEPARTMENT_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        LM_DEPARTMENT byId1 = this.unitOfWork.Departments.GetById(categoryModel.LM_DEPARTMENT_ID);
        byId1.DEPARTMENT_NAME = categoryModel.DEPARTMENT_NAME;
        byId1.DEPARTMENT_CODE = categoryModel.DEPARTMENT_CODE;
        byId1.DEPARTMENT_PHONE = categoryModel.DEPARTMENT_PHONE;
        byId1.DEPARTMENT_FAX = categoryModel.DEPARTMENT_FAX;
        byId1.DEPARTMENT_ADDRESS = categoryModel.DEPARTMENT_ADDRESS;
        byId1.DESCRIPTION = categoryModel.DESCRIPTION;
        this.unitOfWork.Departments.Update(byId1);
        this.unitOfWork.Departments.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", categoryModel.LM_DEPARTMENT_ID, "", "");
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
