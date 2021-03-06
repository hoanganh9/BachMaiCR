﻿using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Common.Helpers;
using BachMaiCR.Web.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class TemplateController : BaseController
    {
        public string sViewPath = "~/Views/Template/";

        public TemplateController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        public ActionResult Index()
        {
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            ViewBag.RootDepartment = RootDepartment();
            return View(sViewPath + "Index.cshtml");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "Template_Insert", ActionName = "Lập biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        public ActionResult AddTemPlate()
        {
            ViewBag.RootDepartment = RootDepartment();
            ViewData["DuLieuMoi"] = "1";
            return View(sViewPath + "_Add.cshtml");
        }

        private int? RootDepartment()
        {
            return unitOfWork.Users.GetByUserName(User.Identity.Name).LM_DEPARTMENT_ID;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "Template_Edit", ActionName = "Sửa biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        public PartialViewResult Edit(string id)
        {
            ViewBag.RootDepartment = RootDepartment();
            ViewData["DuLieuMoi"] = "0";
            ViewData["TEMPLATES_ID"] = id;
            TEMPLATE byId = unitOfWork.Templates.GetById(Convert.ToInt32(id));
            ViewBag.templateItem = byId;
            return PartialView(sViewPath + "_Add.cshtml");
        }

        private int getUserId(string userName)
        {
            return unitOfWork.Users.GetByUserName(userName).ADMIN_USER_ID;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "Template_Insert", ActionName = "Lập biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        public ActionResult UpdateSubmit()
        {
            string name = User.Identity.Name;
            int userId = getUserId(name);
            string str1 = Request.Form["DuLieuMoi"];
            string str2 = Request.Form["TEMPLATES_ID"];
            if (string.IsNullOrEmpty(str2))
                str2 = "-1";
            string str3 = Request.Form["txtName"];
            string str4 = Request.Form["cboDeparment"];
            string str5 = Request.Form["txtAbb"];
            string str6 = Request.Form["txtNote"];
            string str7 = Request.Form["cboToMonth"];
            string str8 = Request.Form["cboFromMonth"];
            string str9 = Request.Form["cboToYear"];
            string str10 = Request.Form["cboFromYear"];
            string str11 = "," + str4;
            int int32_1 = Convert.ToInt32(str7);
            int int32_2 = Convert.ToInt32(str8);
            string str12 = str7;
            string str13 = str8;
            try
            {
                if (int32_1 < 10)
                    str7 = "0" + str7;
                if (int32_2 < 10)
                    str8 = "0" + str8;
                int daysInMonth = Utils.Utils.GetDaysInMonth(int32_2, Convert.ToInt32(str10));
                string s1 = "01/" + str7 + "/" + str9;
                string str14 = "01/" + str8 + "/" + str10;
                string s2 = Convert.ToString(daysInMonth) + "/" + str8 + "/" + str10;
                NameValueCollection nameValueCollection = new NameValueCollection();
                if (string.IsNullOrEmpty(str3) || str3.Trim() == string.Empty)
                    nameValueCollection.Add("err_Name", Localization.Template_err_Name);
                if (!string.IsNullOrEmpty(str3) && str3.Trim() != string.Empty && str3.Trim().Length > 250)
                    nameValueCollection.Add("err_Name", Localization.Template_err_Name_maxLengh);
                if (string.IsNullOrEmpty(str5) || str5 == string.Empty)
                    nameValueCollection.Add("err_Abb", Localization.Template_err_Abb);
                if (!string.IsNullOrEmpty(str5) && str5 != string.Empty && str5.Length > 50)
                    nameValueCollection.Add("err_Abb", Localization.Template_err_Abb_maxLengh);
                if (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2) && Utils.Utils.ToDateTime(s1, "dd/mm/yyyy") > Utils.Utils.ToDateTime(str14, "dd/mm/yyyy"))
                    nameValueCollection.Add("err_fromDate", Localization.Template_err_fromDate2);
                if (!string.IsNullOrEmpty(str6) && str6.Trim().Length > 500)
                    nameValueCollection.Add("err_Note", Localization.Template_err_Contents_maxLengh);
                if (nameValueCollection.Count > 0)
                {
                    for (int index = 0; index <= nameValueCollection.Count - 1; ++index)
                        ModelState.AddModelError(nameValueCollection.GetKey(index), nameValueCollection[index]);
                    ViewBag.templateNameValue = str3;
                    ViewBag.templateAbbValue = str5;
                    ViewBag.templatentDeparmentValue = str4;
                    ViewBag.templatentDesValue = str6;
                    ViewBag.toMonth = str12;
                    ViewBag.toYear = str9;
                    ViewBag.fromMonth = str13;
                    ViewBag.fromYear = str10;
                    ViewBag.RootDepartment = RootDepartment();
                    return View(sViewPath + "_Add.cshtml");
                }
                List<SqlParameter> sqlParameterList1 = new List<SqlParameter>();
                if (str1 == "1")
                {
                    SqlParameter sqlParameter = new SqlParameter("@TEMPLATES_ID", SqlDbType.Int);
                    sqlParameter.Direction = ParameterDirection.Output;
                    sqlParameterList1.Add(sqlParameter);
                    sqlParameterList1.Add(new SqlParameter("@USER_CREATE", name));
                    sqlParameterList1.Add(new SqlParameter("@USER_CREATE_ID", userId));
                    sqlParameterList1.Add(new SqlParameter("@STATUS", CalendarDutyStatus.Created.GetHashCode()));
                }
                else
                    sqlParameterList1.Add(new SqlParameter("@TEMPLATES_ID", Convert.ToInt32(str2.Trim())));
                sqlParameterList1.Add(new SqlParameter("@TEMPLATE_NAME", str3.Trim()));
                sqlParameterList1.Add(new SqlParameter("@ABBREVIATION", str5.Trim()));
                sqlParameterList1.Add(new SqlParameter("@DATE_START", DateTime.Parse(s1)));
                sqlParameterList1.Add(new SqlParameter("@DATE_END", DateTime.Parse(s2)));
                sqlParameterList1.Add(new SqlParameter("@DESCRIPTION", str6.Trim()));
                sqlParameterList1.Add(new SqlParameter("@LM_DEPARTMENT_ID", str4));
                BACHMAICRContext bachmaicrContext = new BACHMAICRContext();
                int num1 = 0;
                num1 = (str1 != "1") ? bachmaicrContext.Database.ExecuteSqlCommand("exec sp_template_update @TEMPLATE_NAME, @ABBREVIATION, @DATE_START, @DATE_END, @DESCRIPTION, @LM_DEPARTMENT_ID, @TEMPLATES_ID", (object[])sqlParameterList1.ToArray()) : bachmaicrContext.Database.ExecuteSqlCommand("exec sp_template_insert @TEMPLATE_NAME, @ABBREVIATION, @DATE_START, @DATE_END, @STATUS, @USER_CREATE, @USER_CREATE_ID, @DESCRIPTION, @LM_DEPARTMENT_ID, @TEMPLATES_ID", (object[])sqlParameterList1.ToArray());
                List<SqlParameter> sqlParameterList2 = null;
                int int32_3 = Convert.ToInt32(Request.Form["txtNumber"]);
                int num2 = 0;
                for (int index = 1; index <= int32_3; ++index)
                {
                    int int32_4 = Convert.ToInt32(Request.Form["cboDeparment_" + index]);
                    string str15 = Request.Form["field_" + index];
                    string str16 = Request.Form["fieldselect_" + index];
                    if (!string.IsNullOrEmpty(str16))
                    {
                        str11 = str11 + "," + int32_4;
                        string str17 = "," + str4 + "," + int32_4;
                        if (index == int32_3)
                            num2 = 1;
                        bachmaicrContext.Database.ExecuteSqlCommand("exec sp_template_column_insert @COLUM_NAME, @COLUM_ORDER, @LM_DEPARTMENT_ID, @USERNAME, @DOCTOR_LEVEL, @STATUS_DATA, @TEMPLATES, @LM_DEPARTMENT_PATH, @CHECK, @LM_DEPARTMENT_PATH_CHILD", (object[])new List<SqlParameter>()
            {
              new SqlParameter("@COLUM_NAME", str15.Trim()),
              new SqlParameter("@COLUM_ORDER", index),
              new SqlParameter("@LM_DEPARTMENT_ID", int32_4),
              new SqlParameter("@USERNAME", name),
              new SqlParameter("@DOCTOR_LEVEL", str16.Trim()),
              new SqlParameter("@STATUS_DATA", str1),
              new SqlParameter("@TEMPLATES", Convert.ToInt32(str2)),
              new SqlParameter("@LM_DEPARTMENT_PATH", str11.Trim()),
              new SqlParameter("@CHECK", num2),
              new SqlParameter("@LM_DEPARTMENT_PATH_CHILD", str17)
            }.ToArray());
                        sqlParameterList2 = null;
                    }
                }
                if (str1 == "1")
                {
                    Notice.Show(Localization.MsgAddSuccess, NoticeType.Success);
                    WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgAddSuccess, "N/A", 0, string.Empty, string.Empty);
                    return RedirectToAction("AddTemPlate", "Template");
                }
                Notice.Show(Localization.MsgEditSuccess, NoticeType.Success);
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", Convert.ToInt32(str2.Trim()), string.Empty, string.Empty);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                if (str1 == "1")
                {
                    WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                    return RedirectToAction("AddTemPlate", "Template");
                }
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [HttpGet]
        [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [CustomAuthorize]
        public PartialViewResult List(string template_name, string template_abb, string dateApproved, string usernameCreate, string usernameAppoved, string departmentName = null, int status = 0, string sortFiled = null, string sortDir = null, int pageIndex = 0)
        {
            SearchTemplate calendarSearch = new SearchTemplate();
            calendarSearch.DEPARTMENTS = departmentName;
            calendarSearch.TEMPLATE_NAME = template_name;
            calendarSearch.ABBREVIATION = template_abb;
            calendarSearch.DATE_APPROVED = Utils.Utils.ConvetDateVNToDate(dateApproved);
            calendarSearch.ADMIN_USER_CREATE = usernameCreate;
            calendarSearch.ADMIN_USER_APPROVED = usernameAppoved;
            calendarSearch.STATUS = status;
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "List",
                ModelName = "Template",
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render"
            };
            sortFiled = string.IsNullOrEmpty(sortFiled) ? "LM_DEPARTMENT.DEPARTMENT_NAME" : sortFiled;
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            int totalRow = 0;
            PagedList<TEMPLATE> all = unitOfWork.Templates.GetAll(calendarSearch, pageIndex, pagination.PageSize, sortFiled, sortDir, 3, out totalRow);
            pagination.TotalRows = totalRow;
            pagination.CurrentRow = all.Count;
            ViewBag.calendarDuty = all;
            ViewBag.Pagination = pagination;
            if (string.IsNullOrEmpty(calendarSearch.DEPARTMENTS))
            {
                ViewBag.departmentName = string.Empty;
            }
            else
            {
                ViewBag.departmentName = calendarSearch.DEPARTMENTS.Trim();
            }
            if (string.IsNullOrEmpty(calendarSearch.TEMPLATE_NAME))
            {
                ViewBag.template_name = string.Empty;
            }
            else
            {
                ViewBag.template_name = calendarSearch.TEMPLATE_NAME.Trim();
            }
            if (string.IsNullOrEmpty(calendarSearch.ABBREVIATION))
            {
                ViewBag.template_abb = string.Empty;
            }
            else
            {
                ViewBag.template_abb = calendarSearch.ABBREVIATION.Trim();
            }
            ViewBag.dateApproved = calendarSearch.DATE_APPROVED;
            if (string.IsNullOrEmpty(calendarSearch.ADMIN_USER_CREATE))
            {
                ViewBag.usernameCreate = string.Empty;
            }
            else
            {
                ViewBag.usernameCreate = calendarSearch.ADMIN_USER_CREATE.Trim();
            }
            if (string.IsNullOrEmpty(calendarSearch.ADMIN_USER_APPROVED))
            {
                ViewBag.usernameAppoved = string.Empty;
            }
            else
            {
                ViewBag.usernameAppoved = calendarSearch.ADMIN_USER_APPROVED.Trim();
            }
            ViewBag.status = calendarSearch.STATUS;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_List", all);
        }

        [CustomAuthorize]
        [HttpPost]
        [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [ValidateInput(false)]
        public PartialViewResult CalendarLists(SearchTemplate calendarSearch)
        {
            int page = 0;
            Pagination pagination = new Pagination()
            {
                ActionName = "List",
                ModelName = "Template",
                CurrentPage = page,
                TagetReLoadId = "cat_list_render"
            };
            string sort = "TEMPLATE_NAME";
            string sortDir = "ASC";
            int totalRow = 0;
            if (calendarSearch.DATE_APPROVEDS != null)
                calendarSearch.DATE_APPROVED = Utils.Utils.ConvetDateVNToDate(calendarSearch.DATE_APPROVEDS.ToString());
            PagedList<TEMPLATE> all = unitOfWork.Templates.GetAll(calendarSearch, page, pagination.PageSize, sort, sortDir, 3, out totalRow);
            pagination.TotalRows = totalRow;
            pagination.CurrentRow = all.Count;
            ViewBag.calendarDuty = all;
            ViewBag.Pagination = pagination;
            if (string.IsNullOrEmpty(calendarSearch.DEPARTMENTS))
            {
                ViewBag.departmentName = string.Empty;
            }
            else
            {
                ViewBag.departmentName = calendarSearch.DEPARTMENTS.Trim();
            }
            if (string.IsNullOrEmpty(calendarSearch.TEMPLATE_NAME))
            {
                ViewBag.template_name = string.Empty;
            }
            else
            {
                ViewBag.template_name = calendarSearch.TEMPLATE_NAME.Trim();
            }
            if (string.IsNullOrEmpty(calendarSearch.ABBREVIATION))
            {
                ViewBag.template_abb = string.Empty;
            }
            else
            {
                ViewBag.template_abb = calendarSearch.ABBREVIATION.Trim();
            }
            ViewBag.dateApproved = calendarSearch.DATE_APPROVED;
            if (string.IsNullOrEmpty(calendarSearch.ADMIN_USER_CREATE))
            {
                ViewBag.usernameCreate = string.Empty;
            }
            else
            {
                ViewBag.usernameCreate = calendarSearch.ADMIN_USER_CREATE.Trim();
            }
            if (string.IsNullOrEmpty(calendarSearch.ADMIN_USER_APPROVED))
            {
                ViewBag.usernameAppoved = string.Empty;
            }
            else
            {
                ViewBag.usernameAppoved = calendarSearch.ADMIN_USER_APPROVED.Trim();
            }
            ViewBag.status = calendarSearch.STATUS;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            ViewBag.RootDepartment = RootDepartment();
            return PartialView("_List", all);
        }

        [ActionDescription(ActionCode = "Template_Deleted", ActionName = "Xóa biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [HttpPost]
        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                    return RedirectToAction("Index");
                if (id <= 0)
                    return Json(JsonResponse.Json500("Thao tác không hợp lệ!"));
                if (unitOfWork.CalendarDuty.ExistTemplateId(id))
                    throw new Exception("Biểu mẫu đã tồn tại trong bảng lịch trực Khoa/Viện/Trung tâm");
                TEMPLATE byId = unitOfWork.Templates.GetById(id);
                if (byId == null)
                    return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
                int? status = byId.STATUS;
                if ((status.GetValueOrDefault() != 2 ? 0 : (status.HasValue ? 1 : 0)) != 0)
                    throw new Exception("Biểu mẫu đã được phê duyệt không cho phép xóa!");
                byId.ISDELETE = true;
                unitOfWork.Templates.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json200(Localization.MsgDelSuccess));
                return null;
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [HttpGet]
        [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [CustomAuthorize]
        public ActionResult Detail(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            if (id <= 0)
                return Json(JsonResponse.Json500(Localization.MsgItemNotExist));
            TEMPLATE byId = unitOfWork.Templates.GetById(id);
            if (byId == null)
                return Json(JsonResponse.Json500(Localization.MsgItemNotExist));
            TemplateModel templateModel = new TemplateModel(byId);
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_Detail", templateModel);
        }

        [ActionDescription(ActionCode = "Template_Approved", ActionName = "Phê duyệt biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [HttpPost]
        [CustomAuthorize]
        public ActionResult CancelTemplate(string idValue, string statusValueID)
        {
            try
            {
                string name = User.Identity.Name;
                int userId = getUserId(name);
                DateTime? nullable = Utils.Utils.sysDate();
                int hashCode1 = TemplateStatus.Aproved.GetHashCode();
                int hashCode2 = TemplateStatus.CancelAproved.GetHashCode();
                int hashCode3 = TemplateStatus.Create.GetHashCode();
                int int32_1 = Convert.ToInt32(idValue);
                int int32_2 = Convert.ToInt32(statusValueID);
                TEMPLATE byId = unitOfWork.Templates.GetById(int32_1);
                if (byId == null)
                    return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
                if (int32_2 == hashCode3)
                {
                    byId.STATUS = hashCode1;
                    byId.USER_APPROVED_ID = userId;
                    byId.USER_APPROVED = name;
                    byId.DATE_APPROVED = nullable;
                }
                else if (int32_2 == hashCode1)
                    byId.STATUS = hashCode2;
                else if (int32_2 == hashCode2)
                {
                    byId.STATUS = hashCode1;
                    byId.USER_APPROVED_ID = userId;
                    byId.USER_APPROVED = name;
                    byId.DATE_APPROVED = nullable;
                }
                byId.TEMPLATES_ID = int32_1;
                unitOfWork.Templates.Update(byId);
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json200(Localization.MsgDelSuccess));
                if (int32_2 == hashCode3)
                {
                    WriteLog(enLogType.NomalLog, enActionType.Approve, "N/A", Localization.MsgstatusApproved + "[" + byId.TEMPLATE_NAME + "]", "N/A", int32_1, string.Empty, string.Empty);
                    Notice.Show(Localization.MsgstatusApproved, NoticeType.Success);
                }
                else if (int32_2 == hashCode1)
                {
                    WriteLog(enLogType.NomalLog, enActionType.ApproveCancel, "N/A", Localization.MsgstatusCancel + "[" + byId.TEMPLATE_NAME + "]", "N/A", int32_1, string.Empty, string.Empty);
                    Notice.Show(Localization.MsgstatusCancel, NoticeType.Success);
                }
                else if (int32_2 == hashCode2)
                {
                    WriteLog(enLogType.NomalLog, enActionType.Approve, "N/A", Localization.MsgstatusResore + "[" + byId.TEMPLATE_NAME + "]", "N/A", int32_1, string.Empty, string.Empty);
                    Notice.Show(Localization.MsgstatusResore, NoticeType.Success);
                }
                return ReturnValue(Localization.MsgAddSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Approve, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ActionDescription(ActionCode = "Template_Deleted", ActionName = "Xóa biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize]
        public ActionResult DelTemplate()
        {
            try
            {
                int int32 = Convert.ToInt32(Request.Form["template_Id"]);
                if (unitOfWork.CalendarDuty.ExistTemplateId(int32))
                    throw new Exception("Biểu mẫu đã tồn tại trong bảng lịch trực Khoa/Viện/Trung tâm");
                TEMPLATE byId = unitOfWork.Templates.GetById(int32);
                if (byId == null)
                    return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
                int? status = byId.STATUS;
                if ((status.GetValueOrDefault() != 2 ? 0 : (status.HasValue ? 1 : 0)) != 0)
                    throw new Exception("Biểu mẫu đã được phê duyệt không cho phép xóa!");
                byId.ISDELETE = true;
                byId.TEMPLATES_ID = int32;
                unitOfWork.Templates.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", int32, string.Empty, string.Empty);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        public JsonResult GetBranchesByCustomer(int parrentId)
        {
            string str = string.Empty;
            List<DEPARTMENTLIST> departmentAll = GenerateDepartmentTreeHelper.GenerateDepartmentAll(parrentId);
            if (departmentAll != null && departmentAll.Count > 0)
            {
                for (int index = 0; index < departmentAll.Count; ++index)
                    str = str + "<option value=" + departmentAll[index].LM_DEPARTMENT_ID + ">" + HttpUtility.JavaScriptStringEncode(departmentAll[index].DEPARTMENT_NAME) + "</option>";
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }
    }
}