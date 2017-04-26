using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                if (byUserName != null)
                {
                    if (ModelState.IsValid && role.LM_DEPARTMENT_ID > 0)
                    {
                        if (role.WEBPAGES_ROLES_ID > 0)
                        {
                            WEBPAGES_ROLES byId = unitOfWork.Roles.GetById(role.WEBPAGES_ROLES_ID);
                            if (role.ROLES_NAME != byId.ROLE_NAME && unitOfWork.Roles.IsExistRoleName(role.ROLES_NAME.Trim()) && Request.IsAjaxRequest())
                                return Json(JsonResponse.Json500(("Quyền <strong>" + Server.HtmlEncode(role.ROLES_NAME) + "</strong> đã tồn tại")));
                            byId.ROLE_NAME = role.ROLES_NAME.Trim();
                            byId.DESCRIPTION = role.DESCRIPTION == null ? string.Empty : role.DESCRIPTION.Trim();
                            byId.ABBREVIATION = role.ABBREVIATION == null ? string.Empty : role.ABBREVIATION.Trim();
                            byId.ISACTIVE = role.ISACTIVE;
                            byId.WEBPAGES_ROLE_ID = role.WEBPAGES_ROLES_ID;
                            byId.LM_DEPARTMENT_ID = role.LM_DEPARTMENT_ID;
                            unitOfWork.Roles.Update(byId);
                            WriteLog(enLogType.NomalLog, enActionType.Update, " Cập nhật nhóm quyền [" + byId.ROLE_NAME + "]", "N/A", "N/A", byId.WEBPAGES_ROLE_ID, string.Empty, string.Empty);
                            if (Request.IsAjaxRequest())
                                return Json(JsonResponse.Json200("Cập nhập thành công"));
                        }
                        else
                        {
                            if (unitOfWork.Roles.IsExistRoleName(role.ROLES_NAME.Trim()) && Request.IsAjaxRequest())
                                return Json(JsonResponse.Json500(("Quyền <strong>" + Server.HtmlEncode(role.ROLES_NAME) + "</strong> đã tồn tại")));
                            WEBPAGES_ROLES entity = new WEBPAGES_ROLES()
                            {
                                ISACTIVE = role.ISACTIVE,
                                ROLE_NAME = role.ROLES_NAME.Trim(),
                                ABBREVIATION = role.ABBREVIATION == null ? string.Empty : role.ABBREVIATION.Trim(),
                                DESCRIPTION = role.DESCRIPTION == null ? string.Empty : role.DESCRIPTION.Trim(),
                                CREATE_DATE = DateTime.Now,
                                CREATE_USERID = byUserName.ADMIN_USER_ID,
                                LM_DEPARTMENT_ID = role.LM_DEPARTMENT_ID
                            };
                            unitOfWork.Roles.Add(entity);
                            WriteLog(enLogType.NomalLog, enActionType.Insert, " Thêm mới nhóm quyền [" + entity.ROLE_NAME + "]", "N/A", "N/A", entity.WEBPAGES_ROLE_ID, string.Empty, string.Empty);
                        }
                        if (Request.IsAjaxRequest())
                            return Json(JsonResponse.Json200("Tạo thành công"));
                        return RedirectToAction("ManageRoles", "Admin");
                    }
                    if (Request.IsAjaxRequest())
                        return Json(JsonResponse.Json500("Kiểm tra lại thông tin trên form"));
                    return null;
                }
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json500("Phiên đăng nhập đã hết, hãy đăn nhập lại !"));
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionDescription(ActionCode = "WebpageRoles_AddRole", ActionName = "Cập nhật nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
        [CustomAuthorize]
        [HttpGet]
        public ActionResult AddRole(int? id)
        {
            RoleModel roleModel = null;
            List<LM_DEPARTMENT> lmDepartmentList1 = new List<LM_DEPARTMENT>();
            if (id.HasValue)
            {
                WEBPAGES_ROLES byId = unitOfWork.Roles.GetById(id.Value);
                if (byId != null)
                    roleModel = new RoleModel(byId);
            }
            else
                roleModel = new RoleModel() { ISACTIVE = true };
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            if (byUserName != null)
            {
                if (byUserName.USERNAME == "admin")
                {
                    lmDepartmentList1 = unitOfWork.Departments.GetChildDepartment(0).ToList();
                }
                else if (byUserName.LM_DEPARTMENT_ID.HasValue && byUserName.LM_DEPARTMENT_ID.Value >0)
                {
                    LM_DEPARTMENT byId = unitOfWork.Departments.GetById(byUserName.LM_DEPARTMENT_ID.Value);
                    lmDepartmentList1.Add(byId);
                }
            }
            ViewBag.RootDepartment = lmDepartmentList1;
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_AddRole.cshtml", roleModel);
            RedirectToAction("ManageRoles", "Admin");
            return null;
        }

        [ActionDescription(ActionCode = "WebpageRoles_AddUser", ActionName = "Thêm người dùng", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
        [CustomAuthorize]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddUser(int id, string key, int deparmentId = 0)
        {
            WEBPAGES_ROLES webpagesRoles = null;
            if (id > 0)
                webpagesRoles = unitOfWork.Roles.GetById(id);
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            List<int> returnDepartments1 = new List<int>();
            List<int> returnDepartments2 = new List<int>();
            if (webpagesRoles != null && webpagesRoles.LM_DEPARTMENT_ID.HasValue)
            {
                GetDepartment(webpagesRoles.LM_DEPARTMENT_ID.Value, returnDepartments2);
                returnDepartments2.Add(webpagesRoles.LM_DEPARTMENT_ID.Value);
                if (deparmentId > 0 && returnDepartments2.Contains(deparmentId))
                {
                    GetDepartment(deparmentId, returnDepartments1);
                    returnDepartments1.Add(deparmentId);
                }
                else
                {
                    returnDepartments1 = returnDepartments2;
                    returnDepartments1.Add(webpagesRoles.LM_DEPARTMENT_ID.Value);
                }
            }
            IUserRepository users = unitOfWork.Users;
            List<int> departments = returnDepartments1;
            string key1 = key.Trim();
            int adminUserId = byUserName.ADMIN_USER_ID;
            int createdUserId;
            if (webpagesRoles.CREATE_USERID.HasValue)
            {
                createdUserId = webpagesRoles.CREATE_USERID.Value;
            }
            else
                createdUserId = 0;
            List<ADMIN_USER> list = users.GetAll(departments, key1, adminUserId, createdUserId).ToList();
            ViewBag.ListUser = list;
            ViewBag.Role = webpagesRoles;
            List<LM_DEPARTMENT> deptCurrent = GetDeptCurrent();
            ViewBag.RootDepartment = deptCurrent;
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_AddUserToRole.cshtml");
            RedirectToAction("ManageRoles", "Admin");
            return null;
        }

        [ValidateInput(false)]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "WebpageRoles_AddUser", ActionName = "Thêm người dùng", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
        [HttpGet]
        public ActionResult SearchUser(int id, string key, int deparmentId = 0)
        {
            WEBPAGES_ROLES webpagesRoles = null;
            if (id > 0)
                webpagesRoles = unitOfWork.Roles.GetById(id);
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            List<int> returnDepartments1 = new List<int>();
            List<int> returnDepartments2 = new List<int>();
            if (webpagesRoles != null && webpagesRoles.LM_DEPARTMENT_ID.HasValue)
            {
                GetDepartment(webpagesRoles.LM_DEPARTMENT_ID.Value, returnDepartments2);
                returnDepartments2.Add(webpagesRoles.LM_DEPARTMENT_ID.Value);
                if (deparmentId > 0 && returnDepartments2.Contains(deparmentId))
                {
                    GetDepartment(deparmentId, returnDepartments1);
                    returnDepartments1.Add(deparmentId);
                }
                else
                {
                    returnDepartments1 = returnDepartments2;
                    returnDepartments1.Add(webpagesRoles.LM_DEPARTMENT_ID.Value);
                }
            }
            IUserRepository users = unitOfWork.Users;
            List<int> departments = returnDepartments1;
            string key1 = key.Trim();
            int adminUserId = byUserName.ADMIN_USER_ID;
            int createdUserId;
            if (webpagesRoles.CREATE_USERID.HasValue)
            {
                createdUserId = webpagesRoles.CREATE_USERID.Value;
            }
            else
                createdUserId = 0;
            List<ADMIN_USER> list = users.GetAll(departments, key1, adminUserId, createdUserId).ToList();
            ViewBag.ListUser = list;
            ViewBag.Role = webpagesRoles;
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_SearchUserToRole.cshtml");
            RedirectToAction("ManageRoles", "Admin");
            return null;
        }

        [CustomAuthorize]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "WebpageRoles_Delete", ActionName = "Xóa nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền")]
        public ActionResult Delete(int id)
        {
            try
            {
                WEBPAGES_ROLES byId = unitOfWork.Roles.GetById(id);
                if (byId != null)
                {
                    if (byId.ADMIN_USER.Any() || byId.WEBPAGES_ACTIONS.Any())
                        return Json(JsonResponse.Json500("Có ràng buộc bản ghi, không thể xóa được"));
                    unitOfWork.Roles.Delete(byId);
                    WriteLog(enLogType.NomalLog, enActionType.Delete, " Xóa nhóm quyền [" + byId.ROLE_NAME + "]. Người xóa [" + User.Identity.Name + "]", "N/A", "N/A", byId.WEBPAGES_ROLE_ID, "ROLES_GROUP_CODE", "Quản lý nhóm quyền");
                }
                return Json(JsonResponse.Json200("Xóa thành công"));
            }
            catch (Exception ex)
            {
                return Json(JsonResponse.Json500("Xóa không thành công"));
            }
        }

        [HttpPost]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "WebpageRoles_UpdateActive", ActionName = "active/deactive nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
        public ActionResult UpdateActive(int roleId, bool? active)
        {
            WEBPAGES_ROLES byId = unitOfWork.Roles.GetById(roleId);
            if (byId == null)
                return Json(JsonResponse.Json404("Not found"));
            WEBPAGES_ROLES webpagesRoles = byId;
            webpagesRoles.ISACTIVE = active.GetValueOrDefault();
            unitOfWork.Roles.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive nhóm quyền [" + byId.ROLE_NAME + "]", "N/A", "N/A", byId.WEBPAGES_ROLE_ID, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Cập nhập thành công"));
        }

        [ActionDescription(ActionCode = "WebpageRoles_UpdateRoleWithActions", ActionName = "Cập nhật quyền cho nhóm quyền", GroupCode = "ROLES_GROUP_CODE", GroupName = "Quản lý nhóm quyền", IsMenu = false)]
        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateRoleWithActions(int roleId, string[] actions)
        {
            WEBPAGES_ROLES byId1 = unitOfWork.Roles.GetById(roleId);
            if (byId1 == null)
                return Json(JsonResponse.Json404("Not found"));
            ITransaction transaction = null;
            try
            {
                transaction = unitOfWork.BeginTransaction();
                List<WEBPAGES_ACTIONS> list = byId1.WEBPAGES_ACTIONS.ToList();
                if (list.Any())
                {
                    foreach (WEBPAGES_ACTIONS webpagesActions in list)
                        byId1.WEBPAGES_ACTIONS.Remove(webpagesActions);
                }
                if (actions != null && (actions).Any())
                {
                    foreach (string action in actions)
                    {
                        WEBPAGES_ACTIONS byId2 = unitOfWork.Actions.GetById(int.Parse(action));
                        if (byId2 != null)
                            byId1.WEBPAGES_ACTIONS.Add(byId2);
                    }
                }
                WriteLog(enLogType.NomalLog, enActionType.Permission, "Cập nhật quyền cho nhóm quyền [" + byId1.ROLE_NAME + "]", "N/A", "N/A", byId1.WEBPAGES_ROLE_ID, string.Empty, string.Empty);
                transaction.Commit();
                return Json(JsonResponse.Json200("Cập nhập thành công"));
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
            if (!roleId.HasValue)
                return Json(JsonResponse.Json404("Role not found"), JsonRequestBehavior.AllowGet);
            WEBPAGES_ROLES byId = unitOfWork.Roles.GetById(roleId.Value);
            if (byId == null)
                return Json(JsonResponse.Json404("Role not found"), JsonRequestBehavior.AllowGet);
            List<int> intList = new List<int>();
            if (byId.WEBPAGES_ACTIONS != null)
                intList = byId.WEBPAGES_ACTIONS.Select((o => o.WEBPAGES_ACTION_ID)).ToList();
            return Json(JsonResponse.Json200(intList), JsonRequestBehavior.AllowGet);
        }

        public void GetDepartment(int parentDepartment, List<int> returnDepartments)
        {
            foreach (LM_DEPARTMENT lmDepartment in unitOfWork.Departments.GetChildDepartment(parentDepartment))
            {
                returnDepartments.Add(lmDepartment.LM_DEPARTMENT_ID);
                GetDepartment(lmDepartment.LM_DEPARTMENT_ID, returnDepartments);
            }
        }

        [ActionDescription(ActionCode = "WebpageRoles_AddUser", ActionName = "Thêm người dùng", GroupCode = "USERS_GROUP_CODE", GroupName = "Quản lý người dùng", IsMenu = false)]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveConfigUser(int roleId, List<int> userIds, bool isAdd)
        {
            if (userIds == null || !userIds.Any())
                return Json(JsonResponse.Json404("Không tìm thấy người dùng cần phân quyền"));
            ITransaction transaction = null;
            try
            {
                foreach (int userId in userIds)
                {
                    ADMIN_USER byId1 = unitOfWork.Users.GetById(userId);
                    ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                    if (byUserName == null)
                        return Json(JsonResponse.Json404("Không tìm thấy người phân quyền"));
                    if (byId1 == null)
                        return Json(JsonResponse.Json404("Không tìm thấy người dùng cần phân quyền"));
                    if (byUserName.USERNAME != "admin" && byUserName.WEBPAGES_ROLES == null)
                        return Json(JsonResponse.Json400("Không được phép phân những chức năng đã chọn"));
                    if (byUserName.USERNAME != "admin")
                    {
                        int? lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
                        if (byUserName.LM_DEPARTMENT_ID != byId1.LM_DEPARTMENT_ID)
                        {
                            int num = 0;
                            bool flag = false;
                            int? departmentParentId;
                            if (byId1.LM_DEPARTMENT != null)
                            {
                                departmentParentId = byId1.LM_DEPARTMENT.DEPARTMENT_PARENT_ID;
                            }
                            else
                            {
                                departmentParentId = null;
                            }
                            do
                            {
                                if (departmentParentId.GetValueOrDefault() == lmDepartmentId.GetValueOrDefault())
                                {
                                    flag = true;
                                    break;
                                }
                                int id = departmentParentId ?? 0;
                                LM_DEPARTMENT byId2 = unitOfWork.Departments.GetById(id);
                                if (byId2 != null)
                                {
                                    departmentParentId = byId2.DEPARTMENT_PARENT_ID;
                                }
                                else
                                {
                                    departmentParentId = null;
                                }
                                ++num;
                            }
                            while (departmentParentId.HasValue && num < 100);
                            if (!flag)
                                return Json(JsonResponse.Json400(("Người dùng này thuộc phòng ban, bạn không được phép phân quyền " + byId1.USERNAME)));
                        }
                    }
                }
                transaction = unitOfWork.BeginTransaction();
                WEBPAGES_ROLES byId3 = unitOfWork.Roles.GetById(roleId);
                if (byId3 != null)
                {
                    foreach (int userId in userIds)
                    {
                        ADMIN_USER byId1 = unitOfWork.Users.GetById(userId);
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
                            string menuCode = string.Empty;
                            string menuName = string.Empty;
                            WriteLog((enLogType)num1, (enActionType)num2, content, description, errorCode, objId, menuCode, menuName);
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
                            string menuCode = string.Empty;
                            string menuName = string.Empty;
                            WriteLog((enLogType)num1, (enActionType)num2, content, description, errorCode, objId, menuCode, menuName);
                            byId1.WEBPAGES_ROLES.Remove(byId3);
                        }
                    }
                }
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
    }
}