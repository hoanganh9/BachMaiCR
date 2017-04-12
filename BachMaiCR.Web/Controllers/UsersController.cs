// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.UsersController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class UsersController : BaseController
  {
    public UsersController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [CustomAuthorize]
    [HttpGet]
    [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    public ActionResult AddUser(int? userId)
    {
      UserModel userModel = (UserModel) null;
      ADMIN_USER u = (ADMIN_USER) null;
      int num1;
      if (userId.HasValue)
      {
        int? nullable = userId;
        num1 = (nullable.GetValueOrDefault() <= 0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
      }
      else
        num1 = 1;
      if (num1 == 0)
      {
        u = this.unitOfWork.Users.GetById(userId.Value);
        if (u != null)
          userModel = new UserModel(u);
      }
      if (userModel == null)
        userModel = new UserModel()
        {
          USERNAME = "",
          ISACTIVED = true
        };
      ADMIN_USER currentUser = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
      List<SelectListItem> selectListItemList1 = new List<SelectListItem>();
      if (currentUser != null)
      {
        List<int> intList1 = new List<int>();
        List<DOCTOR> source = new List<DOCTOR>();
        int? lmDepartmentId;
        int num2;
        if (u != null)
        {
          lmDepartmentId = u.LM_DEPARTMENT_ID;
          num2 = !lmDepartmentId.HasValue ? 1 : 0;
        }
        else
          num2 = 1;
        if (num2 == 0)
        {
          IDepartmentRepository departments = this.unitOfWork.Departments;
          lmDepartmentId = u.LM_DEPARTMENT_ID;
          int id = lmDepartmentId.Value;
          bool? isdelete = departments.GetById(id).ISDELETE;
          if ((isdelete.GetValueOrDefault() ? 0 : (isdelete.HasValue ? 1 : 0)) != 0)
          {
            IDoctorRepository doctors = this.unitOfWork.Doctors;
            lmDepartmentId = u.LM_DEPARTMENT_ID;
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
          int num3;
          if (lmDepartmentId.HasValue)
          {
            lmDepartmentId = currentUser.LM_DEPARTMENT_ID;
            num3 = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          }
          else
            num3 = 1;
          if (num3 == 0)
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
              int num4 = lmDepartmentId.Value;
              intList2.Add(num4);
              lmDepartmentList.Add(byId);
            }
          }
        }
        selectListItemList1 = source.Where<DOCTOR>((Func<DOCTOR, bool>) (d =>
        {
          bool? isdelete = d.ISDELETE;
          return !isdelete.GetValueOrDefault() && isdelete.HasValue;
        })).OrderBy<DOCTOR, string>((Func<DOCTOR, string>) (d => d.DOCTOR_NAME)).Select<DOCTOR, SelectListItem>((Func<DOCTOR, SelectListItem>) (d =>
        {
          SelectListItem selectListItem1 = new SelectListItem();
          selectListItem1.Text = d.DOCTOR_NAME;
          selectListItem1.Value = d.DOCTORS_ID.ToString();
          SelectListItem selectListItem2 = selectListItem1;
          if (currentUser.DOCTORS_ID.HasValue)
          {
            int doctorsId1 = d.DOCTORS_ID;
            int? doctorsId2 = currentUser.DOCTORS_ID;
            if ((doctorsId1 != doctorsId2.GetValueOrDefault() ? 0 : (doctorsId2.HasValue ? 1 : 0)) == 0)
            {
              num2 = 0;
              goto label_4;
            }
          }
          num2 = 1;
label_4:
          selectListItem2.Selected = num2 != 0;
          return selectListItem1;
        })).ToList<SelectListItem>();
      }
      SelectListItem selectListItem = new SelectListItem()
      {
        Text = Localization.LabelSelect,
        Value = "0"
      };
      selectListItemList1.Insert(0, selectListItem);
ViewBag.RootDepartment = lmDepartmentList;
ViewBag.ListDoctor = selectListItemList1;
      List<MENULIST> allActive = this.unitOfWork.AdminMenu.GetAll_Active();
      List<SelectListItem> selectListItemList2 = new List<SelectListItem>();
      List<SelectListItem> list = allActive.Select<MENULIST, SelectListItem>((Func<MENULIST, SelectListItem>) (d => new SelectListItem()
      {
        Text = d.MENU_NAME,
        Value = d.MENU_URL
      })).ToList<SelectListItem>();
ViewBag.ListUrlMenu = list;
      selectListItemList2 = (List<SelectListItem>) null;
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_AddUser.cshtml", (object) userModel);
      this.RedirectToAction("ManageUsers", "Admin");
      return (ActionResult) null;
    }

    [CustomAuthorize]
    [ValidateJsonAntiForgeryToken]
    [ValidateInput(false)]
    [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [HttpPost]
    public ActionResult AddUser(UserModel model)
    {
      bool result;
      bool.TryParse(this.Request.QueryString["back"] ?? "False", out result);
      if (model.ADMIN_USER_ID > 0)
      {
        if (this.ModelState.IsValidField("Email") && this.ModelState.IsValidField("FullName"))
        {
          ADMIN_USER byId = this.unitOfWork.Users.GetById(model.ADMIN_USER_ID);
          if (byId != null)
          {
            if (!this.CheckPermission(byId))
              return (ActionResult) this.Json(JsonResponse.Json400((object) "Bạn không có quyền với người dùng này !"));
            byId.FULLNAME = model.FULLNAME == null ? (string) null : model.FULLNAME.Trim();
            byId.PHONE = model.PHONE == null ? (string) null : model.PHONE.Trim();
            byId.MAIL = model.MAIL == null ? (string) null : model.MAIL.Trim();
            byId.DOCTORS_ID = new int?(model.DOCTORS_ID);
            byId.USERCODE = model.USERCODE;
            byId.LM_DEPARTMENT_ID = new int?(model.LM_DEPARTMENT_ID);
            byId.ACTIVE_URL = model.ACTIVE_URL;
            this.unitOfWork.Users.Update(byId);
            this.WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật người dùng [" + byId.USERNAME + "]", "N/A", "N/A", 0, "", "");
            if (this.Request.IsAjaxRequest())
              return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật người dùng thành công !"));
            Notice.Show("Cập nhật người dùng thành công !", NoticeType.Success);
          }
        }
        else
        {
          if (this.Request.IsAjaxRequest())
            return (ActionResult) this.Json(JsonResponse.Json500((object) "Không tìm thấy thông tin người dùng !"));
          Notice.Show("Không tìm thấy thông tin người dùng", NoticeType.Error);
          return (ActionResult) this.View("AddUser", (object) model);
        }
      }
      else if (this.ModelState.IsValid)
      {
        if (this.unitOfWork.Users.CheckUserExit(model.USERNAME.Trim()) && this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json500((object) ("Tên người dùng <strong>" + model.USERNAME + "</strong> đã tồn tại, hãy nhập tên khác !")));
        ITransaction transaction = (ITransaction) null;
        string str1 = Encrypt.RandomString(3);
        string str2 = Encrypt.Sha1HashWithHex(model.PASSWORD + str1);
        ADMIN_USER entity = new ADMIN_USER()
        {
          USERNAME = Regex.Replace(model.USERNAME.Trim(), "<(.|\\n)*?>", string.Empty),
          CREATE_DATE = new DateTime?(DateTime.Now),
          FULLNAME = model.FULLNAME == null ? (string) null : model.FULLNAME.Trim(),
          MAIL = model.MAIL == null ? (string) null : model.MAIL.Trim(),
          PHONE = model.PHONE == null ? (string) null : model.PHONE.Trim(),
          ACTIVE_URL = model.ACTIVE_URL,
          ISDELETE = new bool?(false),
          PASSWORD = str2,
          SALT = str1,
          USERCODE = model.USERCODE,
          DOCTORS_ID = new int?(model.DOCTORS_ID),
          LM_DEPARTMENT_ID = new int?(model.LM_DEPARTMENT_ID),
          ACTIVE_ENDDATE = new DateTime?(DateTime.Now.AddDays(1.0)),
          REQIURE_CHANGE_PASS = true
        };
        try
        {
          transaction = this.unitOfWork.BeginTransaction();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, "Thêm mới người dùng [" + entity.USERNAME + "]", "N/A", "N/A", 0, "", "");
          this.unitOfWork.Users.Add(entity);
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
        if (this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json200((object) "Thêm mới người dùng thành công !"));
        Notice.Show("Thêm mới người dùng thành công !", NoticeType.Success);
      }
      else
      {
        if (this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json500((object) "Lỗi dữ liệu!"));
        Notice.Show("Invalid form", NoticeType.Error);
        return (ActionResult) this.View("AddUser", (object) model);
      }
      if (!result)
        return (ActionResult) this.RedirectToAction("AddUser");
      string str = this.Request.Cookies["manageusers"] == null ? "" : this.Request.Cookies["manageusers"].Value;
      if (string.IsNullOrWhiteSpace(str))
        return (ActionResult) this.RedirectToAction("ManageUsers", "Admin");
      return (ActionResult) this.Redirect(HttpUtility.UrlDecode(str));
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "Users_ResetPass", ActionName = "Reset mật khẩu người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [HttpGet]
    public ActionResult ResetPass(int userId)
    {
      UserResetPasswordModel resetPasswordModel = (UserResetPasswordModel) null;
      ADMIN_USER byId = this.unitOfWork.Users.GetById(userId);
      if (!this.CheckPermission(byId))
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Bạn không có quyền với người dùng này !"));
      if (byId != null)
        resetPasswordModel = new UserResetPasswordModel(byId);
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_ResetPassword.cshtml", (object) resetPasswordModel);
      this.RedirectToAction("ManageUsers", "Admin");
      return (ActionResult) null;
    }

    [ActionDescription(ActionCode = "Users_ResetPass", ActionName = "Reset mật khẩu người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [HttpPost]
    [ValidateJsonAntiForgeryToken]
    [CustomAuthorize]
    [ValidateInput(false)]
    public ActionResult ResetPass(UserResetPasswordModel model)
    {
      int adminUserId = model.ADMIN_USER_ID;
      if (model.ADMIN_USER_ID <= 0)
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Người dùng không tồn tại "));
      ADMIN_USER byId1 = this.unitOfWork.Users.GetById(model.ADMIN_USER_ID);
      if (!this.CheckPermission(byId1))
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Bạn không có quyền với người dùng này !"));
      if (model.ADMIN_USER_ID > 0)
      {
        if (this.ModelState.IsValid && model.NewPassword == model.ConfirmPassword)
        {
          ADMIN_USER byId2 = this.unitOfWork.Users.GetById(model.ADMIN_USER_ID);
          if (byId2 != null)
          {
            ITransaction transaction = (ITransaction) null;
            string str1 = Encrypt.RandomString(3);
            string str2 = Encrypt.Sha1HashWithHex(model.NewPassword + str1);
            byId2.PASSWORD = str2;
            byId2.SALT = str1;
            try
            {
              transaction = this.unitOfWork.BeginTransaction();
              this.unitOfWork.Users.Update(byId2);
              this.WriteLog(enLogType.NomalLog, enActionType.Update, "Reset mật khẩu người dùng :" + byId1.USERNAME, "N/A", "N/A", byId2.ADMIN_USER_ID, "", "");
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
          }
        }
        else if (this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json500((object) "Mật khẩu chưa đủ mạnh hoặc chưa điền đủ thông tin !"));
        if (this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json200((object) "Cấp lại mật khẩu thành công !"));
        return (ActionResult) null;
      }
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Cấp lại mật khẩu thành công !"));
      return (ActionResult) null;
    }

    [HttpPost]
    [CustomAuthorize]
    [ValidateJsonAntiForgeryToken]
    [ActionDescription(ActionCode = "Users_Delete", ActionName = "Xóa người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    public ActionResult Delete(int userId)
    {
      if (userId <= 0)
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Người dùng không tồn tại "));
      ADMIN_USER byId = this.unitOfWork.Users.GetById(userId);
      if (!this.CheckPermission(byId))
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Bạn không có quyền với người dùng này !"));
      if (byId != null)
      {
        if (byId.CALENDAR_DUTY.Where<CALENDAR_DUTY>((Func<CALENDAR_DUTY, bool>) (o =>
        {
          bool? isdelete = o.ISDELETE;
          return isdelete.GetValueOrDefault() && isdelete.HasValue;
        })).Any<CALENDAR_DUTY>())
          return (ActionResult) this.Json(JsonResponse.Json400((object) "Người dùng đang tồn tại liên kết với chức năng lịch "));
        if (byId.TEMPLATES.Where<TEMPLATE>((Func<TEMPLATE, bool>) (o =>
        {
          bool? isdelete = o.ISDELETE;
          return isdelete.GetValueOrDefault() && isdelete.HasValue;
        })).Any<TEMPLATE>())
          return (ActionResult) this.Json(JsonResponse.Json400((object) "Người dùng đang tồn tại liên kết với chức năng biểu mẫu"));
      }
      if (byId == null)
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Xóa người dùng thành công !"));
      if (byId.USERNAME == "admin" || byId.USERNAME == this.User.Identity.Name)
        return (ActionResult) this.Json(JsonResponse.Json400((object) ("Không thể thay đổi thông tin của tài khoản " + byId.USERNAME)));
      if (byId.USERS_ACTIONS != null && byId.USERS_ACTIONS.Any<USERS_ACTIONS>() || byId.WEBPAGES_ROLES != null && byId.WEBPAGES_ROLES.Any<WEBPAGES_ROLES>())
        return (ActionResult) this.Json(JsonResponse.Json500((object) "Người dùng đã được phân quyền. Hãy bỏ hết quyền trước khi xóa !"));
      byId.ISDELETE = true;
      this.unitOfWork.Users.Update(byId);
      this.WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa người dùng [" + byId.USERNAME + "]", "N/A", "N/A", userId, "", "");
      return (ActionResult) this.Json(JsonResponse.Json200((object) "Xóa user thành công !"));
    }

    [ValidateJsonAntiForgeryToken]
    [HttpPost]
    [ActionDescription(ActionCode = "Users_ActiveChage", ActionName = "Active/deactive người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [CustomAuthorize]
    public ActionResult ActiveChage(int userId, bool active)
    {
      if (userId <= 0)
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Người dùng không tồn tại "));
      ADMIN_USER byId = this.unitOfWork.Users.GetById(userId);
      if (!this.CheckPermission(byId))
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Bạn không có quyền với người dùng này !"));
      if (byId == null)
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật thành công !"));
      byId.ISACTIVED = new bool?(active);
      this.unitOfWork.Users.Update(byId);
      this.WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive người dùng [" + byId.USERNAME + "]", "N/A", "N/A", userId, "", "");
      return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật thành công !"));
    }

    [ValidateJsonAntiForgeryToken]
    [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [CustomAuthorize]
    public JsonResult CheckExistUserName(string userName, int? id)
    {
      int num;
      if (id.HasValue)
      {
        int? nullable = id;
        num = (nullable.GetValueOrDefault() != 0 ? 1 : (!nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0 || this.unitOfWork.Users.GetById(id.Value) == null)
        ;
      if (this.unitOfWork.Users.GetByUserName(Regex.Replace(userName, "<(.|\\n)*?>", string.Empty)) != null)
        return this.Json((object) Localization.UserModelUserNameExist);
      return this.Json((object) true, JsonRequestBehavior.AllowGet);
    }

    [ActionDescription(ActionCode = "Users_SaveConfigUser", ActionName = "Phân quyền truy cập cho người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [CustomAuthorize]
    [HttpGet]
    public ActionResult ConfigUser(int? userId)
    {
      int num1;
      if (userId.HasValue)
      {
        int? nullable = userId;
        num1 = (nullable.GetValueOrDefault() != 0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 == 0)
        return (ActionResult) this.RedirectToAction("manageusers", "admin");
      ADMIN_USER byId = this.unitOfWork.Users.GetById(userId.Value);
      if (byId == null)
        return (ActionResult) this.RedirectToAction("manageusers", "admin");
      if (byId.USERNAME == "admin")
      {
        Notice.Show("Không thể thực hiện phân quyền cho tài khoản: " + byId.USERNAME, NoticeType.Warning);
        return (ActionResult) this.RedirectToAction("manageusers", "admin");
      }
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      List<WEBPAGES_ROLES> source = byId.WEBPAGES_ROLES != null ? byId.WEBPAGES_ROLES.ToList<WEBPAGES_ROLES>() : new List<WEBPAGES_ROLES>();
      List<int> actionOfUserIds = this.unitOfWork.Actions.GetActiveActionsByUser(byId).ToList<WEBPAGES_ACTIONS>().Select<WEBPAGES_ACTIONS, int>((Func<WEBPAGES_ACTIONS, int>) (o => o.WEBPAGES_ACTION_ID)).ToList<int>();
      List<int> roleOfUserIds = source.Select<WEBPAGES_ROLES, int>((Func<WEBPAGES_ROLES, int>) (o => o.WEBPAGES_ROLE_ID)).ToList<int>();
      List<int> department = new List<int>();
      if (byId != null)
      {
        LM_DEPARTMENT lmDepartment = new LM_DEPARTMENT();
        int? lmDepartmentId;
        int num2;
        if (byId != null)
        {
          lmDepartmentId = byId.LM_DEPARTMENT_ID;
          if (lmDepartmentId.HasValue)
          {
            lmDepartmentId = byId.LM_DEPARTMENT_ID;
            num2 = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
            goto label_14;
          }
        }
        num2 = 1;
label_14:
        if (num2 == 0)
        {
          List<int> intList = department;
          lmDepartmentId = byId.LM_DEPARTMENT_ID;
          int num3 = lmDepartmentId.Value;
          intList.Add(num3);
        }
      }
      List<WEBPAGES_ROLES> all = this.unitOfWork.Roles.GetAll(department);
      List<WEBPAGES_ROLES> list1;
      if (byUserName.USERNAME == "admin")
      {
        List<WEBPAGES_ACTIONS> list2 = this.unitOfWork.Actions.GetAll().ToList<WEBPAGES_ACTIONS>();
        list1 = this.unitOfWork.Roles.GetAll().ToList<WEBPAGES_ROLES>();
        List<UserMenuModel> list3 = list2.Select<WEBPAGES_ACTIONS, UserMenuModel>((Func<WEBPAGES_ACTIONS, UserMenuModel>) (o =>
        {
          UserMenuModel userMenuModel1 = new UserMenuModel();
          userMenuModel1.Name = o.MENU_NAME;
          userMenuModel1.GroupCode = o.GROUP_CODE;
          userMenuModel1.GroupName = o.GROUP_NAME;
          userMenuModel1.Description = o.DESCRIPTION;
          userMenuModel1.Id = o.WEBPAGES_ACTION_ID;
          UserMenuModel userMenuModel2 = userMenuModel1;
          bool? isMenu = o.IS_MENU;
          int num2 = isMenu.HasValue ? (isMenu.GetValueOrDefault() ? 1 : 0) : 0;
          userMenuModel2.IsMenu = num2 != 0;
          userMenuModel1.Selected = actionOfUserIds.Contains(o.WEBPAGES_ACTION_ID);
          userMenuModel1.Roles = o.WEBPAGES_ROLES == null ? string.Empty : string.Join(", ", o.WEBPAGES_ROLES.Where<WEBPAGES_ROLES>((Func<WEBPAGES_ROLES, bool>) (k => roleOfUserIds.Contains(k.WEBPAGES_ROLE_ID))).Select<WEBPAGES_ROLES, string>((Func<WEBPAGES_ROLES, string>) (k => k.ROLE_NAME)).ToArray<string>());
          return userMenuModel1;
        })).ToList<UserMenuModel>();
        List<KeyTextItem> list4 = all.Select<WEBPAGES_ROLES, KeyTextItem>((Func<WEBPAGES_ROLES, KeyTextItem>) (o => new KeyTextItem()
        {
          Text = o.ROLE_NAME,
          Id = o.WEBPAGES_ROLE_ID.ToString(),
          Selected = roleOfUserIds.Contains(o.WEBPAGES_ROLE_ID)
        })).OrderBy<KeyTextItem, string>((Func<KeyTextItem, string>) (o => o.Text)).ToList<KeyTextItem>();
ViewBag.UserMenuModel = list3;
ViewBag.Roles = list4;
ViewBag.User = byId;
      }
      else
      {
        list1 = (byUserName.WEBPAGES_ROLES ?? (ICollection<WEBPAGES_ROLES>) new List<WEBPAGES_ROLES>()).ToList<WEBPAGES_ROLES>();
        List<UserMenuModel> list2 = this.unitOfWork.Actions.GetActiveActionsByUser(byUserName).Select<WEBPAGES_ACTIONS, UserMenuModel>((Func<WEBPAGES_ACTIONS, UserMenuModel>) (o =>
        {
          UserMenuModel userMenuModel1 = new UserMenuModel();
          userMenuModel1.Name = o.MENU_NAME;
          userMenuModel1.GroupCode = o.GROUP_CODE;
          userMenuModel1.GroupName = o.GROUP_NAME;
          userMenuModel1.Description = o.DESCRIPTION;
          userMenuModel1.Id = o.WEBPAGES_ACTION_ID;
          UserMenuModel userMenuModel2 = userMenuModel1;
          bool? isMenu = o.IS_MENU;
          int num2 = isMenu.HasValue ? (isMenu.GetValueOrDefault() ? 1 : 0) : 0;
          userMenuModel2.IsMenu = num2 != 0;
          userMenuModel1.Selected = actionOfUserIds.Contains(o.WEBPAGES_ACTION_ID);
          userMenuModel1.Roles = o.WEBPAGES_ROLES == null ? string.Empty : string.Join(", ", o.WEBPAGES_ROLES.Where<WEBPAGES_ROLES>((Func<WEBPAGES_ROLES, bool>) (k => roleOfUserIds.Contains(k.WEBPAGES_ROLE_ID))).Select<WEBPAGES_ROLES, string>((Func<WEBPAGES_ROLES, string>) (k => k.ROLE_NAME)).ToArray<string>());
          return userMenuModel1;
        })).ToList<UserMenuModel>();
        List<KeyTextItem> list3 = all.Select<WEBPAGES_ROLES, KeyTextItem>((Func<WEBPAGES_ROLES, KeyTextItem>) (o => new KeyTextItem()
        {
          Text = o.ROLE_NAME,
          Id = o.WEBPAGES_ROLE_ID.ToString(),
          Selected = roleOfUserIds.Contains(o.WEBPAGES_ROLE_ID)
        })).OrderBy<KeyTextItem, string>((Func<KeyTextItem, string>) (o => o.Text)).ToList<KeyTextItem>();
ViewBag.UserMenuModel = list2;
ViewBag.Roles = list3;
ViewBag.User = byId;
      }
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("_ConfigUser");
      return (ActionResult) null;
    }

    [HttpPost]
    [CustomAuthorize]
    [ValidateJsonAntiForgeryToken]
    [ActionDescription(ActionCode = "Users_SaveConfigUser", ActionName = "Phân quyền truy cập cho người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    public ActionResult SaveConfigUser(List<int> actionIds, List<int> roleIds, int? userId)
    {
      if (actionIds == null)
        actionIds = new List<int>();
      if (roleIds == null)
        roleIds = new List<int>();
      int num;
      if (userId.HasValue)
      {
        int? nullable = userId;
        num = (nullable.GetValueOrDefault() != 0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 1 : 0;
      }
      else
        num = 0;
      if (num == 0)
        return (ActionResult) this.Json(JsonResponse.Json404((object) "Không tìm thấy người dùng cần phân quyền"));
      ITransaction transaction = (ITransaction) null;
      try
      {
        transaction = this.unitOfWork.BeginTransaction();
        ADMIN_USER user = this.unitOfWork.Users.GetById(userId.Value);
        ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
        if (byUserName == null)
          return (ActionResult) this.Json(JsonResponse.Json404((object) "Không tìm thấy người phân quyền"));
        if (user == null)
          return (ActionResult) this.Json(JsonResponse.Json404((object) "Không tìm thấy người dùng cần phân quyền"));
        if (byUserName.USERNAME != "admin" && !ListHelper.ContainsAllItems<int>(this.unitOfWork.Actions.GetActiveActionsByUser(byUserName).Select<WEBPAGES_ACTIONS, int>((Func<WEBPAGES_ACTIONS, int>) (o => o.WEBPAGES_ACTION_ID)).ToList<int>(), actionIds))
          return (ActionResult) this.Json(JsonResponse.Json400((object) "Không được phép phân những chức năng đã chọn"));
        if (!this.CheckPermission(user))
          return (ActionResult) this.Json(JsonResponse.Json400((object) "Bạn không có quyền với người dùng này !"));
        if (user.WEBPAGES_ROLES != null)
        {
          foreach (WEBPAGES_ROLES webpagesRoles in user.WEBPAGES_ROLES.ToList<WEBPAGES_ROLES>())
            user.WEBPAGES_ROLES.Remove(webpagesRoles);
        }
        if (user.USERS_ACTIONS != null)
        {
          foreach (USERS_ACTIONS usersActions in user.USERS_ACTIONS.ToList<USERS_ACTIONS>())
            user.USERS_ACTIONS.Remove(usersActions);
        }
        List<int> source = new List<int>();
        if (roleIds.Any<int>())
        {
          if (user.WEBPAGES_ROLES == null)
            user.WEBPAGES_ROLES = (ICollection<WEBPAGES_ROLES>) new Collection<WEBPAGES_ROLES>();
          foreach (int roleId in roleIds)
          {
            WEBPAGES_ROLES byId = this.unitOfWork.Roles.GetById(roleId);
            if (byId != null)
            {
              user.WEBPAGES_ROLES.Add(byId);
              if (byId.WEBPAGES_ACTIONS != null)
                source.AddRange((IEnumerable<int>) byId.WEBPAGES_ACTIONS.Select<WEBPAGES_ACTIONS, int>((Func<WEBPAGES_ACTIONS, int>) (o => o.WEBPAGES_ACTION_ID)).ToList<int>());
            }
          }
        }
        if (actionIds.Any<int>())
        {
          foreach (int actionId1 in actionIds)
          {
            int actionId = actionId1;
            if (source.Contains(actionId))
            {
              source.Remove(actionId);
              USERS_ACTIONS entity = user.USERS_ACTIONS.Where<USERS_ACTIONS>((Func<USERS_ACTIONS, bool>) (ua => ua.ADMIN_USER_ID == user.ADMIN_USER_ID && ua.WEBPAGES_ACTION_ID == actionId)).Select<USERS_ACTIONS, USERS_ACTIONS>((Func<USERS_ACTIONS, USERS_ACTIONS>) (ua => new USERS_ACTIONS()
              {
                ADMIN_USER = ua.ADMIN_USER,
                ADMIN_USER_ID = ua.ADMIN_USER_ID,
                UPDATE_DATE = ua.UPDATE_DATE,
                WEBPAGES_ACTIONS = ua.WEBPAGES_ACTIONS,
                WEBPAGES_ACTION_ID = ua.WEBPAGES_ACTION_ID
              })).FirstOrDefault<USERS_ACTIONS>();
              if (entity != null)
                this.unitOfWork.UsersInActions.Delete(entity);
            }
            else if (user.USERS_ACTIONS.Where<USERS_ACTIONS>((Func<USERS_ACTIONS, bool>) (ua => ua.ADMIN_USER_ID == user.ADMIN_USER_ID && ua.WEBPAGES_ACTION_ID == actionId)).Select<USERS_ACTIONS, USERS_ACTIONS>((Func<USERS_ACTIONS, USERS_ACTIONS>) (ua => new USERS_ACTIONS()
            {
              ADMIN_USER = ua.ADMIN_USER,
              ADMIN_USER_ID = ua.ADMIN_USER_ID,
              UPDATE_DATE = ua.UPDATE_DATE,
              WEBPAGES_ACTIONS = ua.WEBPAGES_ACTIONS,
              WEBPAGES_ACTION_ID = ua.WEBPAGES_ACTION_ID
            })).FirstOrDefault<USERS_ACTIONS>() == null)
              this.unitOfWork.UsersInActions.Add(new USERS_ACTIONS()
              {
                WEBPAGES_ACTION_ID = actionId,
                ADMIN_USER_ID = user.ADMIN_USER_ID,
                CREATE_DATE = new DateTime?(DateTime.Now),
                UPDATE_DATE = DateTime.Now,
                IS_ACTIVE = true
              });
          }
          if (source.Any<int>())
          {
            foreach (int actionId in source)
            {
              USERS_ACTIONS byKey = this.unitOfWork.UsersInActions.GetByKey(actionId, user.ADMIN_USER_ID);
              if (byKey != null)
              {
                byKey.IS_ACTIVE = new bool?(false);
                this.unitOfWork.UsersInActions.Update(byKey);
              }
              else
                this.unitOfWork.UsersInActions.Add(new USERS_ACTIONS()
                {
                  WEBPAGES_ACTION_ID = actionId,
                  ADMIN_USER_ID = user.ADMIN_USER_ID,
                  CREATE_DATE = new DateTime?(DateTime.Now),
                  UPDATE_DATE = DateTime.Now,
                  IS_ACTIVE = new bool?(false)
                });
            }
          }
        }
        this.WriteLog(enLogType.NomalLog, enActionType.Permission, "Phân quyền cho người dùng [" + user.FULLNAME + "] - [" + user.USERNAME + "]", "N/A", "N/A", 0, "", "");
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

    [CustomAuthorize]
    [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [HttpGet]
    public ActionResult GetDoctor(int departmentId, int currentDoctorId)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      List<SelectListItem> list = this.unitOfWork.Doctors.GetAllByDepartmentId(departmentId).Where<DOCTOR>((Func<DOCTOR, bool>) (d =>
      {
        bool? isdelete = d.ISDELETE;
        return !isdelete.GetValueOrDefault() && isdelete.HasValue;
      })).OrderBy<DOCTOR, string>((Func<DOCTOR, string>) (d => d.DOCTOR_NAME)).Select<DOCTOR, SelectListItem>((Func<DOCTOR, SelectListItem>) (d => new SelectListItem()
      {
        Text = this.Server.HtmlEncode(d.DOCTOR_NAME),
        Value = d.DOCTORS_ID.ToString(),
        Selected = d.DOCTORS_ID == currentDoctorId
      })).ToList<SelectListItem>();
      SelectListItem selectListItem = new SelectListItem()
      {
        Text = Localization.LabelSelect,
        Value = "0"
      };
      list.Insert(0, selectListItem);
      return (ActionResult) this.Json((object) list, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [CustomAuthorize]
    public ActionResult GetDoctorInfo(int doctorId)
    {
      DOCTOR byId = this.unitOfWork.Doctors.GetById(doctorId);
      var data = new
      {
        email = byId.EMAIL,
        phone = byId.PHONE
      };
      return (ActionResult) this.Json((object) data, JsonRequestBehavior.AllowGet);
    }

    [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
    [CustomAuthorize]
    [HttpGet]
    public ActionResult ViewTreeDepartment()
    {
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      if (byUserName != null)
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
            ViewBag.RootDepartment = this.unitOfWork.Departments.GetChildDepartment(lmDepartmentId.Value);
        }
        else
        {
ViewBag.RootDepartment = this.unitOfWork.Departments.GetChildDepartment(0);
        }
      }
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_TreeDepartment.cshtml");
      return (ActionResult) null;
    }

    public bool CheckPermission(ADMIN_USER user)
    {
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      if (!(byUserName.USERNAME != "admin"))
        return true;
      int? nullable1 = byUserName.LM_DEPARTMENT_ID;
      int? lmDepartmentId = user.LM_DEPARTMENT_ID;
      if ((nullable1.GetValueOrDefault() != lmDepartmentId.GetValueOrDefault() ? 1 : (nullable1.HasValue != lmDepartmentId.HasValue ? 1 : 0)) != 0)
      {
        int num = 0;
        bool flag = false;
        int? nullable2;
        if (user.LM_DEPARTMENT != null)
        {
          nullable2 = user.LM_DEPARTMENT.DEPARTMENT_PARENT_ID;
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
          LM_DEPARTMENT byId = departments.GetById(id);
          int? nullable4;
          if (byId != null)
          {
            nullable4 = byId.DEPARTMENT_PARENT_ID;
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
        return flag;
      }
      int adminUserId = byUserName.ADMIN_USER_ID;
      nullable1 = user.LM_DEPARTMENT_ID;
      return (adminUserId != nullable1.GetValueOrDefault() ? 0 : (nullable1.HasValue ? 1 : 0)) == 0;
    }

    public void GetDoctorByDepartment(int departmentId, List<int> listAllDepartment)
    {
      foreach (LM_DEPARTMENT lmDepartment in this.unitOfWork.Departments.GetChildDepartment(departmentId))
      {
        listAllDepartment.Add(lmDepartment.LM_DEPARTMENT_ID);
        this.GetDoctorByDepartment(lmDepartment.LM_DEPARTMENT_ID, listAllDepartment);
      }
    }
  }
}
