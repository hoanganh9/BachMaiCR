using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Utilities.ReportForm;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Common.CustomActionResult;
using BachMaiCR.Web.Common.Helpers;
using BachMaiCR.Web.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class CalendarDutyController : BaseController
    {
        public CalendarDutyController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult GetTemplateByDate(int departmentId, int iMonth, int iYear, int status, int templateId)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            DateTime dateTime = Convert.ToDateTime(DateTime.DaysInMonth(iYear, iMonth).ToString() + "/" + iMonth.ToString() + "/" + iYear.ToString());
            List<TEMPLATE> listByDate = unitOfWork.Templates.GetListByDate(departmentId, dateTime, status);
            int currentTemplateId = 0;
            if (listByDate.Count > 0)
                currentTemplateId = templateId == 0 ? listByDate[0].TEMPLATES_ID : templateId;
            List<SelectListItem> list = listByDate.Where((d =>
           {
               bool? isdelete = d.ISDELETE;
               return !isdelete.GetValueOrDefault() && isdelete.HasValue;
           })).OrderBy((d => d.TEMPLATE_NAME)).Select((d => new SelectListItem()
           {
               Text = Server.HtmlEncode(d.TEMPLATE_NAME),
               Value = d.TEMPLATES_ID.ToString(),
               Selected = d.TEMPLATES_ID == currentTemplateId
           })).ToList();
            SelectListItem selectListItem = new SelectListItem()
            {
                Text = Localization.CalendarDefault,
                Value = "0",
                Selected = false
            };
            list.Insert(0, selectListItem);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult HistoryCalendarDuty(int idCalendarDuty)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            if (idCalendarDuty <= 0)
                return Json(JsonResponse.Json500(Localization.MsgItemNotExist));
            List<CALENDAR_CHANGE> calendarChangeList = unitOfWork.CalendarChanges.ListCalendarChange(idCalendarDuty);
            if (calendarChangeList == null)
                return Json(JsonResponse.Json500(Localization.MsgItemNotExist));
            ViewBag.calendarChange = calendarChangeList;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(idCalendarDuty);
            ViewBag.objCalendarDuty = byId;
            return PartialView("_HistoryCalendar");
        }

        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDoctorInCalendar(int calendarDutyId, int doctorId, DateTime date)
        {
            unitOfWork.CalendarDoctors.DeleteByDoctorId(calendarDutyId, doctorId, date);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        public ActionResult Index()
        {
            ViewBag.types = 3;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            ViewBag.RootDepartment = RootDepartment();
            return View();
        }

        private int? RootDepartment()
        {
            return unitOfWork.Users.GetByUserName(User.Identity.Name).LM_DEPARTMENT_ID;
        }

        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public PartialViewResult List(string dateCreate, string dateApproved, string usernameCreate, string usernameAppoved, int calendarStatus = 0, int dateMonth = 0, int dateYear = 0, string feast = null, string departmentName = null, string sortFiled = null, string sortDir = null, int pageIndex = 0, int types = 0)
        {
            SearchCalendarDuty calendarSearch = new SearchCalendarDuty();
            calendarSearch.DATE_CREATE = Utils.Utils.ConvetDateVNToDate(dateCreate);
            calendarSearch.DATE_APPROVED = Utils.Utils.ConvetDateVNToDate(dateApproved);
            calendarSearch.ADMIN_USER_CREATE = usernameCreate;
            calendarSearch.ADMIN_USER_APPROVED = usernameAppoved;
            calendarSearch.ADMIN_USER_CREATE = usernameCreate;
            calendarSearch.ADMIN_USER_APPROVED = usernameAppoved;
            calendarSearch.CALENDAR_STATUS = calendarStatus;
            calendarSearch.DATE_MONTH = dateMonth;
            calendarSearch.DATE_YEAR = dateYear;
            calendarSearch.FEAST = feast;
            calendarSearch.DEPARTMENTS = departmentName;
            pageIndex = pageIndex <= 0 ? 0 : pageIndex;
            Pagination pagination = new Pagination()
            {
                ActionName = "List",
                ModelName = "CalendarDuty",
                CurrentPage = pageIndex,
                InputSearchId = "txt_search_form",
                TagetReLoadId = "cat_list_render"
            };
            sortFiled = string.IsNullOrEmpty(sortFiled) ? "CALENDAR_NAME" : sortFiled;
            sortDir = "ASC";
            int totalRow = 0;
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            PagedList<CALENDAR_DUTY> all = unitOfWork.CalendarDuty.GetAll(calendarSearch, pageIndex, pagination.PageSize, sortFiled, sortDir, types, byUserName.LM_DEPARTMENT_ID.ToString(), out totalRow);
            pagination.TotalRows = totalRow;
            pagination.CurrentRow = all.Count;
            ViewBag.calendarDuty = all;
            ViewBag.Pagination = pagination;
            ViewBag.dateCreate = calendarSearch.DATE_CREATE;
            ViewBag.dateApproved = calendarSearch.DATE_APPROVED;
            ViewBag.usernameCreate = calendarSearch.ADMIN_USER_CREATE;
            ViewBag.usernameAppoved = calendarSearch.ADMIN_USER_APPROVED;
            ViewBag.calendarStatus = calendarSearch.CALENDAR_STATUS;
            ViewBag.dateMonth = calendarSearch.DATE_MONTH;
            ViewBag.dateYear = calendarSearch.DATE_YEAR;
            ViewBag.feast = calendarSearch.FEAST;
            ViewBag.departmentName = calendarSearch.DEPARTMENTS;
            ViewBag.types = types;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            return PartialView("_CalendarList", all);
        }

        [ValidateInput(false)]
        [ActionDescription(ActionCode = "CalendarDuty_Insert", ActionName = "Lập lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AddValuesCalendarDuty(string contentDuty, string strValues, string strInfor)
        {
            string[] strArray1 = strInfor.Split('_');
            if (strArray1.Length == 3)
            {
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                int month = 0;
                int year = 0;
                int templatesId = 0;
                int templateColumId = 0;
                int doctorsId = 0;
                if (int.TryParse(strArray1[0], out month) && int.TryParse(strArray1[1], out year) && int.TryParse(strArray1[2], out templatesId))
                {
                    if (unitOfWork.CalendarDuty.CheckCalendarDuty(month, year, templatesId, Convert.ToInt32(byUserName.LM_DEPARTMENT_ID)) != 0)
                        return Json(Localization.UpdateCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
                    TEMPLATE byId = unitOfWork.Templates.GetById(templatesId);
                    CALENDAR_DUTY entity1 = new CALENDAR_DUTY();
                    entity1.CALENDAR_NAME = contentDuty;
                    entity1.CALENDAR_MONTH = month;
                    entity1.CALENDAR_YEAR = year;
                    entity1.CALENDAR_STATUS = 1;
                    entity1.TEMPLATES_ID = templatesId;
                    entity1.LM_DEPARTMENT_ID = byId.LM_DEPARTMENT_ID;
                    entity1.DUTY_TYPE = 3;
                    entity1.ISDELETE = false;
                    entity1.USER_CREATE_ID = byUserName.ADMIN_USER_ID;
                    entity1.LM_DEPARTMENT_PARTS = byUserName.LM_DEPARTMENT.DEPARTMENT_PATH + byId.LM_DEPARTMENT_PATH + ",";
                    unitOfWork.CalendarDuty.Add(entity1);
                    string[] strArray2 = strValues.Split('-');
                    foreach (string str in strArray2)
                    {
                        char[] chArray = new char[1] { '_' };
                        string[] strArray3 = str.Split(chArray);
                        if ((strArray3).Count<string>() == 2)
                        {
                            int.TryParse(strArray3[1], out doctorsId);
                            string[] strArray4 = strArray3[0].Split(',');
                            int.TryParse(strArray4[0], out templateColumId);
                            DateTime? nullable2;
                            try
                            {
                                nullable2 = Utils.Utils.ConvetDateVNToDates(strArray4[1].ToString());
                            }
                            catch
                            {
                                nullable2 = null;
                            }
                            CALENDAR_DATA entity2 = new CALENDAR_DATA();
                            entity2.CALENDAR_DUTY_ID = entity1.CALENDAR_DUTY_ID;
                            entity2.TEMPLATE_COLUM_ID = templateColumId;
                            entity2.DATE_START = nullable2;
                            entity2.ISDELETE = false;
                            unitOfWork.CalendarDatas.Add(entity2);
                            unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                            {
                                CALENDAR_DATA_ID = entity2.CALENDAR_DATA_ID,
                                DOCTORS_ID = doctorsId,
                                CALENDAR_DUTY_ID = entity1.CALENDAR_DUTY_ID,
                                COLUMN_LEVEL_ID = templateColumId
                            });
                        }
                    }
                    WriteLog(enLogType.NomalLog, enActionType.Insert, "Thêm mới thông tin lịch trực đơn vị", "N/A", "N/A", 0, string.Empty, string.Empty);
                    return Json(Localization.AddCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDuty_Edit", ActionName = "Sửa thông tin lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpGet]
        public ActionResult ChangeCalendarDuty(string idCalendarDuty, string idColumn, string idDoctorChange, string dateDoctorChange, string idDoctorIsChange, string DatesIsChange, int idWeek = -1, int idColumeIsChange = 0, int isExist = 0)
        {
            if (!Request.IsAjaxRequest())
                return null;
            if (idCalendarDuty == null || idColumn == null || (idDoctorChange == null || dateDoctorChange == null) || idDoctorIsChange == null || DatesIsChange == null)
                return Json(JsonResponse.Json500("Đổi lịch không thành công!"), JsonRequestBehavior.AllowGet);
            List<DOCTOR> list = unitOfWork.Doctors.GetAll().ToList();
            int calendarDutyId = 0;
            int templateColumnId = 0;
            int idDoctorChangeX = 0;
            int idDoctorIsChangeX = 0;
            CalendarChangeModel calendarChangeModel = new CalendarChangeModel();
            if (!int.TryParse(idCalendarDuty, out calendarDutyId) || !int.TryParse(idColumn, out templateColumnId) || !int.TryParse(idDoctorChange, out idDoctorChangeX) || !int.TryParse(idDoctorIsChange, out idDoctorIsChangeX))
                return Json(JsonResponse.Json500("Đổi lịch không thành công!"), JsonRequestBehavior.AllowGet);
            CALENDAR_CHANGE entity1 = new CALENDAR_CHANGE();
            DateTime? dateStart;
            try
            {
                dateStart = Utils.Utils.ConvetDateVNToDates(dateDoctorChange);
            }
            catch
            {
                dateStart = null;
            }
            DateTime? dateChangeStart;
            try
            {
                dateChangeStart = Utils.Utils.ConvetDateVNToDates(DatesIsChange);
            }
            catch
            {
                dateChangeStart = null;
            }
            if (dateChangeStart.HasValue)
            {
                entity1.CALENDAR_DUTY_ID = calendarDutyId;
                entity1.TEMPLATE_COLUMN_ID = templateColumnId;
                entity1.DOCTORS_ID = idDoctorChangeX;
                entity1.DATE_START = dateStart;
                entity1.DOCTORS_CHANGE_ID = idDoctorIsChangeX;
                entity1.DATE_CHANGE_START = dateChangeStart;
                entity1.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                entity1.STATUS = 1;
                entity1.STATUS_APPROVED = 1;
                entity1.DATE_CHANGE = DateTime.Now;
                entity1.DOCTORS_NAME = list.Where((o => o.DOCTORS_ID == idDoctorChangeX)).ToList()[0].ABBREVIATION;
                entity1.DOCTORS_CHANGE_NAME = list.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                entity1.CALENDAR_DELETE = 1;
                entity1.COLUMN_CHANGE_ID = idColumeIsChange;
                unitOfWork.CalendarChanges.Add(entity1);
                calendarChangeModel.DOCTORS_ID = entity1.DOCTORS_ID;
                calendarChangeModel.DOCTORS_NAME = entity1.DOCTORS_NAME;
                calendarChangeModel.DATE_START = entity1.DATE_START;
                calendarChangeModel.DOCTORS_CHANGE_NAME = entity1.DOCTORS_CHANGE_NAME;
                calendarChangeModel.DOCTORS_CHANGE_ID = entity1.DOCTORS_CHANGE_ID;
                calendarChangeModel.DATE_CHANGE_START = entity1.DATE_CHANGE_START;
                calendarChangeModel.COLUMN_CHANGE_ID = idColumeIsChange;
                calendarChangeModel.TEMPLATE_COLUMN_ID = templateColumnId;
                CALENDAR_CHANGE entity2 = new CALENDAR_CHANGE();
                entity2.CALENDAR_DUTY_ID = calendarDutyId;
                entity2.TEMPLATE_COLUMN_ID = idColumeIsChange;
                entity2.DOCTORS_ID = idDoctorIsChangeX;
                entity2.DATE_START = dateChangeStart;
                entity2.DOCTORS_CHANGE_ID = idDoctorChangeX;
                entity2.DATE_CHANGE_START = dateStart;
                entity2.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                entity2.STATUS = 1;
                entity2.STATUS_APPROVED = 1;
                entity2.DATE_CHANGE = DateTime.Now;
                entity2.DOCTORS_NAME = list.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                entity2.DOCTORS_CHANGE_NAME = list.Where((o => o.DOCTORS_ID == idDoctorChangeX)).ToList()[0].ABBREVIATION;
                entity2.CALENDAR_DELETE = entity1.CALENDAR_CHANGE_ID;
                entity2.COLUMN_CHANGE_ID = templateColumnId;
                unitOfWork.CalendarChanges.Add(entity2);
                WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật thông tin lịch trực đơn vị ", "N/A", "N/A", 0, string.Empty, string.Empty);
            }
            else
            {
                entity1.CALENDAR_DUTY_ID = calendarDutyId;
                entity1.TEMPLATE_COLUMN_ID = templateColumnId;
                entity1.DOCTORS_ID = idDoctorChangeX;
                entity1.DATE_START = dateStart;
                entity1.DOCTORS_CHANGE_ID = idDoctorIsChangeX;
                entity1.DATE_CHANGE_START = null;
                entity1.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                entity1.STATUS = 1;
                entity1.STATUS_APPROVED = 1;
                entity1.DATE_CHANGE = DateTime.Now;
                entity1.DOCTORS_NAME = list.Where((o => o.DOCTORS_ID == idDoctorChangeX)).ToList()[0].ABBREVIATION;
                entity1.DOCTORS_CHANGE_NAME = list.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                entity1.COLUMN_CHANGE_ID = idColumeIsChange;
                unitOfWork.CalendarChanges.Add(entity1);
                WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật thông tin lịch trực đơn vị ", "N/A", "N/A", 0, string.Empty, string.Empty);
                calendarChangeModel.DOCTORS_ID = entity1.DOCTORS_ID;
                calendarChangeModel.DOCTORS_NAME = entity1.DOCTORS_NAME;
                calendarChangeModel.DATE_START = entity1.DATE_START;
                calendarChangeModel.DOCTORS_CHANGE_NAME = entity1.DOCTORS_CHANGE_NAME;
                calendarChangeModel.DOCTORS_CHANGE_ID = entity1.DOCTORS_CHANGE_ID;
                calendarChangeModel.COLUMN_CHANGE_ID = idColumeIsChange;
                calendarChangeModel.TEMPLATE_COLUMN_ID = templateColumnId;
            }
            return Json(calendarChangeModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDuty_Edit", ActionName = "Sửa thông tin lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult EditValuesCalendarDuty(string strValues, string strNew, string strDelete, string strChangeOne, string strChangeTwo, string idCalendarDuty, string listColumnX, int idWeek = -1, int types = 0, int isExist = 0, string inforContent = "")
        {
            if (!Request.IsAjaxRequest())
                return null;
            List<string> list1 = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list1;
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            int calendarDutyId = 0;
            if (!int.TryParse(idCalendarDuty, out calendarDutyId))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(calendarDutyId);
            if (inforContent.Trim() != byId.CALENDAR_NAME)
            {
                byId.CALENDAR_NAME = inforContent.Trim();
                unitOfWork.CalendarDuty.Update(byId);
            }
            List<TimeCalendarDuty> timeCalendarDutyList = new List<TimeCalendarDuty>();
            Random random = new Random();
            int month = byId.CALENDAR_MONTH.GetValueOrDefault();
            int year = byId.CALENDAR_YEAR.GetValueOrDefault();
            if (byId.CALENDAR_YEAR.HasValue && byId.CALENDAR_MONTH.HasValue)
            {
                DateTime dateTime1 = new DateTime(year, month, 1);
                if (idWeek == -1)
                {
                    DateTime dayFirstMonth = dateTime1.AddDays(-dateTime1.Day + 1);
                    DateTime dayLastMonth = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                    for (int index = 0; index < dayLastMonth.Day; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList.Add(timeCalendarDuty);
                    }
                }
                else if (idWeek < 3)
                {
                    int num2 = idWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays(-dateTime1.Day + 1).AddDays(num2);
                    for (int index = 0; index < 7; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList.Add(timeCalendarDuty);
                    }
                }
                else
                {
                    int num2 = DateTime.DaysInMonth(year, month) - 28;
                    int num3 = idWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays(-dateTime1.Day + 1).AddDays(num3);
                    int num4 = num2 + 7;
                    for (int index = 0; index < num4; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList.Add(timeCalendarDuty);
                    }
                }
            }
            if (byId.ISAPPROVED == 1)
            {
                List<string> stringList1 = new List<string>();
                List<string> stringList2 = new List<string>();
                List<string> stringList3 = new List<string>();
                List<string> stringList4 = new List<string>();
                int idDoctor = 0;
                int result4 = 0;
                int idDoctorIsChangeX = 0;
                List<string> list2 = (strNew.Split('-')).ToList();
                List<string> list3 = (strDelete.Split('-')).ToList();
                List<string> list4 = (strChangeOne.Split('-')).ToList();
                List<string> list5 = (strChangeTwo.Split('-')).ToList();
                unitOfWork.CalendarChanges.DeleteCalendarByIDAndDay(calendarDutyId, Convert.ToInt32(byId.CALENDAR_MONTH));
                List<DOCTOR> list6 = unitOfWork.Doctors.GetAll().ToList();
                foreach (string str in list3)
                {
                    char[] chArray = new char[1] { '_' };
                    string[] strArray1 = str.Split(chArray);
                    if ((strArray1).Count<string>() == 2 && int.TryParse(strArray1[1].ToString(), out idDoctor))
                    {
                        string[] strArray2 = strArray1[0].Split(',');
                        CALENDAR_CHANGE entity = new CALENDAR_CHANGE();
                        entity.CALENDAR_DUTY_ID = calendarDutyId;
                        int.TryParse(strArray2[0], out result4);
                        entity.TEMPLATE_COLUMN_ID = result4;
                        DateTime? dateStart;
                        try
                        {
                            dateStart = Utils.Utils.ConvetDateVNToDates(strArray2[1].ToString());
                        }
                        catch
                        {
                            dateStart = null;
                        }
                        entity.DATE_START = dateStart;
                        entity.DOCTORS_ID = idDoctor;
                        entity.DOCTORS_CHANGE_ID = 0;
                        entity.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        entity.STATUS = 2;
                        entity.STATUS_APPROVED = 1;
                        entity.DATE_CHANGE = DateTime.Now;
                        entity.DOCTORS_CHANGE_NAME = string.Empty;
                        entity.DOCTORS_NAME = list6.Where((o => o.DOCTORS_ID == idDoctor)).ToList()[0].ABBREVIATION;
                        unitOfWork.CalendarChanges.Add(entity);
                    }
                }
                foreach (string str in list2)
                {
                    char[] chArray = new char[1] { '_' };
                    string[] strArray1 = str.Split(chArray);
                    if ((strArray1).Count<string>() == 2 && int.TryParse(strArray1[1].ToString(), out idDoctor))
                    {
                        string[] strArray2 = strArray1[0].Split(',');
                        CALENDAR_CHANGE entity = new CALENDAR_CHANGE();
                        entity.CALENDAR_DUTY_ID = calendarDutyId;
                        int.TryParse(strArray2[0], out result4);
                        entity.TEMPLATE_COLUMN_ID = result4;
                        DateTime? dateStart;
                        try
                        {
                            dateStart = Utils.Utils.ConvetDateVNToDates(strArray2[1].ToString());
                        }
                        catch
                        {
                            dateStart = null;
                        }
                        entity.DATE_START = dateStart;
                        entity.DOCTORS_ID = idDoctor;
                        entity.DOCTORS_CHANGE_ID = 0;
                        entity.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        entity.STATUS = 3;
                        entity.STATUS_APPROVED = 1;
                        entity.DATE_CHANGE = DateTime.Now;
                        entity.DOCTORS_CHANGE_NAME = string.Empty;
                        entity.DOCTORS_NAME = list6.Where((o => o.DOCTORS_ID == idDoctor)).ToList()[0].ABBREVIATION;
                        unitOfWork.CalendarChanges.Add(entity);
                    }
                }
                foreach (string str in list4)
                {
                    char[] chArray = new char[1] { '_' };
                    string[] strArray1 = str.Split(chArray);
                    if ((strArray1).Count<string>() == 3 && (int.TryParse(strArray1[1].ToString(), out idDoctor) && int.TryParse(strArray1[2].ToString(), out idDoctorIsChangeX)))
                    {
                        string[] strArray2 = strArray1[0].Split(',');
                        CALENDAR_CHANGE entity = new CALENDAR_CHANGE();
                        entity.CALENDAR_DUTY_ID = calendarDutyId;
                        int.TryParse(strArray2[0], out result4);
                        entity.TEMPLATE_COLUMN_ID = result4;
                        DateTime? dateStart;
                        try
                        {
                            dateStart = Utils.Utils.ConvetDateVNToDates(strArray2[1].ToString());
                        }
                        catch
                        {
                            dateStart = null;
                        }
                        entity.CALENDAR_DUTY_ID = calendarDutyId;
                        entity.DOCTORS_ID = idDoctor;
                        entity.DATE_START = dateStart;
                        entity.DOCTORS_CHANGE_ID = idDoctorIsChangeX;
                        entity.DATE_CHANGE_START = null;
                        entity.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        entity.STATUS = 1;
                        entity.STATUS_APPROVED = 1;
                        entity.DATE_CHANGE = DateTime.Now;
                        entity.DOCTORS_NAME = list6.Where((o => o.DOCTORS_ID == idDoctor)).ToList()[0].ABBREVIATION;
                        entity.DOCTORS_CHANGE_NAME = list6.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                        entity.COLUMN_CHANGE_ID = null;
                        unitOfWork.CalendarChanges.Add(entity);
                    }
                }
                int templateColumnId = 0;
                foreach (string str in list5)
                {
                    char[] chArray = new char[1] { '_' };
                    string[] strArray1 = str.Split(chArray);
                    if ((strArray1).Count<string>() == 5 && (int.TryParse(strArray1[1].ToString(), out idDoctor) && int.TryParse(strArray1[3].ToString(), out idDoctorIsChangeX) && int.TryParse(strArray1[4].ToString(), out templateColumnId)))
                    {
                        string[] strArray2 = strArray1[0].Split(',');
                        CALENDAR_CHANGE calendarChange1 = new CALENDAR_CHANGE();
                        calendarChange1.CALENDAR_DUTY_ID = calendarDutyId;
                        int.TryParse(strArray2[0], out result4);
                        calendarChange1.TEMPLATE_COLUMN_ID = result4;
                        DateTime? dateStart;
                        try
                        {
                            dateStart = Utils.Utils.ConvetDateVNToDates(strArray2[1].ToString());
                        }
                        catch
                        {
                            dateStart = null;
                        }
                        DateTime? dateChangeStart;
                        try
                        {
                            dateChangeStart = Utils.Utils.ConvetDateVNToDates(strArray1[2].ToString());
                        }
                        catch
                        {
                            dateChangeStart = null;
                        }
                        calendarChange1.CALENDAR_DUTY_ID = calendarDutyId;
                        calendarChange1.TEMPLATE_COLUMN_ID = result4;
                        calendarChange1.DOCTORS_ID = idDoctor;
                        calendarChange1.DATE_START = dateStart;
                        calendarChange1.DOCTORS_CHANGE_ID = idDoctorIsChangeX;
                        calendarChange1.DATE_CHANGE_START = dateChangeStart;
                        calendarChange1.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        calendarChange1.STATUS = 1;
                        calendarChange1.STATUS_APPROVED = 1;
                        calendarChange1.DATE_CHANGE = DateTime.Now;
                        calendarChange1.DOCTORS_NAME = list6.Where((o => o.DOCTORS_ID == idDoctor)).ToList()[0].ABBREVIATION;
                        calendarChange1.DOCTORS_CHANGE_NAME = list6.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                        calendarChange1.CALENDAR_DELETE = 1;
                        calendarChange1.COLUMN_CHANGE_ID = templateColumnId;
                        if (unitOfWork.CalendarChanges.CheckChangeCaledar(calendarChange1, 1) == 0)
                            unitOfWork.CalendarChanges.Add(calendarChange1);
                        CALENDAR_CHANGE calendarChange2 = new CALENDAR_CHANGE();
                        calendarChange2.CALENDAR_DUTY_ID = calendarDutyId;
                        calendarChange2.TEMPLATE_COLUMN_ID = templateColumnId;
                        calendarChange2.DOCTORS_ID = idDoctorIsChangeX;
                        calendarChange2.DATE_START = dateChangeStart;
                        calendarChange2.DOCTORS_CHANGE_ID = idDoctor;
                        calendarChange2.DATE_CHANGE_START = dateStart;
                        calendarChange2.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        calendarChange2.STATUS = 1;
                        calendarChange2.STATUS_APPROVED = 1;
                        calendarChange2.DATE_CHANGE = DateTime.Now;
                        calendarChange2.DOCTORS_NAME = list6.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                        calendarChange2.DOCTORS_CHANGE_NAME = list6.Where((o => o.DOCTORS_ID == idDoctor)).ToList()[0].ABBREVIATION;
                        calendarChange2.CALENDAR_DELETE = calendarChange1.CALENDAR_CHANGE_ID;
                        calendarChange2.COLUMN_CHANGE_ID = result4;
                        if (unitOfWork.CalendarChanges.CheckChangeCaledar(calendarChange2, 1) == 0)
                            unitOfWork.CalendarChanges.Add(calendarChange2);
                    }
                }
                WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật thông tin lịch trực đơn vị " + byId.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
                ViewBag.objCalendarDuty = byId;
                List<DoctorCalendar> doctorCalendar = unitOfWork.CalendarDuty.GetDoctorCalendar(calendarDutyId);
                ViewBag.idDepartment = byUserName.LM_DEPARTMENT_ID;
                ViewBag.types = types;
                ViewBag.doctors = doctorCalendar;
                ViewBag.idCalendarDuty = idCalendarDuty;
                ViewBag.idWeek = idWeek;
                ViewBag.times = timeCalendarDutyList;
                if (isExist == 1)
                    return PartialView("_IsExistCalendar");
                return PartialView("_EditCalendarDuty");
            }
            if (byId.CALENDAR_STATUS.HasValue && byId.CALENDAR_STATUS.Value != 1)
                return null;
            int columId = 0;
            int doctorsId = 0;
            if (byId == null)
                return Json(Localization.UpdateCalendarIsNotPermission.ToString(), JsonRequestBehavior.AllowGet);
            string[] strArray3 = listColumnX.Split('_');
            int result8 = 0;
            foreach (object obj2 in strArray3)
            {
                if (int.TryParse(obj2.ToString(), out result8))
                {
                    if (idWeek == -1)
                    {
                        unitOfWork.CalendarDoctors.DeleteByIDCalendarDuty(calendarDutyId, result8);
                        unitOfWork.CalendarDatas.DeleteByIDCalendarDuty(calendarDutyId, result8);
                    }
                    else
                    {
                        unitOfWork.CalendarDoctors.DeleteByIDCalendarDuty(calendarDutyId, result8, timeCalendarDutyList[0].DATES, timeCalendarDutyList[timeCalendarDutyList.Count - 1].DATES);
                        unitOfWork.CalendarDatas.DeleteByIDCalendarDuty(calendarDutyId, result8, timeCalendarDutyList[0].DATES, timeCalendarDutyList[timeCalendarDutyList.Count - 1].DATES);
                    }
                }
            }
            string[] strArray4 = strValues.Split('-');
            foreach (string str in strArray4)
            {
                char[] chArray = new char[1] { '_' };
                string[] strArray1 = str.Split(chArray);
                if ((strArray1).Count<string>() == 2)
                {
                    int.TryParse(strArray1[1], out doctorsId);
                    string[] strArray2 = strArray1[0].Split(',');
                    int.TryParse(strArray2[0], out columId);
                    DateTime? dateStart;
                    try
                    {
                        dateStart = Utils.Utils.ConvetDateVNToDates(strArray2[1].ToString());
                    }
                    catch
                    {
                        dateStart = null;
                    }
                    CALENDAR_DATA entity = new CALENDAR_DATA();
                    entity.CALENDAR_DUTY_ID = byId.CALENDAR_DUTY_ID;
                    entity.TEMPLATE_COLUM_ID = columId;
                    entity.DATE_START = dateStart;
                    entity.ISDELETE = false;
                    unitOfWork.CalendarDatas.Add(entity);
                    unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                    {
                        CALENDAR_DATA_ID = entity.CALENDAR_DATA_ID,
                        DOCTORS_ID = doctorsId,
                        CALENDAR_DUTY_ID = entity.CALENDAR_DUTY_ID,
                        COLUMN_LEVEL_ID = columId
                    });
                }
            }
            WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật thông tin lịch trực đơn vị " + byId.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
            List<DoctorCalendar> doctorCalendar1 = unitOfWork.CalendarDuty.GetDoctorCalendar(calendarDutyId);
            ViewBag.objCalendarDuty = byId;
            ViewBag.idDepartment = byUserName.LM_DEPARTMENT_ID;
            ViewBag.types = types;
            ViewBag.doctors = doctorCalendar1;
            ViewBag.idCalendarDuty = idCalendarDuty;
            ViewBag.idWeek = idWeek;
            ViewBag.times = timeCalendarDutyList;
            if (isExist == 0)
                return PartialView("_EditCalendarDuty");
            return PartialView("_IsExistCalendar");
        }

        [HttpGet]
        public PartialViewResult ListDateChange(string idDoctorChange, string idDoctorIsChange, string idCalendarDuty)
        {
            if (idDoctorChange != null && idDoctorIsChange != null && idCalendarDuty != null)
            {
                int idDoctor = 0;
                int idDoctorIs = 0;
                int idCalendar = 0;
                if (int.TryParse(idDoctorChange, out idDoctor) && int.TryParse(idDoctorIsChange, out idDoctorIs) && int.TryParse(idCalendarDuty, out idCalendar))
                {
                    List<DocTorDate> allDayByDoctor = unitOfWork.Doctors.GetAllDayByDoctor(idDoctorIs, idCalendar);
                    ViewBag.Dates = allDayByDoctor;
                    return PartialView("_ListDateChange");
                }
            }
            return null;
        }

        [HttpGet]
        public PartialViewResult ListDoctors(string arrayid, string arrayView, string idColumn, string idTemplate)
        {
            if (!Request.IsAjaxRequest())
                return null;
            string[] strArray1 = arrayid.Split('_');
            string[] strArray2 = arrayView.Split('_');
            ViewBag.arrayid = strArray1;
            ViewBag.arrayView = strArray2;
            int result1 = 0;
            string[] strArray3 = idColumn.Split(',');
            int.TryParse((strArray3).ToArray<string>()[0].ToString(), out result1);
            DateTime? date;
            try
            {
                date = Utils.Utils.ConvetDateVNToDates((strArray3).ToArray<string>()[1].ToString());
            }
            catch
            {
                date = null;
            }
            ViewBag.DateX = date;
            TEMPLATE_COLUM templateColum = new TEMPLATE_COLUM();
            List<DOCTOR> allByDepartmentId = unitOfWork.Doctors.GetAllByDepartmentId(Convert.ToInt32(unitOfWork.TemplatesColumn.GetById(result1).LM_DEPARTMENT_ID));
            List<DoctorColumn> byTemplateColumn = unitOfWork.Doctors.GetAllByTemplateColumn(result1);
            ViewBag.allDoctor = byTemplateColumn;
            int result2 = 0;
            int.TryParse(idTemplate, out result2);
            List<DoctorInCalendar> allDoctor = unitOfWork.Doctors.GetAllDoctor(date, result2);
            ViewBag.allDoctorCalendar = allDoctor;
            return PartialView("_ListDoctors", allByDepartmentId);
        }

        [HttpGet]
        public PartialViewResult ListDoctorsChanges(string DoctorName, int idDoctorChange, int idColumn, int idCalendarDuty, string dateDoctorChange, int idWeek = -1, int isExist = 0, string arrayDoctorIsExit = "")
        {
            if (!Request.IsAjaxRequest())
                return null;
            TEMPLATE_COLUM templateColum = new TEMPLATE_COLUM();
            List<DOCTOR> allByDepartmentId = unitOfWork.Doctors.GetAllByDepartmentId(Convert.ToInt32(unitOfWork.TemplatesColumn.GetById(idColumn).LM_DEPARTMENT_ID));
            ViewBag.idDoctorChange = idDoctorChange;
            ViewBag.idCalendarDuty = idCalendarDuty;
            List<DoctorColumn> byTemplateColumn = unitOfWork.Doctors.GetAllByTemplateColumn(idColumn);
            ViewBag.allDoctor = byTemplateColumn;
            ViewBag.dateDoctorChange = dateDoctorChange;
            ViewBag.idColumn = idColumn;
            ViewBag.idWeek = idWeek;
            ViewBag.isExist = isExist;
            ViewBag.arrayid = arrayDoctorIsExit;
            ViewBag.DoctorName = DoctorName;
            return PartialView("_ListDoctorsChange", allByDepartmentId);
        }

        [HttpGet]
        public PartialViewResult GetInforDoctor(string idDoctor)
        {
            int id;
            try
            {
                id = Convert.ToInt32(idDoctor);
            }
            catch
            {
                id = 0;
            }
            return PartialView("ViewDoctor", unitOfWork.Doctors.GetById(id));
        }

        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpPost]
        [CustomAuthorize]
        [ValidateInput(false)]
        public PartialViewResult CalendarLists(SearchCalendarDuty calendarSearch, int types)
        {
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            int page = 0;
            Pagination pagination = new Pagination()
            {
                ActionName = "List",
                ModelName = "CalendarDuty",
                CurrentPage = page,
                TagetReLoadId = "cat_list_render"
            };
            string sort = "CALENDAR_NAME";
            string sortDir = "ASC";
            int totalRow = 0;
            if (calendarSearch.DATE_CREATES != null)
                calendarSearch.DATE_CREATE = Utils.Utils.ConvetDateVNToDate(calendarSearch.DATE_CREATES.ToString());
            if (calendarSearch.DATE_APPROVEDS != null)
                calendarSearch.DATE_APPROVED = Utils.Utils.ConvetDateVNToDate(calendarSearch.DATE_APPROVEDS.ToString());
            if (!string.IsNullOrEmpty(calendarSearch.ADMIN_USER_APPROVED))
                calendarSearch.ADMIN_USER_APPROVED = calendarSearch.ADMIN_USER_APPROVED.Trim();
            if (!string.IsNullOrEmpty(calendarSearch.ADMIN_USER_CREATE))
                calendarSearch.ADMIN_USER_CREATE = calendarSearch.ADMIN_USER_CREATE.Trim();
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            PagedList<CALENDAR_DUTY> all = unitOfWork.CalendarDuty.GetAll(calendarSearch, page, pagination.PageSize, sort, sortDir, types, byUserName.LM_DEPARTMENT_ID.ToString(), out totalRow);
            pagination.TotalRows = totalRow;
            pagination.CurrentRow = all.Count;
            ViewBag.calendarDuty = all;
            ViewBag.Pagination = pagination;
            ViewBag.dateCreate = calendarSearch.DATE_CREATES;
            ViewBag.dateApproved = calendarSearch.DATE_APPROVEDS;
            ViewBag.usernameCreate = calendarSearch.ADMIN_USER_CREATE;
            ViewBag.usernameAppoved = calendarSearch.ADMIN_USER_APPROVED;
            ViewBag.calendarStatus = calendarSearch.CALENDAR_STATUS;
            ViewBag.dateMonth = calendarSearch.DATE_MONTH;
            ViewBag.dateYear = calendarSearch.DATE_YEAR;
            ViewBag.feast = calendarSearch.FEAST;
            ViewBag.departmentName = calendarSearch.DEPARTMENTS;
            ViewBag.types = types;
            return PartialView("_CalendarList", all);
        }

        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDuty_Insert", ActionName = "Lập lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpGet]
        [CustomAuthorize]
        public PartialViewResult LoadTemplate(string strDate, string idTemplate)
        {
            int result1 = 0;
            int.TryParse(idTemplate, out result1);
            ADMIN_USER byUserName = new UnitOfWork().Users.GetByUserName(User.Identity.Name);
            if (result1 == 0)
            {
                DateTime dateTime = Utils.Utils.GetDateTime();
                int month = dateTime.Month;
                int year = dateTime.Year;
                if (strDate != null)
                {
                    string[] strArray = strDate.Split('_');
                    if ((strArray).Count<string>() > 2 && (int.TryParse(strArray[1].ToString(), out month) && int.TryParse(strArray[2].ToString(), out year)))
                        dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(month), Convert.ToString(year));
                }
                int hashCode1 = DutyType.Deparment.GetHashCode();
                int? lmDepartmentId = 0;
                try
                {
                    lmDepartmentId = byUserName.LM_DEPARTMENT_ID;
                }
                catch
                {
                    lmDepartmentId = 0;
                }
                List<DoctorCalendarLeader> doctorCalendarDefault = unitOfWork.CalendarDuty.GetDoctorCalendarDefault(month, year, hashCode1, lmDepartmentId);
                ViewBag.times = dateTime;
                ViewBag.doctors = doctorCalendarDefault;
                List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
                ViewBag.ActionPermission = list;
                List<CALENDAR_DUTY> calendarByDeparment = unitOfWork.CalendarDuty.GetCalendarByDeparment(month, year, hashCode1, lmDepartmentId);
                if (calendarByDeparment.Count == 0)
                    return PartialView("_AddCalendarDefault");
                int? calendarStatus = 0;
                int? isApproved = 0;
                if (calendarByDeparment != null && calendarByDeparment.Count > 0)
                {
                    calendarStatus = calendarByDeparment[0].CALENDAR_STATUS;
                    isApproved = new int?(calendarByDeparment[0].ISAPPROVED);
                }
                int statusCreated = CalendarDutyStatus.Created.GetHashCode();
                if (calendarStatus.HasValue && calendarStatus != statusCreated && isApproved.HasValue && isApproved != 0)
                {
                    ViewBag.objCalendarDuty = calendarByDeparment;
                    return PartialView("_AddCalendarDefault");
                }
                CALENDAR_DUTY calendarDuty = null;
                if (calendarByDeparment != null && calendarByDeparment.Count > 0)
                    calendarDuty = calendarByDeparment[0];
                ViewBag.objCalendarDuty = calendarDuty;
                ViewBag.typeEdit = 0;
                return PartialView("_EditCalendarDefault");
            }
            List<string> list1 = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list1;
            ViewBag.idDepartment = byUserName.LM_DEPARTMENT_ID;
            List<TimeCalendarDuty> timeCalendarDutyList1 = new List<TimeCalendarDuty>();
            Random random = new Random();
            DateTime now = DateTime.Now;
            int idWeek = 0;
            int calendarMonth = 0;
            int calendarYear = 0;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                if (int.TryParse(strArray[0].ToString(), out idWeek) && int.TryParse(strArray[1].ToString(), out calendarMonth) && int.TryParse(strArray[2].ToString(), out calendarYear))
                {
                    int num1 = unitOfWork.CalendarDuty.CheckCalendarDuty(calendarMonth, calendarYear, result1, Convert.ToInt32(byUserName.LM_DEPARTMENT_ID));
                    if (num1 != 0)
                    {
                        CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
                        CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(num1);
                        List<DoctorCalendar> doctorCalendar = unitOfWork.CalendarDuty.GetDoctorCalendar(num1);
                        ViewBag.objCalendarDuty = byId;
                        List<TimeCalendarDuty> timeCalendarDutyList2 = new List<TimeCalendarDuty>();
                        random = new Random();
                        if (int.TryParse(byId.CALENDAR_MONTH.ToString(), out calendarMonth) && int.TryParse(byId.CALENDAR_YEAR.ToString(), out calendarYear))
                        {
                            DateTime dateTime1 = Convert.ToDateTime("1/" + calendarMonth + "/" + calendarYear);
                            if (idWeek == -1)
                            {
                                DateTime dayFirstMonth = dateTime1.AddDays((-dateTime1.Day + 1));
                                DateTime dayLastMonth = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                                for (int index = 0; index < dayLastMonth.Day; ++index)
                                {
                                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                                    timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                                    timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                                    timeCalendarDutyList2.Add(timeCalendarDuty);
                                }
                            }
                            else if (idWeek < 3)
                            {
                                int num2 = idWeek * 7;
                                DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                                for (int index = 0; index < 7; ++index)
                                {
                                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                                    timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                                    timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                                    timeCalendarDutyList2.Add(timeCalendarDuty);
                                }
                            }
                            else
                            {
                                int num2 = DateTime.DaysInMonth(calendarYear, calendarMonth) - 28;
                                int num3 = idWeek * 7;
                                DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num3);
                                int num4 = num2 + 7;
                                for (int index = 0; index < num4; ++index)
                                {
                                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                                    timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                                    timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                                    timeCalendarDutyList2.Add(timeCalendarDuty);
                                }
                            }
                        }
                        ViewBag.doctors = doctorCalendar;
                        ViewBag.idWeek = idWeek;
                        ViewBag.times = timeCalendarDutyList2;
                        ViewBag.types = 3;
                        ViewBag.idCalendarDuty = num1;
                        return PartialView("_IsExistCalendar");
                    }
                    DateTime dateTime4 = Convert.ToDateTime("1/" + calendarMonth + "/" + calendarYear);
                    if (idWeek == -1)
                    {
                        DateTime dayFirstMonth = dateTime4.AddDays((-dateTime4.Day + 1));
                        DateTime dayLastYear = dateTime4.AddMonths(1).AddDays(-dateTime4.Day);
                        for (int index = 0; index < dayLastYear.Day; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                            timeCalendarDutyList1.Add(timeCalendarDuty);
                        }
                    }
                    else if (idWeek < 3)
                    {
                        int num2 = idWeek * 7;
                        DateTime dayFirstWeek = dateTime4.AddDays((-dateTime4.Day + 1)).AddDays(num2);
                        for (int index = 0; index < 7; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                            timeCalendarDutyList1.Add(timeCalendarDuty);
                        }
                    }
                    else
                    {
                        int num2 = DateTime.DaysInMonth(calendarYear, calendarMonth) - 28;
                        int num3 = idWeek * 7;
                        DateTime dayFirstWeek = dateTime4.AddDays((-dateTime4.Day + 1)).AddDays(num3);
                        int num4 = num2 + 7;
                        for (int index = 0; index < num4; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                            timeCalendarDutyList1.Add(timeCalendarDuty);
                        }
                    }
                }
            }
            else
            {
                DateTime dateTime1 = Convert.ToDateTime("1/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                DateTime dayFirstMonth = dateTime1.AddDays((-dateTime1.Day + 1));
                DateTime dayLastMonth = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                for (int index = 0; index < dayLastMonth.Day; ++index)
                {
                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                    timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                    timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                    timeCalendarDutyList1.Add(timeCalendarDuty);
                }
            }
            ViewBag.times = timeCalendarDutyList1;
            ViewBag.idTemplate = idTemplate;
            return PartialView("_AddCalendarDutyData");
        }

        [ValidateJsonAntiForgeryToken]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDuty_Insert", ActionName = "Lập lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpGet]
        public ActionResult AddCalendarDuty()
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            ViewBag.idDepartment = byUserName.LM_DEPARTMENT_ID;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            ViewBag.months = DateTime.Now.Month;
            ViewBag.years = DateTime.Now.Year;
            return View("_AddCalendarDuty");
        }

        public ActionResult LoadDays(int Months, int Years)
        {
            ViewBag.months = Months;
            ViewBag.years = Years;
            return PartialView("_ListWeeks");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LoadDay(int Months, int Years, int currentWeek)
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            List<string> stringList = new List<string>();
            int num = DateTime.DaysInMonth(Years, Months);
            stringList.Add("1-7/" + Months);
            stringList.Add("8-14/" + Months);
            stringList.Add("15-21/" + Months);
            stringList.Add("22-" + num.ToString() + "/" + Months);
            for (int index = 0; index <= 3; ++index)
                selectListItemList.Add(new SelectListItem()
                {
                    Text = "Tuần " + (index + 1).ToString() + "(" + stringList[index] + ")",
                    Value = index.ToString(),
                    Selected = index == currentWeek
                });
            return Json(selectListItemList, JsonRequestBehavior.AllowGet);
        }

        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        public PartialViewResult ViewCalendarDuty(string idCalendarDuty, int idWeek = -1, int types = 0, int isExist = 0)
        {
            if (!Request.IsAjaxRequest())
                return null;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            Random random;
            if (types == 0)
            {
                int result1 = 0;
                List<DoctorCalendar> doctorCalendarList = null;
                CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
                if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result1))
                {
                    calendarDuty = unitOfWork.CalendarDuty.GetById(result1);
                    doctorCalendarList = unitOfWork.CalendarDuty.GetDoctorCalendar(result1);
                }
                List<TimeCalendarDuty> timeCalendarDutyList = new List<TimeCalendarDuty>();
                random = new Random();
                if (calendarDuty.CALENDAR_MONTH.GetValueOrDefault() > 0 && calendarDuty.CALENDAR_YEAR.GetValueOrDefault() > 0)
                {
                    DateTime dateTime1 = Convert.ToDateTime("1/" + calendarDuty.CALENDAR_MONTH.Value + "/" + calendarDuty.CALENDAR_YEAR.Value);
                    if (idWeek == -1)
                    {
                        DateTime dayFirstMonth = dateTime1.AddDays((-dateTime1.Day + 1));
                        DateTime dayLastMont = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                        for (int index = 0; index < dayLastMont.Day; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                            timeCalendarDutyList.Add(timeCalendarDuty);
                        }
                    }
                    else if (idWeek < 3)
                    {
                        int num2 = idWeek * 7;
                        DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                        for (int index = 0; index < 7; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                            timeCalendarDutyList.Add(timeCalendarDuty);
                        }
                    }
                    else
                    {
                        int num2 = DateTime.DaysInMonth(calendarDuty.CALENDAR_YEAR.Value, calendarDuty.CALENDAR_MONTH.Value) - 28;
                        int num3 = idWeek * 7;
                        DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num3);
                        int num4 = num2 + 7;
                        for (int index = 0; index < num4; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                            timeCalendarDutyList.Add(timeCalendarDuty);
                        }
                    }
                }
                ViewBag.objCalendarDuty = calendarDuty;
                ViewBag.doctors = doctorCalendarList;
                ViewBag.idCalendarDuty = idCalendarDuty;
                ViewBag.idWeek = idWeek;
                ViewBag.times = timeCalendarDutyList;
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                ViewBag.idDepartment = byUserName.LM_DEPARTMENT_ID;
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem thông tin lịch trực đơn vị", "N/A", "N/A", 0, string.Empty, string.Empty);
                return PartialView("_ViewCalenDarDuty");
            }
            int idCalendar = 0;
            List<DoctorCalendar> doctorCalendarList1 = null;
            CALENDAR_DUTY calendarDuty1 = new CALENDAR_DUTY();
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out idCalendar))
            {
                calendarDuty1 = unitOfWork.CalendarDuty.GetById(idCalendar);
                doctorCalendarList1 = unitOfWork.CalendarDuty.GetDoctorCalendar(idCalendar);
            }
            ViewBag.objCalendarDuty = calendarDuty1;
            List<TimeCalendarDuty> timeCalendarDutyList1 = new List<TimeCalendarDuty>();
            random = new Random();
            int result5 = 0;
            int result6 = 0;
            int? nullable1 = calendarDuty1.CALENDAR_MONTH;
            int num5;
            if (int.TryParse(nullable1.ToString(), out result5))
            {
                nullable1 = calendarDuty1.CALENDAR_YEAR;
                num5 = !int.TryParse(nullable1.ToString(), out result6) ? 1 : 0;
            }
            else
                num5 = 1;
            if (calendarDuty1.CALENDAR_MONTH.GetValueOrDefault() > 0 && calendarDuty1.CALENDAR_YEAR.GetValueOrDefault() > 0)
            {
                DateTime dateTime1 = Convert.ToDateTime("1/" + calendarDuty1.CALENDAR_MONTH.Value + "/" + calendarDuty1.CALENDAR_YEAR.Value);
                if (idWeek == -1)
                {
                    DateTime dayFirstMonth = dateTime1.AddDays((-dateTime1.Day + 1));
                    DateTime dayLastMont = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                    for (int index = 0; index < dayLastMont.Day; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList1.Add(timeCalendarDuty);
                    }
                }
                else if (idWeek < 3)
                {
                    int num1 = idWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num1);
                    for (int index = 0; index < 7; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList1.Add(timeCalendarDuty);
                    }
                }
                else
                {
                    int num1 = DateTime.DaysInMonth(result6, result5) - 28;
                    int num2 = idWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                    int num3 = num1 + 7;
                    for (int index = 0; index < num3; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList1.Add(timeCalendarDuty);
                    }
                }
            }
            ViewBag.doctors = doctorCalendarList1;
            ViewBag.idCalendarDuty = idCalendarDuty;
            ViewBag.idWeek = idWeek;
            ViewBag.times = timeCalendarDutyList1;
            ADMIN_USER byUserName1 = unitOfWork.Users.GetByUserName(User.Identity.Name);
            ViewBag.idDepartment = byUserName1.LM_DEPARTMENT_ID;
            ViewBag.types = types;
            WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật thông tin lịch trực đơn vị " + calendarDuty1.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
            if (isExist == 0)
                return PartialView("_EditCalendarDuty");
            return PartialView("_IsExistCalendar");
        }

        [ValidateJsonAntiForgeryToken]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDuty_Delete", ActionName = "Xóa thông tin lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            if (id <= 0)
                return Json(JsonResponse.Json500("Thao tác không hợp lệ!"));
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(id);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.ISDELETE = true;
            unitOfWork.CalendarDuty.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Update, "Xóa thông tin lịch trực đơn vị", "N/A", "N/A", 0, string.Empty, string.Empty);
            return Json(JsonResponse.Json200(Localization.MsgDelSuccess));
        }

        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDuty_Send", ActionName = "Gửi duyệt lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpGet]
        public ActionResult SendApproved(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            int result1 = 0;
            int result2 = 0;
            if (!int.TryParse(idCalendarDuty, out result1) || !int.TryParse(types, out result2))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result1);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 2;
            byId.USER_CREATE_ID = UserX.ADMIN_USER_ID;
            byId.DATE_CREATE = DateTime.Now;
            unitOfWork.CalendarDuty.Update(byId);
            unitOfWork.CalendarDuty.Save();
            ViewBag.objCalendarDuty = byId;
            WriteLog(enLogType.NomalLog, enActionType.SendApproved, "Gửi duyệt lịch trực đơn vị " + byId.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
            int result3 = 0;
            CALENDAR_DUTY calendarDuty1 = unitOfWork.CalendarDuty.CheckCalendarHospital(Convert.ToInt32(byId.CALENDAR_MONTH), Convert.ToInt32(byId.CALENDAR_YEAR), 4);
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            if (calendarDuty1 == null)
            {
                CALENDAR_DUTY entity = new CALENDAR_DUTY();
                entity.CALENDAR_NAME = "Lịch trực toàn bệnh viện tháng " + byId.CALENDAR_MONTH + " năm " + byId.CALENDAR_YEAR;
                entity.CALENDAR_MONTH = byId.CALENDAR_MONTH;
                entity.CALENDAR_YEAR = byId.CALENDAR_YEAR;
                entity.CALENDAR_STATUS = 1;
                entity.LM_DEPARTMENT_ID = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
                entity.DUTY_TYPE = 4;
                entity.ISDELETE = false;
                entity.USER_CREATE_ID = null;
                entity.LM_DEPARTMENT_PARTS = null;
                unitOfWork.CalendarDuty.Add(entity);
                foreach (CALENDAR_DUTY calendarDuty2 in unitOfWork.CalendarDuty.GetByApproved(Convert.ToInt32(byId.CALENDAR_MONTH), Convert.ToInt32(byId.CALENDAR_YEAR), 3, 0))
                    unitOfWork.CalendarGroups.Add(new CALENDAR_GROUP()
                    {
                        CALENDAR_ID = entity.CALENDAR_DUTY_ID,
                        CALENDAR_PARENT_ID = calendarDuty2.CALENDAR_DUTY_ID,
                        CALENDAR_MONTH = entity.CALENDAR_MONTH,
                        CALENDAR_YEAR = entity.CALENDAR_YEAR,
                        LM_DEPARTMENT_ID = entity.LM_DEPARTMENT_ID,
                        CALENDAR_TYPE = result2,
                        CALENDAR_STATUS = 0
                    });
            }
            else if (unitOfWork.CalendarGroups.CheckIsExist(calendarDuty1.CALENDAR_DUTY_ID, result1, Convert.ToInt32(byId.CALENDAR_MONTH), Convert.ToInt32(byId.CALENDAR_YEAR)) == null)
                unitOfWork.CalendarGroups.Add(new CALENDAR_GROUP()
                {
                    CALENDAR_ID = calendarDuty1.CALENDAR_DUTY_ID,
                    CALENDAR_PARENT_ID = result1,
                    CALENDAR_MONTH = calendarDuty1.CALENDAR_MONTH,
                    CALENDAR_YEAR = calendarDuty1.CALENDAR_YEAR,
                    LM_DEPARTMENT_ID = new int?(Convert.ToInt32(byUserName.LM_DEPARTMENT_ID)),
                    CALENDAR_TYPE = result2,
                    CALENDAR_STATUS = 0
                });
            if (byId.ISAPPROVED == 1)
                unitOfWork.CalendarChanges.UpdateStatus(result1, 2);
            if (int.TryParse(types, out result3))
            {
                List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
                ViewBag.ActionPermission = list;
                if (result3 == 1)
                    return PartialView("_ListButtons");
                if (result3 == 2)
                    return PartialView("_ListButtonView");
            }
            return null;
        }

        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDuty_Approved", ActionName = "Duyệt lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult ApprovedCalendarDuty(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            int result1 = 0;
            DoctorLevelView doctorLevelView = new DoctorLevelView();
            DoctorData doctorData1 = new DoctorData();
            CALENDAR_DOCTOR calendarDoctor = new CALENDAR_DOCTOR();
            CALENDAR_DATA calendarData = new CALENDAR_DATA();
            if (int.TryParse(idCalendarDuty, out result1))
            {
                CALENDAR_DUTY byId1 = unitOfWork.CalendarDuty.GetById(result1);
                if (byId1 == null)
                    return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
                if (byId1.ISAPPROVED == 1)
                {
                    foreach (CALENDAR_CHANGE entity1 in unitOfWork.CalendarChanges.GetListByIdCalendar(result1, 2))
                    {
                        DoctorLevelView doctorLevelByIdDoctor = unitOfWork.DoctorLevels.GetDoctorLevelByIdDoctor(Convert.ToInt32(entity1.DOCTORS_ID));
                        bool flag = unitOfWork.CalendarGroups.CheckIsCalendarApproved(result1);
                        int hashCode1 = CalendarChangeApproved.Approved.GetHashCode();
                        int? nullable;
                        if (flag)
                        {
                            nullable = doctorLevelByIdDoctor.LEVEL_NUMBER;
                            if ((nullable.GetValueOrDefault() <= 2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                            {
                                DoctorData doctorData2 = unitOfWork.DoctorDatas.CheckDoctorData(Convert.ToInt32(entity1.CALENDAR_DUTY_ID), Convert.ToInt32(entity1.DOCTORS_ID), Convert.ToInt32(entity1.TEMPLATE_COLUMN_ID), Convert.ToDateTime(entity1.DATE_START));
                                if (doctorData2 != null)
                                {
                                    nullable = entity1.STATUS;
                                    int hashCode2 = CalendarChangeType.Change.GetHashCode();
                                    if ((nullable.GetValueOrDefault() != hashCode2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                    {
                                        CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(doctorData2.CALENDAR_DOCTOR_ID);
                                        byId2.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_CHANGE_ID);
                                        unitOfWork.CalendarDoctors.Update(byId2);
                                        entity1.STATUS_APPROVED = hashCode1;
                                        unitOfWork.CalendarChanges.Update(entity1);
                                        unitOfWork.Doctors.Save();
                                    }
                                    nullable = entity1.STATUS;
                                    int hashCode3 = CalendarChangeType.Deleted.GetHashCode();
                                    if ((nullable.GetValueOrDefault() != hashCode3 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                    {
                                        unitOfWork.CalendarDoctors.Delete(unitOfWork.CalendarDoctors.GetById(doctorData2.CALENDAR_DOCTOR_ID));
                                        unitOfWork.CalendarDatas.Delete(unitOfWork.CalendarDatas.GetById(doctorData2.CALENDAR_DATA_ID));
                                        entity1.STATUS_APPROVED = hashCode1;
                                        unitOfWork.CalendarChanges.Update(entity1);
                                        unitOfWork.Doctors.Save();
                                    }
                                }
                                else
                                {
                                    nullable = entity1.STATUS;
                                    int hashCode2 = CalendarChangeType.Add.GetHashCode();
                                    if ((nullable.GetValueOrDefault() != hashCode2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                    {
                                        nullable = doctorLevelByIdDoctor.LEVEL_NUMBER;
                                        if ((nullable.GetValueOrDefault() <= 2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                        {
                                            CALENDAR_DATA entity2 = new CALENDAR_DATA();
                                            entity2.CALENDAR_DUTY_ID = result1;
                                            entity2.TEMPLATE_COLUM_ID = entity1.TEMPLATE_COLUMN_ID;
                                            entity2.DATE_START = entity1.DATE_START;
                                            entity2.ISDELETE = false;
                                            unitOfWork.CalendarDatas.Add(entity2);
                                            unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                                            {
                                                DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_ID),
                                                CALENDAR_DUTY_ID = result1,
                                                COLUMN_LEVEL_ID = new int?(Convert.ToInt32(entity1.TEMPLATE_COLUMN_ID)),
                                                CALENDAR_DATA_ID = entity2.CALENDAR_DATA_ID
                                            });
                                            entity1.STATUS_APPROVED = hashCode1;
                                            unitOfWork.CalendarChanges.Update(entity1);
                                            unitOfWork.Doctors.Save();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            DoctorData doctorData2 = unitOfWork.DoctorDatas.CheckDoctorData(Convert.ToInt32(entity1.CALENDAR_DUTY_ID), Convert.ToInt32(entity1.DOCTORS_ID), Convert.ToInt32(entity1.TEMPLATE_COLUMN_ID), Convert.ToDateTime(entity1.DATE_START));
                            if (doctorData2 != null)
                            {
                                nullable = entity1.STATUS;
                                int hashCode2 = CalendarChangeType.Change.GetHashCode();
                                if ((nullable.GetValueOrDefault() != hashCode2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                {
                                    CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(doctorData2.CALENDAR_DOCTOR_ID);
                                    byId2.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_CHANGE_ID);
                                    unitOfWork.CalendarDoctors.Update(byId2);
                                    entity1.STATUS_APPROVED = hashCode1;
                                    unitOfWork.CalendarChanges.Update(entity1);
                                    unitOfWork.Doctors.Save();
                                }
                                nullable = entity1.STATUS;
                                int hashCode3 = CalendarChangeType.Deleted.GetHashCode();
                                if ((nullable.GetValueOrDefault() != hashCode3 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                {
                                    unitOfWork.CalendarDoctors.Delete(unitOfWork.CalendarDoctors.GetById(doctorData2.CALENDAR_DOCTOR_ID));
                                    unitOfWork.CalendarDatas.Delete(unitOfWork.CalendarDatas.GetById(doctorData2.CALENDAR_DATA_ID));
                                    entity1.STATUS_APPROVED = hashCode1;
                                    unitOfWork.CalendarChanges.Update(entity1);
                                    unitOfWork.Doctors.Save();
                                }
                            }
                            else
                            {
                                nullable = entity1.STATUS;
                                int hashCode2 = CalendarChangeType.Add.GetHashCode();
                                if ((nullable.GetValueOrDefault() != hashCode2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                                {
                                    CALENDAR_DATA entity2 = new CALENDAR_DATA();
                                    entity2.CALENDAR_DUTY_ID = result1;
                                    entity2.TEMPLATE_COLUM_ID = entity1.TEMPLATE_COLUMN_ID;
                                    entity2.DATE_START = entity1.DATE_START;
                                    entity2.ISDELETE = false;
                                    unitOfWork.CalendarDatas.Add(entity2);
                                    unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                                    {
                                        DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_ID),
                                        CALENDAR_DUTY_ID = result1,
                                        COLUMN_LEVEL_ID = new int?(Convert.ToInt32(entity1.TEMPLATE_COLUMN_ID)),
                                        CALENDAR_DATA_ID = entity2.CALENDAR_DATA_ID
                                    });
                                    entity1.STATUS_APPROVED = hashCode1;
                                    unitOfWork.CalendarChanges.Update(entity1);
                                    unitOfWork.Doctors.Save();
                                }
                            }
                        }
                    }
                    byId1.CALENDAR_STATUS = 3;
                    byId1.USER_APPROVED_ID = UserX.ADMIN_USER_ID;
                    byId1.DATE_APPROVED = DateTime.Now;
                    byId1.ISAPPROVED = 1;
                    unitOfWork.CalendarDuty.Update(byId1);
                    WriteLog(enLogType.NomalLog, enActionType.Approve, "Duyệt lịch trực đơn vị" + byId1.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
                    ViewBag.objCalendarDuty = byId1;
                    int result2 = 0;
                    if (int.TryParse(types, out result2))
                    {
                        List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
                        ViewBag.ActionPermission = list;
                        if (result2 == 1)
                            return PartialView("_ListButtons");
                        if (result2 == 2)
                            return PartialView("_ListButtonView");
                    }
                }
                else
                {
                    byId1.CALENDAR_STATUS = 3;
                    byId1.USER_APPROVED_ID = UserX.ADMIN_USER_ID;
                    byId1.DATE_APPROVED = DateTime.Now;
                    byId1.ISAPPROVED = 1;
                    unitOfWork.CalendarDuty.Update(byId1);
                    WriteLog(enLogType.NomalLog, enActionType.Approve, "Duyệt lịch trực đơn vị " + byId1.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
                    ViewBag.objCalendarDuty = byId1;
                    int result2 = 0;
                    if (int.TryParse(types, out result2))
                    {
                        List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
                        ViewBag.ActionPermission = list;
                        if (result2 == 1)
                            return PartialView("_ListButtons");
                        if (result2 == 2)
                            return PartialView("_ListButtonView");
                    }
                }
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetNamesCreate(string term)
        {
            List<ADMIN_USER> all = unitOfWork.Users.GetAll("FULLNAME", string.Empty);
            IEnumerable<string> source = all.Any() ? all.Where((n => n.FULLNAME.ToLower().Contains(term.ToLower()))).Select((t => t.FULLNAME)) : new List<string>();
            return new JsonResult()
            {
                Data = source.ToArray<string>(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public JsonResult GetNamesApproved(string termApproved)
        {
            List<ADMIN_USER> all = unitOfWork.Users.GetAll("FULLNAME", string.Empty);
            IEnumerable<string> strings = all.Any() ? all.Where((n => n.FULLNAME.ToLower().Contains(termApproved.ToLower()))).Select((t => t.FULLNAME)) : new List<string>();
            return new JsonResult()
            {
                Data = strings,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public JsonResult GetDepartment(string termDepartment)
        {
            List<LM_DEPARTMENT> list = unitOfWork.Departments.GetAll().ToList();
            IEnumerable<string> strings = list.Any() ? list.Where((n => n.DEPARTMENT_NAME.ToLower().Contains(termDepartment.ToLower()))).Select((t => t.DEPARTMENT_NAME)) : new List<string>();
            return new JsonResult()
            {
                Data = strings,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ApprovedRequest(string idCalendarDuty, string types, string userCreate)
        {
            ViewBag.idCalendarDuty = idCalendarDuty;
            int result1 = 0;
            if (!int.TryParse(idCalendarDuty, out result1))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result1);
            CalendarDutyModel calendarDutyModel = new CalendarDutyModel();
            calendarDutyModel.CALENDAR_DUTY_ID = byId.CALENDAR_DUTY_ID;
            calendarDutyModel.CALENDAR_NAME = byId.CALENDAR_NAME;
            calendarDutyModel.COMMENTS = byId.COMMENTS;
            int result2 = 0;
            int result3 = 0;
            int.TryParse(userCreate, out result3);
            if (!int.TryParse(types, out result2))
                return null;
            calendarDutyModel.DUTY_TYPE = result2;
            calendarDutyModel.USER_CREATE_ID = result3;
            return PartialView("_ApprovedRequest", calendarDutyModel);
        }

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDuty_Cancel", ActionName = "Hủy duyệt lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        public ActionResult CancelApproved(CalendarDutyModel objCalendarDuty, string types)
        {
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(objCalendarDuty.CALENDAR_DUTY_ID);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 4;
            byId.COMMENTS = objCalendarDuty.COMMENTS;
            unitOfWork.CalendarDuty.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.ApproveCancel, "Hủy duyệt lịch trực đơn vị " + objCalendarDuty.CALENDAR_NAME, "N/A", "N/A", 0, string.Empty, string.Empty);
            ViewBag.objCalendarDuty = byId;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            int? nullable = objCalendarDuty.DUTY_TYPE;
            if ((nullable.GetValueOrDefault() != 1 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                return PartialView("_ListButtons");
            nullable = objCalendarDuty.DUTY_TYPE;
            if ((nullable.GetValueOrDefault() != 2 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                return PartialView("_ListButtonView");
            nullable = objCalendarDuty.DUTY_TYPE;
            if ((nullable.GetValueOrDefault() != 3 ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
                return null;
            nullable = objCalendarDuty.USER_CREATE_ID;
            if ((nullable.GetValueOrDefault() != 1 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                return PartialView("_ListButtons");
            return PartialView("_ListButtonView");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [HttpGet]
        public ActionResult DetailCalendar(int deparmentId, int monthx, int yearx)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            if (deparmentId <= 0)
                return Json(JsonResponse.Json500(Localization.MsgItemNotExist));
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            List<DoctorCalendarLeader> calendarByDeparment = unitOfWork.CalendarDuty.GetDoctorCalendarByDeparment(deparmentId);
            Utils.Utils.GetDateTime();
            DateTime dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(monthx), Convert.ToString(yearx));
            string str = Localization.LableCalendarDuty + ": '" + unitOfWork.Departments.GetById(deparmentId).DEPARTMENT_CODE.ToUpper() + "' tháng " + monthx.ToString() + " năm " + yearx.ToString();
            ViewBag.doctors = calendarByDeparment;
            ViewBag.title = str;
            ViewBag.times = dateTime;
            doctorCalendarLeaderList = null;
            return PartialView("_DetailCalendar");
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [HttpGet]
        [CustomAuthorize]
        public ActionResult DetailLeader(int monthx, int yearx)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            List<DoctorCalendarLeader> calendarDirector = unitOfWork.CalendarDuty.GetDoctorCalendarDirector(monthx, yearx, DutyType.Leader.GetHashCode());
            Utils.Utils.GetDateTime();
            DateTime dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(monthx), Convert.ToString(yearx));
            string str = "Lịch lãnh đạo tháng " + monthx.ToString() + " năm " + yearx.ToString();
            ViewBag.doctors = calendarDirector;
            ViewBag.title = str;
            ViewBag.times = dateTime;
            doctorCalendarLeaderList = null;
            return PartialView("_DetailCalendar");
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        [HttpGet]
        public ActionResult DetailCalendarPersonal(int doctorId, int monthx, int yearx)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            List<DoctorCalendarLeader> doctorCalendarPesonal = unitOfWork.CalendarDuty.GetDoctorCalendarPesonal(monthx, yearx, doctorId);
            Utils.Utils.GetDateTime();
            DateTime dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(monthx), Convert.ToString(yearx));
            string str = "Lịch trực cá nhân: '" + unitOfWork.Doctors.GetById(doctorId).DOCTOR_NAME.ToUpper() + "' tháng " + monthx.ToString() + " năm " + yearx.ToString();
            ViewBag.doctors = doctorCalendarPesonal;
            ViewBag.title = str;
            ViewBag.times = dateTime;
            doctorCalendarLeaderList = null;
            return PartialView("_DetailCalendar");
        }

        [HttpGet]
        public PartialViewResult ApprovedCalendarHospital(string idCalendardutyX, string idDoctorX, string idDoctorChangeX, string idColumnX, string typeActionX, string idDateX, string idItem)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DOCTOR doctor1;
            DOCTOR doctor2;
            if (idColumnX == string.Empty || idColumnX == null)
            {
                if (idCalendardutyX != null && idDoctorX != null && (idDoctorChangeX != null && typeActionX != null) && idDateX != null)
                {
                    int calendarDutyId = 0;
                    int doctorsId = 0;
                    int idDoctorChange = 0;
                    int typeAction = 0;
                    if (int.TryParse(idCalendardutyX, out calendarDutyId) && int.TryParse(idDoctorX, out doctorsId) && int.TryParse(idDoctorChangeX, out idDoctorChange) && int.TryParse(typeActionX, out typeAction))
                    {
                        DateTime? nullable = new DateTime?(Convert.ToDateTime(idDateX.ToString()));
                        string str1 = string.Empty;
                        if (nullable.HasValue)
                            str1 = " vào ngày " + nullable.Value.ToString("dd/MM/yyyy");
                        doctor1 = new DOCTOR();
                        DOCTOR byId1 = unitOfWork.Doctors.GetById(doctorsId);
                        CALENDAR_CHANGE objCalendarChange = new CALENDAR_CHANGE();
                        if (typeAction == 1 || typeAction == 4)
                        {
                            objCalendarChange.CALENDAR_DUTY_ID = calendarDutyId;
                            objCalendarChange.TEMPLATE_COLUMN_ID = null;
                            objCalendarChange.DOCTORS_ID = doctorsId;
                            objCalendarChange.DATE_START = nullable;
                            CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange, 1);
                            if (infor != null)
                            {
                                doctor2 = new DOCTOR();
                                DOCTOR byId2 = unitOfWork.Doctors.GetById(Convert.ToInt32(infor.DOCTORS_CHANGE_ID));
                                ViewBag.educationName = byId1.EDUCATION_NAMEs;
                                ViewBag.doctorName = byId1.DOCTOR_NAME;
                                ViewBag.idCalendarduty = calendarDutyId;
                                ViewBag.idDoctor = doctorsId;
                                ViewBag.idDoctorChange = idDoctorChange;
                                ViewBag.idColumn = null;
                                ViewBag.idDate = idDateX;
                                ViewBag.typeAction = typeAction;
                                ViewBag.idItem = idItem;
                                string str2 = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + " đã đổi lịch trực cho " + byId2.EDUCATION_NAMEs + " " + byId2.DOCTOR_NAME + str1;
                                ViewBag.Content = str2;
                                return PartialView("_ApprovedCalendarHospital");
                            }
                            objCalendarChange.DOCTORS_CHANGE_ID = doctorsId;
                            CALENDAR_CHANGE inforDoctorChange = unitOfWork.CalendarChanges.GetInforDoctorChange(objCalendarChange, 1);
                            doctor2 = new DOCTOR();
                            DOCTOR byId3 = unitOfWork.Doctors.GetById(Convert.ToInt32(inforDoctorChange.DOCTORS_ID));
                            ViewBag.educationName = byId1.EDUCATION_NAMEs;
                            ViewBag.doctorName = byId1.DOCTOR_NAME;
                            ViewBag.idCalendarduty = calendarDutyId;
                            ViewBag.idDoctor = doctorsId;
                            ViewBag.idDoctorChange = idDoctorChange;
                            ViewBag.idColumn = null;
                            ViewBag.idDate = idDateX;
                            ViewBag.typeAction = typeAction;
                            ViewBag.idItem = idItem;
                            string str3 = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + " đã đổi lịch trực cho " + byId3.EDUCATION_NAMEs + " " + byId3.DOCTOR_NAME + str1;
                            ViewBag.Content = str3;
                            return PartialView("_ApprovedCalendarHospital");
                        }
                        if (typeAction == 2)
                        {
                            objCalendarChange.CALENDAR_DUTY_ID = calendarDutyId;
                            objCalendarChange.TEMPLATE_COLUMN_ID = null;
                            objCalendarChange.DOCTORS_ID = doctorsId;
                            objCalendarChange.DATE_START = nullable;
                            if (unitOfWork.CalendarChanges.GetInfor(objCalendarChange, typeAction) == null)
                                return null;
                            ViewBag.educationName = byId1.EDUCATION_NAMEs;
                            ViewBag.doctorName = byId1.DOCTOR_NAME;
                            ViewBag.idCalendarduty = calendarDutyId;
                            ViewBag.idDoctor = doctorsId;
                            ViewBag.idColumn = null;
                            ViewBag.idDate = idDateX;
                            ViewBag.typeAction = typeAction;
                            ViewBag.idItem = idItem;
                            ViewBag.Content = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + "  đã hủy trực " + str1;
                            return PartialView("_ApprovedCalendarHospital");
                        }
                        if (typeAction == 3)
                        {
                            objCalendarChange.CALENDAR_DUTY_ID = calendarDutyId;
                            objCalendarChange.TEMPLATE_COLUMN_ID = null;
                            objCalendarChange.DOCTORS_ID = doctorsId;
                            objCalendarChange.DATE_START = nullable;
                            if (unitOfWork.CalendarChanges.GetInfor(objCalendarChange, typeAction) == null)
                                return null;
                            ViewBag.idCalendarduty = calendarDutyId;
                            ViewBag.idDoctor = doctorsId;
                            ViewBag.idColumn = null;
                            ViewBag.idDate = idDateX;
                            ViewBag.typeAction = typeAction;
                            ViewBag.Content = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + " đã bổ sung lịch trực " + str1;
                            ViewBag.idItem = idItem;
                            return PartialView("_ApprovedCalendarHospital");
                        }
                    }
                }
            }
            else if (idCalendardutyX != null && idDoctorX != null && (idDoctorChangeX != null && typeActionX != null) && idDateX != null)
            {
                int calendarDutyId = 0;
                int doctorsId = 0;
                int idDoctorChange = 0;
                int templateColumnId = 0;
                int typeAction = 0;
                if (int.TryParse(idCalendardutyX, out calendarDutyId) && int.TryParse(idDoctorX, out doctorsId) && (int.TryParse(idDoctorChangeX, out idDoctorChange) && int.TryParse(idColumnX, out templateColumnId)) && int.TryParse(typeActionX, out typeAction))
                {
                    DateTime? nullable = new DateTime?(Convert.ToDateTime(idDateX.ToString()));
                    string str1 = string.Empty;
                    if (nullable.HasValue)
                        str1 = " vào ngày " + nullable.Value.ToString("dd/MM/yyyy");
                    doctor1 = new DOCTOR();
                    DOCTOR byId1 = unitOfWork.Doctors.GetById(doctorsId);
                    CALENDAR_CHANGE objCalendarChange = new CALENDAR_CHANGE();
                    if (typeAction == 1 || typeAction == 4)
                    {
                        objCalendarChange.CALENDAR_DUTY_ID = calendarDutyId;
                        objCalendarChange.TEMPLATE_COLUMN_ID = templateColumnId;
                        objCalendarChange.DOCTORS_ID = doctorsId;
                        objCalendarChange.DATE_START = nullable;
                        CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange, 1);
                        if (infor == null)
                            return null;
                        doctor2 = new DOCTOR();
                        DOCTOR byId2 = unitOfWork.Doctors.GetById(Convert.ToInt32(infor.DOCTORS_CHANGE_ID));
                        ViewBag.educationName = byId1.EDUCATION_NAMEs;
                        ViewBag.doctorName = byId1.DOCTOR_NAME;
                        ViewBag.idCalendarduty = calendarDutyId;
                        ViewBag.idDoctor = doctorsId;
                        ViewBag.idDoctorChange = idDoctorChange;
                        ViewBag.idColumn = templateColumnId;
                        ViewBag.idDate = idDateX;
                        ViewBag.typeAction = typeAction;
                        ViewBag.idItem = idItem;
                        string str2 = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + " đã đổi lịch trực cho " + byId2.EDUCATION_NAMEs + " " + byId2.DOCTOR_NAME + str1;
                        ViewBag.Content = str2;
                        return PartialView("_ApprovedCalendarHospital");
                    }
                    if (typeAction == 2)
                    {
                        objCalendarChange.CALENDAR_DUTY_ID = calendarDutyId;
                        objCalendarChange.TEMPLATE_COLUMN_ID = templateColumnId;
                        objCalendarChange.DOCTORS_ID = doctorsId;
                        objCalendarChange.DATE_START = nullable;
                        if (unitOfWork.CalendarChanges.GetInfor(objCalendarChange, typeAction) == null)
                            return null;
                        ViewBag.educationName = byId1.EDUCATION_NAMEs;
                        ViewBag.doctorName = byId1.DOCTOR_NAME;
                        ViewBag.idCalendarduty = calendarDutyId;
                        ViewBag.idDoctor = doctorsId;
                        ViewBag.idColumn = templateColumnId;
                        ViewBag.idDate = idDateX;
                        ViewBag.typeAction = typeAction;
                        ViewBag.idItem = idItem;
                        ViewBag.Content = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + "  đã hủy trực " + str1;
                        return PartialView("_ApprovedCalendarHospital");
                    }
                    if (typeAction == 3)
                    {
                        objCalendarChange.CALENDAR_DUTY_ID = calendarDutyId;
                        objCalendarChange.TEMPLATE_COLUMN_ID = templateColumnId;
                        objCalendarChange.DOCTORS_ID = doctorsId;
                        objCalendarChange.DATE_START = nullable;
                        if (unitOfWork.CalendarChanges.GetInfor(objCalendarChange, typeAction) == null)
                            return null;
                        ViewBag.idCalendarduty = calendarDutyId;
                        ViewBag.idDoctor = doctorsId;
                        ViewBag.idColumn = templateColumnId;
                        ViewBag.idDate = idDateX;
                        ViewBag.typeAction = typeAction;
                        ViewBag.Content = byId1.EDUCATION_NAMEs + " " + byId1.DOCTOR_NAME + " đã bổ sung lịch trực " + str1;
                        ViewBag.idItem = idItem;
                        return PartialView("_ApprovedCalendarHospital");
                    }
                }
            }
            return PartialView("_ApprovedCalendarHospital");
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [HttpGet]
        [CustomAuthorize]
        public ActionResult CalendarHospitalAll()
        {
            ViewBag.types = 4;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.ActionPermission = actionCodesByUserName;
            WriteLog(enLogType.NomalLog, enActionType.View, "Xem trực đơn vị toàn bệnh viện", "N/A", "N/A", 0, string.Empty, string.Empty);
            return View("~/Views/CalendarDuty/Index.cshtml");
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        [HttpGet]
        public ActionResult ViewCalendarHospital(int idCalendarDuty = 0, int idWeek = 0)
        {
            if (idCalendarDuty == 0)
            {
                int month = DateTime.Now.Month;
                DateTime dateTime1 = DateTime.Now;
                int year = dateTime1.Year;
                CALENDAR_DUTY calendarDuty1 = unitOfWork.CalendarDuty.CheckCalendarHospital(month, year, 4);
                CALENDAR_DUTY entity = new CALENDAR_DUTY();
                if (calendarDuty1 == null)
                {
                    ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                    entity.CALENDAR_NAME = "Lịch trực toàn bệnh viện tháng " + month + " năm " + year;
                    entity.CALENDAR_MONTH = month;
                    entity.CALENDAR_YEAR = year;
                    entity.CALENDAR_STATUS = 1;
                    entity.LM_DEPARTMENT_ID = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
                    entity.DUTY_TYPE = 4;
                    entity.ISDELETE = false;
                    entity.USER_CREATE_ID = null;
                    entity.LM_DEPARTMENT_PARTS = null;
                    unitOfWork.CalendarDuty.Add(entity);
                    foreach (CALENDAR_DUTY calendarDuty2 in unitOfWork.CalendarDuty.GetByApproved(month, year, 3, 0))
                        unitOfWork.CalendarGroups.Add(new CALENDAR_GROUP()
                        {
                            CALENDAR_ID = entity.CALENDAR_DUTY_ID,
                            CALENDAR_PARENT_ID = calendarDuty2.CALENDAR_DUTY_ID,
                            CALENDAR_MONTH = entity.CALENDAR_MONTH,
                            CALENDAR_YEAR = entity.CALENDAR_YEAR,
                            LM_DEPARTMENT_ID = entity.LM_DEPARTMENT_ID
                        });
                    List<TimeCalendarDuty> timeCalendarDutyList = new List<TimeCalendarDuty>();
                    if (entity.CALENDAR_MONTH.GetValueOrDefault() > 0 && entity.CALENDAR_YEAR.GetValueOrDefault() > 0)
                    {
                        DateTime dateTime2 = Convert.ToDateTime("1/" + entity.CALENDAR_MONTH.Value + "/" + entity.CALENDAR_YEAR.Value);
                        if (idWeek == -1)
                        {
                            DateTime dayFirstMonth = dateTime2.AddDays((-dateTime2.Day + 1));
                            DateTime dayLastMonth = dateTime2.AddMonths(1).AddDays(-dateTime2.Day);
                            for (int index = 0; index < dayLastMonth.Day; ++index)
                            {
                                TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                                timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                                timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                                timeCalendarDutyList.Add(timeCalendarDuty);
                            }
                        }
                        else if (idWeek < 3)
                        {
                            int num2 = idWeek * 7;
                            DateTime dayFirstWeek = dateTime2.AddDays((-dateTime2.Day + 1)).AddDays(num2);
                            for (int index = 0; index < 7; ++index)
                            {
                                TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                                timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                                timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                                timeCalendarDutyList.Add(timeCalendarDuty);
                            }
                        }
                        else
                        {
                            int num2 = DateTime.DaysInMonth(entity.CALENDAR_YEAR.Value, entity.CALENDAR_MONTH.Value) - 28;
                            int num3 = idWeek * 7;
                            DateTime dayFirstWeek = dateTime2.AddDays((-dateTime2.Day + 1)).AddDays(num3);
                            int num4 = num2 + 7;
                            for (int index = 0; index < num4; ++index)
                            {
                                TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                                timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                                timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                                timeCalendarDutyList.Add(timeCalendarDuty);
                            }
                        }
                    }
                    ViewBag.objCalendarDuty = entity;
                    ViewBag.idWeek = idWeek;
                    ViewBag.times = timeCalendarDutyList;
                    return View("_ViewCalendarHospital");
                }
                int monthNow = DateTime.Today.Month;
                int yearNow = DateTime.Today.Year;
                List<TimeCalendarDuty> timeCalendarDutyList1 = new List<TimeCalendarDuty>();
                if (calendarDuty1.CALENDAR_MONTH.GetValueOrDefault() > 0 && calendarDuty1.CALENDAR_YEAR.GetValueOrDefault() > 0)
                {
                    DateTime dateTime2 = Convert.ToDateTime("1/" + calendarDuty1.CALENDAR_MONTH.Value + "/" + calendarDuty1.CALENDAR_YEAR.Value);
                    if (idWeek == -1)
                    {
                        DateTime dayFirstMonth = dateTime2.AddDays((-dateTime2.Day + 1));
                        DateTime datLastMonth = dateTime1.AddMonths(1).AddDays(-dateTime2.Day);
                        for (int index = 0; index < datLastMonth.Day; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                            timeCalendarDutyList1.Add(timeCalendarDuty);
                        }
                    }
                    else if (idWeek < 3)
                    {
                        int num1 = idWeek * 7;
                        dateTime1 = dateTime2.AddDays((-dateTime2.Day + 1));
                        DateTime dayFisrtWeek = dateTime1.AddDays(num1);
                        for (int index = 0; index < 7; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFisrtWeek.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                            timeCalendarDutyList1.Add(timeCalendarDuty);
                        }
                    }
                    else
                    {
                        int num1 = DateTime.DaysInMonth(calendarDuty1.CALENDAR_YEAR.Value, calendarDuty1.CALENDAR_MONTH.Value) - 28;
                        int num2 = idWeek * 7;
                        DateTime dayFisrtWeek = dateTime1.AddDays((-dateTime2.Day + 1)).AddDays(num2);
                        int num3 = num1 + 7;
                        for (int index = 0; index < num3; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dayFisrtWeek.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                            timeCalendarDutyList1.Add(timeCalendarDuty);
                        }
                    }
                }
                ViewBag.objCalendarDuty = calendarDuty1;
                ViewBag.idWeek = idWeek;
                ViewBag.times = timeCalendarDutyList1;
                return View("_ViewCalendarHospital");
            }
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(idCalendarDuty);
            List<TimeCalendarDuty> timeCalendarDutyList2 = new List<TimeCalendarDuty>();
            int result5 = 0;
            int result6 = 0;
            int? nullable2 = byId.CALENDAR_MONTH;
            int num6;
            if (int.TryParse(nullable2.ToString(), out result5))
            {
                nullable2 = byId.CALENDAR_YEAR;
                num6 = !int.TryParse(nullable2.ToString(), out result6) ? 1 : 0;
            }
            else
                num6 = 1;
            if (byId.CALENDAR_MONTH.GetValueOrDefault() > 0 && byId.CALENDAR_YEAR.GetValueOrDefault() > 0)
            {
                DateTime dateTime1 = Convert.ToDateTime("1/" + byId.CALENDAR_MONTH.Value + "/" + byId.CALENDAR_YEAR.Value);
                if (idWeek == -1)
                {
                    DateTime dayFirstMonth = dateTime1.AddDays((-dateTime1.Day + 1));
                    DateTime dayLastMonth = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                    for (int index = 0; index < dayLastMonth.Day; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList2.Add(timeCalendarDuty);
                    }
                }
                else if (idWeek < 3)
                {
                    int num1 = idWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num1);
                    for (int index = 0; index < 7; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList2.Add(timeCalendarDuty);
                    }
                }
                else
                {
                    int num1 = DateTime.DaysInMonth(byId.CALENDAR_YEAR.Value, byId.CALENDAR_MONTH.Value) - 28;
                    int num2 = idWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                    int num3 = num1 + 7;
                    for (int index = 0; index < num3; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayName(timeCalendarDuty.DATES);
                        timeCalendarDutyList2.Add(timeCalendarDuty);
                    }
                }
            }
            ViewBag.objCalendarDuty = byId;
            ViewBag.idWeek = idWeek;
            ViewBag.times = timeCalendarDutyList2;
            return PartialView("_ViewCalendarHospital");
        }

        [CustomAuthorize]
        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        public PartialViewResult LoadCalendarHospital(int idweek = 0, int nextMonth = 0, int nextYear = 0, int idCalendarDuty = 0)
        {
            if (!Request.IsAjaxRequest())
                return null;
            int result1 = 0;
            List<string> list1 = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list1;
            int typeWeek;
            int month;
            int year;
            if (int.TryParse(idweek.ToString(), out typeWeek) && int.TryParse(nextMonth.ToString(), out month) && int.TryParse(nextYear.ToString(), out year) && int.TryParse(idCalendarDuty.ToString(), out result1))
            {
                Utils.Utils.GetDateTime();
                List<TimeCalendarDuty> timeCalendarDutyList = new List<TimeCalendarDuty>();
                DateTime dateTime1 = Convert.ToDateTime("1/" + month + "/" + year);
                if (typeWeek == -1)
                {
                    DateTime dayFirstMonth = dateTime1.AddDays((-dateTime1.Day + 1));
                    DateTime dayLastMonth = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                    for (int index = 0; index < dayLastMonth.Day; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstMonth.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                        timeCalendarDutyList.Add(timeCalendarDuty);
                    }
                }
                else if (typeWeek < 3)
                {
                    int num = typeWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num);
                    for (int index = 0; index < 7; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                        timeCalendarDutyList.Add(timeCalendarDuty);
                    }
                }
                else
                {
                    int num1 = DateTime.DaysInMonth(year, month) - 28;
                    int num2 = typeWeek * 7;
                    DateTime dayFirstWeek = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                    int num3 = num1 + 7;
                    for (int index = 0; index < num3; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dayFirstWeek.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                        timeCalendarDutyList.Add(timeCalendarDuty);
                    }
                }
                List<DEPARTMENTLIST> departmentByLevelVt = unitOfWork.Departments.GetAllDepartmentByLevelVT(LevelDeparment.Level1.GetHashCode());
                CALENDAR_DUTY calendarDuty = unitOfWork.CalendarDuty.CheckCalendarHospital(nextMonth, year, 4);
                List<DoctorHospital> list2 = unitOfWork.CalendarDuty.GetDoctorHospital(nextMonth, year).Where((x =>
               {
                   int? levelNumber = x.LEVEL_NUMBER;
                   return levelNumber.GetValueOrDefault() < 3 && levelNumber.HasValue;
               })).OrderBy((x => x.DOCTOR_NAME)).ToList();
                List<CALENDAR_GROUP> calendarGroupList = unitOfWork.CalendarGroups.ListCalendarGroup(nextMonth, year);
                ViewBag.times = dateTime1;
                ViewBag.doctors = list2;
                ViewBag.timeCalendar = timeCalendarDutyList;
                ViewBag.objDepartment = departmentByLevelVt;
                ViewBag.idCalendarDuty = calendarDuty;
                ViewBag.calendarDepartment = calendarGroupList;
            }
            return PartialView("_ViewCalendarHospitalData");
        }

        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ApprovedDoctor(int idCalendarduty, int idDoctor, int idColumn, string idDate, int typeAction)
        {
            DateTime dateTime1 = Convert.ToDateTime(idDate.ToString());
            string str1 = " ngày " + dateTime1.ToString("dd/MM/yyyy");
            DoctorData doctorData = unitOfWork.DoctorDatas.CheckDoctorData(idCalendarduty, idDoctor, idColumn, dateTime1);
            int num1 = 0;
            CALENDAR_CHANGE objCalendarChange1 = new CALENDAR_CHANGE();
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            if (doctorData != null)
            {
                objCalendarChange1.CALENDAR_DUTY_ID = idCalendarduty;
                objCalendarChange1.TEMPLATE_COLUMN_ID = idColumn;
                objCalendarChange1.DOCTORS_ID = idDoctor;
                objCalendarChange1.DATE_START = dateTime1;
                CALENDAR_CHANGE calendarChange = new CALENDAR_CHANGE();
                CALENDAR_CHANGE entity;
                if (typeAction == 4)
                {
                    entity = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 1);
                }
                else
                {
                    entity = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, typeAction);
                    if (entity == null)
                    {
                        num1 = 1;
                        objCalendarChange1.TEMPLATE_COLUMN_ID = idColumn;
                        objCalendarChange1.DOCTORS_CHANGE_ID = idDoctor;
                        objCalendarChange1.DATE_CHANGE_START = dateTime1;
                        entity = unitOfWork.CalendarChanges.GetInforDoctorChange(objCalendarChange1, typeAction);
                    }
                }
                if (entity != null)
                {
                    DateTime dateTime2;
                    int num2;
                    DateTime? dateChange;
                    if (typeAction == 1)
                    {
                        if (idColumn != 0)
                        {
                            CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                            byId1.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_CHANGE_ID);
                            unitOfWork.CalendarDoctors.Update(byId1);
                            entity.STATUS_APPROVED = 3;
                            entity.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(entity);
                            CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(Convert.ToInt32(unitOfWork.DoctorDatas.CheckDoctorData(idCalendarduty, Convert.ToInt32(entity.DOCTORS_CHANGE_ID), idColumn, Convert.ToDateTime(entity.DATE_CHANGE_START)).CALENDAR_DOCTOR_ID));
                            byId2.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_ID);
                            unitOfWork.CalendarDoctors.Update(byId2);
                            CALENDAR_CHANGE inforCh = unitOfWork.CalendarChanges.GetInforCh(new CALENDAR_CHANGE()
                            {
                                CALENDAR_DUTY_ID = idCalendarduty,
                                TEMPLATE_COLUMN_ID = entity.TEMPLATE_COLUMN_ID,
                                CALENDAR_DELETE = entity.CALENDAR_CHANGE_ID,
                                DOCTORS_ID = new int?(Convert.ToInt32(entity.DOCTORS_CHANGE_ID)),
                                DATE_START = entity.DATE_CHANGE_START
                            }, typeAction);
                            inforCh.STATUS_APPROVED = 3;
                            inforCh.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(inforCh);
                            unitOfWork.CalendarChanges.Save();
                            string[] strArray1 = new string[18]
                            {
                byId1.DOCTOR.EDUCATION_NAMEs,
                " ",
                byId1.DOCTOR.DOCTOR_NAME,
                " đã đổi lịch trực cho ",
                entity.DOCTOR.EDUCATION_NAMEs,
                " ",
                entity.DOCTOR.DOCTOR_NAME,
                str1,
                " lúc ",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                            };
                            string[] strArray2 = strArray1;
                            int index1 = 9;
                            dateTime2 = entity.DATE_CHANGE.Value;
                            string str2 = dateTime2.Hour.ToString();
                            strArray2[index1] = str2;
                            strArray1[10] = ":";
                            string[] strArray3 = strArray1;
                            int index2 = 11;
                            dateTime2 = entity.DATE_CHANGE.Value;
                            string str3 = dateTime2.Minute.ToString();
                            strArray3[index2] = str3;
                            strArray1[12] = " ";
                            string[] strArray4 = strArray1;
                            int index3 = 13;
                            dateTime2 = entity.DATE_CHANGE.Value;
                            num2 = dateTime2.Day;
                            string str4 = num2.ToString();
                            strArray4[index3] = str4;
                            strArray1[14] = "/";
                            string[] strArray5 = strArray1;
                            int index4 = 15;
                            dateTime2 = entity.DATE_CHANGE.Value;
                            num2 = dateTime2.Month;
                            string str5 = num2.ToString();
                            strArray5[index4] = str5;
                            strArray1[16] = "/";
                            string[] strArray6 = strArray1;
                            int index5 = 17;
                            dateTime2 = entity.DATE_CHANGE.Value;
                            num2 = dateTime2.Year;
                            string str6 = num2.ToString();
                            strArray6[index5] = str6;
                            string str7 = string.Concat(strArray1);
                            List<SendSms6x00> listSms = new List<SendSms6x00>();
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = byId1.DOCTOR.PHONE,
                                Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                                Types = "0",
                                DoctorName = byId1.DOCTOR.DOCTOR_NAME
                            });
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = entity.DOCTOR.PHONE,
                                Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                                Types = "0",
                                DoctorName = entity.DOCTOR.DOCTOR_NAME
                            });
                            sendSMSBrandname(listSms);
                        }
                        else if (num1 == 0)
                        {
                            CALENDAR_CHANGE objCalendarChange2 = new CALENDAR_CHANGE();
                            objCalendarChange2.CALENDAR_DUTY_ID = idCalendarduty;
                            objCalendarChange2.DOCTORS_ID = new int?(Convert.ToInt32(entity.DOCTORS_ID));
                            objCalendarChange2.DATE_START = entity.DATE_START;
                            objCalendarChange2.DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(entity.DOCTORS_CHANGE_ID));
                            objCalendarChange2.DATE_CHANGE_START = entity.DATE_CHANGE_START;
                            CALENDAR_CHANGE inforHospital = unitOfWork.CalendarChanges.GetInforHospital(objCalendarChange2, typeAction);
                            if (inforHospital != null)
                            {
                                inforHospital.STATUS_APPROVED = 3;
                                inforHospital.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                unitOfWork.CalendarChanges.Update(inforHospital);
                                unitOfWork.CalendarChanges.Save();
                                CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                                byId1.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_CHANGE_ID);
                                unitOfWork.CalendarDoctors.Update(byId1);
                                CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(Convert.ToInt32(unitOfWork.DoctorDatas.CheckDoctorData(idCalendarduty, Convert.ToInt32(entity.DOCTORS_CHANGE_ID), idColumn, Convert.ToDateTime(entity.DATE_CHANGE_START)).CALENDAR_DOCTOR_ID));
                                byId2.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_ID);
                                unitOfWork.CalendarDoctors.Update(byId2);
                                string[] strArray1 = new string[18]
                                {
                  entity.DOCTOR.EDUCATION_NAMEs,
                  " ",
                  entity.DOCTOR.DOCTOR_NAME,
                  " đã đổi lịch trực cho ",
                  inforHospital.DOCTOR.EDUCATION_NAMEs,
                  " ",
                  inforHospital.DOCTOR.DOCTOR_NAME,
                  str1,
                  " lúc ",
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null
                                };
                                string[] strArray2 = strArray1;
                                int index1 = 9;
                                dateTime2 = inforHospital.DATE_CHANGE.Value;
                                num2 = dateTime2.Hour;
                                string str2 = num2.ToString();
                                strArray2[index1] = str2;
                                strArray1[10] = ":";
                                string[] strArray3 = strArray1;
                                int index2 = 11;
                                dateTime2 = inforHospital.DATE_CHANGE.Value;
                                num2 = dateTime2.Minute;
                                string str3 = num2.ToString();
                                strArray3[index2] = str3;
                                strArray1[12] = " ";
                                string[] strArray4 = strArray1;
                                int index3 = 13;
                                dateChange = inforHospital.DATE_CHANGE;
                                dateTime2 = dateChange.Value;
                                num2 = dateTime2.Day;
                                string str4 = num2.ToString();
                                strArray4[index3] = str4;
                                strArray1[14] = "/";
                                string[] strArray5 = strArray1;
                                int index4 = 15;
                                dateChange = inforHospital.DATE_CHANGE;
                                dateTime2 = dateChange.Value;
                                num2 = dateTime2.Month;
                                string str5 = num2.ToString();
                                strArray5[index4] = str5;
                                strArray1[16] = "/";
                                string[] strArray6 = strArray1;
                                int index5 = 17;
                                dateChange = inforHospital.DATE_CHANGE;
                                dateTime2 = dateChange.Value;
                                num2 = dateTime2.Year;
                                string str6 = num2.ToString();
                                strArray6[index5] = str6;
                                string str7 = string.Concat(strArray1);
                                List<SendSms6x00> listSms = new List<SendSms6x00>();
                                listSms.Add(new SendSms6x00()
                                {
                                    Phone = objCalendarChange2.DOCTOR.PHONE,
                                    Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                                    Types = "0",
                                    DoctorName = objCalendarChange2.DOCTOR.DOCTOR_NAME
                                });
                                listSms.Add(new SendSms6x00()
                                {
                                    Phone = entity.DOCTOR.PHONE,
                                    Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                                    Types = "0",
                                    DoctorName = entity.DOCTOR.DOCTOR_NAME
                                });
                                sendSMSBrandname(listSms);
                            }
                        }
                        else
                        {
                            CALENDAR_CHANGE objCalendarChange2 = new CALENDAR_CHANGE();
                            objCalendarChange2.CALENDAR_DUTY_ID = idCalendarduty;
                            objCalendarChange2.DOCTORS_ID = new int?(Convert.ToInt32(entity.DOCTORS_ID));
                            objCalendarChange2.DATE_START = entity.DATE_START;
                            objCalendarChange2.DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(entity.DOCTORS_CHANGE_ID));
                            objCalendarChange2.DATE_CHANGE_START = entity.DATE_CHANGE_START;
                            CALENDAR_CHANGE inforHospital = unitOfWork.CalendarChanges.GetInforHospital(objCalendarChange2, typeAction);
                            if (inforHospital != null)
                            {
                                inforHospital.STATUS_APPROVED = 3;
                                inforHospital.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                unitOfWork.CalendarChanges.Update(inforHospital);
                                unitOfWork.CalendarChanges.Save();
                                CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                                byId1.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_ID);
                                unitOfWork.CalendarDoctors.Update(byId1);
                                CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(Convert.ToInt32(unitOfWork.DoctorDatas.CheckDoctorData(idCalendarduty, Convert.ToInt32(entity.DOCTORS_ID), idColumn, Convert.ToDateTime(entity.DATE_START)).CALENDAR_DOCTOR_ID));
                                byId2.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_CHANGE_ID);
                                unitOfWork.CalendarDoctors.Update(byId2);
                            }
                            string[] strArray1 = new string[18]
                            {
                inforHospital.DOCTOR.EDUCATION_NAMEs,
                " ",
                inforHospital.DOCTOR.DOCTOR_NAME,
                " đã đổi lịch trực cho ",
                entity.DOCTOR.EDUCATION_NAMEs,
                " ",
                entity.DOCTOR.DOCTOR_NAME,
                str1,
                " lúc ",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                            };
                            string[] strArray2 = strArray1;
                            int index1 = 9;
                            dateChange = entity.DATE_CHANGE;
                            string str2 = dateChange.Value.Hour.ToString();
                            strArray2[index1] = str2;
                            strArray1[10] = ":";
                            string[] strArray3 = strArray1;
                            int index2 = 11;
                            dateChange = entity.DATE_CHANGE;
                            string str3 = dateChange.Value.Minute.ToString();
                            strArray3[index2] = str3;
                            strArray1[12] = " ";
                            string[] strArray4 = strArray1;
                            int index3 = 13;
                            dateChange = entity.DATE_CHANGE;
                            string str4 = dateChange.Value.Day.ToString();
                            strArray4[index3] = str4;
                            strArray1[14] = "/";
                            string[] strArray5 = strArray1;
                            int index4 = 15;
                            dateChange = entity.DATE_CHANGE;
                            string str5 = dateChange.Value.Month.ToString();
                            strArray5[index4] = str5;
                            strArray1[16] = "/";
                            string[] strArray6 = strArray1;
                            int index5 = 17;
                            dateChange = entity.DATE_CHANGE;
                            num2 = dateChange.Value.Year;
                            string str6 = num2.ToString();
                            strArray6[index5] = str6;
                            string str7 = string.Concat(strArray1);
                            List<SendSms6x00> listSms = new List<SendSms6x00>();
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = objCalendarChange2.DOCTOR.PHONE,
                                Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                                Types = "0",
                                DoctorName = objCalendarChange2.DOCTOR.DOCTOR_NAME
                            });
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = entity.DOCTOR.PHONE,
                                Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                                Types = "0",
                                DoctorName = entity.DOCTOR.DOCTOR_NAME
                            });
                            sendSMSBrandname(listSms);
                        }
                    }
                    if (typeAction == 4)
                    {
                        CALENDAR_DOCTOR byId = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                        byId.DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_CHANGE_ID);
                        unitOfWork.CalendarDoctors.Update(byId);
                        entity.STATUS_APPROVED = 3;
                        entity.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(entity);
                        string[] strArray1 = new string[18]
                        {
              byId.DOCTOR.EDUCATION_NAMEs,
              " ",
              byId.DOCTOR.DOCTOR_NAME,
              " đã đổi lịch trực cho ",
              entity.DOCTOR.EDUCATION_NAMEs,
              " ",
              entity.DOCTOR.DOCTOR_NAME,
              str1,
              " lúc ",
              null,
              null,
              null,
              null,
              null,
              null,
              null,
              null,
              null
                        };
                        string[] strArray2 = strArray1;
                        int index1 = 9;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Hour;
                        string str2 = num2.ToString();
                        strArray2[index1] = str2;
                        strArray1[10] = ":";
                        string[] strArray3 = strArray1;
                        int index2 = 11;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Minute;
                        string str3 = num2.ToString();
                        strArray3[index2] = str3;
                        strArray1[12] = " ";
                        string[] strArray4 = strArray1;
                        int index3 = 13;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Day;
                        string str4 = num2.ToString();
                        strArray4[index3] = str4;
                        strArray1[14] = "/";
                        string[] strArray5 = strArray1;
                        int index4 = 15;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Month;
                        string str5 = num2.ToString();
                        strArray5[index4] = str5;
                        strArray1[16] = "/";
                        string[] strArray6 = strArray1;
                        int index5 = 17;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Year;
                        string str6 = num2.ToString();
                        strArray6[index5] = str6;
                        string str7 = string.Concat(strArray1);
                        List<SendSms6x00> listSms = new List<SendSms6x00>();
                        listSms.Add(new SendSms6x00()
                        {
                            Phone = byId.DOCTOR.PHONE,
                            Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                            Types = "0",
                            DoctorName = byId.DOCTOR.DOCTOR_NAME
                        });
                        listSms.Add(new SendSms6x00()
                        {
                            Phone = entity.DOCTOR.PHONE,
                            Contents = "BM-Lichtruc: Xác nhận đổi lịch " + str7,
                            Types = "0",
                            DoctorName = entity.DOCTOR.DOCTOR_NAME
                        });
                        sendSMSBrandname(listSms);
                    }
                    if (typeAction == 2)
                    {
                        CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                        CALENDAR_DATA byId2 = unitOfWork.CalendarDatas.GetById(doctorData.CALENDAR_DATA_ID);
                        entity.STATUS_APPROVED = 3;
                        entity.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(entity);
                        List<SendSms6x00> listSms = new List<SendSms6x00>();
                        SendSms6x00 sendSms6x00_2 = new SendSms6x00();
                        sendSms6x00_2.Phone = byId1.DOCTOR.PHONE;
                        SendSms6x00 sendSms6x00_3 = sendSms6x00_2;
                        string[] strArray1 = new string[16];
                        strArray1[0] = "BM-Lichtruc: Xác nhận hủy lịch trực ";
                        strArray1[1] = byId1.DOCTOR.EDUCATION_NAMEs;
                        strArray1[2] = " ";
                        strArray1[3] = byId1.DOCTOR.DOCTOR_NAME;
                        strArray1[4] = "  đã hủy trực ";
                        strArray1[5] = str1;
                        strArray1[6] = " lúc ";
                        string[] strArray2 = strArray1;
                        int index1 = 7;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Hour;
                        string str2 = num2.ToString();
                        strArray2[index1] = str2;
                        strArray1[8] = ":";
                        string[] strArray3 = strArray1;
                        int index2 = 9;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Minute;
                        string str3 = num2.ToString();
                        strArray3[index2] = str3;
                        strArray1[10] = " ngày ";
                        string[] strArray4 = strArray1;
                        int index3 = 11;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Day;
                        string str4 = num2.ToString();
                        strArray4[index3] = str4;
                        strArray1[12] = "/";
                        string[] strArray5 = strArray1;
                        int index4 = 13;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Month;
                        string str5 = num2.ToString();
                        strArray5[index4] = str5;
                        strArray1[14] = "/";
                        string[] strArray6 = strArray1;
                        int index5 = 15;
                        dateChange = entity.DATE_CHANGE;
                        dateTime2 = dateChange.Value;
                        num2 = dateTime2.Year;
                        string str6 = num2.ToString();
                        strArray6[index5] = str6;
                        string str7 = string.Concat(strArray1);
                        sendSms6x00_3.Contents = str7;
                        sendSms6x00_2.Types = "0";
                        sendSms6x00_2.DoctorName = byId1.DOCTOR.DOCTOR_NAME;
                        listSms.Add(sendSms6x00_2);
                        sendSMSBrandname(listSms);
                        unitOfWork.CalendarDoctors.Delete(byId1);
                        unitOfWork.CalendarDatas.Delete(byId2);
                    }
                }
            }
            else if (typeAction == 3)
            {
                CALENDAR_DATA entity1 = new CALENDAR_DATA();
                entity1.CALENDAR_DUTY_ID = idCalendarduty;
                if (idColumn != 0)
                    entity1.TEMPLATE_COLUM_ID = idColumn;
                entity1.DATE_START = dateTime1;
                entity1.ISDELETE = false;
                unitOfWork.CalendarDatas.Add(entity1);
                CALENDAR_DOCTOR entity2 = new CALENDAR_DOCTOR();
                entity2.DOCTORS_ID = idDoctor;
                entity2.CALENDAR_DUTY_ID = idCalendarduty;
                if (idColumn != 0)
                    entity2.COLUMN_LEVEL_ID = idColumn;
                entity2.CALENDAR_DATA_ID = entity1.CALENDAR_DATA_ID;
                unitOfWork.CalendarDoctors.Add(entity2);
                objCalendarChange1.CALENDAR_DUTY_ID = idCalendarduty;
                objCalendarChange1.TEMPLATE_COLUMN_ID = idColumn;
                objCalendarChange1.DOCTORS_ID = idDoctor;
                objCalendarChange1.DATE_START = dateTime1;
                CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, typeAction);
                infor.STATUS_APPROVED = 3;
                infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                unitOfWork.CalendarChanges.Update(infor);
                DOCTOR byId = unitOfWork.Doctors.GetById(idDoctor);
                List<SendSms6x00> listSms = new List<SendSms6x00>();
                SendSms6x00 sendSms6x00_2 = new SendSms6x00();
                sendSms6x00_2.Phone = byId.PHONE;
                SendSms6x00 sendSms6x00_3 = sendSms6x00_2;
                string[] strArray1 = new string[16];
                strArray1[0] = "BM-Lichtruc: Xác nhận bổ sung lịch trực ";
                strArray1[1] = byId.EDUCATION_NAMEs;
                strArray1[2] = " ";
                strArray1[3] = byId.DOCTOR_NAME;
                strArray1[4] = " đã bổ sung lịch trực ";
                strArray1[5] = str1;
                strArray1[6] = " lúc ";
                string[] strArray2 = strArray1;
                int index1 = 7;
                int num2 = infor.DATE_CHANGE.Value.Hour;
                string str2 = num2.ToString();
                strArray2[index1] = str2;
                strArray1[8] = ":";
                string[] strArray3 = strArray1;
                int index2 = 9;
                DateTime dateTime2 = infor.DATE_CHANGE.Value;
                num2 = dateTime2.Minute;
                string str3 = num2.ToString();
                strArray3[index2] = str3;
                strArray1[10] = " ngày ";
                string[] strArray4 = strArray1;
                int index3 = 11;
                DateTime? dateChange = infor.DATE_CHANGE;
                dateTime2 = dateChange.Value;
                num2 = dateTime2.Day;
                string str4 = num2.ToString();
                strArray4[index3] = str4;
                strArray1[12] = "/";
                string[] strArray5 = strArray1;
                int index4 = 13;
                dateChange = infor.DATE_CHANGE;
                dateTime2 = dateChange.Value;
                num2 = dateTime2.Month;
                string str5 = num2.ToString();
                strArray5[index4] = str5;
                strArray1[14] = "/";
                string[] strArray6 = strArray1;
                int index5 = 15;
                dateChange = infor.DATE_CHANGE;
                dateTime2 = dateChange.Value;
                num2 = dateTime2.Year;
                string str6 = num2.ToString();
                strArray6[index5] = str6;
                string str7 = string.Concat(strArray1);
                sendSms6x00_3.Contents = str7;
                sendSms6x00_2.Types = "0";
                sendSms6x00_2.DoctorName = byId.DOCTOR_NAME;
                listSms.Add(sendSms6x00_2);
                sendSMSBrandname(listSms);
            }
            WriteLog(enLogType.NomalLog, enActionType.Approve, "Phê duyệt đổi lịch", "N/A", "N/A", 0, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Xác nhận lịch trực thành công!"));
        }

        [CustomAuthorize]
        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [ValidateJsonAntiForgeryToken]
        public ActionResult NotApprovedDoctor(int idCalendarduty, int idDoctor, int idColumn, string idDate, int typeAction)
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            DateTime dateTime = Convert.ToDateTime(idDate.ToString());
            CALENDAR_CHANGE objCalendarChange1 = new CALENDAR_CHANGE();
            objCalendarChange1.CALENDAR_DUTY_ID = idCalendarduty;
            objCalendarChange1.TEMPLATE_COLUMN_ID = idColumn;
            objCalendarChange1.DOCTORS_ID = idDoctor;
            objCalendarChange1.DATE_START = dateTime;
            CALENDAR_CHANGE calendarChange = new CALENDAR_CHANGE();
            CALENDAR_CHANGE entity;
            if (typeAction == 4)
            {
                entity = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 1);
            }
            else
            {
                entity = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, typeAction);
                if (entity == null)
                {
                    objCalendarChange1.TEMPLATE_COLUMN_ID = idColumn;
                    objCalendarChange1.DOCTORS_CHANGE_ID = idDoctor;
                    objCalendarChange1.DATE_CHANGE_START = dateTime;
                    entity = unitOfWork.CalendarChanges.GetInforDoctorChange(objCalendarChange1, typeAction);
                }
            }
            if (entity != null)
            {
                if (typeAction == 1)
                {
                    if (idColumn != 0)
                    {
                        entity.STATUS_APPROVED = 4;
                        entity.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(entity);
                        CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(new CALENDAR_CHANGE()
                        {
                            CALENDAR_DUTY_ID = idCalendarduty,
                            TEMPLATE_COLUMN_ID = idColumn,
                            DOCTORS_ID = Convert.ToInt32(entity.DOCTORS_CHANGE_ID),
                            DATE_START = entity.DATE_CHANGE_START
                        }, typeAction);
                        infor.STATUS_APPROVED = 4;
                        infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(infor);
                    }
                    else
                    {
                        CALENDAR_CHANGE objCalendarChange2 = new CALENDAR_CHANGE();
                        objCalendarChange2.CALENDAR_DUTY_ID = idCalendarduty;
                        objCalendarChange2.DOCTORS_ID = new int?(Convert.ToInt32(entity.DOCTORS_ID));
                        objCalendarChange2.DATE_START = entity.DATE_START;
                        objCalendarChange2.DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(entity.DOCTORS_CHANGE_ID));
                        objCalendarChange2.DATE_CHANGE_START = entity.DATE_CHANGE_START;
                        CALENDAR_CHANGE inforHospital1 = unitOfWork.CalendarChanges.GetInforHospital(objCalendarChange2, typeAction);
                        if (inforHospital1 != null)
                        {
                            inforHospital1.STATUS_APPROVED = 4;
                            inforHospital1.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(inforHospital1);
                            unitOfWork.CalendarChanges.Save();
                        }
                        else
                        {
                            objCalendarChange2.CALENDAR_DUTY_ID = idCalendarduty;
                            objCalendarChange2.DOCTORS_ID = new int?(Convert.ToInt32(entity.DOCTORS_CHANGE_ID));
                            objCalendarChange2.DATE_START = entity.DATE_CHANGE_START;
                            objCalendarChange2.DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(entity.DOCTORS_ID));
                            objCalendarChange2.DATE_CHANGE_START = entity.DATE_START;
                            CALENDAR_CHANGE inforHospital2 = unitOfWork.CalendarChanges.GetInforHospital(objCalendarChange2, typeAction);
                            inforHospital2.STATUS_APPROVED = 4;
                            inforHospital2.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(inforHospital2);
                            unitOfWork.CalendarChanges.Save();
                        }
                    }
                }
                else
                {
                    entity.STATUS_APPROVED = 4;
                    entity.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                    unitOfWork.CalendarChanges.Update(entity);
                }
            }
            WriteLog(enLogType.NomalLog, enActionType.ApproveCancel, "Hủy duyệt đổi lịch", "N/A", "N/A", 0, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Hủy lịch trực thành công!"));
        }

        [HttpPost]
        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ApprovedCalendarDutyHospital(string idCalendarDuty, string types, List<CALENDAR_GROUP> lstApproved)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            int result = 0;
            if (lstApproved != null)
            {
                List<CALENDAR_GROUP> allByDate = unitOfWork.CalendarGroups.GetAllByDate(lstApproved[0]);
                if (int.TryParse(idCalendarDuty, out result))
                {
                    CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
                    if (byId == null)
                        return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
                    byId.CALENDAR_STATUS = 3;
                    byId.USER_CREATE_ID = UserX.ADMIN_USER_ID;
                    byId.DATE_CREATE = DateTime.Now;
                    byId.USER_APPROVED_ID = UserX.ADMIN_USER_ID;
                    byId.DATE_APPROVED = DateTime.Now;
                    unitOfWork.CalendarDuty.Update(byId);
                    WriteLog(enLogType.NomalLog, enActionType.Approve, "Phê duyệt lịch trực toàn bệnh viện", "N/A", "N/A", result, string.Empty, string.Empty);
                }
                List<SendSms6x00> listSms = new List<SendSms6x00>();
                CALENDAR_GROUP objLeader = lstApproved.Where((x =>
               {
                   int? calendarType = x.CALENDAR_TYPE;
                   return calendarType.GetValueOrDefault() == 2 && calendarType.HasValue;
               })).FirstOrDefault<CALENDAR_GROUP>();
                List<DoctorHospital> doctorHospitalList1;
                string str1;
                if (objLeader != null)
                {
                    CALENDAR_GROUP entity = allByDate.Where((x =>
                   {
                       int? calendarType1 = x.CALENDAR_TYPE;
                       int? calendarType2 = objLeader.CALENDAR_TYPE;
                       return calendarType1.GetValueOrDefault() == calendarType2.GetValueOrDefault() && calendarType1.HasValue == calendarType2.HasValue;
                   })).FirstOrDefault<CALENDAR_GROUP>();
                    if (entity != null)
                    {
                        entity.CALENDAR_STATUS = 1;
                        unitOfWork.CalendarGroups.Update(entity);
                        List<int> listIdDoctor = null;
                        doctorHospitalList1 = null;
                        str1 = string.Empty;
                        List<DoctorHospital> doctorHospitalList2 = new List<DoctorHospital>();
                        List<DoctorHospital> doctorHospitalLeader = unitOfWork.CalendarDuty.GetDoctorHospitalLeader(Convert.ToInt32(entity.CALENDAR_MONTH), Convert.ToInt32(entity.CALENDAR_YEAR));
                        listIdDoctor = doctorHospitalLeader.Select((x => x.DOCTORS_ID)).Distinct<int>().ToList();
                        for (int j = 0; j < listIdDoctor.Count; ++j)
                        {
                            string str2 = string.Empty;
                            List<DoctorHospital> list = doctorHospitalLeader.Where((x => x.DOCTORS_ID == listIdDoctor[j])).ToList();
                            foreach (DoctorHospital doctorHospital in list)
                            {
                                object[] objArray1 = new object[5]
                                {
                  str2,
                  "-",
                  null,
                  null,
                  null
                                };
                                object[] objArray2 = objArray1;
                                int index1 = 2;
                                DateTime? dateStart = doctorHospital.DATE_START;
                                int day = dateStart.Value.Day;
                                objArray2[index1] = day;
                                objArray1[3] = "/";
                                object[] objArray3 = objArray1;
                                int index2 = 4;
                                dateStart = doctorHospital.DATE_START;
                                int month = dateStart.Value.Month;
                                objArray3[index2] = month;
                                str2 = string.Concat(objArray1);
                            }
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = list[0].PHONE,
                                Contents = "BM-Lichtruc: Đ/C có lịch trực lãnh đạo các ngày sau " + str2,
                                Types = "0",
                                DoctorName = list[0].DOCTOR_NAME
                            });
                        }
                    }
                }
                if (lstApproved != null && lstApproved.Any())
                {
                    foreach (CALENDAR_GROUP calendarGroup in lstApproved)
                    {
                        CALENDAR_GROUP objlstApproved = calendarGroup;
                        List<CALENDAR_GROUP> list1 = allByDate.Where((x =>
                       {
                           int? calendarType = x.CALENDAR_TYPE;
                           int? nullable = objlstApproved.CALENDAR_TYPE;
                           int num;
                           if ((calendarType.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (calendarType.HasValue == nullable.HasValue ? 1 : 0)) != 0)
                           {
                               int? lmDepartmentId = x.LM_DEPARTMENT_ID;
                               nullable = objlstApproved.LM_DEPARTMENT_ID;
                               num = lmDepartmentId.GetValueOrDefault() != nullable.GetValueOrDefault() ? 0 : (lmDepartmentId.HasValue == nullable.HasValue ? 1 : 0);
                           }
                           else
                               num = 0;
                           return num != 0;
                       })).ToList();
                        if (list1.Count > 0)
                        {
                            foreach (CALENDAR_GROUP entity in list1)
                            {
                                entity.CALENDAR_STATUS = 1;
                                unitOfWork.CalendarGroups.Update(entity);
                                LM_DEPARTMENT lmDepartment = new LM_DEPARTMENT();
                                List<int> listIdDoctor = null;
                                doctorHospitalList1 = null;
                                str1 = string.Empty;
                                LM_DEPARTMENT byId = unitOfWork.Departments.GetById(Convert.ToInt32(entity.LM_DEPARTMENT_ID));
                                List<DoctorHospital> hospitalByDepartment = unitOfWork.CalendarDuty.GetDoctorHospitalByDepartment(Convert.ToInt32(entity.CALENDAR_MONTH), Convert.ToInt32(entity.CALENDAR_YEAR), Convert.ToInt32(entity.LM_DEPARTMENT_ID));
                                listIdDoctor = hospitalByDepartment.Select((x => x.DOCTORS_ID)).Distinct<int>().ToList();
                                for (int j = 0; j < listIdDoctor.Count; ++j)
                                {
                                    string str2 = string.Empty;
                                    List<DoctorHospital> list2 = hospitalByDepartment.Where((x => x.DOCTORS_ID == listIdDoctor[j])).ToList();
                                    foreach (DoctorHospital doctorHospital in list2)
                                    {
                                        object[] objArray1 = new object[5]
                                        {
                      str2,
                      "-",
                      null,
                      null,
                      null
                                        };
                                        object[] objArray2 = objArray1;
                                        int index1 = 2;
                                        DateTime? dateStart = doctorHospital.DATE_START;
                                        int day = dateStart.Value.Day;
                                        objArray2[index1] = day;
                                        objArray1[3] = "/";
                                        object[] objArray3 = objArray1;
                                        int index2 = 4;
                                        dateStart = doctorHospital.DATE_START;
                                        int month = dateStart.Value.Month;

                                        objArray3[index2] = month;
                                        str2 = string.Concat(objArray1);
                                    }
                                    listSms.Add(new SendSms6x00()
                                    {
                                        Phone = list2[0].PHONE,
                                        Contents = "BM-Lichtruc: Đ/C Trực các ngày sau " + str2 + " tại " + byId.DEPARTMENT_CODE,
                                        Types = "0",
                                        DoctorName = list2[0].DOCTOR_NAME
                                    });
                                }
                                listSms.Add(new SendSms6x00()
                                {
                                    Phone = byId.DEPARTMENT_PHONE,
                                    Contents = "Phòng KHTH đã duyệt lịch trực " + byId.DEPARTMENT_CODE + " tháng " + entity.CALENDAR_MONTH + "/" + entity.CALENDAR_YEAR,
                                    Types = "1",
                                    DoctorName = byId.DEPARTMENT_CODE
                                });
                            }
                        }
                    }
                }
                sendSMSBrandname(listSms);
            }
            return null;
        }

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        public ActionResult ApprovedDoctors(List<DoctorApproved> lstDoctorApproved)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            List<SendSms6x00> listSms = new List<SendSms6x00>();
            DateTime now = DateTime.Now;
            if (lstDoctorApproved != null)
            {
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                foreach (DoctorApproved doctorApproved in lstDoctorApproved)
                {
                    DateTime DateStart;
                    try
                    {
                        DateStart = Convert.ToDateTime(doctorApproved.dateStart);
                    }
                    catch
                    {
                        DateStart = DateTime.Now;
                    }
                    DoctorData doctorData = unitOfWork.DoctorDatas.CheckDoctorData(doctorApproved.idCalendarduty, doctorApproved.idDoctor, doctorApproved.idColumn, DateStart);
                    if (doctorData != null)
                    {
                        CALENDAR_CHANGE objCalendarChange = new CALENDAR_CHANGE();
                        objCalendarChange.CALENDAR_DUTY_ID = doctorApproved.idCalendarduty;
                        objCalendarChange.TEMPLATE_COLUMN_ID = doctorApproved.idColumn;
                        objCalendarChange.DOCTORS_ID = doctorApproved.idDoctor;
                        objCalendarChange.DATE_START = DateStart;
                        CALENDAR_CHANGE calendarChange = new CALENDAR_CHANGE();
                        DateTime dateTime;
                        DateTime? dateChange;
                        int num1;
                        if (doctorApproved.typeAction == 1)
                        {
                            CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange, 2);
                            CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                            CALENDAR_DATA byId2 = unitOfWork.CalendarDatas.GetById(doctorData.CALENDAR_DATA_ID);
                            infor.STATUS_APPROVED = 3;
                            infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(infor);
                            SendSms6x00 sendSms6x00_2 = new SendSms6x00();
                            sendSms6x00_2.Phone = byId1.DOCTOR.PHONE;
                            SendSms6x00 sendSms6x00_3 = sendSms6x00_2;
                            string[] strArray1 = new string[14];
                            strArray1[0] = "BM-Lichtruc: Đã xác nhận hủy lịch trực ";
                            strArray1[1] = byId1.DOCTOR.EDUCATION_NAMEs;
                            strArray1[2] = " ";
                            strArray1[3] = byId1.DOCTOR.DOCTOR_NAME;
                            strArray1[4] = "  đã hủy trực lúc ";
                            string[] strArray2 = strArray1;
                            int index1 = 5;
                            dateTime = infor.DATE_CHANGE.Value;
                            string str1 = dateTime.Hour.ToString();
                            strArray2[index1] = str1;
                            strArray1[6] = ":";
                            string[] strArray3 = strArray1;
                            int index2 = 7;
                            dateTime = infor.DATE_CHANGE.Value;
                            string str2 = dateTime.Minute.ToString();
                            strArray3[index2] = str2;
                            strArray1[8] = " ngày ";
                            string[] strArray4 = strArray1;
                            int index3 = 9;
                            dateTime = infor.DATE_CHANGE.Value;
                            string str3 = dateTime.Day.ToString();
                            strArray4[index3] = str3;
                            strArray1[10] = "/";
                            string[] strArray5 = strArray1;
                            int index4 = 11;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            string str4 = dateTime.Month.ToString();
                            strArray5[index4] = str4;
                            strArray1[12] = "/";
                            string[] strArray6 = strArray1;
                            int index5 = 13;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            num1 = dateTime.Year;
                            string str5 = num1.ToString();
                            strArray6[index5] = str5;
                            string str6 = string.Concat(strArray1);
                            sendSms6x00_3.Contents = str6;
                            sendSms6x00_2.Types = "0";
                            sendSms6x00_2.DoctorName = byId1.DOCTOR.DOCTOR_NAME;
                            listSms.Add(sendSms6x00_2);
                            unitOfWork.CalendarDoctors.Delete(byId1);
                            unitOfWork.CalendarDatas.Delete(byId2);
                        }
                        if (doctorApproved.typeAction == 2)
                        {
                            CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange, 1);
                            CALENDAR_DOCTOR byId = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                            byId.DOCTORS_ID = Convert.ToInt32(infor.DOCTORS_CHANGE_ID);
                            unitOfWork.CalendarDoctors.Update(byId);
                            infor.STATUS_APPROVED = 3;
                            infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(infor);
                            string[] strArray1 = new string[17];
                            strArray1[0] = byId.DOCTOR.EDUCATION_NAMEs;
                            strArray1[1] = " ";
                            strArray1[2] = byId.DOCTOR.DOCTOR_NAME;
                            strArray1[3] = " đã đổi lịch trực cho ";
                            strArray1[4] = infor.DOCTOR.EDUCATION_NAMEs;
                            strArray1[5] = " ";
                            strArray1[6] = infor.DOCTOR.DOCTOR_NAME;
                            strArray1[7] = " lúc ";
                            string[] strArray2 = strArray1;
                            int index1 = 8;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            num1 = dateTime.Hour;
                            string str1 = num1.ToString();
                            strArray2[index1] = str1;
                            strArray1[9] = ":";
                            string[] strArray3 = strArray1;
                            int index2 = 10;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            num1 = dateTime.Minute;
                            string str2 = num1.ToString();
                            strArray3[index2] = str2;
                            strArray1[11] = " ngày ";
                            string[] strArray4 = strArray1;
                            int index3 = 12;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            num1 = dateTime.Day;
                            string str3 = num1.ToString();
                            strArray4[index3] = str3;
                            strArray1[13] = "/";
                            string[] strArray5 = strArray1;
                            int index4 = 14;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            num1 = dateTime.Month;
                            string str4 = num1.ToString();
                            strArray5[index4] = str4;
                            strArray1[15] = "/";
                            string[] strArray6 = strArray1;
                            int index5 = 16;
                            dateChange = infor.DATE_CHANGE;
                            dateTime = dateChange.Value;
                            num1 = dateTime.Year;
                            string str5 = num1.ToString();
                            strArray6[index5] = str5;
                            string str6 = string.Concat(strArray1);
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = byId.DOCTOR.PHONE,
                                Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                Types = "0",
                                DoctorName = byId.DOCTOR.DOCTOR_NAME
                            });
                            listSms.Add(new SendSms6x00()
                            {
                                Phone = infor.DOCTOR.PHONE,
                                Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                Types = "0",
                                DoctorName = infor.DOCTOR.DOCTOR_NAME
                            });
                        }
                        int num2 = 0;
                        if (doctorApproved.typeAction == 22)
                        {
                            CALENDAR_CHANGE entity1 = unitOfWork.CalendarChanges.GetInfor(objCalendarChange, 1);
                            if (entity1 == null)
                            {
                                num2 = 1;
                                objCalendarChange.TEMPLATE_COLUMN_ID = doctorApproved.idColumn;
                                objCalendarChange.DOCTORS_CHANGE_ID = doctorApproved.idDoctor;
                                objCalendarChange.DATE_CHANGE_START = DateStart;
                                entity1 = unitOfWork.CalendarChanges.GetInforDoctorChange(objCalendarChange, 1);
                            }
                            if (entity1 != null)
                            {
                                if (doctorApproved.idColumn != 0)
                                {
                                    CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                                    byId1.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_CHANGE_ID);
                                    unitOfWork.CalendarDoctors.Update(byId1);
                                    entity1.STATUS_APPROVED = 3;
                                    entity1.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                    unitOfWork.CalendarChanges.Update(entity1);
                                    CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(Convert.ToInt32(unitOfWork.DoctorDatas.CheckDoctorData(doctorApproved.idCalendarduty, Convert.ToInt32(entity1.DOCTORS_CHANGE_ID), Convert.ToInt32(entity1.COLUMN_CHANGE_ID), Convert.ToDateTime(entity1.DATE_CHANGE_START)).CALENDAR_DOCTOR_ID));
                                    byId2.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_ID);
                                    unitOfWork.CalendarDoctors.Update(byId2);
                                    unitOfWork.CalendarChanges.Save();
                                    CALENDAR_CHANGE entity2 = unitOfWork.CalendarChanges.GetChangeByIdDuty(entity1.CALENDAR_CHANGE_ID) ?? unitOfWork.CalendarChanges.GetById(Convert.ToInt32(entity1.CALENDAR_DELETE));
                                    entity2.STATUS_APPROVED = 3;
                                    entity2.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                    unitOfWork.CalendarChanges.Update(entity2);
                                    unitOfWork.CalendarChanges.Save();
                                    string[] strArray1 = new string[17];
                                    strArray1[0] = byId1.DOCTOR.EDUCATION_NAMEs;
                                    strArray1[1] = " ";
                                    strArray1[2] = byId1.DOCTOR.DOCTOR_NAME;
                                    strArray1[3] = " đã đổi lịch trực cho ";
                                    strArray1[4] = entity1.DOCTOR.EDUCATION_NAMEs;
                                    strArray1[5] = " ";
                                    strArray1[6] = entity1.DOCTOR.DOCTOR_NAME;
                                    strArray1[7] = " lúc ";
                                    string[] strArray2 = strArray1;
                                    int index1 = 8;
                                    dateChange = entity1.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Hour;
                                    string str1 = num1.ToString();
                                    strArray2[index1] = str1;
                                    strArray1[9] = ":";
                                    string[] strArray3 = strArray1;
                                    int index2 = 10;
                                    dateChange = entity1.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Minute;
                                    string str2 = num1.ToString();
                                    strArray3[index2] = str2;
                                    strArray1[11] = " ngày ";
                                    string[] strArray4 = strArray1;
                                    int index3 = 12;
                                    dateChange = entity1.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Day;
                                    string str3 = num1.ToString();
                                    strArray4[index3] = str3;
                                    strArray1[13] = "/";
                                    string[] strArray5 = strArray1;
                                    int index4 = 14;
                                    dateChange = entity1.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Month;
                                    string str4 = num1.ToString();
                                    strArray5[index4] = str4;
                                    strArray1[15] = "/";
                                    string[] strArray6 = strArray1;
                                    int index5 = 16;
                                    dateChange = entity1.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Year;
                                    string str5 = num1.ToString();
                                    strArray6[index5] = str5;
                                    string str6 = string.Concat(strArray1);
                                    listSms.Add(new SendSms6x00()
                                    {
                                        Phone = byId1.DOCTOR.PHONE,
                                        Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                        Types = "0",
                                        DoctorName = byId1.DOCTOR.DOCTOR_NAME
                                    });
                                    listSms.Add(new SendSms6x00()
                                    {
                                        Phone = entity1.DOCTOR.PHONE,
                                        Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                        Types = "0",
                                        DoctorName = entity1.DOCTOR.DOCTOR_NAME
                                    });
                                }
                                else if (num2 == 0)
                                {
                                    CALENDAR_CHANGE inforHospital = unitOfWork.CalendarChanges.GetInforHospital(new CALENDAR_CHANGE()
                                    {
                                        CALENDAR_DUTY_ID = doctorApproved.idCalendarduty,
                                        DOCTORS_ID = new int?(Convert.ToInt32(entity1.DOCTORS_ID)),
                                        DATE_START = entity1.DATE_START,
                                        DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(entity1.DOCTORS_CHANGE_ID)),
                                        DATE_CHANGE_START = entity1.DATE_CHANGE_START
                                    }, 1);
                                    if (inforHospital != null)
                                    {
                                        inforHospital.STATUS_APPROVED = 3;
                                        inforHospital.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                        unitOfWork.CalendarChanges.Update(inforHospital);
                                        unitOfWork.CalendarChanges.Save();
                                        CALENDAR_DOCTOR byId1 = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                                        byId1.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_CHANGE_ID);
                                        unitOfWork.CalendarDoctors.Update(byId1);
                                        CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(Convert.ToInt32(unitOfWork.DoctorDatas.CheckDoctorData(doctorApproved.idCalendarduty, Convert.ToInt32(entity1.DOCTORS_CHANGE_ID), doctorApproved.idColumn, Convert.ToDateTime(entity1.DATE_CHANGE_START)).CALENDAR_DOCTOR_ID));
                                        byId2.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_ID);
                                        unitOfWork.CalendarDoctors.Update(byId2);
                                        string[] strArray1 = new string[17];
                                        strArray1[0] = entity1.DOCTOR.EDUCATION_NAMEs;
                                        strArray1[1] = " ";
                                        strArray1[2] = entity1.DOCTOR.DOCTOR_NAME;
                                        strArray1[3] = " đã đổi lịch trực cho ";
                                        strArray1[4] = byId1.DOCTOR.EDUCATION_NAMEs;
                                        strArray1[5] = " ";
                                        strArray1[6] = byId1.DOCTOR.DOCTOR_NAME;
                                        strArray1[7] = " lúc ";
                                        string[] strArray2 = strArray1;
                                        int index1 = 8;
                                        dateChange = inforHospital.DATE_CHANGE;
                                        dateTime = dateChange.Value;
                                        num1 = dateTime.Hour;
                                        string str1 = num1.ToString();
                                        strArray2[index1] = str1;
                                        strArray1[9] = ":";
                                        string[] strArray3 = strArray1;
                                        int index2 = 10;
                                        dateChange = inforHospital.DATE_CHANGE;
                                        dateTime = dateChange.Value;
                                        num1 = dateTime.Minute;
                                        string str2 = num1.ToString();
                                        strArray3[index2] = str2;
                                        strArray1[11] = " ngày ";
                                        string[] strArray4 = strArray1;
                                        int index3 = 12;
                                        dateChange = inforHospital.DATE_CHANGE;
                                        dateTime = dateChange.Value;
                                        num1 = dateTime.Day;
                                        string str3 = num1.ToString();
                                        strArray4[index3] = str3;
                                        strArray1[13] = "/";
                                        string[] strArray5 = strArray1;
                                        int index4 = 14;
                                        dateChange = inforHospital.DATE_CHANGE;
                                        dateTime = dateChange.Value;
                                        num1 = dateTime.Month;
                                        string str4 = num1.ToString();
                                        strArray5[index4] = str4;
                                        strArray1[15] = "/";
                                        string[] strArray6 = strArray1;
                                        int index5 = 16;
                                        dateChange = inforHospital.DATE_CHANGE;
                                        dateTime = dateChange.Value;
                                        num1 = dateTime.Year;
                                        string str5 = num1.ToString();
                                        strArray6[index5] = str5;
                                        string str6 = string.Concat(strArray1);
                                        listSms.Add(new SendSms6x00()
                                        {
                                            Phone = entity1.DOCTOR.PHONE,
                                            Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                            Types = "0",
                                            DoctorName = entity1.DOCTOR.DOCTOR_NAME
                                        });
                                        listSms.Add(new SendSms6x00()
                                        {
                                            Phone = byId1.DOCTOR.PHONE,
                                            Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                            Types = "0",
                                            DoctorName = byId1.DOCTOR.DOCTOR_NAME
                                        });
                                    }
                                }
                                else
                                {
                                    CALENDAR_CHANGE inforHospital = unitOfWork.CalendarChanges.GetInforHospital(new CALENDAR_CHANGE()
                                    {
                                        CALENDAR_DUTY_ID = doctorApproved.idCalendarduty,
                                        DOCTORS_ID = new int?(Convert.ToInt32(entity1.DOCTORS_ID)),
                                        DATE_START = entity1.DATE_START,
                                        DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(entity1.DOCTORS_CHANGE_ID)),
                                        DATE_CHANGE_START = entity1.DATE_CHANGE_START
                                    }, 1);
                                    CALENDAR_DOCTOR entity2 = new CALENDAR_DOCTOR();
                                    if (inforHospital != null)
                                    {
                                        inforHospital.STATUS_APPROVED = 3;
                                        inforHospital.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                        unitOfWork.CalendarChanges.Update(inforHospital);
                                        unitOfWork.CalendarChanges.Save();
                                        CALENDAR_DOCTOR byId = unitOfWork.CalendarDoctors.GetById(doctorData.CALENDAR_DOCTOR_ID);
                                        byId.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_ID);
                                        unitOfWork.CalendarDoctors.Update(byId);
                                        entity2 = unitOfWork.CalendarDoctors.GetById(Convert.ToInt32(unitOfWork.DoctorDatas.CheckDoctorData(doctorApproved.idCalendarduty, Convert.ToInt32(entity1.DOCTORS_ID), doctorApproved.idColumn, Convert.ToDateTime(entity1.DATE_START)).CALENDAR_DOCTOR_ID));
                                        entity2.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_CHANGE_ID);
                                        unitOfWork.CalendarDoctors.Update(entity2);
                                    }
                                    string[] strArray1 = new string[17];
                                    strArray1[0] = entity2.DOCTOR.EDUCATION_NAMEs;
                                    strArray1[1] = " ";
                                    strArray1[2] = entity2.DOCTOR.DOCTOR_NAME;
                                    strArray1[3] = " đã đổi lịch trực cho ";
                                    strArray1[4] = inforHospital.DOCTOR.EDUCATION_NAMEs;
                                    strArray1[5] = " ";
                                    strArray1[6] = inforHospital.DOCTOR.DOCTOR_NAME;
                                    strArray1[7] = " lúc ";
                                    string[] strArray2 = strArray1;
                                    int index1 = 8;
                                    dateChange = inforHospital.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Hour;
                                    string str1 = num1.ToString();
                                    strArray2[index1] = str1;
                                    strArray1[9] = ":";
                                    string[] strArray3 = strArray1;
                                    int index2 = 10;
                                    dateChange = inforHospital.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Minute;
                                    string str2 = num1.ToString();
                                    strArray3[index2] = str2;
                                    strArray1[11] = " ngày ";
                                    string[] strArray4 = strArray1;
                                    int index3 = 12;
                                    dateChange = inforHospital.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Day;
                                    string str3 = num1.ToString();
                                    strArray4[index3] = str3;
                                    strArray1[13] = "/";
                                    string[] strArray5 = strArray1;
                                    int index4 = 14;
                                    dateChange = inforHospital.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Month;
                                    string str4 = num1.ToString();
                                    strArray5[index4] = str4;
                                    strArray1[15] = "/";
                                    string[] strArray6 = strArray1;
                                    int index5 = 16;
                                    dateChange = inforHospital.DATE_CHANGE;
                                    dateTime = dateChange.Value;
                                    num1 = dateTime.Year;
                                    string str5 = num1.ToString();
                                    strArray6[index5] = str5;
                                    string str6 = string.Concat(strArray1);
                                    listSms.Add(new SendSms6x00()
                                    {
                                        Phone = entity2.DOCTOR.PHONE,
                                        Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                        Types = "0",
                                        DoctorName = entity2.DOCTOR.DOCTOR_NAME
                                    });
                                    listSms.Add(new SendSms6x00()
                                    {
                                        Phone = inforHospital.DOCTOR.PHONE,
                                        Contents = "BM-Lichtruc: Đã xác nhận đổi lịch " + str6,
                                        Types = "0",
                                        DoctorName = inforHospital.DOCTOR.DOCTOR_NAME
                                    });
                                }
                            }
                        }
                    }
                    else if (doctorApproved.typeAction == 3)
                    {
                        CALENDAR_DATA entity1 = new CALENDAR_DATA();
                        entity1.CALENDAR_DUTY_ID = doctorApproved.idCalendarduty;
                        if (doctorApproved.idColumn != 0)
                            entity1.TEMPLATE_COLUM_ID = doctorApproved.idColumn;
                        entity1.DATE_START = DateStart;
                        entity1.ISDELETE = false;
                        unitOfWork.CalendarDatas.Add(entity1);
                        CALENDAR_DOCTOR entity2 = new CALENDAR_DOCTOR();
                        entity2.DOCTORS_ID = doctorApproved.idDoctor;
                        entity2.CALENDAR_DUTY_ID = doctorApproved.idCalendarduty;
                        if (doctorApproved.idColumn != 0)
                            entity2.COLUMN_LEVEL_ID = doctorApproved.idColumn;
                        entity2.CALENDAR_DATA_ID = entity1.CALENDAR_DATA_ID;
                        unitOfWork.CalendarDoctors.Add(entity2);
                        CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(new CALENDAR_CHANGE()
                        {
                            CALENDAR_DUTY_ID = doctorApproved.idCalendarduty,
                            TEMPLATE_COLUMN_ID = doctorApproved.idColumn,
                            DOCTORS_ID = doctorApproved.idDoctor,
                            DATE_START = DateStart
                        }, 3);
                        infor.STATUS_APPROVED = 3;
                        infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(infor);
                        DOCTOR byId = unitOfWork.Doctors.GetById(doctorApproved.idDoctor);
                        SendSms6x00 sendSms6x00_2 = new SendSms6x00();
                        sendSms6x00_2.Phone = byId.PHONE;
                        SendSms6x00 sendSms6x00_3 = sendSms6x00_2;
                        string[] strArray1 = new string[14]
                        {
              "BM-Lichtruc: Đã xác nhận bổ sung lịch trực ",
              byId.EDUCATION_NAMEs,
              " ",
              byId.DOCTOR_NAME,
              " đã bổ sung lịch trực lúc ",
              infor.DATE_CHANGE.Value.Hour.ToString(),
              ":",
              infor.DATE_CHANGE.Value.Minute.ToString(),
              " ngày ",
              null,
              null,
              null,
              null,
              null
                        };
                        string[] strArray2 = strArray1;
                        int index1 = 9;
                        DateTime dateTime = infor.DATE_CHANGE.Value;
                        string str1 = dateTime.Day.ToString();
                        strArray2[index1] = str1;
                        strArray1[10] = "/";
                        string[] strArray3 = strArray1;
                        int index2 = 11;
                        DateTime? dateChange = infor.DATE_CHANGE;
                        dateTime = dateChange.Value;
                        string str2 = dateTime.Month.ToString();
                        strArray3[index2] = str2;
                        strArray1[12] = "/";
                        string[] strArray4 = strArray1;
                        int index3 = 13;
                        dateChange = infor.DATE_CHANGE;
                        dateTime = dateChange.Value;
                        string str3 = dateTime.Year.ToString();
                        strArray4[index3] = str3;
                        string str4 = string.Concat(strArray1);
                        sendSms6x00_3.Contents = str4;
                        sendSms6x00_2.Types = "0";
                        sendSms6x00_2.DoctorName = byId.DOCTOR_NAME;
                        listSms.Add(sendSms6x00_2);
                    }
                }
                sendSMSBrandname(listSms);
            }
            return null;
        }

        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [HttpPost]
        public ActionResult NoApprovedDoctors(List<DoctorApproved> lstDoctorApproved)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            List<SendSms6x00> sendSms6x00List = new List<SendSms6x00>();
            DateTime now = DateTime.Now;
            CALENDAR_CHANGE calendarChange = new CALENDAR_CHANGE();
            if (lstDoctorApproved != null)
            {
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                foreach (DoctorApproved doctorApproved in lstDoctorApproved)
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = Convert.ToDateTime(doctorApproved.dateStart);
                    }
                    catch
                    {
                        dateTime = DateTime.Now;
                    }
                    CALENDAR_CHANGE objCalendarChange1 = new CALENDAR_CHANGE();
                    objCalendarChange1.CALENDAR_DUTY_ID = doctorApproved.idCalendarduty;
                    objCalendarChange1.TEMPLATE_COLUMN_ID = doctorApproved.idColumn;
                    objCalendarChange1.DOCTORS_ID = doctorApproved.idDoctor;
                    objCalendarChange1.DATE_START = dateTime;
                    if (doctorApproved.typeAction == 1)
                    {
                        CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 2);
                        infor.STATUS_APPROVED = 4;
                        infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(infor);
                    }
                    if (doctorApproved.typeAction == 3)
                    {
                        CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 3);
                        infor.STATUS_APPROVED = 4;
                        infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(infor);
                    }
                    if (doctorApproved.typeAction == 2)
                    {
                        CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 1);
                        infor.STATUS_APPROVED = 4;
                        infor.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                        unitOfWork.CalendarChanges.Update(infor);
                    }
                    if (doctorApproved.typeAction == 22)
                    {
                        if (doctorApproved.idColumn != 0)
                        {
                            CALENDAR_CHANGE infor1 = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 1);
                            infor1.STATUS_APPROVED = 4;
                            infor1.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(infor1);
                            CALENDAR_CHANGE infor2 = unitOfWork.CalendarChanges.GetInfor(new CALENDAR_CHANGE()
                            {
                                CALENDAR_DUTY_ID = doctorApproved.idCalendarduty,
                                TEMPLATE_COLUMN_ID = infor1.COLUMN_CHANGE_ID,
                                DOCTORS_ID = new int?(Convert.ToInt32(infor1.DOCTORS_CHANGE_ID)),
                                DATE_START = infor1.DATE_CHANGE_START
                            }, 1);
                            infor2.STATUS_APPROVED = 4;
                            infor2.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                            unitOfWork.CalendarChanges.Update(infor2);
                        }
                        else
                        {
                            CALENDAR_CHANGE infor = unitOfWork.CalendarChanges.GetInfor(objCalendarChange1, 1);
                            CALENDAR_CHANGE objCalendarChange2 = new CALENDAR_CHANGE();
                            objCalendarChange2.CALENDAR_DUTY_ID = doctorApproved.idCalendarduty;
                            objCalendarChange2.DOCTORS_ID = new int?(Convert.ToInt32(infor.DOCTORS_ID));
                            objCalendarChange2.DATE_START = infor.DATE_START;
                            objCalendarChange2.DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(infor.DOCTORS_CHANGE_ID));
                            objCalendarChange2.DATE_CHANGE_START = infor.DATE_CHANGE_START;
                            CALENDAR_CHANGE inforHospital1 = unitOfWork.CalendarChanges.GetInforHospital(objCalendarChange2, 1);
                            if (inforHospital1 != null)
                            {
                                inforHospital1.STATUS_APPROVED = 4;
                                inforHospital1.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                unitOfWork.CalendarChanges.Update(inforHospital1);
                                unitOfWork.CalendarChanges.Save();
                            }
                            else
                            {
                                objCalendarChange2.CALENDAR_DUTY_ID = doctorApproved.idCalendarduty;
                                objCalendarChange2.DOCTORS_ID = new int?(Convert.ToInt32(infor.DOCTORS_CHANGE_ID));
                                objCalendarChange2.DATE_START = infor.DATE_CHANGE_START;
                                objCalendarChange2.DOCTORS_CHANGE_ID = new int?(Convert.ToInt32(infor.DOCTORS_ID));
                                objCalendarChange2.DATE_CHANGE_START = infor.DATE_START;
                                CALENDAR_CHANGE inforHospital2 = unitOfWork.CalendarChanges.GetInforHospital(objCalendarChange2, 1);
                                inforHospital2.STATUS_APPROVED = 4;
                                inforHospital2.USER_NOT_APPROVED = byUserName.ADMIN_USER_ID;
                                unitOfWork.CalendarChanges.Update(inforHospital2);
                                unitOfWork.CalendarChanges.Save();
                            }
                        }
                    }
                }
            }
            return null;
        }

        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [HttpGet]
        public ActionResult SendSMSHospital(string deparmentIdx)
        {
            int result = 0;
            DateTime now = DateTime.Now;
            int.TryParse(deparmentIdx, out result);
            LM_DEPARTMENT byId = unitOfWork.Departments.GetById(result);
            sendSMSBrandname(new List<SendSms6x00>()
      {
        new SendSms6x00()
        {
          Phone = byId.DEPARTMENT_PHONE,
          Contents = "Phòng KHTH yêu cầu đơn vị " + byId.DEPARTMENT_CODE + " gửi lịch trực tháng " + now.Month + "/" + now.Year + " đúng thời gian quy định",
          Types = "1",
          DoctorName = byId.DEPARTMENT_CODE
        }
      });
            WriteLog(enLogType.NomalLog, enActionType.SendSMS, "Gửi nhắc lịch trực đơn vị ", byId.DEPARTMENT_NAME, "N/A", 0, string.Empty, string.Empty);
            return Json("Gửi nhắc lịch thành công", JsonRequestBehavior.AllowGet);
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_View", ActionName = "Tổng hợp lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SendSMSHospitalLeader()
        {
            DateTime now = DateTime.Now;
            sendSMSBrandname(new List<SendSms6x00>()
      {
        new SendSms6x00()
        {
          Phone = ConfigurationManager.AppSettings["smsPhone"],
          Contents = "Phòng KHTH yêu cầu gửi lịch trực lãnh đạo tháng " + now.Month + "/" + now.Year + " đúng thời gian quy định",
          Types = "1",
          DoctorName = "Lịch lãnh đạo"
        }
      });
            WriteLog(enLogType.NomalLog, enActionType.SendSMS, "Gửi nhắc lịch lãnh đạo ", "N/A", "N/A", 0, string.Empty, string.Empty);
            return Json("Gửi nhắc lịch thành công", JsonRequestBehavior.AllowGet);
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        public ActionResult ExportCalendarHospital(string strDate)
        {
            int monthx = 0;
            int yearx = 0;
            int result3 = 0;
            int result4 = 0;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                int.TryParse(strArray[0].ToString(), out result3);
                int.TryParse(strArray[1].ToString(), out monthx);
                int.TryParse(strArray[2].ToString(), out yearx);
                if ((strArray).Count<string>() > 3)
                    int.TryParse(strArray[3], out result4);
            }
            FileInfo fileInfo = new FileInfo(Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfAllHospital.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Export");
            List<TimeCalendarDuty> times = new List<TimeCalendarDuty>();
            DateTime dateTime1 = Convert.ToDateTime("1/" + monthx + "/" + yearx);
            DateTime dateTime2;
            if (result3 == -1)
            {
                DateTime dateTime3 = dateTime1.AddDays((-dateTime1.Day + 1));
                dateTime2 = dateTime1.AddMonths(1);
                DateTime dateTime4 = dateTime2.AddDays(-dateTime1.Day);
                for (int index = 0; index < dateTime4.Day; ++index)
                {
                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                    timeCalendarDuty.DATES = dateTime3.AddDays(index);
                    timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                    times.Add(timeCalendarDuty);
                }
            }
            else if (result3 < 3)
            {
                int num = result3 * 7;
                dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1));
                DateTime dateTime3 = dateTime2.AddDays(num);
                for (int index = 0; index < 7; ++index)
                {
                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                    timeCalendarDuty.DATES = dateTime3.AddDays(index);
                    timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                    times.Add(timeCalendarDuty);
                }
            }
            else
            {
                int num1 = DateTime.DaysInMonth(yearx, monthx) - 28;
                int num2 = result3 * 7;
                DateTime dateTime3 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                int num3 = num1 + 7;
                for (int index = 0; index < num3; ++index)
                {
                    TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                    timeCalendarDuty.DATES = dateTime3.AddDays(index);
                    timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                    times.Add(timeCalendarDuty);
                }
            }
            byte[] asByteArray;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
                ExcelRange excelRange1 = excelWorksheet.Cells["C1"];
                object[] objArray1 = new object[10];
                objArray1[0] = "BẢNG TRỰC TOÀN BỆNH VIỆN TỪ ";
                object[] objArray2 = objArray1;
                int index1 = 1;
                dateTime2 = times[0].DATES;
                int day1 = dateTime2.Day;
                objArray2[index1] = day1;
                objArray1[2] = "-";
                objArray1[3] = monthx.ToString();
                objArray1[4] = " ĐẾN ";
                object[] objArray3 = objArray1;
                int index2 = 5;
                dateTime2 = times[times.Count - 1].DATES;
                string str1 = dateTime2.Day.ToString();
                objArray3[index2] = str1;
                objArray1[6] = "-";
                objArray1[7] = monthx.ToString();
                objArray1[8] = "-";
                objArray1[9] = yearx.ToString();
                string str2 = string.Concat(objArray1);
                (excelRange1).Value = str2;
                for (int index3 = 0; index3 < times.Count; ++index3)
                {
                    if (times[index3].DAYS.Equals("CN") || times[index3].DAYS.Equals("T7"))
                    {
                        (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[4, index3 + 3]).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        (excelWorksheet.Cells[4, index3 + 3]).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }
                  (excelWorksheet.Cells[4, index3 + 3]).Value = (times[index3].DAYS + "-" + string.Format("{0:dd/MM}", times[index3].DATES).ToString());
                    (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[4, index3 + 3]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[4, index3 + 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[4, index3 + 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                List<DEPARTMENTLIST> objDepartment = unitOfWork.Departments.GetAllDepartmentByLevelVT(LevelDeparment.Level1.GetHashCode());
                List<CALENDAR_GROUP> byDateIsApproved = unitOfWork.CalendarGroups.GetAllByDateIsApproved(monthx, yearx);
                int num1 = 0;
                List<DoctorHospital> doctorHospitalList = new List<DoctorHospital>();
                List<DoctorHospital> list1 = unitOfWork.CalendarDuty.GetDoctorHospital(monthx, yearx).Where((x => Convert.ToInt32(x.LEVEL_NUMBER) < 3)).OrderBy((x => x.DOCTOR_NAME)).ToList();
                if (byDateIsApproved.Where((x =>
               {
                   int? calendarType = x.CALENDAR_TYPE;
                   return calendarType.GetValueOrDefault() == 2 && calendarType.HasValue;
               })).FirstOrDefault<CALENDAR_GROUP>() != null)
                {
                    for (int i = 0; i < times.Count; ++i)
                    {
                        List<DoctorHospital> list2 = list1.Where((o =>
                       {
                           int? dutyType = o.DUTY_TYPE;
                           int num;
                           if ((dutyType.GetValueOrDefault() != 1 ? 0 : (dutyType.HasValue ? 1 : 0)) != 0)
                           {
                               DateTime dates = o.DATE_START.Value;
                               int day = dates.Day;
                               dates = times[i].DATES;
                               int day2 = dates.Day;
                               if (day == day2)
                               {
                                   dates = o.DATE_START.Value;
                                   int month1 = dates.Month;
                                   dates = times[i].DATES;
                                   int month2 = dates.Month;
                                   if (month1 == month2)
                                   {
                                       dates = o.DATE_START.Value;
                                       int year1 = dates.Year;
                                       dates = times[i].DATES;
                                       int year2 = dates.Year;
                                       num = year1 == year2 ? 1 : 0;
                                       goto label_5;
                                   }
                               }
                           }
                           num = 0;
                           label_5:
                           return num != 0;
                       })).GroupBy<DoctorHospital, string>((x => x.ABBREVIATION)).Select((Func<IGrouping<string, DoctorHospital>, DoctorHospital>)(x => x.First())).ToList();
                        if (times[i].DAYS.Equals("CN") || times[i].DAYS.Equals("T7"))
                        {
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Fill.PatternType = ExcelFillStyle.Solid;
                            (excelWorksheet.Cells[5, i + 3]).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            (excelWorksheet.Cells[5, i + 3]).IsRichText = true;
                            (excelWorksheet.Cells[5, i + 3]).Style.WrapText = true;
                            ExcelRichTextCollection richText = (excelWorksheet.Cells[5, i + 3]).RichText;
                            foreach (DoctorHospital doctorHospital in list2)
                            {
                                DoctorHospital objDoctor = doctorHospital;
                                List<DEPARTMENTLIST> list3 = objDepartment.Where((x => objDoctor.LM_DEPARTMENT_IDs.Contains(x.LM_DEPARTMENT_ID.ToString()))).ToList();
                                if (list3 != null)
                                {
                                    richText.Add(objDoctor.ABBREVIATION + "\r\n").Bold = true;
                                    for (int index3 = 0; index3 < list3.Count; ++index3)
                                        richText.Add(list3[index3].DEPARTMENT_NAME + "\r\n").Bold = false;
                                }
                                else
                                    richText.Add(objDoctor.ABBREVIATION + "\r\n").Bold = true;
                            }
                          (excelWorksheet.Cells[5, i + 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            (excelWorksheet.Cells[5, i + 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            (excelWorksheet.Cells[5, i + 3]).IsRichText = true;
                            (excelWorksheet.Cells[5, i + 3]).Style.WrapText = true;
                            ExcelRichTextCollection richText = (excelWorksheet.Cells[5, i + 3]).RichText;
                            foreach (DoctorHospital doctorHospital in list2)
                            {
                                DoctorHospital objDoctor = doctorHospital;
                                List<DEPARTMENTLIST> list3 = objDepartment.Where((x => objDoctor.LM_DEPARTMENT_IDs.Contains(x.LM_DEPARTMENT_ID.ToString()))).ToList();
                                if (list3 != null)
                                {
                                    richText.Add(objDoctor.ABBREVIATION + "\r\n").Bold = true;
                                    for (int index3 = 0; index3 < list3.Count; ++index3)
                                        richText.Add(list3[index3].DEPARTMENT_NAME + "\r\n").Bold = false;
                                }
                                else
                                    richText.Add(objDoctor.ABBREVIATION + "\r\n").Bold = true;
                            }
                          (excelWorksheet.Cells[5, i + 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            (excelWorksheet.Cells[5, i + 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                    }
                }
                else
                {
                    (excelWorksheet.Cells[5, 2]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 2]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 2]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 2]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 1]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 1]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 1]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 1]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 1]).Value = (num1 + 1).ToString();
                    (excelWorksheet.Cells[5, 1]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[5, 1]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    (excelWorksheet.Cells[5, 2]).Value = "LÃNH ĐẠO";
                    (excelWorksheet.Cells[5, 2]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    (excelWorksheet.Cells[5, 2]).Style.Font.Bold = true;
                    (excelWorksheet.Cells[5, 3, 5, times.Count + 2]).Merge = true;
                    (excelWorksheet.Cells[5, 3, 5, times.Count + 2]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 3, 5, times.Count + 2]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 3, 5, times.Count + 2]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    (excelWorksheet.Cells[5, 3, 5, times.Count + 2]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                for (int i = 0; i < objDepartment.Count; ++i)
                {
                    if (byDateIsApproved.Where((x =>
                   {
                       int? lmDepartmentId1 = x.LM_DEPARTMENT_ID;
                       int lmDepartmentId2 = objDepartment[i].LM_DEPARTMENT_ID;
                       return lmDepartmentId1.GetValueOrDefault() == lmDepartmentId2 && lmDepartmentId1.HasValue;
                   })).FirstOrDefault<CALENDAR_GROUP>() != null)
                    {
                        ++num1;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Value = (num1 + 1).ToString();
                        (excelWorksheet.Cells[5 + num1, 1]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        (excelWorksheet.Cells[5 + num1, 2]).Value = objDepartment[i].DEPARTMENT_NAME.ToUpper();
                        (excelWorksheet.Cells[5 + num1, 2]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Font.Bold = true;
                        ExcelRichText excelRichText;
                        for (int j = 0; j < times.Count; ++j)
                        {
                            if (times[j].DAYS.Equals("CN") || times[j].DAYS.Equals("T7"))
                            {
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                List<DoctorHospital> list2 = list1.Where((o =>
                               {
                                   int num;
                                   if (o.LM_DEPARTMENT_ID == objDepartment[i].LM_DEPARTMENT_ID)
                                   {
                                       int? dutyType = o.DUTY_TYPE;
                                       if ((dutyType.GetValueOrDefault() != 3 ? 0 : (dutyType.HasValue ? 1 : 0)) != 0 && o.DATE_START.Value.Day == times[j].DATES.Day && o.DATE_START.Value.Month == times[j].DATES.Month)
                                       {
                                           num = o.DATE_START.Value.Year == times[j].DATES.Year ? 1 : 0;
                                           goto label_4;
                                       }
                                   }
                                   num = 0;
                                   label_4:
                                   return num != 0;
                               })).GroupBy<DoctorHospital, string>((x => x.ABBREVIATION)).Select((Func<IGrouping<string, DoctorHospital>, DoctorHospital>)(x => x.First())).ToList();
                                (excelWorksheet.Cells[5 + num1, j + 3]).IsRichText = true;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.WrapText = true;
                                ExcelRichTextCollection richText = (excelWorksheet.Cells[5 + num1, j + 3]).RichText;
                                foreach (DoctorHospital doctorHospital in list2)
                                    excelRichText = richText.Add(doctorHospital.ABBREVIATION + "\r\n");
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            else
                            {
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                List<DoctorHospital> list2 = list1.Where((o =>
                               {
                                   int num;
                                   if (o.LM_DEPARTMENT_ID == objDepartment[i].LM_DEPARTMENT_ID)
                                   {
                                       int? dutyType = o.DUTY_TYPE;
                                       if ((dutyType.GetValueOrDefault() != 3 ? 0 : (dutyType.HasValue ? 1 : 0)) != 0 && o.DATE_START.Value.Day == times[j].DATES.Day && o.DATE_START.Value.Month == times[j].DATES.Month)
                                       {
                                           num = o.DATE_START.Value.Year == times[j].DATES.Year ? 1 : 0;
                                           goto label_4;
                                       }
                                   }
                                   num = 0;
                                   label_4:
                                   return num != 0;
                               })).GroupBy<DoctorHospital, string>((x => x.ABBREVIATION)).Select((Func<IGrouping<string, DoctorHospital>, DoctorHospital>)(x => x.First())).ToList();
                                (excelWorksheet.Cells[5 + num1, j + 3]).IsRichText = true;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.WrapText = true;
                                ExcelRichTextCollection richText = (excelWorksheet.Cells[5 + num1, j + 3]).RichText;
                                foreach (DoctorHospital doctorHospital in list2)
                                    excelRichText = richText.Add(doctorHospital.ABBREVIATION + "\r\n");
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                (excelWorksheet.Cells[5 + num1, j + 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }
                    }
                    else
                    {
                        ++num1;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 1]).Value = (num1 + 1).ToString();
                        (excelWorksheet.Cells[5 + num1, 1]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        (excelWorksheet.Cells[5 + num1, 1]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        (excelWorksheet.Cells[5 + num1, 2]).Value = objDepartment[i].DEPARTMENT_NAME.ToUpper();
                        (excelWorksheet.Cells[5 + num1, 2]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        (excelWorksheet.Cells[5 + num1, 2]).Style.Font.Bold = true;
                        (excelWorksheet.Cells[5 + num1, 3, 5 + num1, times.Count + 2]).Merge = true;
                        (excelWorksheet.Cells[5 + num1, 3, 5 + num1, times.Count + 2]).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 3, 5 + num1, times.Count + 2]).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 3, 5 + num1, times.Count + 2]).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        (excelWorksheet.Cells[5 + num1, 3, 5 + num1, times.Count + 2]).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
                int num3 = num1 + 3;
                List<CONFIG_REPORT> allByExcelName = unitOfWork.ConfigReports.GetAllByExcelName("ReportOfAllHospital.xlsx");
                if (allByExcelName != null && allByExcelName.Count > 0)
                {
                    (excelWorksheet.Cells[5 + num3, 2, 5 + num3, 3]).Merge = true;
                    (excelWorksheet.Cells[5 + num3, 6, 5 + num3, 8]).Merge = true;
                    (excelWorksheet.Cells[5 + num3, 2, 5 + num3, 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    (excelWorksheet.Cells[5 + num3, 2, 5 + num3, 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[5 + num3, 6, 5 + num3, 8]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    (excelWorksheet.Cells[5 + num3, 6, 5 + num3, 8]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[5 + num3, 2]).Value = allByExcelName[0].FUNCTION1;
                    (excelWorksheet.Cells[5 + num3, 2, 5 + num3, 3]).Style.Font.Bold = true;
                    (excelWorksheet.Cells[11 + num3, 2]).Value = allByExcelName[0].SIGNATURE1;
                    (excelWorksheet.Cells[11 + num3, 2]).Style.Font.Bold = true;
                    (excelWorksheet.Cells[11 + num3, 2, 11 + num3, 3]).Merge = true;
                    (excelWorksheet.Cells[11 + num3, 2, 11 + num3, 3]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    (excelWorksheet.Cells[11 + num3, 2, 11 + num3, 3]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[4 + num3, 6, 4 + num3, 8]).Merge = true;
                    ExcelRange excelRange2 = excelWorksheet.Cells[4 + num3, 6];
                    object[] objArray4 = new object[6];
                    objArray4[0] = " Ngày ";
                    object[] objArray5 = objArray4;
                    int index3 = 1;
                    dateTime2 = DateTime.Now;
                    int day2 = dateTime2.Day;

                    objArray5[index3] = day2;
                    objArray4[2] = " tháng ";
                    object[] objArray6 = objArray4;
                    int index4 = 3;
                    dateTime2 = DateTime.Now;
                    int month = dateTime2.Month;

                    objArray6[index4] = month;
                    objArray4[4] = " năm ";
                    object[] objArray7 = objArray4;
                    int index5 = 5;
                    dateTime2 = DateTime.Now;
                    int year = dateTime2.Year;

                    objArray7[index5] = year;
                    string str3 = string.Concat(objArray4);
                    (excelRange2).Value = str3;
                    (excelWorksheet.Cells[4 + num3, 6, 4 + num3, 8]).Style.Font.Bold = true;
                    (excelWorksheet.Cells[4 + num3, 6, 4 + num3, 8]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[5 + num3, 6]).Value = allByExcelName[0].FUNCTION2;
                    (excelWorksheet.Cells[5 + num3, 6]).Style.Font.Bold = true;
                    (excelWorksheet.Cells[11 + num3, 6, 11 + num3, 8]).Merge = true;
                    (excelWorksheet.Cells[11 + num3, 6, 11 + num3, 8]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    (excelWorksheet.Cells[11 + num3, 6, 11 + num3, 8]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    (excelWorksheet.Cells[11 + num3, 6]).Value = allByExcelName[0].SIGNATURE2;
                    (excelWorksheet.Cells[11 + num3, 6]).Style.Font.Bold = true;
                }
                if (result3 < 3)
                {
                    excelWorksheet.Column(3).Width = 14.0;
                    excelWorksheet.Column(4).Width = 14.0;
                    excelWorksheet.Column(5).Width = 14.0;
                    excelWorksheet.Column(6).Width = 14.0;
                    excelWorksheet.Column(7).Width = 14.0;
                    excelWorksheet.Column(8).Width = 14.0;
                    excelWorksheet.Column(9).Width = 14.0;
                }
                asByteArray = excelPackage.GetAsByteArray();
            }
            WriteLog(enLogType.NomalLog, enActionType.ExportExcel, "Xuất Excel lịch trực toàn bệnh viện tháng " + monthx + " năm " + yearx, "N/A", "N/A", 0, string.Empty, string.Empty);
            return new DownloadResult(asByteArray, "ReportOfAllHospital.xlsx");
        }

        [ActionDescription(ActionCode = "CalendarDutyHospital_List", ActionName = "Xem lịch trực", GroupCode = "CalendarDutyHospital", GroupName = "Lịch trực toàn Bệnh viện")]
        [CustomAuthorize]
        [HttpGet]
        public ActionResult ViewCalendarHospitalDetail(int idWeek = 0)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int result1 = 0;
            int result2 = 0;
            CALENDAR_DUTY calendarDuty1 = unitOfWork.CalendarDuty.CheckCalendarHospital(month, year, 4);
            CALENDAR_DUTY entity = new CALENDAR_DUTY();
            if (calendarDuty1 == null)
            {
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                entity.CALENDAR_NAME = "Lịch trực toàn bệnh viện tháng " + month + " năm " + year;
                entity.CALENDAR_MONTH = month;
                entity.CALENDAR_YEAR = year;
                entity.CALENDAR_STATUS = 1;
                entity.LM_DEPARTMENT_ID = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
                entity.DUTY_TYPE = 4;
                entity.ISDELETE = false;
                entity.USER_CREATE_ID = null;
                entity.LM_DEPARTMENT_PARTS = null;
                unitOfWork.CalendarDuty.Add(entity);
                foreach (CALENDAR_DUTY calendarDuty2 in unitOfWork.CalendarDuty.GetByApproved(month, year, 3, 0))
                    unitOfWork.CalendarGroups.Add(new CALENDAR_GROUP()
                    {
                        CALENDAR_ID = entity.CALENDAR_DUTY_ID,
                        CALENDAR_PARENT_ID = calendarDuty2.CALENDAR_DUTY_ID,
                        CALENDAR_MONTH = entity.CALENDAR_MONTH,
                        CALENDAR_YEAR = entity.CALENDAR_YEAR,
                        LM_DEPARTMENT_ID = entity.LM_DEPARTMENT_ID
                    });
                List<TimeCalendarDuty> timeCalendarDutyList = new List<TimeCalendarDuty>();
                if (int.TryParse(entity.CALENDAR_MONTH.ToString(), out result1) && int.TryParse(entity.CALENDAR_YEAR.ToString(), out result2))
                {
                    DateTime dateTime1 = Convert.ToDateTime("1/" + result1 + "/" + result2);
                    if (idWeek == -1)
                    {
                        DateTime dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1));
                        DateTime dateTime3 = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                        for (int index = 0; index < dateTime3.Day; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dateTime2.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                            timeCalendarDutyList.Add(timeCalendarDuty);
                        }
                    }
                    else if (idWeek < 3)
                    {
                        int num = idWeek * 7;
                        DateTime dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num);
                        for (int index = 0; index < 7; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dateTime2.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                            timeCalendarDutyList.Add(timeCalendarDuty);
                        }
                    }
                    else
                    {
                        int num1 = DateTime.DaysInMonth(result2, result1) - 28;
                        int num2 = idWeek * 7;
                        DateTime dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                        int num3 = num1 + 7;
                        for (int index = 0; index < num3; ++index)
                        {
                            TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                            timeCalendarDuty.DATES = dateTime2.AddDays(index);
                            timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                            timeCalendarDutyList.Add(timeCalendarDuty);
                        }
                    }
                }
                ViewBag.objCalendarDuty = entity;
                ViewBag.idWeek = idWeek;
                ViewBag.times = timeCalendarDutyList;
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem lịch trực toàn bệnh viện tháng " + month + " năm " + year, "N/A", "N/A", 0, string.Empty, string.Empty);
                return View("_ViewCalendarHospitalDetail");
            }
            int result3 = DateTime.Now.Month;
            int result4 = DateTime.Now.Year;
            List<TimeCalendarDuty> timeCalendarDutyList1 = new List<TimeCalendarDuty>();
            int? nullable = calendarDuty1.CALENDAR_MONTH;
            int num4;
            if (int.TryParse(nullable.ToString(), out result3))
            {
                nullable = calendarDuty1.CALENDAR_YEAR;
                num4 = !int.TryParse(nullable.ToString(), out result4) ? 1 : 0;
            }
            else
                num4 = 1;
            if (num4 == 0)
            {
                DateTime dateTime1 = Convert.ToDateTime("1/" + result3 + "/" + result4);
                if (idWeek == -1)
                {
                    DateTime dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1));
                    DateTime dateTime3 = dateTime1.AddMonths(1).AddDays(-dateTime1.Day);
                    for (int index = 0; index < dateTime3.Day; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dateTime2.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                        timeCalendarDutyList1.Add(timeCalendarDuty);
                    }
                }
                else if (idWeek < 3)
                {
                    int num1 = idWeek * 7;
                    DateTime dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num1);
                    for (int index = 0; index < 7; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dateTime2.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                        timeCalendarDutyList1.Add(timeCalendarDuty);
                    }
                }
                else
                {
                    int num1 = DateTime.DaysInMonth(result4, result3) - 28;
                    int num2 = idWeek * 7;
                    DateTime dateTime2 = dateTime1.AddDays((-dateTime1.Day + 1)).AddDays(num2);
                    int num3 = num1 + 7;
                    for (int index = 0; index < num3; ++index)
                    {
                        TimeCalendarDuty timeCalendarDuty = new TimeCalendarDuty();
                        timeCalendarDuty.DATES = dateTime2.AddDays(index);
                        timeCalendarDuty.DAYS = Utils.Utils.GetDayNameVT(timeCalendarDuty.DATES);
                        timeCalendarDutyList1.Add(timeCalendarDuty);
                    }
                }
            }
            ViewBag.objCalendarDuty = calendarDuty1;
            ViewBag.idWeek = idWeek;
            ViewBag.times = timeCalendarDutyList1;
            WriteLog(enLogType.NomalLog, enActionType.View, "Xem lịch trực toàn bệnh viện tháng " + result3 + " năm " + result4, "N/A", "N/A", 0, string.Empty, string.Empty);
            return View("_ViewCalendarHospitalDetail");
        }

        [HttpGet]
        public PartialViewResult ListDoctorsDefault(string arrayid, string changeId, string dateX)
        {
            if (!Request.IsAjaxRequest())
                return null;
            string[] strArray = arrayid.Split('_');
            ViewBag.arrayid = strArray;
            ViewBag.changeId = changeId;
            List<DoctorList> departmentUsername = unitOfWork.Doctors.GetAllByDepartmentUsername(User.Identity.Name);
            ViewBag.allDoctor = departmentUsername;
            DateTime? nullable = null;
            DateTime? date;
            try
            {
                date = Utils.Utils.ConvetDateVNToDates(dateX);
            }
            catch
            {
                date = null;
            }
            List<DoctorInCalendar> allDoctorNotLeader = unitOfWork.Doctors.GetAllDoctorNotLeader(date, 0);
            ViewBag.allDoctorCalendar = allDoctorNotLeader;
            ViewBag.DateX = date;
            return PartialView("_ListDoctorsDefault");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionDescription(ActionCode = "CalendarDuty_Insert", ActionName = "Lập lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        [ValidateInput(false)]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SaveCalendarDutyDefault(string strValues, string strInfor, string calendarContent)
        {
            string[] strArray = strInfor.Split('_');
            (strArray).Count<string>();
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            int result1 = 0;
            int result2 = 0;
            int int32 = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
            if (int.TryParse(strArray[0], out result1) && int.TryParse(strArray[1], out result2))
            {
                int hashCode = DutyType.Deparment.GetHashCode();
                if (!unitOfWork.CalendarDuty.checkCalendar(result1, result2, hashCode, int32, 1))
                {
                    string str = GenerateDepartmentTreeHelper.GenerateDeparmentTree(int32) + ",";
                    CALENDAR_DUTY entity = new CALENDAR_DUTY();
                    entity.CALENDAR_NAME = calendarContent.Trim();
                    entity.CALENDAR_MONTH = result1;
                    entity.CALENDAR_YEAR = result2;
                    entity.CALENDAR_STATUS = 1;
                    entity.LM_DEPARTMENT_ID = int32;
                    entity.DUTY_TYPE = 3;
                    entity.LM_DEPARTMENT_PARTS = byUserName.LM_DEPARTMENT.DEPARTMENT_PATH + str;
                    entity.ISDELETE = false;
                    entity.USER_CREATE_ID = byUserName.ADMIN_USER_ID;
                    entity.DATE_CREATE = new DateTime?(Utils.Utils.GetDateTime());
                    unitOfWork.CalendarDuty.Add(entity);
                    CALENDAR_DATA objCalendarData = null;
                    CALENDAR_DOCTOR objCalendarDoctor = null;
                    int calendarDutyId = entity.CALENDAR_DUTY_ID;
                    SaveDataCalendarDefault(strValues, objCalendarData, objCalendarDoctor, calendarDutyId);
                    return Json(Localization.AddCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
                }
                int calendarDutyId1 = unitOfWork.CalendarDuty.GetCalendarDutyId(result1, result2, hashCode, int32, 1);
                unitOfWork.CalendarDoctors.DeleteCalendarDoctorById(calendarDutyId1);
                unitOfWork.CalendarDatas.DeleteCalendarDataById(calendarDutyId1);
                CALENDAR_DATA objCalendarData1 = null;
                CALENDAR_DOCTOR objCalendarDoctor1 = null;
                SaveDataCalendarDefault(strValues, objCalendarData1, objCalendarDoctor1, calendarDutyId1);
                return Json(Localization.UpdateCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [ActionDescription(ActionCode = "CalendarDuty_Index", ActionName = "Danh sách lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public PartialViewResult ViewCalendarDefault(string idCalendarDuty, int types = 0, int typeEdit = 1)
        {
            if (!Request.IsAjaxRequest())
                return null;
            List<string> list = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list;
            int result1 = 0;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result1))
            {
                calendarDuty = unitOfWork.CalendarDuty.GetById(result1);
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarLeader(result1);
            }
            ViewBag.objCalendarDuty = calendarDuty;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result2 = dateTime.Month;
            int result3 = dateTime.Year;
            int? nullable = calendarDuty.CALENDAR_MONTH;
            int num;
            if (int.TryParse(nullable.ToString(), out result2))
            {
                nullable = calendarDuty.CALENDAR_YEAR;
                num = !int.TryParse(nullable.ToString(), out result3) ? 1 : 0;
            }
            else
                num = 1;
            if (num == 0)
                dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result2), Convert.ToString(result3));
            ViewBag.doctors = doctorCalendarLeaderList;
            ViewBag.times = dateTime;
            ViewBag.typeEdit = typeEdit;
            if (types == 0)
                return PartialView("_ViewCalendarDefault");
            return PartialView("_EditCalendarDefault");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionDescription(ActionCode = "CalendarDuty_Edit", ActionName = "Sửa thông tin lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult EditValuesCalendarDefault(string strAdd, string strDeleted, string strChanged, string strValues, string idCalendarDuty)
        {
            if (!Request.IsAjaxRequest())
                return null;
            List<string> list1 = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list1;
            int result = 0;
            int hashCode1 = CalendarChangeType.Add.GetHashCode();
            int hashCode2 = CalendarChangeType.Change.GetHashCode();
            int hashCode3 = CalendarChangeType.Deleted.GetHashCode();
            if (!int.TryParse(idCalendarDuty, out result))
                return null;
            if (unitOfWork.CalendarDuty.GetById(result).ISAPPROVED == 1)
            {
                List<string> stringList1 = new List<string>();
                List<string> stringList2 = new List<string>();
                List<string> stringList3 = new List<string>();
                List<string> stringList4 = new List<string>();
                List<string> stringList5 = new List<string>();
                List<string> stringList6 = new List<string>();
                List<DOCTOR> list2 = unitOfWork.Doctors.GetAll().ToList();
                List<CALENDAR_CHANGE> calendarChangeList1 = new List<CALENDAR_CHANGE>();
                List<CALENDAR_CHANGE> calendarChangeList2 = new List<CALENDAR_CHANGE>();
                List<CALENDAR_CHANGE> calendarChangeList3 = new List<CALENDAR_CHANGE>();
                CALENDAR_CHANGE calendarChange1 = new CALENDAR_CHANGE();
                string[] strArray1 = (string[])null;
                int idDoctor = 0;
                int idDoctorOld = 0;
                DateTime? nullable1 = null;
                List<string> list3 = (strAdd.Split('-')).ToList();
                unitOfWork.CalendarChanges.DeleteCalendarChangeByStatus(result, hashCode1);
                foreach (string str in list3)
                {
                    char[] chArray = new char[1] { ',' };
                    string[] strArray2 = str.Split(chArray);
                    if ((strArray2).Count<string>() == 2 && int.TryParse(strArray2[1].ToString(), out idDoctor))
                    {
                        CALENDAR_CHANGE calendarChange2 = new CALENDAR_CHANGE();
                        calendarChange2.CALENDAR_DUTY_ID = result;
                        DateTime? nullable2;
                        try
                        {
                            nullable2 = new DateTime?(Utils.Utils.ToDateTime(strArray2[0].ToString(), "dd/MM/yyyy"));
                        }
                        catch
                        {
                            nullable2 = null;
                        }
                        calendarChange2.DATE_START = nullable2;
                        calendarChange2.DOCTORS_ID = idDoctor;
                        calendarChange2.DOCTORS_CHANGE_ID = 0;
                        calendarChange2.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        calendarChange2.STATUS = hashCode1;
                        calendarChange2.STATUS_APPROVED = 1;
                        calendarChange2.DOCTORS_CHANGE_NAME = string.Empty;
                        calendarChange2.DATE_CHANGE = DateTime.Now;
                        List<CALENDAR_CHANGE> source = unitOfWork.CalendarChanges.ListNewChange(calendarChange2, hashCode1, 1);
                        if (source.Count == 0)
                        {
                            calendarChange2.DOCTORS_NAME = list2.Where((o => o.DOCTORS_ID == idDoctor)).ToList()[0].ABBREVIATION;
                            unitOfWork.CalendarChanges.Add(calendarChange2);
                        }
                        else
                            unitOfWork.CalendarChanges.Delete(source.First());
                    }
                }
                string str1 = strDeleted;
                char[] chArray1 = new char[1] { '-' };
                foreach (string str2 in (str1.Split(chArray1)).ToList())
                {
                    CALENDAR_CHANGE calendarChange2 = new CALENDAR_CHANGE();
                    string[] strArray2 = str2.Split(',');
                    if ((strArray2).Count<string>() == 2 && int.TryParse(strArray2[1].ToString(), out idDoctorOld))
                    {
                        strArray1 = strArray2[0].Split(',');
                        calendarChange2.CALENDAR_DUTY_ID = result;
                        DateTime? nullable2;
                        try
                        {
                            nullable2 = new DateTime?(Utils.Utils.ToDateTime(strArray2[0].ToString(), "dd/MM/yyyy"));
                        }
                        catch
                        {
                            nullable2 = null;
                        }
                        calendarChange2.DATE_START = nullable2;
                        calendarChange2.DOCTORS_ID = idDoctorOld;
                        calendarChange2.DOCTORS_CHANGE_ID = 0;
                        calendarChange2.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        calendarChange2.STATUS = hashCode3;
                        calendarChange2.STATUS_APPROVED = 1;
                        calendarChange2.DATE_CHANGE = DateTime.Now;
                        calendarChange2.DOCTORS_CHANGE_NAME = string.Empty;
                        List<CALENDAR_CHANGE> source = unitOfWork.CalendarChanges.ListNewChange(calendarChange2, hashCode3, 1);
                        if (source.Count == 0)
                        {
                            calendarChange2.DOCTORS_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorOld)).ToList()[0].ABBREVIATION;
                            unitOfWork.CalendarChanges.Add(calendarChange2);
                        }
                        else
                            unitOfWork.CalendarChanges.Delete(source.First());
                    }
                }
                string str3 = strChanged;
                char[] chArray2 = new char[1] { '-' };
                foreach (string str2 in (str3.Split(chArray2)).ToList())
                {
                    CALENDAR_CHANGE calendarChange2 = new CALENDAR_CHANGE();
                    string[] strArray2 = str2.Split(',');
                    if ((strArray2).Count<string>() >= 3 && int.TryParse(strArray2[1].ToString(), out idDoctorOld))
                    {
                        strArray1 = strArray2[0].Split(',');
                        calendarChange2.CALENDAR_DUTY_ID = result;
                        DateTime? nullable2;
                        try
                        {
                            nullable2 = new DateTime?(Utils.Utils.ToDateTime(strArray2[0].ToString(), "dd/MM/yyyy"));
                        }
                        catch
                        {
                            nullable2 = null;
                        }
                        int idDoctorChange = 0;
                        int.TryParse(strArray2[2].ToString(), out idDoctorChange);
                        DateTime? nullable3 = null;
                        string str4 = strArray2[3].ToString();
                        if (str4 != string.Empty && str4 != "undefined")
                        {
                            try
                            {
                                nullable3 = new DateTime?(Utils.Utils.ToDateTime((str4 + "/" + nullable2.Value.Month + "/" + nullable2.Value.Year), "dd/MM/yyyy"));
                            }
                            catch
                            {
                                nullable3 = null;
                            }
                        }
                        calendarChange2.CALENDAR_DUTY_ID = result;
                        calendarChange2.DOCTORS_ID = idDoctorOld;
                        calendarChange2.DATE_START = nullable2;
                        calendarChange2.DOCTORS_CHANGE_ID = idDoctorChange;
                        calendarChange2.DATE_CHANGE_START = nullable3;
                        calendarChange2.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                        calendarChange2.STATUS = 1;
                        calendarChange2.STATUS_APPROVED = 1;
                        calendarChange2.DATE_CHANGE = DateTime.Now;
                        calendarChange2.DOCTORS_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorOld)).ToList()[0].ABBREVIATION;
                        calendarChange2.DOCTORS_CHANGE_NAME = idDoctorChange != 0 ? list2.Where((o => o.DOCTORS_ID == idDoctorChange)).ToList()[0].ABBREVIATION : string.Empty;
                        calendarChange2.CALENDAR_DELETE = 1;
                        List<CALENDAR_CHANGE> source = unitOfWork.CalendarChanges.ListNewChange(calendarChange2, hashCode2, 0);
                        if (source.Count != 0)
                            unitOfWork.CalendarChanges.Delete(source.First());
                        if (idDoctorChange != 0)
                        {
                            calendarChange2.DOCTORS_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorOld)).ToList()[0].ABBREVIATION;
                            unitOfWork.CalendarChanges.Add(calendarChange2);
                        }
                    }
                }
                WriteLog(enLogType.NomalLog, enActionType.Insert, "Đổi lịch trực đơn vị ", "N/A", "N/A", Convert.ToInt32(idCalendarDuty), string.Empty, string.Empty);
                return RedirectToAction("ViewCalendarDefault", "CalendarDuty", new
                {
                    idCalendarDuty = idCalendarDuty,
                    types = 1,
                    isExist = 0
                });
            }
            DateTime dateTime = Utils.Utils.GetDateTime();
            int month = dateTime.Month;
            int year = dateTime.Year;
            CALENDAR_DUTY calendarDuty = null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
            int? calendarMonth = byId.CALENDAR_MONTH;
            int? calendarYear = byId.CALENDAR_YEAR;
            if (calendarMonth.HasValue)
                month = calendarMonth.Value;
            if (calendarYear.HasValue)
                year = calendarYear.Value;
            calendarDuty = null;
            unitOfWork.CalendarDoctors.DeleteCalendarDoctorById(result);
            unitOfWork.CalendarDatas.DeleteCalendarDataById(result);
            CALENDAR_DATA objCalendarData = null;
            CALENDAR_DOCTOR objCalendarDoctor = null;
            SaveDataCalendarDefault(strValues, objCalendarData, objCalendarDoctor, result);
            WriteLog(enLogType.NomalLog, enActionType.Update, "Sửa lịch trực đơn vị ", "N/A", "N/A", result, string.Empty, string.Empty);
            return RedirectToAction("ViewCalendarDefault", "CalendarDuty", new
            {
                idCalendarDuty = idCalendarDuty,
                types = 1,
                isExist = 0
            });
        }

        private void SaveDataCalendarDefault(string strValues, CALENDAR_DATA objCalendarData, CALENDAR_DOCTOR objCalendarDoctor, int calendarDutyId)
        {
            string[] strArray1 = strValues.Split('-');
            int result = 0;
            DateTime? nullable1 = null;
            foreach (string str in strArray1)
            {
                char[] chArray = new char[1] { ',' };
                string[] strArray2 = str.Split(chArray);
                if ((strArray2).Count<string>() >= 2)
                {
                    string s = strArray2[1];
                    if (s != "undefined")
                    {
                        int.TryParse(s, out result);
                        DateTime? nullable2;
                        try
                        {
                            nullable2 = new DateTime?(Utils.Utils.ToDateTime(strArray2[0], "dd/MM/yyyy"));
                        }
                        catch
                        {
                            nullable2 = null;
                        }
                        objCalendarData = new CALENDAR_DATA();
                        objCalendarData.CALENDAR_DUTY_ID = calendarDutyId;
                        objCalendarData.DATE_START = nullable2;
                        objCalendarData.DATE_END = nullable2;
                        objCalendarData.ISDELETE = false;
                        unitOfWork.CalendarDatas.Add(objCalendarData);
                        objCalendarDoctor = new CALENDAR_DOCTOR();
                        objCalendarDoctor.CALENDAR_DATA_ID = objCalendarData.CALENDAR_DATA_ID;
                        objCalendarDoctor.DOCTORS_ID = result;
                        objCalendarDoctor.CALENDAR_DUTY_ID = calendarDutyId;
                        unitOfWork.CalendarDoctors.Add(objCalendarDoctor);
                        objCalendarData = null;
                        objCalendarDoctor = null;
                    }
                }
            }
        }

        [HttpGet]
        public PartialViewResult ListDoctorsChangesDefault(int idDoctorChange, int idCalendarDuty, string dateDoctorChange, string arrayid, string changeId)
        {
            if (!Request.IsAjaxRequest())
                return null;
            List<DoctorList> departmentUsername = unitOfWork.Doctors.GetAllByDepartmentUsername(User.Identity.Name);
            ViewBag.allDoctor = departmentUsername;
            ViewBag.idDoctorChange = idDoctorChange;
            ViewBag.idCalendarDuty = idCalendarDuty;
            ViewBag.dateDoctorChange = dateDoctorChange;
            ViewBag.arrayid = arrayid;
            ViewBag.changeId = changeId;
            return PartialView("_ListDoctorsChangeDefault");
        }

        [HttpGet]
        public PartialViewResult ListDateChangeDefault(string idDoctorChange, string idDoctorIsChange, string idCalendarDuty)
        {
            if (idDoctorChange != null && idDoctorIsChange != null && idCalendarDuty != null)
            {
                int result1 = 0;
                int result2 = 0;
                int result3 = 0;
                if (int.TryParse(idDoctorChange, out result1) && int.TryParse(idDoctorIsChange, out result2) && int.TryParse(idCalendarDuty, out result3))
                {
                    List<DateChangeList> dayByDoctorDefault = unitOfWork.Doctors.GetAllDayByDoctorDefault(result2, result3, DateTime.Now);
                    ViewBag.Dates = dayByDoctorDefault;
                    ViewBag.Type = "1";
                    return PartialView("_ListDateChange");
                }
            }
            return null;
        }

        [ActionDescription(ActionCode = "CalendarDuty_Edit", ActionName = "Sửa thông tin lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpGet]
        public ActionResult ChangeCalendarDutyDefault(string idCalendarDuty, string idDoctorChange, string dateDoctorChange, string idDoctorIsChange, string DatesIsChange)
        {
            if (!Request.IsAjaxRequest())
                return null;
            List<string> list1 = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            ViewBag.ActionPermission = list1;
            if (idCalendarDuty == null || idDoctorChange == null || (dateDoctorChange == null || idDoctorIsChange == null) || DatesIsChange == null)
                return null;
            List<DOCTOR> list2 = unitOfWork.Doctors.GetAll().ToList();
            int result = 0;
            int idDoctorChangeX = 0;
            int idDoctorIsChangeX = 0;
            DateTime? nullable1 = null;
            DateTime? nullable2 = null;
            CalendarChangeModel calendarChangeModel = new CalendarChangeModel();
            if (!int.TryParse(idCalendarDuty, out result) || !int.TryParse(idDoctorChange, out idDoctorChangeX) || !int.TryParse(idDoctorIsChange, out idDoctorIsChangeX))
                return null;
            CALENDAR_CHANGE calendarChange = new CALENDAR_CHANGE();
            DateTime? nullable3;
            try
            {
                nullable3 = Utils.Utils.ConvetDateVNToDates(dateDoctorChange.ToString());
            }
            catch
            {
                nullable3 = null;
            }
            DateTime? nullable4;
            try
            {
                nullable4 = Utils.Utils.ConvetDateVNToDates(DatesIsChange.ToString());
            }
            catch
            {
                nullable4 = null;
            }
            if (nullable4.HasValue)
            {
                calendarChangeModel.DOCTORS_ID = idDoctorChangeX;
                calendarChangeModel.DOCTORS_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorChangeX)).ToList()[0].ABBREVIATION;
                calendarChangeModel.DATE_START = nullable3;
                calendarChangeModel.DOCTORS_CHANGE_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                calendarChangeModel.DOCTORS_CHANGE_ID = idDoctorIsChangeX;
                calendarChangeModel.DATE_CHANGE_START = nullable4;
                calendarChangeModel.CALENDAR_DUTY_ID = new int?(Convert.ToInt32(idCalendarDuty));
            }
            else
            {
                calendarChangeModel.DOCTORS_ID = idDoctorChangeX;
                calendarChangeModel.DOCTORS_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorChangeX)).ToList()[0].ABBREVIATION;
                calendarChangeModel.DATE_START = nullable3;
                calendarChangeModel.DOCTORS_CHANGE_NAME = list2.Where((o => o.DOCTORS_ID == idDoctorIsChangeX)).ToList()[0].ABBREVIATION;
                calendarChangeModel.DOCTORS_CHANGE_ID = idDoctorIsChangeX;
                calendarChangeModel.CALENDAR_DUTY_ID = new int?(Convert.ToInt32(idCalendarDuty));
            }
            WriteLog(enLogType.NomalLog, enActionType.Update, "Đổi lịch trực đơn vị ", "N/A", "N/A", 0, string.Empty, string.Empty);
            return Json(calendarChangeModel, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDuty_Export", ActionName = "Xuất Excel lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        public ActionResult ExportCalendarDefault(string idCalendarDuty)
        {
            int int32;
            try
            {
                int32 = Convert.ToInt32(idCalendarDuty);
            }
            catch
            {
                throw new Exception("Sai định dạng dữ liệu");
            }
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(int32);
            if (byId == null)
                throw new Exception("Sai định dạng dữ liệu");
            int? nullable = byId.CALENDAR_MONTH;
            int iMonth = nullable.Value;
            nullable = byId.CALENDAR_YEAR;
            int iYear = nullable.Value;
            List<DoctorCalendarLeader> doctorCalendarLeader = unitOfWork.CalendarDuty.GetDoctorCalendarLeader(int32);
            List<int> intList = new List<int>();
            List<ItemBase> deptItems = new List<ItemBase>();
            if (!doctorCalendarLeader.Any())
                ;
            List<CALENDAR_CHANGE> listChangeCalendar = unitOfWork.CalendarChanges.GetListChangeCalendar(int32);
            FileInfo fileInfo = new FileInfo(Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfCalendarDefault.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Export");
            byte[] asByteArray;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
                FReport freport = new FReport();
                excelWorksheet.Cells["C2"].Value = ("Tháng " + iMonth.ToString() + " năm " + iYear.ToString()).ToUpper();
                int num1 = Utils.Utils.FirstDate(Utils.Utils.ToDateTime((iMonth.ToString() + "/1/" + iYear.ToString()), "dd/mm/yyyy"));
                string[] strArray = new string[7]
                {
          "Thứ 2",
          "Thứ 3",
          "Thứ 4",
          "Thứ 5",
          "Thứ 6",
          "Thứ 7",
          "Chủ nhật"
                };
                int num2 = 5;
                int day = 0;
                for (int index = 1; index < 7; ++index)
                {
                    excelWorksheet.Column(index).Width = 15.0;
                    for (int dayOfWeek = 0; dayOfWeek < strArray.Length; ++dayOfWeek)
                    {
                        //excelWorksheet.Row(dayOfWeek + num2).Height = 15.0;
                        if (index == 1)
                        {
                            if (dayOfWeek >= 5 && dayOfWeek < 8)
                            {
                                (excelWorksheet.Cells[dayOfWeek + num2, 1]).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                (excelWorksheet.Cells[dayOfWeek + num2, 1]).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            }
                            ExportReport.ApplyStyle(excelWorksheet.Cells[dayOfWeek + num2, 1], strArray[dayOfWeek].ToUpper(), null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                        }
                        else
                        {
                            if (dayOfWeek == num1 && index == 2)
                                day = 1;
                            if (dayOfWeek > num1 && index == 2 || index > 2)
                                ++day;
                            ExcelRange cell = excelWorksheet.Cells[dayOfWeek + num2, index];
                            (cell).IsRichText = true;
                            if (dayOfWeek >= 5 && dayOfWeek < 8)
                            {
                                (cell).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                (cell).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            }
                            ShowDoctorInDay(iYear, iMonth, dayOfWeek, day, doctorCalendarLeader, listChangeCalendar, deptItems, cell);
                        }
                    }
                }
                asByteArray = excelPackage.GetAsByteArray();
            }
            return new DownloadResult(asByteArray, "ReportOfCalendarDefault.xlsx");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDuty_Export", ActionName = "Xuất Excel lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        public ActionResult ExportCalendarDuty(string idCalendarDuty)
        {
            int int32;
            try
            {
                int32 = Convert.ToInt32(idCalendarDuty);
            }
            catch
            {
                throw new Exception("Sai định dạng dữ liệu");
            }
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(int32);
            if (byId == null)
                throw new Exception("Sai định dạng dữ liệu");
            int month = byId.CALENDAR_MONTH.Value;
            int year = byId.CALENDAR_YEAR.Value;
            int num1 = DateTime.DaysInMonth(year, month);
            List<DoctorCalendar> doctorCalendar = unitOfWork.CalendarDuty.GetDoctorCalendar(int32);
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            List<TEMPLATE_COLUM> templateColum = unitOfWork.TemplatesColumn.GetColumnByIDTemplate(byId.TEMPLATES_ID.Value, byUserName.LM_DEPARTMENT_ID.Value).ToList();
            List<int> intList = new List<int>();
            List<ItemBase> itemBaseList = new List<ItemBase>();
            if (!doctorCalendar.Any())
                ;
            unitOfWork.CalendarChanges.GetListChangeCalendar(int32);
            FileInfo fileInfo = new FileInfo(Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfCalendarDuty.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Export");
            byte[] asByteArray;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
                FReport freport = new FReport();
                excelWorksheet.Cells["A1"].Value = byId.CALENDAR_NAME.ToUpper();
                excelWorksheet.Cells["A2"].Value = byUserName.LM_DEPARTMENT.DEPARTMENT_NAME.ToUpper();
                excelWorksheet.Cells["A3"].Value = ("Tháng " + month.ToString() + " năm " + year.ToString()).ToUpper();
                ExportReport.ApplyStyle(excelWorksheet.Cells[5, 1], "Thứ", null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                ExportReport.ApplyStyle(excelWorksheet.Cells[5, 2], "Ngày trực", null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                for (int index = 0; index < templateColum.Count; ++index)
                    ExportReport.ApplyStyle(excelWorksheet.Cells[5, 3 + index], templateColum[index].COLUM_NAME, null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                int num2 = 0;
                int num3 = 5;
                int day;
                for (day = 1; day <= num1; ++day)
                {
                    DateTime dayRow = new DateTime(year, month, day);
                    ExportReport.ApplyStyle(excelWorksheet.Cells[day + num3, 1], Utils.Utils.Getday(dayRow, 1), null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                    ExportReport.ApplyStyle(excelWorksheet.Cells[day + num3, 2], day.ToString() + "/" + month.ToString(), null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                    for (int i = 0; i < templateColum.Count; ++i)
                    {
                        IEnumerable<DoctorCalendar> source = doctorCalendar.Where((t => t.TEMPLATE_COLUM_ID == templateColum[i].TEMPLATE_COLUM_ID && t.DATE_START.Value == dayRow));
                        ExportReport.ApplyStyle(excelWorksheet.Cells[day + num3, 3 + i], string.Join("\r\n", source.Select((t => t.ABBREVIATION))), null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                        num2 = source.Count<DoctorCalendar>() > num2 ? source.Count<DoctorCalendar>() : num2;
                    }
                    excelWorksheet.Row(day + num3).Height = num2 == 0 ? 25.0 : (double)(35 * num2);
                    if (dayRow.DayOfWeek == DayOfWeek.Sunday)
                    {
                        (excelWorksheet.Cells[day + num3, 1, day + num3, 2 + templateColum.Count]).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        (excelWorksheet.Cells[day + num3, 1, day + num3, 2 + templateColum.Count]).Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    }
                }
                int num4 = day + num3;
                (excelWorksheet.Cells[5 + num4, 3, 5 + num4, 6]).Merge = true;
                (excelWorksheet.Cells[5 + num4, 3, 5 + num4, 6]).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                (excelWorksheet.Cells[5 + num4, 3, 5 + num4, 6]).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                (excelWorksheet.Cells[5 + num4, 3]).Value = ("TRƯỞNG ĐƠN VỊ " + byUserName.LM_DEPARTMENT.DEPARTMENT_NAME.ToUpper());
                (excelWorksheet.Cells[5 + num4, 3]).Style.Font.Bold = true;
                asByteArray = excelPackage.GetAsByteArray();
            }
            return new DownloadResult(asByteArray, "ReportOfCalendarDuty.xlsx");
        }

        [CustomAuthorize]
        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Insert", ActionName = "Lập lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        public PartialViewResult LoadCalendarLeader(string strDate)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result1 = dateTime.Month;
            int result2 = dateTime.Year;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                if (int.TryParse(strArray[0], out result1) && int.TryParse(strArray[1], out result2))
                    dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result1), Convert.ToString(result2));
            }
            int hashCode = DutyType.Leader.GetHashCode();
            List<CALENDAR_DUTY> calendarDirector = unitOfWork.CalendarDuty.GetCalendarDirector(result1, result2, hashCode);
            int num = calendarDirector.Any() ? calendarDirector[0].CALENDAR_DUTY_ID : 0;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = num == 0 ? unitOfWork.CalendarDuty.GetDoctorCalendarDirector(result1, result2, hashCode) : unitOfWork.CalendarDuty.GetDoctorCalendarLeader(num);
            ViewBag.times = dateTime;
            ViewBag.doctors = doctorCalendarLeaderList;
            ViewBag.objCalendarDuty = calendarDirector.Any() ? calendarDirector[0] : new CALENDAR_DUTY();
            ViewBag.lstCalendarChange = num == 0 ? new List<CALENDAR_CHANGE>() : unitOfWork.CalendarChanges.GetListChangeCalendar(num);
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_AddCalendarLeaderData");
        }

        [HttpGet]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Insert", ActionName = "Lập lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        public ActionResult AddCalendarLeader()
        {
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return View("_AddCalendarLeader");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyLeader_View", ActionName = "Xem lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        public ActionResult CalendarLeader()
        {
            int hashCode = DutyType.Leader.GetHashCode();
            ViewBag.types = hashCode;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.ActionPermission = actionCodesByUserName;
            return View("~/Views/CalendarDuty/Index.cshtml");
        }

        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Insert", ActionName = "Lập lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SaveCalendarLeader(string strValues, string strInfor, string calendarName)
        {
            string[] strArray = strInfor.Split('_');
            (strArray).Count<string>();
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            int result1 = 0;
            int result2 = 0;
            int int32 = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
            if (int.TryParse(strArray[0], out result1) && int.TryParse(strArray[1], out result2))
            {
                int hashCode = DutyType.Leader.GetHashCode();
                CALENDAR_DUTY entity1 = unitOfWork.CalendarDuty.CheckCalendarHospital(result1, result2, hashCode);
                if (entity1 == null)
                {
                    CALENDAR_DUTY entity2 = new CALENDAR_DUTY();
                    entity2.CALENDAR_NAME = calendarName.Trim();
                    entity2.CALENDAR_MONTH = result1;
                    entity2.CALENDAR_YEAR = result2;
                    entity2.CALENDAR_STATUS = 1;
                    entity2.LM_DEPARTMENT_ID = int32;
                    entity2.DUTY_TYPE = 1;
                    entity2.ISDELETE = false;
                    entity2.USER_CREATE_ID = byUserName.ADMIN_USER_ID;
                    unitOfWork.CalendarDuty.Add(entity2);
                    CALENDAR_DATA objCalendarData = null;
                    CALENDAR_DOCTOR objCalendarDoctor = null;
                    int calendarDutyId = entity2.CALENDAR_DUTY_ID;
                    SaveDataCalendar(strValues, objCalendarData, objCalendarDoctor, calendarDutyId, result1, result2);
                    return Json(Localization.AddCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
                }
                int calendarDutyId1 = entity1.CALENDAR_DUTY_ID;
                entity1.CALENDAR_NAME = calendarName;
                unitOfWork.CalendarDuty.Update(entity1);
                unitOfWork.CalendarDoctors.DeleteCalendarDoctorById(calendarDutyId1);
                unitOfWork.CalendarDatas.DeleteCalendarDataById(calendarDutyId1);
                CALENDAR_DATA objCalendarData1 = null;
                CALENDAR_DOCTOR objCalendarDoctor1 = null;
                SaveDataCalendar(strValues, objCalendarData1, objCalendarDoctor1, calendarDutyId1, result1, result2);
                return Json(Localization.UpdateCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Edit", ActionName = "Sửa lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult UpdateCalendarLeader(string strValues, string strInfor, string strCalendarName, List<CALENDAR_CHANGE> lstCalendarChange)
        {
            string[] strArray = strInfor.Split('_');
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            int.TryParse(strArray[0], out result1);
            int.TryParse(strArray[1], out result2);
            int.TryParse(strArray[2], out result3);
            CALENDAR_DATA objCalendarData = null;
            CALENDAR_DOCTOR objCalendarDoctor = null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result3);
            if (byId == null)
                throw new Exception("UpdateCalendarLeader");
            byId.CALENDAR_NAME = string.IsNullOrEmpty(strCalendarName) ? byId.CALENDAR_NAME : strCalendarName;
            unitOfWork.CalendarDuty.Update(byId);
            int? calendarStatus = byId.CALENDAR_STATUS;
            if ((calendarStatus.GetValueOrDefault() != 4 ? 0 : (calendarStatus.HasValue ? 1 : 0)) != 0)
            {
                unitOfWork.CalendarChanges.DeleteCalendarChangeById(byId.CALENDAR_DUTY_ID);
                if (lstCalendarChange != null && lstCalendarChange.Any())
                {
                    DateTime now = DateTime.Now;
                    foreach (CALENDAR_CHANGE calendarChange in lstCalendarChange)
                    {
                        calendarChange.DATE_CHANGE = now;
                        calendarChange.STATUS_APPROVED = 1;
                        calendarChange.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                    }
                    unitOfWork.CalendarChanges.AddRange(lstCalendarChange);
                }
            }
            else
            {
                unitOfWork.CalendarDoctors.DeleteCalendarDoctorById(result3);
                unitOfWork.CalendarDatas.DeleteCalendarDataById(result3);
                SaveDataCalendar(strValues, objCalendarData, objCalendarDoctor, result3, result1, result2);
            }
            return Json(Localization.UpdateCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
        }

        private void SaveDataCalendar(string strValues, CALENDAR_DATA objCalendarData, CALENDAR_DOCTOR objCalendarDoctor, int calendarDutyId, int monthx, int yearx)
        {
            string[] strArray1 = strValues.Split('-');
            int result1 = 0;
            int result2 = 0;
            DateTime? nullable1 = null;
            foreach (string str in strArray1)
            {
                string[] strArray2 = str.Split('_');
                if ((strArray2).Count<string>() >= 2)
                {
                    int.TryParse(strArray2[0], out result1);
                    string s = strArray2[1];
                    if (s != "undefined")
                    {
                        int.TryParse(s, out result2);
                        DateTime? nullable2;
                        try
                        {
                            nullable2 = new DateTime?(Utils.Utils.ConvertToDateTime(Convert.ToString(result1), Convert.ToString(monthx), Convert.ToString(yearx)));
                        }
                        catch
                        {
                            nullable2 = null;
                        }
                        if (str.Contains("ngay") || str.Contains("dem"))
                        {
                            TimeSpan timeSpan = str.Contains("ngay") ? new TimeSpan(0, 10, 0, 0) : new TimeSpan(0, 22, 0, 0);
                            nullable2 = nullable2.HasValue ? new DateTime?(nullable2.Value.Add(timeSpan)) : nullable2;
                        }
                        objCalendarData = new CALENDAR_DATA();
                        objCalendarData.CALENDAR_DUTY_ID = calendarDutyId;
                        objCalendarData.DATE_START = nullable2;
                        objCalendarData.DATE_END = nullable2;
                        objCalendarData.ISDELETE = false;
                        unitOfWork.CalendarDatas.Add(objCalendarData);
                        objCalendarDoctor = new CALENDAR_DOCTOR();
                        objCalendarDoctor.CALENDAR_DATA_ID = objCalendarData.CALENDAR_DATA_ID;
                        objCalendarDoctor.DOCTORS_ID = result2;
                        objCalendarDoctor.CALENDAR_DUTY_ID = calendarDutyId;
                        unitOfWork.CalendarDoctors.Add(objCalendarDoctor);
                        objCalendarData = null;
                        objCalendarDoctor = null;
                    }
                }
            }
        }

        [ActionDescription(ActionCode = "CalendarDutyLeader_View", ActionName = "Xem lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [ValidateJsonAntiForgeryToken]
        [CustomAuthorize]
        [HttpGet]
        public PartialViewResult ViewCalendarLeader(string idCalendarDuty)
        {
            int result1 = 0;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result1))
            {
                calendarDuty = unitOfWork.CalendarDuty.GetById(result1);
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarLeader(result1);
            }
            ViewBag.objCalendarDuty = calendarDuty;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result2 = dateTime.Month;
            int result3 = dateTime.Year;
            if (int.TryParse(calendarDuty.CALENDAR_MONTH.ToString(), out result2) && int.TryParse(calendarDuty.CALENDAR_YEAR.ToString(), out result3))
                dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result2), Convert.ToString(result3));
            ViewBag.doctors = doctorCalendarLeaderList;
            ViewBag.times = dateTime;
            return PartialView("_ViewCalendarLeader");
        }

        [ActionDescription(ActionCode = "CalendarDutyLeader_View", ActionName = "Xem lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [HttpGet]
        [CustomAuthorize]
        public PartialViewResult LoadListCalendarLeader(string strDate)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result1 = dateTime.Month;
            int result2 = dateTime.Year;
            string str = string.Empty;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                if (int.TryParse(strArray[0].ToString(), out result1) && int.TryParse(strArray[1].ToString(), out result2))
                    dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result1), Convert.ToString(result2));
                if ((strArray).Count<string>() > 2)
                    str = strArray[2].ToString();
            }
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            List<CALENDAR_DUTY> source = null;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            int hashCode = DutyType.Leader.GetHashCode();
            if (!string.IsNullOrEmpty(Convert.ToString(result1)) && !string.IsNullOrEmpty(Convert.ToString(result2)))
            {
                source = unitOfWork.CalendarDuty.GetCalendarDirector(result1, result2, hashCode);
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarDirector(result1, result2, hashCode);
            }
            if (string.IsNullOrEmpty(str))
            {
                if (source != null && source.Count > 0)
                {
                    str = Convert.ToString(source[0].CALENDAR_DUTY_ID);
                    calendarDuty = source.Any() ? source[0] : new CALENDAR_DUTY();
                }
            }
            else
                calendarDuty = unitOfWork.CalendarDuty.GetById(Convert.ToInt32(str));
            List<DoctorCalendarLeader> doctorCalendarLeader = unitOfWork.CalendarDuty.GetDoctorCalendarLeader(calendarDuty.CALENDAR_DUTY_ID);
            ViewBag.lstCalendarChange = unitOfWork.CalendarChanges.GetListChangeCalendar(calendarDuty.CALENDAR_DUTY_ID);
            ViewBag.objCalendarDuty = calendarDuty;
            ViewBag.idCalendarDuty = str;
            ViewBag.doctors = doctorCalendarLeader;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            ViewBag.times = dateTime;
            doctorCalendarLeaderList = null;
            return PartialView("_ViewCalendarLeader");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyLeader_View", ActionName = "Xem lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        [HttpGet]
        public ActionResult ListCalendarLeader(string idCalendarDuty)
        {
            DateTime dateTime = Utils.Utils.GetDateTime();
            int? nullable1 = dateTime.Month;
            int? nullable2 = dateTime.Year;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            int result = 0;
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result))
            {
                CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
                nullable1 = byId.CALENDAR_MONTH;
                nullable2 = byId.CALENDAR_YEAR;
                calendarDuty = null;
            }
            ViewBag.lstCalendarChange = unitOfWork.CalendarChanges.GetListChangeCalendar(result);
            ViewBag.month = nullable1;
            ViewBag.year = nullable2;
            ViewBag.idCalendarDuty = string.Empty;
            return View("_ViewListCalendarLeader");
        }

        [ActionDescription(ActionCode = "CalendarDutyLeader_View", ActionName = "Xem lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        [HttpGet]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public PartialViewResult PartialViewListCalendarLeader(string idCalendarDuty)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int? nullable1 = dateTime.Month;
            int? nullable2 = dateTime.Year;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            int result = 0;
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result))
            {
                CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
                nullable1 = byId.CALENDAR_MONTH;
                nullable2 = byId.CALENDAR_YEAR;
                calendarDuty = null;
            }
            ViewBag.month = nullable1;
            ViewBag.year = nullable2;
            ViewBag.idCalendarDuty = idCalendarDuty;
            return PartialView("_ViewListCalendarLeader");
        }

        [HttpGet]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Edit", ActionName = "Sửa lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [ValidateJsonAntiForgeryToken]
        public PartialViewResult EditCalendarLeader(string idCalendarDuty)
        {
            if (!Request.IsAjaxRequest())
                return null;
            int result1 = 0;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result1))
            {
                calendarDuty = unitOfWork.CalendarDuty.GetById(result1);
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarLeader(result1);
                ViewBag.lstCalendarChange = unitOfWork.CalendarChanges.GetListChangeCalendar(result1);
            }
            ViewBag.objCalendarDuty = calendarDuty;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result2 = dateTime.Month;
            int result3 = dateTime.Year;
            int? nullable = calendarDuty.CALENDAR_MONTH;
            int num;
            if (int.TryParse(nullable.ToString(), out result2))
            {
                nullable = calendarDuty.CALENDAR_YEAR;
                num = !int.TryParse(nullable.ToString(), out result3) ? 1 : 0;
            }
            else
                num = 1;
            if (num == 0)
                dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result2), Convert.ToString(result3));
            ViewBag.doctors = doctorCalendarLeaderList;
            ViewBag.times = dateTime;
            ViewBag.idCalendarDuty = idCalendarDuty;
            return PartialView("_EditCalendarLeader");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Export", ActionName = "Xuất dữ liệu Excel", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo")]
        public ActionResult ExportCalendarLeader(string strDate)
        {
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                int.TryParse(strArray[0].ToString(), out result1);
                int.TryParse(strArray[1].ToString(), out result2);
                if ((strArray).Count<string>() > 2)
                    int.TryParse(strArray[2], out result3);
            }
            List<DoctorCalendarLeader> doctorCalendarLeaderList = result3 == 0 ? unitOfWork.CalendarDuty.GetDoctorCalendarDirector(result1, result2, 1) : unitOfWork.CalendarDuty.GetDoctorCalendarLeader(result3);
            List<int> intList = new List<int>();
            List<ItemBase> deptItems = new List<ItemBase>();
            if (doctorCalendarLeaderList.Any())
                intList = doctorCalendarLeaderList.Select((t => t.LM_DEPARTMENT_ID)).ToList();
            List<CALENDAR_CHANGE> listChangeCalendar = unitOfWork.CalendarChanges.GetListChangeCalendar(result3);
            FileInfo fileInfo = new FileInfo(Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfCalendarLeader.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Export");
            byte[] asByteArray;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
                FReport freport = new FReport();
                excelWorksheet.Cells["C2"].Value = ("Tháng " + (result1 == 0 ? string.Empty : result1.ToString()) + " năm " + (result2 == 0 ? string.Empty : result2.ToString()));
                excelWorksheet.Cells["C2"].Style.Font.Italic = true;
                excelWorksheet.Cells["C2"].Style.Font.Bold = true;
                int num1 = Utils.Utils.FirstDate(Utils.Utils.ToDateTime((result1.ToString() + "/1/" + result2.ToString()), "dd/mm/yyyy"));
                string[] strArray = new string[9]
                {
          "Thứ 2",
          "Thứ 3",
          "Thứ 4",
          "Thứ 5",
          "Thứ 6",
          "Thứ 7 \n Ngày",
          "Thứ 7 \n Đêm",
          "Chủ nhật \n Ngày",
          "Chủ nhật \n Đêm"
                };
                int num2 = 5;
                int day = 0;
                int num3 = 7;
                int num4 = DateTime.DaysInMonth(result2, result1);
                if (num1 > 4 && num4 > 29)
                    num3 = 8;
                if (num1 == 5 && num4 == 30)
                    num3 = 7;
                for (int index = 1; index < num3; ++index)
                {
                    excelWorksheet.Column(index).Width = 12.3;
                    for (int dayOfWeek = 0; dayOfWeek < strArray.Length; ++dayOfWeek)
                    {
                        if (index == 1)
                        {
                            if (dayOfWeek >= 5 && dayOfWeek <= 8)
                            {
                                (excelWorksheet.Cells[dayOfWeek + num2, 1]).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                (excelWorksheet.Cells[dayOfWeek + num2, 1]).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            }
                            ExportReport.ApplyStyle(excelWorksheet.Cells[dayOfWeek + num2, 1], strArray[dayOfWeek].ToUpper(), null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?(ExcelVerticalAlignment.Center), true, null, null);
                        }
                        else
                        {
                            if (num1 <= 4 && dayOfWeek == num1 && index == 2)
                                day = 1;
                            if (num1 == 5 && index == 2 && (dayOfWeek == 5 || dayOfWeek == 6))
                                day = 1;
                            if (num1 == 6 && index == 2 && (dayOfWeek == 7 || dayOfWeek == 8))
                                day = 1;
                            if ((dayOfWeek > num1 && index == 2 && num1 < 6 || index > 2) && (dayOfWeek != 6 && dayOfWeek != 8))
                                ++day;
                            ExcelRange cell = excelWorksheet.Cells[dayOfWeek + num2, index];
                            (cell).IsRichText = true;
                            if (dayOfWeek >= 5 && dayOfWeek <= 8)
                            {
                                (cell).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                (cell).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            }
                            ShowDoctorInDay(result2, result1, dayOfWeek, day, doctorCalendarLeaderList, listChangeCalendar, deptItems, cell);
                        }
                    }
                }
                excelWorksheet.Cells["D14:F14"].Merge = true;
                excelWorksheet.Cells["D14"].Value = ("Hà nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);
                excelWorksheet.Cells["D14"].Style.Font.Italic = true;
                excelWorksheet.Cells["D14"].Style.Font.Bold = true;
                excelWorksheet.Cells["D14:F14"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                excelWorksheet.Cells["D14:F14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                List<CONFIG_REPORT> allByExcelName = unitOfWork.ConfigReports.GetAllByExcelName("ReportOfCalendarLeader.xlsx");
                if (allByExcelName != null && allByExcelName.Count > 0)
                {
                    excelWorksheet.Cells["A15:C15"].Merge = true;
                    excelWorksheet.Cells["A21:C21"].Merge = true;
                    excelWorksheet.Cells["D15:F15"].Merge = true;
                    excelWorksheet.Cells["D21:F21"].Merge = true;
                    excelWorksheet.Cells["A15"].Value = allByExcelName[0].FUNCTION1.ToUpper();
                    excelWorksheet.Cells["A15"].Style.Font.Bold = true;
                    excelWorksheet.Cells["A21"].Value = allByExcelName[0].SIGNATURE1;
                    excelWorksheet.Cells["A21"].Style.Font.Bold = true;
                    excelWorksheet.Cells["D15"].Value = allByExcelName[0].FUNCTION2.ToUpper();
                    excelWorksheet.Cells["D15"].Style.Font.Bold = true;
                    excelWorksheet.Cells["D21"].Value = allByExcelName[0].SIGNATURE2;
                    excelWorksheet.Cells["D21"].Style.Font.Bold = true;
                    excelWorksheet.Cells["A15:D21"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    excelWorksheet.Cells["A15:D21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                excelWorksheet.CustomHeight = false;
                asByteArray = excelPackage.GetAsByteArray();
            }
            return new DownloadResult(asByteArray, "LichTT_LD_Thang" + result1.ToString() + "_nam" + result2.ToString() + ".xlsx");
        }

        private void ShowDoctorInDay(int iYear, int iMonth, int dayOfWeek, int day, List<DoctorCalendarLeader> itemData, List<CALENDAR_CHANGE> itemDataChange, List<ItemBase> deptItems, ExcelRange cell)
        {
            string str1 = string.Empty;
            int num = DateTime.DaysInMonth(iYear, iMonth);
            string str2 = day > num ? string.Empty : day.ToString();
            string str3 = str1 + str2;
            if (day <= num && day > 0)
            {
                DateTime dateTime = new DateTime(iYear, iMonth, day);
                if (dayOfWeek != 0 ? (dayOfWeek != 1 ? (dayOfWeek != 2 ? (dayOfWeek != 3 ? (dayOfWeek != 4 ? (dayOfWeek != 5 ? (dayOfWeek != 6 ? (dayOfWeek != 7 ? dateTime.DayOfWeek == DayOfWeek.Sunday : dateTime.DayOfWeek == DayOfWeek.Sunday) : dateTime.DayOfWeek == DayOfWeek.Saturday) : dateTime.DayOfWeek == DayOfWeek.Saturday) : dateTime.DayOfWeek == DayOfWeek.Friday) : dateTime.DayOfWeek == DayOfWeek.Thursday) : dateTime.DayOfWeek == DayOfWeek.Wednesday) : dateTime.DayOfWeek == DayOfWeek.Tuesday) : dateTime.DayOfWeek == DayOfWeek.Monday)
                {
                    IEnumerable<DoctorCalendarLeader> source1 = itemData.Where((t => t.DATE_START.Value.Day.Equals(day)));
                    IEnumerable<CALENDAR_CHANGE> source2 = itemDataChange.Where((t => t.DATE_START.Value.Day.Equals(day)));
                    if (dayOfWeek == 5 || dayOfWeek == 7)
                    {
                        source1 = source1.Where((t => t.DATE_START.Value.Hour == 10 || t.DATE_START.Value.Hour == 12));
                        source2 = itemDataChange.Where((t => t.DATE_START.Value.Hour == 10 || t.DATE_START.Value.Hour == 12));
                    }
                    if (dayOfWeek == 6 || dayOfWeek == 8)
                    {
                        source1 = source1.Where((t => t.DATE_START.Value.Hour == 22));
                        source2 = itemDataChange.Where((t => t.DATE_START.Value.Hour == 22));
                    }
                    if (source1.Any() || source2.Any())
                    {
                        using (ExcelRange excelRange = cell)
                        {
                            (excelRange).RichText.Add(str2 + "\r\n").Bold = false;
                            (excelRange).Style.WrapText = true;
                            foreach (DoctorCalendarLeader doctorCalendarLeader in source1)
                            {
                                (excelRange).RichText.Add(doctorCalendarLeader.ABBREVIATION + "\r\n").Bold = true;
                                string listDepartmentCode = unitOfWork.Departments.GetListDepartmentCode(doctorCalendarLeader.LM_DEPARTMENT_IDs);
                                ExcelRichText excelRichText = (excelRange).RichText.Add(listDepartmentCode + "\r\n");
                                excelRichText.Bold = false;
                                excelRichText.Size = 11f;
                                excelRichText.Italic = true;
                            }
                            foreach (CALENDAR_CHANGE calendarChange in source2)
                            {
                                ExcelRichText excelRichText = (excelRange).RichText.Add(calendarChange.DOCTORS_NAME + "\r\n");
                                excelRichText.Bold = false;
                                excelRichText.Color = Color.Green;
                            }
                        }
                        ExportReport.ApplyStyle(cell, null, null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?((ExcelVerticalAlignment)0), null, null, null);
                    }
                    else
                        ExportReport.ApplyStyle(cell, str2, null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?((ExcelVerticalAlignment)0), false, null, null);
                }
                else
                    ExportReport.ApplyStyle(cell, str2, null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?((ExcelVerticalAlignment)0), false, null, null);
            }
            else
                ExportReport.ApplyStyle(cell, string.Empty, null, ExcelHorizontalAlignment.Center, new ExcelVerticalAlignment?((ExcelVerticalAlignment)0), true, null, null);
        }

        [HttpPost]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Deleted", ActionName = "Xóa lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteCalendarLeader(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            if (id <= 0)
                return Json(JsonResponse.Json500("Thao tác không hợp lệ!"));
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(id);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.ISDELETE = true;
            unitOfWork.CalendarDuty.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa thông tin lịch lãnh đạo thành công", "N/A", "N/A", id, string.Empty, string.Empty);
            return Json(JsonResponse.Json200(Localization.MsgDelSuccess));
        }

        [ActionDescription(ActionCode = "CalendarDutyLeader_Send", ActionName = "Gửi duyệt lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [HttpGet]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SendApprovedLeader(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            int result = 0;
            if (!int.TryParse(idCalendarDuty, out result))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 2;
            byId.USER_CREATE_ID = UserX.ADMIN_USER_ID;
            byId.DATE_CREATE = DateTime.Now;
            unitOfWork.CalendarDuty.Update(byId);
            ViewBag.objCalendarDuty = byId;
            WriteLog(enLogType.NomalLog, enActionType.SendApproved, "Gửi duyệt lịch trực lãnh đạo thành công", "N/A", "N/A", result, string.Empty, string.Empty);
            if (byId.ISAPPROVED == 1)
                unitOfWork.CalendarChanges.UpdateStatus(result, 2);
            return null;
        }

        [ActionDescription(ActionCode = "CalendarDutyLeader_CancelApproved", ActionName = "Hủy duyệt lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult CancelApprovedLeader(CalendarDutyModel objCalendarDuty, string types)
        {
            int calendarDutyId = objCalendarDuty.CALENDAR_DUTY_ID;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(calendarDutyId);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 4;
            byId.COMMENTS = objCalendarDuty.COMMENTS;
            byId.USER_APPROVED_ID = null;
            byId.DATE_APPROVED = null;
            unitOfWork.CalendarDuty.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.ApproveCancel, "Hủy duyệt lịch trực lãnh đạo thành công ", objCalendarDuty.COMMENTS, "N/A", calendarDutyId, string.Empty, string.Empty);
            ViewBag.objCalendarDuty = byId;
            return null;
        }

        [HttpGet]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Approved", ActionName = "Phê duyệt lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        public ActionResult ApprovedCalendarLeader(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            int result = 0;
            if (!int.TryParse(idCalendarDuty, out result))
                return null;
            CALENDAR_DUTY byId1 = unitOfWork.CalendarDuty.GetById(result);
            if (byId1 == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId1.CALENDAR_STATUS = 3;
            byId1.ISAPPROVED = 1;
            byId1.USER_APPROVED_ID = UserX.ADMIN_USER_ID;
            byId1.DATE_APPROVED = DateTime.Now;
            unitOfWork.CalendarDuty.Update(byId1);
            CALENDAR_DUTY calendarDuty = unitOfWork.CalendarDuty.CheckCalendarHospital(Convert.ToInt32(byId1.CALENDAR_MONTH), Convert.ToInt32(byId1.CALENDAR_YEAR), 4);
            if (calendarDuty == null)
                unitOfWork.CalendarDuty.Add(new CALENDAR_DUTY()
                {
                    CALENDAR_NAME = "Lịch trực toàn bệnh viện tháng " + byId1.CALENDAR_MONTH + " năm " + byId1.CALENDAR_YEAR,
                    CALENDAR_MONTH = byId1.CALENDAR_MONTH,
                    CALENDAR_YEAR = byId1.CALENDAR_YEAR,
                    CALENDAR_STATUS = new int?(CalendarDutyStatus.Created.GetHashCode()),
                    DUTY_TYPE = new int?(DutyType.Hospital.GetHashCode()),
                    ISDELETE = false,
                    USER_CREATE_ID = null,
                    LM_DEPARTMENT_PARTS = null
                });
            if (unitOfWork.CalendarGroups.CheckIsExist(calendarDuty.CALENDAR_DUTY_ID, result, Convert.ToInt32(byId1.CALENDAR_MONTH), Convert.ToInt32(byId1.CALENDAR_YEAR)) == null)
                unitOfWork.CalendarGroups.Add(new CALENDAR_GROUP()
                {
                    CALENDAR_ID = calendarDuty.CALENDAR_DUTY_ID,
                    CALENDAR_PARENT_ID = result,
                    CALENDAR_MONTH = calendarDuty.CALENDAR_MONTH,
                    CALENDAR_YEAR = calendarDuty.CALENDAR_YEAR,
                    CALENDAR_TYPE = 2,
                    CALENDAR_STATUS = 0
                });
            bool flag = unitOfWork.CalendarGroups.CheckIsCalendarApproved(result);
            int hashCode1 = CalendarChangeApproved.Approved.GetHashCode();
            if (!flag)
            {
                List<CALENDAR_CHANGE> listByIdCalendar = unitOfWork.CalendarChanges.GetListByIdCalendar(result, 2);
                if (listByIdCalendar.Count > 0)
                {
                    DoctorData doctorData1 = new DoctorData();
                    CALENDAR_DOCTOR calendarDoctor = new CALENDAR_DOCTOR();
                    CALENDAR_DATA calendarData = new CALENDAR_DATA();
                    foreach (CALENDAR_CHANGE entity1 in listByIdCalendar)
                    {
                        DoctorData doctorData2 = unitOfWork.DoctorDatas.CheckDoctorData(Convert.ToInt32(entity1.CALENDAR_DUTY_ID), Convert.ToInt32(entity1.DOCTORS_ID), Convert.ToInt32(entity1.TEMPLATE_COLUMN_ID), Convert.ToDateTime(entity1.DATE_START));
                        int? status;
                        if (doctorData2 != null)
                        {
                            status = entity1.STATUS;
                            int hashCode2 = CalendarChangeType.Change.GetHashCode();
                            if ((status.GetValueOrDefault() != hashCode2 ? 0 : (status.HasValue ? 1 : 0)) != 0)
                            {
                                CALENDAR_DOCTOR byId2 = unitOfWork.CalendarDoctors.GetById(doctorData2.CALENDAR_DOCTOR_ID);
                                byId2.DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_CHANGE_ID);
                                unitOfWork.CalendarDoctors.Update(byId2);
                                entity1.STATUS_APPROVED = hashCode1;
                                unitOfWork.CalendarChanges.Update(entity1);
                                unitOfWork.Doctors.Save();
                            }
                            status = entity1.STATUS;
                            int hashCode3 = CalendarChangeType.Deleted.GetHashCode();
                            if ((status.GetValueOrDefault() != hashCode3 ? 0 : (status.HasValue ? 1 : 0)) != 0)
                            {
                                unitOfWork.CalendarDoctors.Delete(unitOfWork.CalendarDoctors.GetById(doctorData2.CALENDAR_DOCTOR_ID));
                                unitOfWork.CalendarDatas.Delete(unitOfWork.CalendarDatas.GetById(doctorData2.CALENDAR_DATA_ID));
                                entity1.STATUS_APPROVED = hashCode1;
                                unitOfWork.CalendarChanges.Update(entity1);
                                unitOfWork.Doctors.Save();
                            }
                        }
                        else
                        {
                            status = entity1.STATUS;
                            int hashCode2 = CalendarChangeType.Add.GetHashCode();
                            if ((status.GetValueOrDefault() != hashCode2 ? 0 : (status.HasValue ? 1 : 0)) != 0)
                            {
                                CALENDAR_DATA entity2 = new CALENDAR_DATA();
                                entity2.CALENDAR_DUTY_ID = result;
                                entity2.TEMPLATE_COLUM_ID = entity1.TEMPLATE_COLUMN_ID;
                                entity2.DATE_START = entity1.DATE_START;
                                entity2.ISDELETE = false;
                                unitOfWork.CalendarDatas.Add(entity2);
                                unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                                {
                                    DOCTORS_ID = Convert.ToInt32(entity1.DOCTORS_ID),
                                    CALENDAR_DUTY_ID = result,
                                    COLUMN_LEVEL_ID = new int?(Convert.ToInt32(entity1.TEMPLATE_COLUMN_ID)),
                                    CALENDAR_DATA_ID = entity2.CALENDAR_DATA_ID
                                });
                                entity1.STATUS_APPROVED = hashCode1;
                                unitOfWork.CalendarChanges.Update(entity1);
                                unitOfWork.Doctors.Save();
                            }
                        }
                    }
                    doctorData1 = null;
                    calendarDoctor = null;
                    calendarData = null;
                }
            }
            ViewBag.objCalendarDuty = byId1;
            WriteLog(enLogType.NomalLog, enActionType.Approve, "Phê duyệt lịch lãnh đạo thành công", "N/A", "N/A", result, string.Empty, string.Empty);
            return null;
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_Insert", ActionName = "Lập bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [HttpGet]
        [CustomAuthorize]
        public PartialViewResult LoadCalendarDirectors(string strDate)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result1 = dateTime.Month;
            int result2 = dateTime.Year;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                if (int.TryParse(strArray[0].ToString(), out result1) && int.TryParse(strArray[1].ToString(), out result2))
                    dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result1), Convert.ToString(result2));
            }
            int hashCode = DutyType.Directors.GetHashCode();
            List<DoctorCalendarLeader> calendarDirector1 = unitOfWork.CalendarDuty.GetDoctorCalendarDirector(result1, result2, hashCode);
            List<CALENDAR_DUTY> calendarDirector2 = unitOfWork.CalendarDuty.GetCalendarDirector(result1, result2, hashCode);
            ViewBag.times = dateTime;
            ViewBag.doctors = calendarDirector1;
            ViewBag.objCalendarDuty = calendarDirector2;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_AddCalendarDirectorsData");
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_Insert", ActionName = "Lập bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [HttpGet]
        [CustomAuthorize]
        public ActionResult AddCalendarDirectors()
        {
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return View("_AddCalendarDirectors");
        }

        [CustomAuthorize]
        [HttpGet]
        [ActionDescription(ActionCode = "CalendarDutyDirectors_Edit", ActionName = "Sửa bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [ValidateJsonAntiForgeryToken]
        public PartialViewResult EditCalendarDirectors(string idCalendarDuty)
        {
            if (!Request.IsAjaxRequest())
                return null;
            int result1 = 0;
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result1))
            {
                calendarDuty = unitOfWork.CalendarDuty.GetById(result1);
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarLeader(result1);
            }
            ViewBag.objCalendarDuty = calendarDuty;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result2 = dateTime.Month;
            int result3 = dateTime.Year;
            int? nullable = calendarDuty.CALENDAR_MONTH;
            int num;
            if (int.TryParse(nullable.ToString(), out result2))
            {
                nullable = calendarDuty.CALENDAR_YEAR;
                num = !int.TryParse(nullable.ToString(), out result3) ? 1 : 0;
            }
            else
                num = 1;
            if (num == 0)
                dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result2), Convert.ToString(result3));
            ViewBag.doctors = doctorCalendarLeaderList;
            ViewBag.times = dateTime;
            ViewBag.idCalendarDuty = idCalendarDuty;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_EditCalendarDirectors");
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_View", ActionName = "Xem bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [HttpGet]
        [CustomAuthorize]
        public ActionResult ListCalendarDirectors(string idCalendarDuty)
        {
            DateTime dateTime = Utils.Utils.GetDateTime();
            int? nullable1 = dateTime.Month;
            int? nullable2 = dateTime.Year;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            int result = 0;
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result))
            {
                CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
                nullable1 = byId.CALENDAR_MONTH;
                nullable2 = byId.CALENDAR_YEAR;
                calendarDuty = null;
            }
            ViewBag.month = nullable1;
            ViewBag.year = nullable2;
            ViewBag.idCalendarDuty = string.Empty;
            return View("_ViewListCalendarDirectors");
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyDirectors_View", ActionName = "Xem bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public PartialViewResult PartialViewListCalendarDirectors(string idCalendarDuty)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int? nullable1 = dateTime.Month;
            int? nullable2 = dateTime.Year;
            CALENDAR_DUTY calendarDuty = new CALENDAR_DUTY();
            int result = 0;
            if (!string.IsNullOrEmpty(idCalendarDuty) && int.TryParse(idCalendarDuty, out result))
            {
                CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
                nullable1 = byId.CALENDAR_MONTH;
                nullable2 = byId.CALENDAR_YEAR;
                calendarDuty = null;
            }
            ViewBag.month = nullable1;
            ViewBag.year = nullable2;
            ViewBag.idCalendarDuty = idCalendarDuty;
            return PartialView("_ViewListCalendarDirectors");
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_View", ActionName = "Xem bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [CustomAuthorize]
        [HttpGet]
        public PartialViewResult LoadListCalendarDirectors(string strDate)
        {
            if (!Request.IsAjaxRequest())
                return null;
            DateTime dateTime = Utils.Utils.GetDateTime();
            int result1 = dateTime.Month;
            int result2 = dateTime.Year;
            string str = string.Empty;
            if (strDate != null)
            {
                string[] strArray = strDate.Split('_');
                if (int.TryParse(strArray[0].ToString(), out result1) && int.TryParse(strArray[1].ToString(), out result2))
                    dateTime = Utils.Utils.ConvertToDateTime("1", Convert.ToString(result1), Convert.ToString(result2));
                if ((strArray).Count<string>() > 2)
                    str = strArray[2].ToString();
            }
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            List<CALENDAR_DUTY> calendarDutyList = null;
            int hashCode = DutyType.Directors.GetHashCode();
            if (!string.IsNullOrEmpty(Convert.ToString(result1)) && !string.IsNullOrEmpty(Convert.ToString(result2)))
            {
                calendarDutyList = unitOfWork.CalendarDuty.GetCalendarDirector(result1, result2, hashCode);
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarDirector(result1, result2, hashCode);
            }
            if (string.IsNullOrEmpty(str) && (calendarDutyList != null && calendarDutyList.Count > 0))
                str = Convert.ToString(calendarDutyList[0].CALENDAR_DUTY_ID);
            ViewBag.idCalendarDuty = str;
            ViewBag.objCalendarDuty = calendarDutyList;
            ViewBag.doctors = doctorCalendarLeaderList;
            ViewBag.times = dateTime;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_ViewCalendarDirectors");
        }

        [ValidateJsonAntiForgeryToken]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyDirectors_View", ActionName = "Xem bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        public ActionResult CalendarDirectors()
        {
            int hashCode = DutyType.Directors.GetHashCode();
            ViewBag.types = hashCode;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.ActionPermission = actionCodesByUserName;
            return View("~/Views/CalendarDuty/Index.cshtml");
        }

        [CustomAuthorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [ActionDescription(ActionCode = "CalendarDutyDirectors_Insert", ActionName = "Lập bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SaveCalendarDirectors(string strValues, string strInfor, string calendarContent)
        {
            string[] strArray = strInfor.Split('_');
            (strArray).Count<string>();
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            int result1 = 0;
            int result2 = 0;
            if (!int.TryParse(strArray[0], out result1) || !int.TryParse(strArray[1], out result2))
                return null;
            int hashCode = DutyType.Directors.GetHashCode();
            CALENDAR_DUTY calendarDuty;
            if (!unitOfWork.CalendarDuty.checkCalendar(result1, result2, hashCode, Convert.ToInt32(byUserName.LM_DEPARTMENT_ID), 0))
            {
                CALENDAR_DUTY entity = new CALENDAR_DUTY();
                entity.CALENDAR_NAME = calendarContent.Trim();
                entity.CALENDAR_MONTH = result1;
                entity.CALENDAR_YEAR = result2;
                entity.CALENDAR_STATUS = 1;
                entity.LM_DEPARTMENT_ID = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
                entity.DUTY_TYPE = 2;
                entity.ISDELETE = false;
                entity.USER_CREATE_ID = byUserName.ADMIN_USER_ID;
                entity.DATE_CREATE = new DateTime?(Utils.Utils.GetDateTime());
                unitOfWork.CalendarDuty.Add(entity);
                CALENDAR_DATA objCalendarData = null;
                CALENDAR_DOCTOR objCalendarDoctor = null;
                int calendarDutyId = entity.CALENDAR_DUTY_ID;
                SaveData(strValues, objCalendarData, objCalendarDoctor, calendarDutyId, result1, result2);
                calendarDuty = null;
                WriteLog(enLogType.NomalLog, enActionType.Insert, "Thêm mới lịch trực thường trú Ban giám đốc thành công", "N/A", "N/A", calendarDutyId, string.Empty, string.Empty);
                return Json(Localization.AddCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
            }
            int num = 0;
            List<CALENDAR_DUTY> calendarDutyList = null;
            List<CALENDAR_DUTY> calendarDirector = unitOfWork.CalendarDuty.GetCalendarDirector(result1, result2, hashCode);
            if (calendarDirector.Count > 0)
                num = calendarDirector[0].CALENDAR_DUTY_ID;
            calendarDutyList = null;
            unitOfWork.CalendarDoctors.DeleteCalendarDoctorById(num);
            unitOfWork.CalendarDatas.DeleteCalendarDataById(num);
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(num);
            byId.CALENDAR_NAME = calendarContent.Trim();
            unitOfWork.CalendarDuty.Update(byId);
            calendarDuty = null;
            CALENDAR_DATA objCalendarData1 = null;
            CALENDAR_DOCTOR objCalendarDoctor1 = null;
            SaveData(strValues, objCalendarData1, objCalendarDoctor1, num, result1, result2);
            WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật lịch trực thường trú Ban giám đốc thành công", "N/A", "N/A", num, string.Empty, string.Empty);
            return Json(Localization.UpdateCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_Edit", ActionName = "Sửa bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc")]
        [ValidateJsonAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize]
        public ActionResult UpdateCalendarDirectors(string strValues, string strInfor, string calendarContent)
        {
            try
            {
                string[] strArray = strInfor.Split('_');
                int result1 = 0;
                int result2 = 0;
                int result3 = 0;
                int.TryParse(strArray[0], out result1);
                int.TryParse(strArray[1], out result2);
                int.TryParse(strArray[2], out result3);
                CALENDAR_DATA objCalendarData = null;
                CALENDAR_DOCTOR objCalendarDoctor = null;
                unitOfWork.CalendarDoctors.DeleteCalendarDoctorById(result3);
                unitOfWork.CalendarDatas.DeleteCalendarDataById(result3);
                CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result3);
                byId.CALENDAR_NAME = calendarContent.Trim();
                unitOfWork.CalendarDuty.Update(byId);
                SaveData(strValues, objCalendarData, objCalendarDoctor, result3, result1, result2);
                WriteLog(enLogType.NomalLog, enActionType.Update, "Cập nhật lịch trực thường trú Ban giám đốc thành công", "N/A", "N/A", result3, string.Empty, string.Empty);
                return Json(Localization.UpdateCalendarSuccess.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return null;
            }
        }

        private void SaveData(string strValues, CALENDAR_DATA objCalendarData, CALENDAR_DOCTOR objCalendarDoctor, int calendarDutyId, int monthx, int yearx)
        {
            string[] strArray1 = strValues.Split('-');
            string str1 = string.Empty;
            string str2 = string.Empty;
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            DateTime? nullable1 = null;
            DateTime? nullable2 = null;
            foreach (string str3 in strArray1)
            {
                char[] chArray = new char[1] { '_' };
                string[] strArray2 = str3.Split(chArray);
                if ((strArray2).Count<string>() >= 4)
                {
                    int.TryParse(strArray2[0], out result1);
                    int.TryParse(strArray2[1], out result2);
                    int.TryParse(strArray2[2], out result3);
                    if (!string.IsNullOrEmpty(strArray2[3]))
                        str1 = Convert.ToString(strArray2[3]);
                    DateTime? nullable3;
                    try
                    {
                        nullable3 = new DateTime?(Utils.Utils.ConvertToDateTime(Convert.ToString(result1), Convert.ToString(monthx), Convert.ToString(yearx)));
                    }
                    catch
                    {
                        nullable3 = null;
                    }
                    DateTime? nullable4;
                    try
                    {
                        nullable4 = new DateTime?(Utils.Utils.ConvertToDateTime(Convert.ToString(result2), Convert.ToString(monthx), Convert.ToString(yearx)));
                    }
                    catch
                    {
                        nullable4 = null;
                    }
                    string str4 = result1.ToString() + "/" + monthx + "/" + yearx + " -  " + result2 + "/" + monthx + "/" + yearx;
                    if (result1 == 1)
                        str2 = "Tuần 1";
                    else if (result1 == 8)
                        str2 = "Tuần 2";
                    else if (result1 == 15)
                        str2 = "Tuần 3";
                    else if (result1 == 22)
                        str2 = "Tuần 4";
                    objCalendarData = new CALENDAR_DATA();
                    objCalendarData.CALENDAR_DUTY_ID = calendarDutyId;
                    objCalendarData.DATE_START = nullable3;
                    objCalendarData.DATE_END = nullable4;
                    objCalendarData.CALENDAR_NAME = str2;
                    objCalendarData.CALENDAR_CONTENT = str4;
                    objCalendarData.CALENDAR_PHONE = str1;
                    objCalendarData.ISDELETE = false;
                    unitOfWork.CalendarDatas.Add(objCalendarData);
                    objCalendarDoctor = new CALENDAR_DOCTOR();
                    objCalendarDoctor.CALENDAR_DATA_ID = objCalendarData.CALENDAR_DATA_ID;
                    objCalendarDoctor.DOCTORS_ID = result3;
                    objCalendarDoctor.CALENDAR_DUTY_ID = calendarDutyId;
                    unitOfWork.CalendarDoctors.Add(objCalendarDoctor);
                    objCalendarData = null;
                    objCalendarDoctor = null;
                }
            }
        }

        [HttpGet]
        public PartialViewResult ListDirectors(string arrayid, int groupId, int idCalendarDuty, int isChange, string dateX)
        {
            string[] strArray = arrayid.Split('_');
            ViewBag.arrayid = string.Join(",", strArray);
            ViewBag.groupId = groupId;
            ViewBag.isChange = isChange;
            List<DOCTOR> allDoctorByGroup = HttpContext.Cache["lstDoctorLeader" + groupId] as List<DOCTOR>;
            if (allDoctorByGroup == null || allDoctorByGroup.Count == 0)
            {
                allDoctorByGroup = unitOfWork.Doctors.GetAllDoctorByGroup(groupId);
                HttpContext.Cache.Insert("lstDoctorLeader" + groupId, allDoctorByGroup, null, DateTime.MaxValue, TimeSpan.FromMinutes(20.0));
            }
            if (allDoctorByGroup.Any() && idCalendarDuty > 0)
            {
                unitOfWork.CalendarDuty.GetById(idCalendarDuty);
                List<DoctorCalendarLeader> calendarDirector = unitOfWork.CalendarDuty.GetDoctorCalendarDirector(allDoctorByGroup.Select((t => t.DOCTORS_ID)).ToList(), idCalendarDuty);
                if (calendarDirector.Any())
                {
                    ViewBag.lstCalendarDoctor = calendarDirector
                        .OrderBy((t => t.ABBREVIATION))
                        .Select(
                             (t => new DoctorCalendarLeader()
                             {
                                 CALENDAR_DATA_ID = t.CALENDAR_DATA_ID,
                                 CALENDAR_DUTY_ID = t.CALENDAR_DUTY_ID,
                                 DOCTORS_ID = t.DOCTORS_ID,
                                 DOCTOR_NAME = t.DOCTOR_NAME,
                                 CALENDAR_DUTY_NAME = Utils.Utils.StripHTML(t.CALENDAR_DUTY_NAME),
                                 ABBREVIATION = t.ABBREVIATION,
                                 DATE_START = t.DATE_START,
                                 CALENDAR_MONTH = t.CALENDAR_MONTH,
                                 CALENDAR_YEAR = t.CALENDAR_YEAR,
                                 CALENDAR_STATUS = t.CALENDAR_STATUS
                             }))
                        .StringifyJs(true);
                }
            }
            DateTime? nullable = null;
            DateTime? date;
            try
            {
                date = Utils.Utils.ConvetDateVNToDates(dateX);
            }
            catch
            {
                date = null;
            }
            ViewBag.DateX = date;
            if (groupId == 2)
            {
                List<DoctorInCalendar> allDoctorNotLeader = unitOfWork.Doctors.GetAllDoctorNotLeader(date, 1);
                ViewBag.allDoctorCalendar = allDoctorNotLeader;
            }
            return PartialView("_ListDirectors", allDoctorByGroup);
        }

        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Edit", ActionName = "Sửa lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [HttpPost]
        public ActionResult ChangeCalendarLeader(DoctorCalendarLeader dcRequestChange, DoctorCalendarLeader doctorCalendarChange, int idDoctorChange)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            if (dcRequestChange == null)
                return RedirectToAction("Index");
            ITransaction transaction = null;
            try
            {
                transaction = unitOfWork.BeginTransaction();
                CALENDAR_CHANGE calendarChange = new CALENDAR_CHANGE();
                calendarChange.CALENDAR_DUTY_ID = dcRequestChange.CALENDAR_DUTY_ID;
                calendarChange.DATE_START = dcRequestChange.DATE_START;
                calendarChange.DATE_CHANGE_START = doctorCalendarChange.DATE_START;
                calendarChange.DOCTORS_ID = dcRequestChange.DOCTORS_ID;
                calendarChange.DOCTORS_CHANGE_ID = idDoctorChange;
                calendarChange.ADMIN_USER_ID = new int?(unitOfWork.Users.GetByUserName(User.Identity.Name).ADMIN_USER_ID);
                calendarChange.STATUS = 1;
                calendarChange.STATUS_APPROVED = 1;
                calendarChange.DOCTORS_NAME = dcRequestChange.ABBREVIATION;
                calendarChange.DOCTORS_CHANGE_NAME = doctorCalendarChange.ABBREVIATION;
                calendarChange.DATE_CHANGE = DateTime.Now;
                calendarChange.CALENDAR_DELETE = 0;
                if (!unitOfWork.CalendarChanges.ExistChangeCaledar(calendarChange, 1))
                    unitOfWork.CalendarChanges.Add(calendarChange);
                WriteLog(enLogType.NomalLog, enActionType.Insert, "Đổi lịch trực lãnh đạo", "N/A", "N/A", 0, string.Empty, string.Empty);
                transaction.Commit();
                return Json(new
                {
                    doctorCalendarRequestChange = dcRequestChange,
                    doctorCalendarAcceptChange = doctorCalendarChange,
                    objCalendarChange = calendarChange
                });
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

        [HttpGet]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Edit", ActionName = "Sửa lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        public ActionResult CancelChangeCalendarLeader(int calendarChangeId)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            if (calendarChangeId == 0)
                return RedirectToAction("Index");
            CALENDAR_CHANGE byId = unitOfWork.CalendarChanges.GetById(calendarChangeId);
            if (byId == null)
                return RedirectToAction("Index");
            try
            {
                byId.STATUS_APPROVED = 4;
                unitOfWork.CalendarChanges.Update(byId);
                return Json("Hủy đổi lịch thành công", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Hủy đổi lịch lỗi", JsonRequestBehavior.AllowGet);
            }
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_Deleted", ActionName = "Xóa bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc", IsMenu = false)]
        [HttpPost]
        [CustomAuthorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteDirectors(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            if (id <= 0)
                return Json(JsonResponse.Json500("Thao tác không hợp lệ!"));
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(id);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.ISDELETE = true;
            unitOfWork.CalendarDuty.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Delete, "Xóa thông tin lịch thường trú ban giám đốc thành công", "N/A", "N/A", id, string.Empty, string.Empty);
            return Json(JsonResponse.Json200(Localization.MsgDelSuccess));
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_Send", ActionName = "Gửi duyệt bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc", IsMenu = false)]
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public ActionResult SendApprovedDirectors(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            int result = 0;
            if (!int.TryParse(idCalendarDuty, out result))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 2;
            byId.USER_CREATE_ID = UserX.ADMIN_USER_ID;
            byId.DATE_CREATE = DateTime.Now;
            unitOfWork.CalendarDuty.Update(byId);
            ViewBag.objCalendarDuty = byId;
            WriteLog(enLogType.NomalLog, enActionType.SendApproved, "Gửi duyệt lịch trực thường trú Ban giám đốc thành công", "N/A", "N/A", result, string.Empty, string.Empty);
            return null;
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_CancelApproved", ActionName = "Hủy duyệt bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc", IsMenu = false)]
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public ActionResult CancelApprovedDirectors(CalendarDutyModel objCalendarDuty, string types)
        {
            int calendarDutyId = objCalendarDuty.CALENDAR_DUTY_ID;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(calendarDutyId);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 4;
            byId.COMMENTS = objCalendarDuty.COMMENTS;
            byId.USER_APPROVED_ID = null;
            byId.DATE_APPROVED = null;
            unitOfWork.CalendarDuty.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.ApproveCancel, "Hủy duyệt lịch trực thường trú Ban giám đốc thành công ", objCalendarDuty.COMMENTS, "N/A", calendarDutyId, string.Empty, string.Empty);
            ViewBag.objCalendarDuty = byId;
            return null;
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "CalendarDutyDirectors_Approved", ActionName = "Phê duyệt bảng thường trú Ban giám đốc", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc", IsMenu = false)]
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public ActionResult ApprovedCalendarDirector(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            int result = 0;
            if (!int.TryParse(idCalendarDuty, out result))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result);
            if (byId == null)
                return Json(JsonResponse.Json500("Thông tin không tồn tại!"));
            byId.CALENDAR_STATUS = 3;
            byId.USER_APPROVED_ID = UserX.ADMIN_USER_ID;
            byId.DATE_APPROVED = DateTime.Now;
            unitOfWork.CalendarDuty.Update(byId);
            ViewBag.objCalendarDuty = byId;
            WriteLog(enLogType.NomalLog, enActionType.Approve, "Phê duyệt lịch trực thường trú Ban giám đốc thành công", "N/A", "N/A", result, string.Empty, string.Empty);
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            List<SendSms6x00> listSms = new List<SendSms6x00>();
            List<DoctorCalendarLeader> byCalendarDutyId = unitOfWork.CalendarDuty.GetDoctorByCalendarDutyId(result);
            if (byCalendarDutyId != null && byCalendarDutyId.Count > 0)
            {
                for (int index = 0; index < byCalendarDutyId.Count; ++index)
                    listSms.Add(new SendSms6x00()
                    {
                        Phone = byCalendarDutyId[index].PHONE,
                        DoctorId = byCalendarDutyId[index].DOCTORS_ID,
                        DoctorName = byCalendarDutyId[index].DOCTOR_NAME,
                        Contents = "BM-Lichtruc:Đ/c Trực thường trú BGĐ tháng " + byCalendarDutyId[index].CALENDAR_MONTH + "/" + byCalendarDutyId[index].CALENDAR_YEAR + " từ " + byCalendarDutyId[index].CALENDAR_CONTENT
                    });
                doctorCalendarLeaderList = null;
            }
            sendSMSBrandname(listSms);
            return null;
        }

        public ActionResult ApprovedRequestDirector(string idCalendarDuty, string types)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            ViewBag.idCalendarDuty = idCalendarDuty;
            int result1 = 0;
            if (!int.TryParse(idCalendarDuty, out result1))
                return null;
            CALENDAR_DUTY byId = unitOfWork.CalendarDuty.GetById(result1);
            CalendarDutyModel calendarDutyModel = new CalendarDutyModel();
            calendarDutyModel.CALENDAR_DUTY_ID = byId.CALENDAR_DUTY_ID;
            calendarDutyModel.CALENDAR_NAME = byId.CALENDAR_NAME;
            calendarDutyModel.COMMENTS = byId.COMMENTS;
            int result2 = 0;
            if (!int.TryParse(types, out result2))
                return null;
            calendarDutyModel.DUTY_TYPE = result2;
            return PartialView("_ApprovedRequest", calendarDutyModel);
        }

        [ActionDescription(ActionCode = "CalendarDutyDirectors_Export", ActionName = "Xuất dữ liệu Excel", GroupCode = "CalendarDutyDirectors", GroupName = "Lịch thường trú Ban giám đốc", IsMenu = false)]
        [CustomAuthorize]
        public ActionResult ExportDirector(int iMonth, int iYear)
        {
            try
            {
                string fileName = "LichTT_" + iMonth + "_" + iYear;
                return new DownloadResult(GenerateByteDirector(fileName, iMonth, iYear), fileName + ".xlsx");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.ExportExcel, "N/A", "Xuất danh sách nhân viên thất bại!", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        private byte[] GenerateByteDirector(string fileName, int iMonth, int iYear)
        {
            FileInfo fileInfo = new FileInfo(HttpContext.Server.MapPath("~/Views/Shared/ExcelTemplate/ReportOfCalendarDirector.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Không tìm thấy tệp tin!");
            DateTime now = DateTime.Now;
            DataTable dataTable = Utils.Utils.CalendarWeekly(Utils.Utils.ConvertToDateTime("1", Convert.ToString(iMonth), Convert.ToString(iYear)));
            List<DoctorCalendarLeader> doctorCalendarLeaderList = null;
            if (!string.IsNullOrEmpty(Convert.ToString(iMonth)) && !string.IsNullOrEmpty(Convert.ToString(iYear)))
                doctorCalendarLeaderList = unitOfWork.CalendarDuty.GetDoctorCalendarDirector(iMonth, iYear, DutyType.Directors.GetHashCode());
            int num1 = 1;
            int num2 = 7;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
                excelWorksheet.Name = fileName;
                string str1 = "THÁNG " + iMonth + " NĂM " + iYear;
                excelWorksheet.Cells["A5"].Value = str1.ToUpper();
                //excelWorksheet.Cells["A2"];
                int num3;
                for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
                {
                    DataRow row = dataTable.Rows[index1];
                    string str2 = Convert.ToString(row["Weekly"]);
                    string str3 = Convert.ToString(row["Start"]);
                    string str4 = Convert.ToString(row["End"]);
                    ExcelRange cells1 = excelWorksheet.Cells;
                    string str5 = "B";
                    num3 = num1 + num2;
                    string str6 = num3.ToString();
                    string str7 = str5 + str6;
                    (cells1[str7]).Value = str2;
                    ExcelRange cells2 = excelWorksheet.Cells;
                    string str8 = "A";
                    num3 = num1 + num2;
                    string str9 = num3.ToString();
                    string str10 = str8 + str9;
                    ExcelRange excelRange = cells2[str10];
                    num3 = num1++;
                    string str11 = num3.ToString();
                    (excelRange).Value = str11;
                    string str12 = string.Empty;
                    string str13 = string.Empty;
                    if (doctorCalendarLeaderList != null && doctorCalendarLeaderList.Count > 0)
                    {
                        for (int index2 = 0; index2 < doctorCalendarLeaderList.Count; ++index2)
                        {
                            if (Convert.ToDateTime(doctorCalendarLeaderList[index2].DATE_START).Day >= Convert.ToInt32(str3) && Convert.ToDateTime(doctorCalendarLeaderList[index2].DATE_START).Day <= Convert.ToInt32(str4))
                            {
                                str12 = !(str12 == string.Empty) ? str12 + "\n" + doctorCalendarLeaderList[index2].DOCTOR_NAME : str12 + doctorCalendarLeaderList[index2].DOCTOR_NAME;
                                str13 = !(str13 == string.Empty) ? str13 + "\n" + doctorCalendarLeaderList[index2].CALENDAR_PHONE : str13 + doctorCalendarLeaderList[index2].CALENDAR_PHONE;
                            }
                        }
                    }
                    ExcelRange cells3 = excelWorksheet.Cells;
                    string str14 = "C";
                    num3 = num1 + num2 - 1;
                    string str15 = num3.ToString();
                    string str16 = str14 + str15;
                    (cells3[str16]).Value = str12;
                    ExcelRange cells4 = excelWorksheet.Cells;
                    string str17 = "D";
                    num3 = num1 + num2 - 1;
                    string str18 = num3.ToString();
                    string str19 = str17 + str18;
                    (cells4[str19]).Value = str13;
                    excelWorksheet.Cells["D13"].Value = ("Ngày " + now.Day + " tháng " + now.Month + " năm " + now.Year);
                }
                List<CONFIG_REPORT> allByExcelName = unitOfWork.ConfigReports.GetAllByExcelName("ReportOfCalendarDirector.xlsx");
                if (allByExcelName != null && allByExcelName.Count > 0)
                {
                    excelWorksheet.Cells["A15:B15"].Merge = true;
                    excelWorksheet.Cells["A21:B21"].Merge = true;
                    excelWorksheet.Cells["A15"].Value = allByExcelName[0].FUNCTION1;
                    excelWorksheet.Cells["A15"].Style.Font.Bold = true;
                    excelWorksheet.Cells["A21"].Value = allByExcelName[0].SIGNATURE1;
                    excelWorksheet.Cells["A21"].Style.Font.Bold = true;
                    excelWorksheet.Cells["D15"].Value = allByExcelName[0].FUNCTION2;
                    excelWorksheet.Cells["D15"].Style.Font.Bold = true;
                    excelWorksheet.Cells["D21"].Value = allByExcelName[0].SIGNATURE2;
                    excelWorksheet.Cells["D21"].Style.Font.Bold = true;
                    excelWorksheet.Cells["A15:D21"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    excelWorksheet.Cells["A15:D21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                ExcelRange cells5 = excelWorksheet.Cells;
                string str20 = "A8:D";
                num3 = num1 + num2 - 1;
                string str21 = num3.ToString();
                string str22 = str20 + str21;
                ExcelRange excelRange1 = cells5[str22];
                (excelRange1).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                (excelRange1).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                (excelRange1).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                (excelRange1).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                (excelRange1).Style.Font.Size = 13f;
                (excelRange1).Style.Border.BorderAround(ExcelBorderStyle.Thin);
                (excelRange1).Style.Numberformat.Format = "@";
                (excelRange1).Style.WrapText = true;
                ExcelRange cells6 = excelWorksheet.Cells;
                string str23 = "A8:A";
                num3 = num1 + num2 - 1;
                string str24 = num3.ToString();
                string str25 = str23 + str24;
                ExcelRange excelRange2 = cells6[str25];
                (excelRange2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                (excelRange2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                return excelPackage.GetAsByteArray();
            }
        }

        [ValidateJsonAntiForgeryToken]
        [ActionDescription(ActionCode = "CalendarDuty_Insert", ActionName = "Lập lịch trực", GroupCode = "CalendarDuty", GroupName = "Lịch trực Khoa/Viện/TT", IsMenu = false)]
        [HttpPost]
        [CustomAuthorize]
        [ValidateInput(false)]
        public ActionResult AutoScheduling(int iMonth, int iYear, int templateId, string contentDuty)
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            int int32 = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
            int num1 = unitOfWork.CalendarDuty.CheckCalendarDuty(iMonth, iYear, templateId, int32);
            List<CONFIG_PARAMETES> all = unitOfWork.ConfigParameter.GetAll(int32, iYear, 2);
            int? nullable1 = 1;
            int num2 = 0;
            if (all != null && all.Count > 0)
            {
                nullable1 = all[0].NUMBER_DOCTOR_IN_DAY;
                num2 = all[0].TIME_DISTANCE;
            }
            if (num1 == 0)
            {
                TEMPLATE byId = unitOfWork.Templates.GetById(templateId);
                CALENDAR_DUTY entity1 = new CALENDAR_DUTY();
                entity1.CALENDAR_NAME = contentDuty;
                entity1.CALENDAR_MONTH = iMonth;
                entity1.CALENDAR_YEAR = iYear;
                entity1.CALENDAR_STATUS = new int?(CalendarDutyStatus.Created.GetHashCode());
                entity1.TEMPLATES_ID = templateId;
                entity1.LM_DEPARTMENT_ID = byId.LM_DEPARTMENT_ID;
                entity1.DUTY_TYPE = new int?(DutyType.Deparment.GetHashCode());
                entity1.ISDELETE = false;
                entity1.USER_CREATE_ID = byUserName.ADMIN_USER_ID;
                entity1.LM_DEPARTMENT_PARTS = byUserName.LM_DEPARTMENT.DEPARTMENT_PATH + byId.LM_DEPARTMENT_PATH + ",";
                unitOfWork.CalendarDuty.Add(entity1);
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                sqlParameterList.Add(new SqlParameter("@MONTH", iMonth));
                sqlParameterList.Add(new SqlParameter("@YEAR", iYear));
                sqlParameterList.Add(new SqlParameter("@LM_DEPARTMENT_ID", int32));
                sqlParameterList.Add(new SqlParameter("@TEMPLATE_ID", templateId));
                BACHMAICRContext bachmaicrContext = new BACHMAICRContext();
                List<AUTO_CALENDAR_DOCTOR> list1 = bachmaicrContext.Database.SqlQuery<AUTO_CALENDAR_DOCTOR>("exec sp_insertAutoCalendar @MONTH, @YEAR, @LM_DEPARTMENT_ID, @TEMPLATE_ID", (object[])sqlParameterList.ToArray()).ToList();
                List<DOCTOR> allByDepartmentId = unitOfWork.Doctors.GetAllByDepartmentId(int32);
                bachmaicrContext.Dispose();
                int idColumnX = 0;
                int num3 = 0;
                int num4 = DateTime.DaysInMonth(iYear, iMonth);
                DateTime? date = Utils.Utils.ConvetDateVNToDate("1/" + iMonth + "/" + iYear);
                List<TEMPLATE_COLUM> columnByIdTemplate = unitOfWork.TemplatesColumn.GetColumnByIDTemplate(templateId, int32);
                if (columnByIdTemplate != null)
                {
                    for (int index1 = 0; index1 < columnByIdTemplate.Count; ++index1)
                    {
                        idColumnX = columnByIdTemplate[index1].TEMPLATE_COLUM_ID;
                        int num5 = 0;
                        int num6 = 0;
                        do
                        {
                            foreach (DOCTOR doctor in allByDepartmentId)
                            {
                                DOCTOR item = doctor;
                                List<AUTO_CALENDAR_DOCTOR> list2 = list1.Where((o => o.DOCTORS_ID == item.DOCTORS_ID && o.TEMPLATE_COLUM_ID == idColumnX)).ToList();
                                if (list2 != null && list2.Count > 0)
                                {
                                    ++num6;
                                    for (int index2 = 0; index2 < list2.Count; ++index2)
                                    {
                                        int idDoctor = list2[index2].DOCTORS_ID;
                                        int countHoliday = list2[index2].COUNT_HOLIDAY;
                                        int countNumberDirect = list2[index2].COUNT_NUMBER_DIRECT;
                                        DateTime? dateContinue = list2[index2].DATE_CONTINUE;
                                        DateTime dateTime = date.Value;
                                        DateTime? dateCheck = new DateTime?(dateTime.AddDays(num5));
                                        int num7;
                                        if (dateCheck.HasValue)
                                        {
                                            dateTime = dateCheck.Value;
                                            num7 = dateTime.Day > num4 ? 1 : 0;
                                        }
                                        else
                                            num7 = 1;
                                        if (num7 == 0)
                                        {
                                            if (dateContinue.HasValue)
                                            {
                                                dateTime = dateCheck.Value;
                                                int day1 = dateTime.Day;
                                                dateTime = dateContinue.Value;
                                                int day2 = dateTime.Day;
                                                if (day1 >= day2)
                                                {
                                                    dateTime = dateCheck.Value;
                                                    int month1 = dateTime.Month;
                                                    dateTime = dateContinue.Value;
                                                    int month2 = dateTime.Month;
                                                    if (month1 == month2)
                                                    {
                                                        dateTime = dateCheck.Value;
                                                        int year1 = dateTime.Year;
                                                        dateTime = dateContinue.Value;
                                                        int year2 = dateTime.Year;
                                                        if (year1 == year2)
                                                            goto label_24;
                                                    }
                                                }
                                            }
                                            if (dateContinue.HasValue)
                                            {
                                                dateTime = dateCheck.Value;
                                                int month1 = dateTime.Month;
                                                dateTime = dateContinue.Value;
                                                int month2 = dateTime.Month;
                                                if (month1 > month2)
                                                {
                                                    dateTime = dateCheck.Value;
                                                    int year1 = dateTime.Year;
                                                    dateTime = dateContinue.Value;
                                                    int year2 = dateTime.Year;
                                                    if (year1 == year2)
                                                        goto label_24;
                                                }
                                            }
                                            if (dateContinue.HasValue)
                                            {
                                                dateTime = dateCheck.Value;
                                                int year1 = dateTime.Year;
                                                dateTime = dateContinue.Value;
                                                int year2 = dateTime.Year;
                                                if (year1 > year2)
                                                    goto label_24;
                                            }
                                            int num8 = dateContinue.HasValue ? 1 : 0;
                                            goto label_25;
                                            label_24:
                                            num8 = 0;
                                            label_25:
                                            if (num8 == 0 && !unitOfWork.CalendarAuto.CheckCalendarDoctorAuto(idDoctor, dateCheck))
                                            {
                                                bool flag = false;
                                                if (countHoliday > 0 && unitOfWork.ConfigHolidays.CheckCalendarHoliday(idDoctor, dateCheck))
                                                    flag = true;
                                                if (!flag && !unitOfWork.CalendarAuto.CheckCalendarDoctor(idDoctor, dateCheck))
                                                {
                                                    CALENDAR_DATA entity2 = new CALENDAR_DATA();
                                                    entity2.CALENDAR_DUTY_ID = entity1.CALENDAR_DUTY_ID;
                                                    entity2.TEMPLATE_COLUM_ID = idColumnX;
                                                    entity2.DATE_START = dateCheck;
                                                    entity2.ISDELETE = false;
                                                    unitOfWork.CalendarDatas.Add(entity2);
                                                    unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                                                    {
                                                        CALENDAR_DATA_ID = entity2.CALENDAR_DATA_ID,
                                                        DOCTORS_ID = idDoctor,
                                                        CALENDAR_DUTY_ID = entity1.CALENDAR_DUTY_ID,
                                                        COLUMN_LEVEL_ID = idColumnX
                                                    });
                                                    unitOfWork.CalendarAuto.Add(new CALENDAR_AUTO()
                                                    {
                                                        LM_DEPARTMENT_ID = int32,
                                                        TEMPLATES_ID = templateId,
                                                        TEMPLATE_COLUM_ID = idColumnX,
                                                        DOCTORS_ID = idDoctor,
                                                        DATE_CREATE = dateCheck
                                                    });
                                                    ++num3;
                                                    int num9 = num3;
                                                    int? nullable2 = nullable1;
                                                    if ((num9 != nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                                                    {
                                                        ++num5;
                                                        num3 = 0;
                                                    }
                                                    List<AUTO_CALENDAR_DOCTOR> list3 = list1.Where((o => o.DOCTORS_ID == idDoctor)).ToList();
                                                    if (list3 != null && list3.Count > 0)
                                                    {
                                                        foreach (AUTO_CALENDAR_DOCTOR autoCalendarDoctor in list3)
                                                        {
                                                            dateTime = dateCheck.Value;
                                                            DateTime? nullable3 = new DateTime?(dateTime.AddDays(num2));
                                                            autoCalendarDoctor.DATE_CONTINUE = nullable3;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        while (num6 < num4);
                    }
                }
            }
            return null;
        }

        [HttpPost]
        [ActionDescription(ActionCode = "CalendarDutyLeader_Insert", ActionName = "Lập lịch lãnh đạo", GroupCode = "CalendarDutyLeader", GroupName = "Lịch lãnh đạo", IsMenu = false)]
        [CustomAuthorize]
        [ValidateInput(false)]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AutoSchedulingLeader(int iMonth, int iYear, string contentDuty)
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            int int32 = Convert.ToInt32(byUserName.LM_DEPARTMENT_ID);
            List<CONFIG_PARAMETES> parameterLeader = unitOfWork.ConfigParameter.GetParameterLeader(iYear, 1);
            int? nullable1 = 1;
            int num1 = 0;
            if (parameterLeader != null && parameterLeader.Count > 0)
            {
                nullable1 = parameterLeader[0].NUMBER_DOCTOR_IN_DAY;
                num1 = parameterLeader[0].TIME_DISTANCE;
            }
            int hashCode = DutyType.Leader.GetHashCode();
            if (unitOfWork.CalendarDuty.CheckCalendarHospital(iMonth, iYear, hashCode) == null)
            {
                CALENDAR_DUTY entity1 = new CALENDAR_DUTY();
                entity1.CALENDAR_NAME = contentDuty.Trim();
                entity1.CALENDAR_MONTH = iMonth;
                entity1.CALENDAR_YEAR = iYear;
                entity1.CALENDAR_STATUS = 1;
                entity1.LM_DEPARTMENT_ID = int32;
                entity1.DUTY_TYPE = hashCode;
                entity1.ISDELETE = false;
                entity1.USER_CREATE_ID = byUserName.ADMIN_USER_ID;
                unitOfWork.CalendarDuty.Add(entity1);
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                sqlParameterList.Add(new SqlParameter("@MONTH", iMonth));
                sqlParameterList.Add(new SqlParameter("@YEAR", iYear));
                BACHMAICRContext bachmaicrContext = new BACHMAICRContext();
                List<AUTO_CALENDAR_LEADER> list1 = bachmaicrContext.Database.SqlQuery<AUTO_CALENDAR_LEADER>("exec sp_insertAutoCalendarLeader @MONTH, @YEAR", (object[])sqlParameterList.ToArray()).ToList();
                bachmaicrContext.Dispose();
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5 = DateTime.DaysInMonth(iYear, iMonth);
                DateTime? date = Utils.Utils.ConvetDateVNToDate("1/" + iMonth + "/" + iYear);
                DateTime? dateCheck = date;
                int num6 = 0;
                label_4:
                if (list1 != null && list1.Count > 0)
                {
                    for (int index1 = 0; index1 < list1.Count; ++index1)
                    {
                        ++num6;
                        int idDoctor = list1[index1].DOCTORS_ID;
                        string lmDepartmentIds = list1[index1].LM_DEPARTMENT_IDs;
                        int countHoliday = list1[index1].COUNT_HOLIDAY;
                        int countNumberDirect = list1[index1].COUNT_NUMBER_DIRECT;
                        DateTime? dateContinue = list1[index1].DATE_CONTINUE;
                        // ISSUE: explicit reference operation
                        // ISSUE: variable of a reference type
                        DateTime? local1 = dateCheck;
                        DateTime dateTime1 = date.Value;
                        DateTime dateTime2 = dateTime1.AddDays(num2);
                        local1 = dateTime2;
                        int autoCalendar = list1[index1].AUTO_CALENDAR;
                        int num7;
                        if (dateCheck.HasValue)
                        {
                            dateTime1 = dateCheck.Value;
                            if (dateTime1.Day <= num5)
                            {
                                num7 = autoCalendar != 0 ? 1 : 0;
                                goto label_10;
                            }
                        }
                        num7 = 1;
                        label_10:
                        if (num7 == 0)
                        {
                            if (dateContinue.HasValue)
                            {
                                dateTime1 = dateCheck.Value;
                                int day1 = dateTime1.Day;
                                dateTime1 = dateContinue.Value;
                                int day2 = dateTime1.Day;
                                if (day1 >= day2)
                                {
                                    dateTime1 = dateCheck.Value;
                                    int month1 = dateTime1.Month;
                                    dateTime1 = dateContinue.Value;
                                    int month2 = dateTime1.Month;
                                    if (month1 == month2)
                                    {
                                        dateTime1 = dateCheck.Value;
                                        int year1 = dateTime1.Year;
                                        dateTime1 = dateContinue.Value;
                                        int year2 = dateTime1.Year;
                                        if (year1 == year2)
                                            goto label_21;
                                    }
                                }
                            }
                            if (dateContinue.HasValue)
                            {
                                dateTime1 = dateCheck.Value;
                                int month1 = dateTime1.Month;
                                dateTime1 = dateContinue.Value;
                                int month2 = dateTime1.Month;
                                if (month1 > month2)
                                {
                                    dateTime1 = dateCheck.Value;
                                    int year1 = dateTime1.Year;
                                    dateTime1 = dateContinue.Value;
                                    int year2 = dateTime1.Year;
                                    if (year1 == year2)
                                        goto label_21;
                                }
                            }
                            if (dateContinue.HasValue)
                            {
                                dateTime1 = dateCheck.Value;
                                int year1 = dateTime1.Year;
                                dateTime1 = dateContinue.Value;
                                int year2 = dateTime1.Year;
                                if (year1 > year2)
                                    goto label_21;
                            }
                            int num8 = dateContinue.HasValue ? 1 : 0;
                            goto label_22;
                            label_21:
                            num8 = 0;
                            label_22:
                            if (num8 == 0 && !unitOfWork.CalendarAuto.CheckCalendarDoctorAuto(idDoctor, dateCheck))
                            {
                                bool flag = false;
                                if (countHoliday > 0 && unitOfWork.ConfigHolidays.CheckCalendarHoliday(idDoctor, dateCheck))
                                    flag = true;
                                if (!flag && !unitOfWork.CalendarAuto.CheckCalendarDoctor(idDoctor, dateCheck))
                                {
                                    CALENDAR_DATA entity2 = new CALENDAR_DATA();
                                    entity2.CALENDAR_DUTY_ID = entity1.CALENDAR_DUTY_ID;
                                    entity2.ISDELETE = false;
                                    dateTime1 = dateCheck.Value;
                                    DayOfWeek dayOfWeek = dateTime1.DayOfWeek;
                                    if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                                    {
                                        if (num4 == 0)
                                        {
                                            // ISSUE: explicit reference operation
                                            // ISSUE: variable of a reference type
                                            DateTime? local2 = dateCheck;
                                            object[] objArray1 = new object[4];
                                            object[] objArray2 = objArray1;
                                            int index2 = 0;
                                            dateTime1 = dateCheck.Value;
                                            string str = dateTime1.ToString("dd/MM/yyyy");
                                            objArray2[index2] = str;
                                            objArray1[1] = " ";
                                            objArray1[2] = DayType.Day.GetHashCode();
                                            objArray1[3] = ":00";
                                            DateTime dateTime3 = DateTime.Parse(string.Concat(objArray1));
                                            local2 = dateTime3;
                                        }
                                        else if (num4 == 1)
                                        {
                                            // ISSUE: explicit reference operation
                                            // ISSUE: variable of a reference type
                                            DateTime? local2 = dateCheck;
                                            object[] objArray1 = new object[4];
                                            object[] objArray2 = objArray1;
                                            int index2 = 0;
                                            dateTime1 = dateCheck.Value;
                                            string str = dateTime1.ToString("dd/MM/yyyy");
                                            objArray2[index2] = str;
                                            objArray1[1] = " ";
                                            objArray1[2] = DayType.Night.GetHashCode();
                                            objArray1[3] = ":00";
                                            DateTime dateTime3 = DateTime.Parse(string.Concat(objArray1));
                                            local2 = dateTime3;
                                        }
                                        ++num4;
                                    }
                                    entity2.DATE_START = dateCheck;
                                    entity2.DATE_END = dateCheck;
                                    unitOfWork.CalendarDatas.Add(entity2);
                                    unitOfWork.CalendarDoctors.Add(new CALENDAR_DOCTOR()
                                    {
                                        CALENDAR_DATA_ID = entity2.CALENDAR_DATA_ID,
                                        DOCTORS_ID = idDoctor,
                                        CALENDAR_DUTY_ID = entity1.CALENDAR_DUTY_ID
                                    });
                                    unitOfWork.CalendarAuto.Add(new CALENDAR_AUTO()
                                    {
                                        LM_DEPARTMENT_ID = int32,
                                        DOCTORS_ID = idDoctor,
                                        DATE_CREATE = dateCheck
                                    });
                                    ++num3;
                                    int num9 = num3;
                                    int? nullable2 = nullable1;
                                    if ((num9 != nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                                    {
                                        if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                                        {
                                            if (num4 == 2)
                                            {
                                                ++num2;
                                                num4 = 0;
                                            }
                                        }
                                        else
                                            ++num2;
                                        num3 = 0;
                                    }
                                    List<AUTO_CALENDAR_LEADER> list2 = list1.Where((o => o.DOCTORS_ID == idDoctor)).ToList();
                                    if (list2 != null && list2.Count > 0)
                                    {
                                        foreach (AUTO_CALENDAR_LEADER autoCalendarLeader in list2)
                                        {
                                            dateTime1 = dateCheck.Value;
                                            DateTime? nullable3 = new DateTime?(dateTime1.AddDays(num1));
                                            autoCalendarLeader.DATE_CONTINUE = nullable3;
                                        }
                                    }
                                    string[] arrDeparmentId = lmDepartmentIds.Trim().Split(',');
                                    for (int dept = 0; dept < (arrDeparmentId).Count<string>(); ++dept)
                                    {
                                        if (!string.IsNullOrEmpty(arrDeparmentId[dept].Trim()))
                                        {
                                            List<AUTO_CALENDAR_LEADER> list3 = list1.Where((o => o.LM_DEPARTMENT_IDs.Equals("," + arrDeparmentId[dept].Trim() + ","))).ToList();
                                            if (list3 != null && list3.Count > 0)
                                            {
                                                foreach (AUTO_CALENDAR_LEADER autoCalendarLeader in list3)
                                                    autoCalendarLeader.AUTO_CALENDAR = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (num6 < num5)
                {
                    using (List<AUTO_CALENDAR_LEADER>.Enumerator enumerator = list1.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                            enumerator.Current.AUTO_CALENDAR = 0;
                        goto label_4;
                    }
                }
            }
            return null;
        }
    }
}