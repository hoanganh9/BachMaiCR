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
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

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
            UserModel userModel = null;
            ADMIN_USER u = null;
            if (userId.HasValue && userId.Value > 0)
            {
                u = unitOfWork.Users.GetById(userId.Value);
                if (u != null)
                    userModel = new UserModel(u);
            }
            if (userModel == null)
            {
                userModel = new UserModel()
                {
                    USERNAME = string.Empty,
                    ISACTIVED = true
                };
            }
            ADMIN_USER currentUser = unitOfWork.Users.GetByUserName(User.Identity.Name);
            List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
            List<SelectListItem> selectListItemList1 = new List<SelectListItem>();
            if (currentUser != null)
            {
                List<int> intList1 = new List<int>();
                List<DOCTOR> source = new List<DOCTOR>();
                if (u != null && u.LM_DEPARTMENT_ID.HasValue)
                {
                    IDepartmentRepository departments = unitOfWork.Departments;
                    bool? isdelete = departments.GetById(u.LM_DEPARTMENT_ID.Value).ISDELETE;
                    if (isdelete.GetValueOrDefault())
                    {
                        IDoctorRepository doctors = unitOfWork.Doctors;
                        source = doctors.GetAllByDepartmentId(u.LM_DEPARTMENT_ID.Value);
                    }
                }
                else if (currentUser.USERNAME == "admin")
                {
                    lmDepartmentList = unitOfWork.Departments.GetChildDepartment(0).ToList();
                }
                else
                {
                    if (currentUser.LM_DEPARTMENT_ID.HasValue)
                    {
                        IDepartmentRepository departments = unitOfWork.Departments;
                        LM_DEPARTMENT byId = departments.GetById(currentUser.LM_DEPARTMENT_ID.Value);
                        bool? isdelete = byId.ISDELETE;
                        if ((isdelete.GetValueOrDefault() ? 0 : (isdelete.HasValue ? 1 : 0)) != 0)
                        {
                            List<int> intList2 = intList1;
                            intList2.Add(currentUser.LM_DEPARTMENT_ID.Value);
                            lmDepartmentList.Add(byId);
                        }
                    }
                }
                selectListItemList1 = source.Where(d => d.ISDELETE == false).OrderBy((d => d.DOCTOR_NAME)).Select(d => new SelectListItem()
                {
                    Text = d.DOCTOR_NAME,
                    Value = d.DOCTORS_ID.ToString(),
                    Selected = currentUser.DOCTORS_ID == d.DOCTORS_ID

                }).ToList();
            }
            SelectListItem selectListItem = new SelectListItem()
            {
                Text = Localization.LabelSelect,
                Value = "0"
            };
            selectListItemList1.Insert(0, selectListItem);
            ViewBag.RootDepartment = lmDepartmentList;
            ViewBag.ListDoctor = selectListItemList1;
            List<MENULIST> allActive = unitOfWork.AdminMenu.GetAll_Active();
            List<SelectListItem> list = allActive.Select((d => new SelectListItem()
            {
                Text = d.MENU_NAME,
                Value = d.MENU_URL
            })).ToList();
            ViewBag.ListUrlMenu = list;
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_AddUser.cshtml", userModel);
            return RedirectToAction("ManageUsers", "Admin");
        }

        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ValidateInput(false)]
        [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [HttpPost]
        public ActionResult AddUser(UserModel model)
        {
            bool result;
            bool.TryParse(Request.QueryString["back"] ?? "False", out result);
            if (model.ADMIN_USER_ID > 0)
            {
                if (ModelState.IsValidField("Email") && ModelState.IsValidField("FullName"))
                {
                    ADMIN_USER byId = unitOfWork.Users.GetById(model.ADMIN_USER_ID);
                    if (byId != null)
                    {
                        if (!CheckPermission(byId))
                            return Json(JsonResponse.Json400("Bạn không có quyền với người dùng này !"));
                        byId.FULLNAME = model.FULLNAME == null ? null : model.FULLNAME.Trim();
                        byId.PHONE = model.PHONE == null ? null : model.PHONE.Trim();
                        byId.MAIL = model.MAIL == null ? null : model.MAIL.Trim();
                        byId.DOCTORS_ID = model.DOCTORS_ID;
                        byId.USERCODE = model.USERCODE;
                        byId.LM_DEPARTMENT_ID = model.LM_DEPARTMENT_ID;
                        byId.ACTIVE_URL = model.ACTIVE_URL;
                        unitOfWork.Users.Update(byId);
                        WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật người dùng [" + byId.USERNAME + "]", "N/A", "N/A", 0, string.Empty, string.Empty);
                        if (Request.IsAjaxRequest())
                            return Json(JsonResponse.Json200("Cập nhật người dùng thành công !"));
                        Notice.Show("Cập nhật người dùng thành công !", NoticeType.Success);
                    }
                }
                else
                {
                    if (Request.IsAjaxRequest())
                        return Json(JsonResponse.Json500("Không tìm thấy thông tin người dùng !"));
                    Notice.Show("Không tìm thấy thông tin người dùng", NoticeType.Error);
                    return View("AddUser", model);
                }
            }
            else if (ModelState.IsValid)
            {
                if (unitOfWork.Users.CheckUserExit(model.USERNAME.Trim()) && Request.IsAjaxRequest())
                    return Json(JsonResponse.Json500(("Tên người dùng <strong>" + model.USERNAME + "</strong> đã tồn tại, hãy nhập tên khác !")));
                ITransaction transaction = null;
                string str1 = Encrypt.RandomString(3);
                string str2 = Encrypt.Sha1HashWithHex(model.PASSWORD + str1);
                ADMIN_USER entity = new ADMIN_USER()
                {
                    USERNAME = Regex.Replace(model.USERNAME.Trim(), "<(.|\\n)*?>", string.Empty),
                    CREATE_DATE = DateTime.Now,
                    FULLNAME = model.FULLNAME == null ? null : model.FULLNAME.Trim(),
                    MAIL = model.MAIL == null ? null : model.MAIL.Trim(),
                    PHONE = model.PHONE == null ? null : model.PHONE.Trim(),
                    ACTIVE_URL = model.ACTIVE_URL,
                    ISDELETE = false,
                    PASSWORD = str2,
                    SALT = str1,
                    USERCODE = model.USERCODE,
                    DOCTORS_ID = model.DOCTORS_ID,
                    LM_DEPARTMENT_ID = model.LM_DEPARTMENT_ID,
                    ACTIVE_ENDDATE = new DateTime?(DateTime.Now.AddDays(1.0)),
                    REQIURE_CHANGE_PASS = true
                };
                try
                {
                    transaction = unitOfWork.BeginTransaction();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, "Thêm mới người dùng [" + entity.USERNAME + "]", "N/A", "N/A", 0, string.Empty, string.Empty);
                    unitOfWork.Users.Add(entity);
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
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json200("Thêm mới người dùng thành công !"));
                Notice.Show("Thêm mới người dùng thành công !", NoticeType.Success);
            }
            else
            {
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json500("Lỗi dữ liệu!"));
                Notice.Show("Invalid form", NoticeType.Error);
                return View("AddUser", model);
            }
            if (!result)
                return RedirectToAction("AddUser");
            string str = Request.Cookies["manageusers"] == null ? string.Empty : Request.Cookies["manageusers"].Value;
            if (string.IsNullOrWhiteSpace(str))
                return RedirectToAction("ManageUsers", "Admin");
            return Redirect(HttpUtility.UrlDecode(str));
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "Users_ResetPass", ActionName = "Reset mật khẩu người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [HttpGet]
        public ActionResult ResetPass(int userId)
        {
            UserResetPasswordModel resetPasswordModel = null;
            ADMIN_USER byId = unitOfWork.Users.GetById(userId);
            if (!CheckPermission(byId))
                return Json(JsonResponse.Json400("Bạn không có quyền với người dùng này !"));
            if (byId != null)
                resetPasswordModel = new UserResetPasswordModel(byId);
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_ResetPassword.cshtml", resetPasswordModel);
            RedirectToAction("ManageUsers", "Admin");
            return null;
        }

        [ActionDescription(ActionCode = "Users_ResetPass", ActionName = "Reset mật khẩu người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [CustomAuthorize]
        [ValidateInput(false)]
        public ActionResult ResetPass(UserResetPasswordModel model)
        {
            if (model.ADMIN_USER_ID <= 0)
                return Json(JsonResponse.Json400("Người dùng không tồn tại "));
            ADMIN_USER byId1 = unitOfWork.Users.GetById(model.ADMIN_USER_ID);
            if (!CheckPermission(byId1))
                return Json(JsonResponse.Json400("Bạn không có quyền với người dùng này !"));
            if (model.ADMIN_USER_ID > 0)
            {
                if (ModelState.IsValid && model.NewPassword == model.ConfirmPassword)
                {
                    ADMIN_USER byId2 = unitOfWork.Users.GetById(model.ADMIN_USER_ID);
                    if (byId2 != null)
                    {
                        ITransaction transaction = null;
                        string str1 = Encrypt.RandomString(3);
                        string str2 = Encrypt.Sha1HashWithHex(model.NewPassword + str1);
                        byId2.PASSWORD = str2;
                        byId2.SALT = str1;
                        try
                        {
                            transaction = unitOfWork.BeginTransaction();
                            unitOfWork.Users.Update(byId2);
                            WriteLog(enLogType.NomalLog, enActionType.Update, "Reset mật khẩu người dùng :" + byId1.USERNAME, "N/A", "N/A", byId2.ADMIN_USER_ID, string.Empty, string.Empty);
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
                else if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json500("Mật khẩu chưa đủ mạnh hoặc chưa điền đủ thông tin !"));
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json200("Cấp lại mật khẩu thành công !"));
                return null;
            }
            if (Request.IsAjaxRequest())
                return Json(JsonResponse.Json200("Cấp lại mật khẩu thành công !"));
            return null;
        }

        [HttpPost]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "Users_Delete", ActionName = "Xóa người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        public ActionResult Delete(int userId)
        {
            if (userId <= 0)
                return Json(JsonResponse.Json400("Người dùng không tồn tại "));
            ADMIN_USER byId = unitOfWork.Users.GetById(userId);
            if (!CheckPermission(byId))
                return Json(JsonResponse.Json400("Bạn không có quyền với người dùng này !"));
            if (byId != null)
            {
                if (byId.CALENDAR_DUTY.Any(o => o.ISDELETE.GetValueOrDefault()))
                    return Json(JsonResponse.Json400("Người dùng đang tồn tại liên kết với chức năng lịch "));
                if (byId.TEMPLATES.Any(o => o.ISDELETE.GetValueOrDefault()))
                    return Json(JsonResponse.Json400("Người dùng đang tồn tại liên kết với chức năng biểu mẫu"));
            }
            if (byId == null)
                return Json(JsonResponse.Json200("Xóa người dùng thành công !"));
            if (byId.USERNAME == "admin" || byId.USERNAME == User.Identity.Name)
                return Json(JsonResponse.Json400(("Không thể thay đổi thông tin của tài khoản " + byId.USERNAME)));
            if (byId.USERS_ACTIONS != null && byId.USERS_ACTIONS.Any() || byId.WEBPAGES_ROLES != null && byId.WEBPAGES_ROLES.Any())
                return Json(JsonResponse.Json500("Người dùng đã được phân quyền. Hãy bỏ hết quyền trước khi xóa !"));
            byId.ISDELETE = true;
            unitOfWork.Users.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa người dùng [" + byId.USERNAME + "]", "N/A", "N/A", userId, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Xóa user thành công !"));
        }

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [ActionDescription(ActionCode = "Users_ActiveChage", ActionName = "Active/deactive người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult ActiveChage(int userId, bool active)
        {
            if (userId <= 0)
                return Json(JsonResponse.Json400("Người dùng không tồn tại "));
            ADMIN_USER byId = unitOfWork.Users.GetById(userId);
            if (!CheckPermission(byId))
                return Json(JsonResponse.Json400("Bạn không có quyền với người dùng này !"));
            if (byId == null)
                return Json(JsonResponse.Json200("Cập nhật thành công !"));
            byId.ISACTIVED = active;
            unitOfWork.Users.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive người dùng [" + byId.USERNAME + "]", "N/A", "N/A", userId, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Cập nhật thành công !"));
        }

        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [CustomAuthorize]
        public JsonResult CheckExistUserName(string userName, int? id)
        {
            if (id.HasValue && unitOfWork.Users.GetById(id.Value) != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            if (unitOfWork.Users.GetByUserName(Regex.Replace(userName, "<(.|\\n)*?>", string.Empty)) != null)
                return Json(Localization.UserModelUserNameExist);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ActionDescription(ActionCode = "Users_SaveConfigUser", ActionName = "Phân quyền truy cập cho người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [CustomAuthorize]
        [HttpGet]
        public ActionResult ConfigUser(int? userId)
        {
            if (userId.GetValueOrDefault()==0)
                return RedirectToAction("manageusers", "admin");
            ADMIN_USER byId = unitOfWork.Users.GetById(userId.Value);
            if (byId == null)
                return RedirectToAction("manageusers", "admin");
            if (byId.USERNAME == "admin")
            {
                Notice.Show("Không thể thực hiện phân quyền cho tài khoản: " + byId.USERNAME, NoticeType.Warning);
                return RedirectToAction("manageusers", "admin");
            }
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            List<WEBPAGES_ROLES> source = byId.WEBPAGES_ROLES != null ? byId.WEBPAGES_ROLES.ToList() : new List<WEBPAGES_ROLES>();
            List<int> actionOfUserIds = unitOfWork.Actions.GetActiveActionsByUser(byId).ToList().Select((o => o.WEBPAGES_ACTION_ID)).ToList();
            List<int> roleOfUserIds = source.Select((o => o.WEBPAGES_ROLE_ID)).ToList();
            List<int> department = new List<int>();
            if (byId != null && byId.LM_DEPARTMENT_ID.HasValue)
            {
                department.Add(byId.LM_DEPARTMENT_ID.Value);
            }
            List<WEBPAGES_ROLES> all = unitOfWork.Roles.GetAll(department);
            List<WEBPAGES_ROLES> list1;
            if (byUserName.USERNAME == "admin")
            {
                List<WEBPAGES_ACTIONS> list2 = unitOfWork.Actions.GetAll().ToList();
                list1 = unitOfWork.Roles.GetAll().ToList();
                List<UserMenuModel> list3 = list2.Select((o =>
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
                   userMenuModel1.Roles = o.WEBPAGES_ROLES == null ? string.Empty : string.Join(", ", o.WEBPAGES_ROLES.Where((k => roleOfUserIds.Contains(k.WEBPAGES_ROLE_ID))).Select((k => k.ROLE_NAME)).ToArray<string>());
                   return userMenuModel1;
               })).ToList();
                List<KeyTextItem> list4 = all.Select((o => new KeyTextItem()
                {
                    Text = o.ROLE_NAME,
                    Id = o.WEBPAGES_ROLE_ID.ToString(),
                    Selected = roleOfUserIds.Contains(o.WEBPAGES_ROLE_ID)
                })).OrderBy((o => o.Text)).ToList();
                ViewBag.UserMenuModel = list3;
                ViewBag.Roles = list4;
                ViewBag.User = byId;
            }
            else
            {
                list1 = (byUserName.WEBPAGES_ROLES ?? (ICollection<WEBPAGES_ROLES>)new List<WEBPAGES_ROLES>()).ToList();
                List<UserMenuModel> list2 = unitOfWork.Actions.GetActiveActionsByUser(byUserName).Select((o =>
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
                   userMenuModel1.Roles = o.WEBPAGES_ROLES == null ? string.Empty : string.Join(", ", o.WEBPAGES_ROLES.Where((k => roleOfUserIds.Contains(k.WEBPAGES_ROLE_ID))).Select((k => k.ROLE_NAME)).ToArray<string>());
                   return userMenuModel1;
               })).ToList();
                List<KeyTextItem> list3 = all.Select((o => new KeyTextItem()
                {
                    Text = o.ROLE_NAME,
                    Id = o.WEBPAGES_ROLE_ID.ToString(),
                    Selected = roleOfUserIds.Contains(o.WEBPAGES_ROLE_ID)
                })).OrderBy((o => o.Text)).ToList();
                ViewBag.UserMenuModel = list2;
                ViewBag.Roles = list3;
                ViewBag.User = byId;
            }
            if (Request.IsAjaxRequest())
                return PartialView("_ConfigUser");
            return null;
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
            if (userId.GetValueOrDefault() == 0)
                return Json(JsonResponse.Json404("Không tìm thấy người dùng cần phân quyền"));
            ITransaction transaction = null;
            try
            {
                transaction = unitOfWork.BeginTransaction();
                ADMIN_USER user = unitOfWork.Users.GetById(userId.Value);
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                if (byUserName == null)
                    return Json(JsonResponse.Json404("Không tìm thấy người phân quyền"));
                if (user == null)
                    return Json(JsonResponse.Json404("Không tìm thấy người dùng cần phân quyền"));
                if (byUserName.USERNAME != "admin" && !ListHelper.ContainsAllItems<int>(unitOfWork.Actions.GetActiveActionsByUser(byUserName).Select((o => o.WEBPAGES_ACTION_ID)).ToList(), actionIds))
                    return Json(JsonResponse.Json400("Không được phép phân những chức năng đã chọn"));
                if (!CheckPermission(user))
                    return Json(JsonResponse.Json400("Bạn không có quyền với người dùng này !"));
                if (user.WEBPAGES_ROLES != null)
                {
                    foreach (WEBPAGES_ROLES webpagesRoles in user.WEBPAGES_ROLES.ToList())
                        user.WEBPAGES_ROLES.Remove(webpagesRoles);
                }
                if (user.USERS_ACTIONS != null)
                {
                    foreach (USERS_ACTIONS usersActions in user.USERS_ACTIONS.ToList())
                        user.USERS_ACTIONS.Remove(usersActions);
                }
                List<int> source = new List<int>();
                if (roleIds.Any())
                {
                    if (user.WEBPAGES_ROLES == null)
                        user.WEBPAGES_ROLES = (ICollection<WEBPAGES_ROLES>)new Collection<WEBPAGES_ROLES>();
                    foreach (int roleId in roleIds)
                    {
                        WEBPAGES_ROLES byId = unitOfWork.Roles.GetById(roleId);
                        if (byId != null)
                        {
                            user.WEBPAGES_ROLES.Add(byId);
                            if (byId.WEBPAGES_ACTIONS != null)
                                source.AddRange(byId.WEBPAGES_ACTIONS.Select((o => o.WEBPAGES_ACTION_ID)).ToList());
                        }
                    }
                }
                if (actionIds.Any())
                {
                    foreach (int actionId1 in actionIds)
                    {
                        int actionId = actionId1;
                        if (source.Contains(actionId))
                        {
                            source.Remove(actionId);
                            USERS_ACTIONS entity = user.USERS_ACTIONS.Where((ua => ua.ADMIN_USER_ID == user.ADMIN_USER_ID && ua.WEBPAGES_ACTION_ID == actionId)).Select((ua => new USERS_ACTIONS()
                            {
                                ADMIN_USER = ua.ADMIN_USER,
                                ADMIN_USER_ID = ua.ADMIN_USER_ID,
                                UPDATE_DATE = ua.UPDATE_DATE,
                                WEBPAGES_ACTIONS = ua.WEBPAGES_ACTIONS,
                                WEBPAGES_ACTION_ID = ua.WEBPAGES_ACTION_ID
                            })).FirstOrDefault<USERS_ACTIONS>();
                            if (entity != null)
                                unitOfWork.UsersInActions.Delete(entity);
                        }
                        else if (user.USERS_ACTIONS.Where((ua => ua.ADMIN_USER_ID == user.ADMIN_USER_ID && ua.WEBPAGES_ACTION_ID == actionId)).Select((ua => new USERS_ACTIONS()
                        {
                            ADMIN_USER = ua.ADMIN_USER,
                            ADMIN_USER_ID = ua.ADMIN_USER_ID,
                            UPDATE_DATE = ua.UPDATE_DATE,
                            WEBPAGES_ACTIONS = ua.WEBPAGES_ACTIONS,
                            WEBPAGES_ACTION_ID = ua.WEBPAGES_ACTION_ID
                        })).FirstOrDefault<USERS_ACTIONS>() == null)
                            unitOfWork.UsersInActions.Add(new USERS_ACTIONS()
                            {
                                WEBPAGES_ACTION_ID = actionId,
                                ADMIN_USER_ID = user.ADMIN_USER_ID,
                                CREATE_DATE = DateTime.Now,
                                UPDATE_DATE = DateTime.Now,
                                IS_ACTIVE = true
                            });
                    }
                    if (source.Any())
                    {
                        foreach (int actionId in source)
                        {
                            USERS_ACTIONS byKey = unitOfWork.UsersInActions.GetByKey(actionId, user.ADMIN_USER_ID);
                            if (byKey != null)
                            {
                                byKey.IS_ACTIVE = false;
                                unitOfWork.UsersInActions.Update(byKey);
                            }
                            else
                                unitOfWork.UsersInActions.Add(new USERS_ACTIONS()
                                {
                                    WEBPAGES_ACTION_ID = actionId,
                                    ADMIN_USER_ID = user.ADMIN_USER_ID,
                                    CREATE_DATE = DateTime.Now,
                                    UPDATE_DATE = DateTime.Now,
                                    IS_ACTIVE = false
                                });
                        }
                    }
                }
                WriteLog(enLogType.NomalLog, enActionType.Permission, "Phân quyền cho người dùng [" + user.FULLNAME + "] - [" + user.USERNAME + "]", "N/A", "N/A", 0, string.Empty, string.Empty);
                transaction.Commit();
                return Json(JsonResponse.Json200("Phân quyền cho người dùng thành công"));
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
            List<SelectListItem> list = unitOfWork.Doctors.GetAllByDepartmentId(departmentId).Where((d =>
           {
               bool? isdelete = d.ISDELETE;
               return !isdelete.GetValueOrDefault() && isdelete.HasValue;
           })).OrderBy((d => d.DOCTOR_NAME)).Select((d => new SelectListItem()
           {
               Text = Server.HtmlEncode(d.DOCTOR_NAME),
               Value = d.DOCTORS_ID.ToString(),
               Selected = d.DOCTORS_ID == currentDoctorId
           })).ToList();
            SelectListItem selectListItem = new SelectListItem()
            {
                Text = Localization.LabelSelect,
                Value = "0"
            };
            list.Insert(0, selectListItem);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult GetDoctorInfo(int doctorId)
        {
            DOCTOR byId = unitOfWork.Doctors.GetById(doctorId);
            var data = new
            {
                email = byId.EMAIL,
                phone = byId.PHONE
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [ActionDescription(ActionCode = "Users_AddUser", ActionName = "Cập nhật người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [CustomAuthorize]
        [HttpGet]
        public ActionResult ViewTreeDepartment()
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            if (byUserName != null)
            {
                if (byUserName.LM_DEPARTMENT_ID.HasValue)
                {
                    ViewBag.RootDepartment = unitOfWork.Departments.GetChildDepartment(byUserName.LM_DEPARTMENT_ID.Value);
                }
                else
                {
                    ViewBag.RootDepartment = unitOfWork.Departments.GetChildDepartment(0);
                }
            }
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_TreeDepartment.cshtml");
            return null;
        }

        public bool CheckPermission(ADMIN_USER user)
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            if (byUserName.USERNAME == "admin")
                return true;
            int? lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
            if (byUserName.LM_DEPARTMENT_ID.GetValueOrDefault() != user.LM_DEPARTMENT_ID.GetValueOrDefault())
            {
                int num = 0;
                bool flag = false;
                int? departmentParentId;
                if (user.LM_DEPARTMENT != null)
                {
                    departmentParentId = user.LM_DEPARTMENT.DEPARTMENT_PARENT_ID;
                }
                else
                {
                    departmentParentId = null;
                }
                do
                {
                    if ((departmentParentId.GetValueOrDefault() != lmDepartmentId.GetValueOrDefault() ? 0 : (departmentParentId.HasValue == lmDepartmentId.HasValue ? 1 : 0)) != 0)
                    {
                        flag = true;
                        break;
                    }
                    IDepartmentRepository departments = unitOfWork.Departments;
                    int id = departmentParentId ?? 0;
                    LM_DEPARTMENT byId = departments.GetById(id);
                    if (byId != null)
                    {
                        departmentParentId = byId.DEPARTMENT_PARENT_ID;
                    }
                    else
                    {
                        departmentParentId = null;
                    }
                    ++num;
                }
                while (departmentParentId.HasValue && num < 100);
                return flag;
            }
            int adminUserId = byUserName.ADMIN_USER_ID;
            return (adminUserId != user.LM_DEPARTMENT_ID.GetValueOrDefault() ? 0 : (user.LM_DEPARTMENT_ID.HasValue ? 1 : 0)) == 0;
        }

        public void GetDoctorByDepartment(int departmentId, List<int> listAllDepartment)
        {
            foreach (LM_DEPARTMENT lmDepartment in unitOfWork.Departments.GetChildDepartment(departmentId))
            {
                listAllDepartment.Add(lmDepartment.LM_DEPARTMENT_ID);
                GetDoctorByDepartment(lmDepartment.LM_DEPARTMENT_ID, listAllDepartment);
            }
        }
    }
}