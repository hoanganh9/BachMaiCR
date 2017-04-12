// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.WebpageRolesController
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
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class WebpageRolesController : BaseController
  {
    public WebpageRolesController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [ActionDescription(ActionCode = "WebpageRoles_AddRole", ActionName = "Cập nhật nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    [ValidateAntiForgeryToken]
    [CustomAuthorize]
    [HttpPost]
    [ValidateInput(false)]
    public ActionResult AddRole(RoleModel role)
    {
      try
      {
        ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
        if (byUserName != null)
        {
          if (this.ModelState.IsValid && role.LM_DEPARTMENT_ID > 0)
          {
            if (role.WEBPAGES_ROLES_ID > 0)
            {
              WEBPAGES_ROLES byId = this.unitOfWork.Roles.GetById(role.WEBPAGES_ROLES_ID);
              if (role.ROLES_NAME != byId.ROLE_NAME && this.unitOfWork.Roles.IsExistRoleName(role.ROLES_NAME.Trim()) && this.Request.IsAjaxRequest())
                return (ActionResult) this.Json(JsonResponse.Json500((object) ("Quyền <strong>" + this.Server.HtmlEncode(role.ROLES_NAME) + "</strong> đã tồn tại")));
              byId.ROLE_NAME = role.ROLES_NAME.Trim();
              byId.DESCRIPTION = role.DESCRIPTION == null ? "" : role.DESCRIPTION.Trim();
              byId.ABBREVIATION = role.ABBREVIATION == null ? "" : role.ABBREVIATION.Trim();
              byId.ISACTIVE = new bool?(role.ISACTIVE);
              byId.WEBPAGES_ROLE_ID = role.WEBPAGES_ROLES_ID;
              byId.LM_DEPARTMENT_ID = new int?(role.LM_DEPARTMENT_ID);
              this.unitOfWork.Roles.Update(byId);
              this.WriteLog(enLogType.NomalLog, enActionType.Update, " Cập nhật nhóm quyền [" + byId.ROLE_NAME + "]", "N/A", "N/A", byId.WEBPAGES_ROLE_ID, "", "");
              if (this.Request.IsAjaxRequest())
                return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhập thành công"));
            }
            else
            {
              if (this.unitOfWork.Roles.IsExistRoleName(role.ROLES_NAME.Trim()) && this.Request.IsAjaxRequest())
                return (ActionResult) this.Json(JsonResponse.Json500((object) ("Quyền <strong>" + this.Server.HtmlEncode(role.ROLES_NAME) + "</strong> đã tồn tại")));
              WEBPAGES_ROLES entity = new WEBPAGES_ROLES()
              {
                ISACTIVE = new bool?(role.ISACTIVE),
                ROLE_NAME = role.ROLES_NAME.Trim(),
                ABBREVIATION = role.ABBREVIATION == null ? "" : role.ABBREVIATION.Trim(),
                DESCRIPTION = role.DESCRIPTION == null ? "" : role.DESCRIPTION.Trim(),
                CREATE_DATE = new DateTime?(DateTime.Now),
                CREATE_USERID = new int?(byUserName.ADMIN_USER_ID),
                LM_DEPARTMENT_ID = new int?(role.LM_DEPARTMENT_ID)
              };
              this.unitOfWork.Roles.Add(entity);
              this.WriteLog(enLogType.NomalLog, enActionType.Insert, " Thêm mới nhóm quyền [" + entity.ROLE_NAME + "]", "N/A", "N/A", entity.WEBPAGES_ROLE_ID, "", "");
            }
            if (this.Request.IsAjaxRequest())
              return (ActionResult) this.Json(JsonResponse.Json200((object) "Tạo thành công"));
            return (ActionResult) this.RedirectToAction("ManageRoles", "Admin");
          }
          if (this.Request.IsAjaxRequest())
            return (ActionResult) this.Json(JsonResponse.Json500((object) "Kiểm tra lại thông tin trên form"));
          return (ActionResult) null;
        }
        if (this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json500((object) "Phiên đăng nhập đã hết, hãy đăn nhập lại !"));
        return (ActionResult) null;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    [ActionDescription(ActionCode = "WebpageRoles_AddRole", ActionName = "Cập nhật nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    [CustomAuthorize]
    [HttpGet]
    public ActionResult AddRole(int? id)
    {
      RoleModel roleModel = (RoleModel) null;
      List<LM_DEPARTMENT> lmDepartmentList1 = new List<LM_DEPARTMENT>();
      if (id.HasValue)
      {
        WEBPAGES_ROLES byId = this.unitOfWork.Roles.GetById(id.Value);
        if (byId != null)
          roleModel = new RoleModel(byId);
      }
      else
        roleModel = new RoleModel() { ISACTIVE = true };
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      if (byUserName != null)
      {
        if (byUserName.USERNAME == "admin")
        {
          lmDepartmentList1 = this.unitOfWork.Departments.GetChildDepartment(0).ToList<LM_DEPARTMENT>();
        }
        else
        {
          int? lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
          int num;
          if (lmDepartmentId.HasValue)
          {
            lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            num = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          }
          else
            num = 1;
          if (num == 0)
          {
            List<LM_DEPARTMENT> lmDepartmentList2 = lmDepartmentList1;
            IDepartmentRepository departments = this.unitOfWork.Departments;
            lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            int id1 = lmDepartmentId.Value;
            LM_DEPARTMENT byId = departments.GetById(id1);
            lmDepartmentList2.Add(byId);
          }
        }
      }
ViewBag.RootDepartment = lmDepartmentList1;
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_AddRole.cshtml", (object) roleModel);
      this.RedirectToAction("ManageRoles", "Admin");
      return (ActionResult) null;
    }

    [ActionDescription(ActionCode = "WebpageRoles_AddUser", ActionName = "Thêm người dùng", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    [CustomAuthorize]
    [HttpGet]
    [ValidateInput(false)]
    public ActionResult AddUser(int id, string key, int deparmentId = 0)
    {
      WEBPAGES_ROLES webpagesRoles = (WEBPAGES_ROLES) null;
      if (id > 0)
        webpagesRoles = this.unitOfWork.Roles.GetById(id);
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<int> returnDepartments1 = new List<int>();
      List<int> returnDepartments2 = new List<int>();
      int? nullable;
      int num1;
      if (webpagesRoles != null)
      {
        nullable = webpagesRoles.LM_DEPARTMENT_ID;
        if (nullable.HasValue)
        {
          nullable = webpagesRoles.LM_DEPARTMENT_ID;
          num1 = (nullable.GetValueOrDefault() <= 0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          goto label_6;
        }
      }
      num1 = 1;
label_6:
      if (num1 == 0)
      {
        nullable = webpagesRoles.LM_DEPARTMENT_ID;
        this.GetDepartment(nullable.Value, returnDepartments2);
        List<int> intList1 = returnDepartments2;
        nullable = webpagesRoles.LM_DEPARTMENT_ID;
        int num2 = nullable.Value;
        intList1.Add(num2);
        if (deparmentId > 0)
        {
          if (returnDepartments2.Contains(deparmentId))
          {
            this.GetDepartment(deparmentId, returnDepartments1);
            returnDepartments1.Add(deparmentId);
          }
          else
          {
            returnDepartments1 = returnDepartments2;
            List<int> intList2 = returnDepartments1;
            nullable = webpagesRoles.LM_DEPARTMENT_ID;
            int num3 = nullable.Value;
            intList2.Add(num3);
          }
        }
        else
        {
          returnDepartments1 = returnDepartments2;
          List<int> intList2 = returnDepartments1;
          nullable = webpagesRoles.LM_DEPARTMENT_ID;
          int num3 = nullable.Value;
          intList2.Add(num3);
        }
      }
      IUserRepository users = this.unitOfWork.Users;
      List<int> departments = returnDepartments1;
      string key1 = key.Trim();
      int adminUserId = byUserName.ADMIN_USER_ID;
      nullable = webpagesRoles.CREATE_USERID;
      int createdUserId;
      if (nullable.HasValue)
      {
        nullable = webpagesRoles.CREATE_USERID;
        createdUserId = nullable.Value;
      }
      else
        createdUserId = 0;
      List<ADMIN_USER> list = users.GetAll(departments, key1, adminUserId, createdUserId).ToList<ADMIN_USER>();
ViewBag.ListUser = list;
ViewBag.Role = webpagesRoles;
      List<LM_DEPARTMENT> deptCurrent = this.GetDeptCurrent();
ViewBag.RootDepartment = deptCurrent;
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_AddUserToRole.cshtml");
      this.RedirectToAction("ManageRoles", "Admin");
      return (ActionResult) null;
    }

    [ValidateInput(false)]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "WebpageRoles_AddUser", ActionName = "Thêm người dùng", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    [HttpGet]
    public ActionResult SearchUser(int id, string key, int deparmentId = 0)
    {
      WEBPAGES_ROLES webpagesRoles = (WEBPAGES_ROLES) null;
      if (id > 0)
        webpagesRoles = this.unitOfWork.Roles.GetById(id);
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<int> returnDepartments1 = new List<int>();
      List<int> returnDepartments2 = new List<int>();
      int? nullable;
      int num1;
      if (webpagesRoles != null)
      {
        nullable = webpagesRoles.LM_DEPARTMENT_ID;
        if (nullable.HasValue)
        {
          nullable = webpagesRoles.LM_DEPARTMENT_ID;
          num1 = (nullable.GetValueOrDefault() <= 0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          goto label_6;
        }
      }
      num1 = 1;
label_6:
      if (num1 == 0)
      {
        nullable = webpagesRoles.LM_DEPARTMENT_ID;
        this.GetDepartment(nullable.Value, returnDepartments2);
        List<int> intList1 = returnDepartments2;
        nullable = webpagesRoles.LM_DEPARTMENT_ID;
        int num2 = nullable.Value;
        intList1.Add(num2);
        if (deparmentId > 0)
        {
          if (returnDepartments2.Contains(deparmentId))
          {
            this.GetDepartment(deparmentId, returnDepartments1);
            returnDepartments1.Add(deparmentId);
          }
          else
          {
            returnDepartments1 = returnDepartments2;
            List<int> intList2 = returnDepartments1;
            nullable = webpagesRoles.LM_DEPARTMENT_ID;
            int num3 = nullable.Value;
            intList2.Add(num3);
          }
        }
        else
        {
          returnDepartments1 = returnDepartments2;
          List<int> intList2 = returnDepartments1;
          nullable = webpagesRoles.LM_DEPARTMENT_ID;
          int num3 = nullable.Value;
          intList2.Add(num3);
        }
      }
      IUserRepository users = this.unitOfWork.Users;
      List<int> departments = returnDepartments1;
      string key1 = key.Trim();
      int adminUserId = byUserName.ADMIN_USER_ID;
      nullable = webpagesRoles.CREATE_USERID;
      int createdUserId;
      if (nullable.HasValue)
      {
        nullable = webpagesRoles.CREATE_USERID;
        createdUserId = nullable.Value;
      }
      else
        createdUserId = 0;
      List<ADMIN_USER> list = users.GetAll(departments, key1, adminUserId, createdUserId).ToList<ADMIN_USER>();
ViewBag.ListUser = list;
ViewBag.Role = webpagesRoles;
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_SearchUserToRole.cshtml");
      this.RedirectToAction("ManageRoles", "Admin");
      return (ActionResult) null;
    }

    [CustomAuthorize]
    [HttpPost]
    [ValidateJsonAntiForgeryToken]
    [ActionDescription(ActionCode = "WebpageRoles_Delete", ActionName = "Xóa nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền")]
    public ActionResult Delete(int id)
    {
      try
      {
        WEBPAGES_ROLES byId = this.unitOfWork.Roles.GetById(id);
        if (byId != null)
        {
          if (byId.ADMIN_USER.Any<ADMIN_USER>() || byId.WEBPAGES_ACTIONS.Any<WEBPAGES_ACTIONS>())
            return (ActionResult) this.Json(JsonResponse.Json500((object) "Có ràng buộc bản ghi, không thể xóa được"));
          this.unitOfWork.Roles.Delete(byId);
          this.WriteLog(enLogType.NomalLog, enActionType.Delete, " Xóa nhóm quyền [" + byId.ROLE_NAME + "]. Người xóa [" + this.User.Identity.Name + "]", "N/A", "N/A", byId.WEBPAGES_ROLE_ID, "ROLES_GROUP_CODE", "Quản lý nhóm quyền");
        }
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Xóa thành công"));
      }
      catch (Exception ex)
      {
        return (ActionResult) this.Json(JsonResponse.Json500((object) "Xóa không thành công"));
      }
    }

    [HttpPost]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "WebpageRoles_UpdateActive", ActionName = "active/deactive nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    public ActionResult UpdateActive(int roleId, bool? active)
    {
      WEBPAGES_ROLES byId = this.unitOfWork.Roles.GetById(roleId);
      if (byId == null)
        return (ActionResult) this.Json(JsonResponse.Json404((object) "Not found"));
      WEBPAGES_ROLES webpagesRoles = byId;
      bool? nullable1 = active;
      bool? nullable2 = new bool?(!nullable1.HasValue || nullable1.GetValueOrDefault());
      webpagesRoles.ISACTIVE = nullable2;
      this.unitOfWork.Roles.Update(byId);
      this.WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive nhóm quyền [" + byId.ROLE_NAME + "]", "N/A", "N/A", byId.WEBPAGES_ROLE_ID, "", "");
      return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhập thành công"));
    }

    [ActionDescription(ActionCode = "WebpageRoles_UpdateRoleWithActions", ActionName = "Cập nhật quyền cho nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    [CustomAuthorize]
    [HttpPost]
    public ActionResult UpdateRoleWithActions(int roleId, string[] actions)
    {
      WEBPAGES_ROLES byId1 = this.unitOfWork.Roles.GetById(roleId);
      if (byId1 == null)
        return (ActionResult) this.Json(JsonResponse.Json404((object) "Not found"));
      ITransaction transaction = (ITransaction) null;
      try
      {
        transaction = this.unitOfWork.BeginTransaction();
        List<WEBPAGES_ACTIONS> list = byId1.WEBPAGES_ACTIONS.ToList<WEBPAGES_ACTIONS>();
        if (list.Any<WEBPAGES_ACTIONS>())
        {
          foreach (WEBPAGES_ACTIONS webpagesActions in list)
            byId1.WEBPAGES_ACTIONS.Remove(webpagesActions);
        }
        if (actions != null && ((IEnumerable<string>) actions).Any<string>())
        {
          foreach (string action in actions)
          {
            WEBPAGES_ACTIONS byId2 = this.unitOfWork.Actions.GetById(int.Parse(action));
            if (byId2 != null)
              byId1.WEBPAGES_ACTIONS.Add(byId2);
          }
        }
        this.WriteLog(enLogType.NomalLog, enActionType.Permission, "Cập nhật quyền cho nhóm quyền [" + byId1.ROLE_NAME + "]", "N/A", "N/A", byId1.WEBPAGES_ROLE_ID, "", "");
        transaction.Commit();
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhập thành công"));
      }
      catch (Exception ex)
      {
        if (transaction != null)
          transaction.Rollback();
        throw;
      }
      finally
      {
        if (transaction != null)
          transaction.Dispose();
      }
    }

    [ActionDescription(ActionCode = "WebpageRoles_UpdateRoleWithActions", ActionName = "Cập nhật quyền cho nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
    [CustomAuthorize]
    [HttpGet]
    public ActionResult GetActionIds(int? roleId)
    {
      int num;
      if (roleId.HasValue)
      {
        int? nullable = roleId;
        num = (nullable.GetValueOrDefault() != 0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
      }
      else
        num = 0;
      if (num == 0)
        return (ActionResult) this.Json(JsonResponse.Json404((object) "Role not found"), JsonRequestBehavior.AllowGet);
      WEBPAGES_ROLES byId = this.unitOfWork.Roles.GetById(roleId.Value);
      if (byId == null)
        return (ActionResult) this.Json(JsonResponse.Json404((object) "Role not found"), JsonRequestBehavior.AllowGet);
      List<int> intList = new List<int>();
      if (byId.WEBPAGES_ACTIONS != null)
        intList = byId.WEBPAGES_ACTIONS.Select<WEBPAGES_ACTIONS, int>((Func<WEBPAGES_ACTIONS, int>) (o => o.WEBPAGES_ACTION_ID)).ToList<int>();
      return (ActionResult) this.Json(JsonResponse.Json200((object) intList), JsonRequestBehavior.AllowGet);
    }

    public void GetDepartment(int parentDepartment, List<int> returnDepartments)
    {
      foreach (LM_DEPARTMENT lmDepartment in this.unitOfWork.Departments.GetChildDepartment(parentDepartment))
      {
        returnDepartments.Add(lmDepartment.LM_DEPARTMENT_ID);
        this.GetDepartment(lmDepartment.LM_DEPARTMENT_ID, returnDepartments);
      }
    }

    [ActionDescription(ActionCode = "WebpageRoles_AddUser", ActionName = "Thêm người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [CustomAuthorize]
    [ValidateJsonAntiForgeryToken]
    [HttpPost]
    public ActionResult SaveConfigUser(int roleId, List<int> userIds, bool isAdd)
    {
      if (userIds == null || !userIds.Any<int>())
        return (ActionResult) this.Json(JsonResponse.Json404((object) "Không tìm thấy người dùng cần phân quyền"));
      ITransaction transaction = (ITransaction) null;
      try
      {
        foreach (int userId in userIds)
        {
          ADMIN_USER byId1 = this.unitOfWork.Users.GetById(userId);
          ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
          if (byUserName == null)
            return (ActionResult) this.Json(JsonResponse.Json404((object) "Không tìm thấy người phân quyền"));
          if (byId1 == null)
            return (ActionResult) this.Json(JsonResponse.Json404((object) "Không tìm thấy người dùng cần phân quyền"));
          if (byUserName.USERNAME != "admin" && byUserName.WEBPAGES_ROLES == null)
            return (ActionResult) this.Json(JsonResponse.Json400((object) "Không được phép phân những chức năng đã chọn"));
          if (byUserName.USERNAME != "admin")
          {
            int? nullable1 = byUserName.LM_DEPARTMENT_ID;
            int? lmDepartmentId = byId1.LM_DEPARTMENT_ID;
            if ((nullable1.GetValueOrDefault() != lmDepartmentId.GetValueOrDefault() ? 1 : (nullable1.HasValue != lmDepartmentId.HasValue ? 1 : 0)) != 0)
            {
              int num = 0;
              bool flag = false;
              int? nullable2;
              if (byId1.LM_DEPARTMENT != null)
              {
                nullable2 = byId1.LM_DEPARTMENT.DEPARTMENT_PARENT_ID;
              }
              else
              {
                nullable1 = new int?();
                nullable2 = nullable1;
              }
              int? nullable3 = nullable2;
              do
              {
                nullable1 = nullable3;
                lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
                if ((nullable1.GetValueOrDefault() != lmDepartmentId.GetValueOrDefault() ? 0 : (nullable1.HasValue == lmDepartmentId.HasValue ? 1 : 0)) != 0)
                {
                  flag = true;
                  break;
                }
                IDepartmentRepository departments = this.unitOfWork.Departments;
                nullable1 = nullable3;
                int id = nullable1 ?? 0;
                LM_DEPARTMENT byId2 = departments.GetById(id);
                int? nullable4;
                if (byId2 != null)
                {
                  nullable4 = byId2.DEPARTMENT_PARENT_ID;
                }
                else
                {
                  nullable1 = new int?();
                  nullable4 = nullable1;
                }
                nullable3 = nullable4;
                ++num;
              }
              while (nullable3.HasValue && num < 100);
              if (!flag)
                return (ActionResult) this.Json(JsonResponse.Json400((object) ("Người dùng này thuộc phòng ban, bạn không được phép phân quyền " + byId1.USERNAME)));
            }
          }
        }
        transaction = this.unitOfWork.BeginTransaction();
        WEBPAGES_ROLES byId3 = this.unitOfWork.Roles.GetById(roleId);
        if (byId3 != null)
        {
          foreach (int userId in userIds)
          {
            ADMIN_USER byId1 = this.unitOfWork.Users.GetById(userId);
            int webpagesRoleId;
            if (isAdd)
            {
              int num1 = 0;
              int num2 = 10;
              string str1 = "Thêm người dùng :";
              string username = byId1.USERNAME;
              string str2 = " vào roleId :";
              webpagesRoleId = byId3.WEBPAGES_ROLE_ID;
              string str3 = webpagesRoleId.ToString();
              string content = str1 + username + str2 + str3;
              string description = "N/A";
              string errorCode = "N/A";
              int objId = 0;
              string menuCode = "";
              string menuName = "";
              this.WriteLog((enLogType) num1, (enActionType) num2, content, description, errorCode, objId, menuCode, menuName);
              byId1.WEBPAGES_ROLES.Add(byId3);
            }
            else
            {
              int num1 = 0;
              int num2 = 10;
              string str1 = "Gỡ người dùng :";
              string username = byId1.USERNAME;
              string str2 = " khỏi roleId :";
              webpagesRoleId = byId3.WEBPAGES_ROLE_ID;
              string str3 = webpagesRoleId.ToString();
              string content = str1 + username + str2 + str3;
              string description = "N/A";
              string errorCode = "N/A";
              int objId = 0;
              string menuCode = "";
              string menuName = "";
              this.WriteLog((enLogType) num1, (enActionType) num2, content, description, errorCode, objId, menuCode, menuName);
              byId1.WEBPAGES_ROLES.Remove(byId3);
            }
          }
        }
        transaction.Commit();
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Phân quyền cho người dùng thành công"));
      }
      catch (Exception ex)
      {
        if (transaction != null)
          transaction.Rollback();
        throw;
      }
      finally
      {
        if (transaction != null)
          transaction.Dispose();
      }
    }
  }
}
