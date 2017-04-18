// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.AdminController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
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
using BachMaiCR.Web.Serivces;

namespace BachMaiCR.Web.Controllers
{
  public class AdminController : BaseController
  {
    public AdminController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [HttpGet]
    public ActionResult Index()
    {
      return this.View();
    }

    [ActionDescription(ActionCode = "Admin_ManageActions", ActionName = "Xem danh sách chức năng", GroupCode = "ACTIONS_GROUP_CODE", GroupName = "Quản lý chức năng", IsMenu = true)]
    [CustomAuthorize]
    [HttpGet]
    public ActionResult ManageActions(int pageIndex = 0, string sortFiled = null, string sortDir = null)
    {
      Pagination pagination = new Pagination()
      {
        ActionName = "manageroles",
        ModelName = "admin",
        CurrentPage = pageIndex,
        InputSearchId = "",
        TagetReLoadId = "action-list"
      };
      string sort = "CODE";
      string sortDir1 = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
      PagedList<WEBPAGES_ACTIONS> all = this.unitOfWork.Actions.GetAll(pageIndex, pagination.PageSize, sort, sortDir1);
      pageIndex = pageIndex <= 0 ? 0 : pageIndex;
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
      ViewBag.ActionPermission = list;
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
        ViewBag.Actions = all;

        ViewBag.Pagination = pagination;
            ViewBag.User = this.User.Identity.Name;

      if (this.Request.IsAjaxRequest())
        return this.PartialView("_ActionList", all);
      return this.View();
    }

    [HttpGet]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "Admin_GenerateWebPageActions", ActionName = "Cập nhật năng vào CSDL", GroupCode = "ACTIONS_GROUP_CODE", GroupName = "Quản lý chức năng")]
    public ActionResult GenerateWebPageActions()
    {
      ITransaction transaction = null;
      try
      {
        transaction = this.unitOfWork.BeginTransaction();
        new WebpageActionService(this.unitOfWork).RefreshAllWebpageAction();
        this.WriteLog(enLogType.NomalLog, enActionType.Other, "Cập nhật năng của website vào CSDL", "N/A", "N/A", 0, "", "");
        transaction.Commit();
      }
      catch (Exception ex)
      {
        if (transaction != null)
          transaction.Rollback();
      }
      finally
      {
        if (transaction != null)
          transaction.Dispose();
      }
      return this.RedirectToAction("ManageActions");
    }

    [ActionDescription(ActionCode = "Admin_ManageRoles", ActionName = "Xem danh sách nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = true)]
    [HttpGet]
    [CustomAuthorize]
    [ValidateInput(false)]
    public ActionResult ManageRoles(string key = null, int departmentId = 0, int pageIndex = 0, string sortFiled = null, string sortDir = null)
    {
      Pagination pagination = new Pagination()
      {
        ActionName = "manageroles",
        ModelName = "admin",
        CurrentPage = pageIndex,
        InputSearchId = "",
        TagetReLoadId = "role-action-list"
      };
      string sort = "ROLE_NAME";
      string sortDir1 = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<int> intList1 = new List<int>();
      if (byUserName != null)
      {
        LM_DEPARTMENT lmDepartment = new LM_DEPARTMENT();
        if (byUserName.USERNAME == "admin")
        {
          if (departmentId > 0)
          {
            this.GetDepartment(this.unitOfWork.Departments.GetById(departmentId).LM_DEPARTMENT_ID, intList1);
            intList1.Add(departmentId);
          }
          else
          {
            intList1.Add(0);
            this.GetDepartment(0, intList1);
          }
        }
        else
        {
          int? lmDepartmentId;
          int num1;
          if (byUserName != null && byUserName.LM_DEPARTMENT_ID.HasValue)
          {
            lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            num1 = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          }
          else
            num1 = 1;
          if (num1 == 0)
          {
            IDepartmentRepository departments = this.unitOfWork.Departments;
            lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            int id = lmDepartmentId.Value;
            List<LM_DEPARTMENT> childDepartment = this.unitOfWork.Departments.GetChildDepartment(departments.GetById(id).DEPARTMENT_PATH);
            if (departmentId > 0)
            {
              if (childDepartment.Select<LM_DEPARTMENT, int>((o => o.LM_DEPARTMENT_ID)).Contains<int>(departmentId))
              {
                this.GetDepartment(this.unitOfWork.Departments.GetById(departmentId).LM_DEPARTMENT_ID, intList1);
                intList1.Add(departmentId);
              }
              else
              {
                lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
                this.GetDepartment(lmDepartmentId.Value, intList1);
                intList1.Add(departmentId);
              }
            }
            else
            {
              lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
              this.GetDepartment(lmDepartmentId.Value, intList1);
              List<int> intList2 = intList1;
              lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
              int num2 = lmDepartmentId.Value;
              intList2.Add(num2);
            }
          }
        }
      }
      PagedList<WEBPAGES_ROLES> pagedList = string.IsNullOrEmpty(key) ? this.unitOfWork.Roles.GetAll(pageIndex, pagination.PageSize, sort, sortDir1, "", intList1) : this.unitOfWork.Roles.GetAll(pageIndex, pagination.PageSize, sort, sortDir1, key.Trim(), intList1);
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
ViewBag.Actions = list;
      pagination.TotalRows = pagedList.TotalItemCount;
      pagination.CurrentRow = pagedList.Count;
ViewBag.Roles = pagedList;
ViewBag.User = byUserName;
ViewBag.Pagination = pagination;
ViewBag.RootDepartment = this.GetDeptCurrent();
      if (this.Request.IsAjaxRequest())
        return this.PartialView("_RoleList");
      return this.View();
    }

    [ValidateInput(false)]
    [ActionDescription(ActionCode = "Admin_ManageUsers", ActionName = "Xem danh sách người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = true)]
    [CustomAuthorize]
    [HttpGet]
    public ActionResult ManageUsers(string key = null, int departmentId = 0, int pageIndex = 0, string sortFiled = null, string sortDir = null)
    {
      Pagination pagination = new Pagination()
      {
        ActionName = "manageusers",
        ModelName = "admin",
        CurrentPage = pageIndex,
        InputSearchId = "",
        TagetReLoadId = "user-list"
      };
      string sort = "USERNAME";
      string sortDir1 = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
      bool isdelete = false;
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<int> intList1 = new List<int>();
      if (byUserName.USERNAME == "admin")
      {
        if (departmentId > 0)
        {
          this.GetDepartment(this.unitOfWork.Departments.GetById(departmentId).LM_DEPARTMENT_ID, intList1);
          intList1.Add(departmentId);
        }
        else
        {
          this.GetDepartment(0, intList1);
          intList1.Add(departmentId);
        }
      }
      else
      {
        int? lmDepartmentId;
        int num1;
        if (byUserName != null && byUserName.LM_DEPARTMENT_ID.HasValue)
        {
          lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
          num1 = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 == 0)
        {
          IDepartmentRepository departments = this.unitOfWork.Departments;
          lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
          int id = lmDepartmentId.Value;
          List<LM_DEPARTMENT> childDepartment = this.unitOfWork.Departments.GetChildDepartment(departments.GetById(id).DEPARTMENT_PATH);
          if (departmentId > 0)
          {
            if (childDepartment.Select<LM_DEPARTMENT, int>((o => o.LM_DEPARTMENT_ID)).Contains<int>(departmentId))
            {
              this.GetDepartment(this.unitOfWork.Departments.GetById(departmentId).LM_DEPARTMENT_ID, intList1);
              intList1.Add(departmentId);
            }
            else
            {
              lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
              this.GetDepartment(lmDepartmentId.Value, intList1);
              List<int> intList2 = intList1;
              lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
              int num2 = lmDepartmentId.Value;
              intList2.Add(num2);
            }
          }
          else
          {
            lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            this.GetDepartment(lmDepartmentId.Value, intList1);
            List<int> intList2 = intList1;
            lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            int num2 = lmDepartmentId.Value;
            intList2.Add(num2);
          }
        }
      }
      PagedList<ADMIN_USER> pagedList = !string.IsNullOrWhiteSpace(key) ? this.unitOfWork.Users.SearchAllByUserName(key, pageIndex, pagination.PageSize, sort, sortDir1, intList1) : this.unitOfWork.Users.GetAll(pageIndex, pagination.PageSize, sort, sortDir1, isdelete, intList1);
      pageIndex = pageIndex <= 0 ? 0 : pageIndex;
      pagination.TotalRows = pagedList.TotalItemCount;
      pagination.CurrentRow = pagedList.Count;
            ViewBag.Users = pagedList;
     
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
      ViewBag.Actions = list;
            ViewBag.Pagination = pagination;

            ViewBag.RootDepartment = this.GetDeptCurrent();
            
      if (this.Request.IsAjaxRequest())
        return this.PartialView("_UserList");
      return this.View();
    }

    public void GetDepartment(int parentDepartment, List<int> returnDepartments)
    {
      foreach (LM_DEPARTMENT lmDepartment in this.unitOfWork.Departments.GetChildDepartment(parentDepartment))
      {
        returnDepartments.Add(lmDepartment.LM_DEPARTMENT_ID);
        this.GetDepartment(lmDepartment.LM_DEPARTMENT_ID, returnDepartments);
      }
    }
  }
}
