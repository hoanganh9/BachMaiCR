using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class PersonalCalendarController : Controller
    {
        public string sViewPath = "~/Views/Calendar/";

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "PersonalCalendar_View", ActionName = "Xem lịch cá nhân", GroupCode = "PersonalCalendar", GroupName = "Lịch cá nhân", IsMenu = false)]
        public ActionResult Index()
        {
            return View(sViewPath + "PersonalCalendar.cshtml");
        }

        [CustomAuthorize]
        public PartialViewResult LoadCalendarPersonal()
        {
            return PartialView(sViewPath + "PersonalCalendar.cshtml");
        }

        [ActionDescription(ActionCode = "PersonalCalendar_View", ActionName = "Xem lịch cá nhân", GroupCode = "PersonalCalendar", GroupName = "Lịch cá nhân", IsMenu = false)]
        [CustomAuthorize]
        public JsonResult GetEvents(string imonth, string iyear)
        {
            string name = User.Identity.Name;
            DateTime dateTime1 = Utils.Utils.ToDateTime(("15/" + imonth + "/" + iyear), "dd/mm/yyyy");
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("@USERNAME", name));
            sqlParameterList.Add(new SqlParameter("@DATE_START", dateTime1));
            BACHMAICRContext context = new BACHMAICRContext();
            List<CALENDAR> list = context.Database.SqlQuery<CALENDAR>("exec sp_getEvents @USERNAME, @DATE_START ", sqlParameterList.ToArray()).ToList();
            List<SqlParameter> paramlist = null;
            int startHour = DayShifts.StartHour.GetHashCode();
            int startMin = DayShifts.StartMinute.GetHashCode();
            int endHour = DayShifts.EndHour.GetHashCode();
            int endMin = DayShifts.EndMinute.GetHashCode();
            IList<CalendarDTO> calendarDtoList = new List<CalendarDTO>();
            if (list.Count > 0)
            {
                for (int index = 0; index < list.Count; ++index)
                {
                    CalendarDTO calendarDto = new CalendarDTO();
                    DateTime dateStart = list[index].DATE_START;
                    DateTime? dateEnd = list[index].DATE_END;
                    int dutyType = list[index].DUTY_TYPE;
                    int hour1 = dateStart.Hour;
                    dateStart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, endHour, endMin, 0);
                    DateTime date = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, startHour, startMin, 0);
                    if (dutyType == DutyType.Directors.GetHashCode())
                    {
                        if (dateEnd.HasValue)
                        {
                            date = new DateTime(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day, endHour, endMin, 0);
                        }
                        else
                            date = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, endHour, endMin, 0);
                    }
                    if (dutyType == DutyType.Leader.GetHashCode())
                    {
                        if (hour1 == DayType.Day.GetHashCode())
                        {
                            dateStart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, startHour, startMin, 0);
                            date = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, endHour, endMin, 0);
                        }
                        else
                        {
                            dateStart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, endHour, endMin, 0);
                            date = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, startHour, startMin, 0);
                        }
                    }
                    calendarDto.id = index;
                    string doctorName = getDoctorName(list[index].CALENDAR_DUTY_ID, name, dateStart, hour1, paramlist, context);
                    string str1 = !string.IsNullOrEmpty(doctorName) ? doctorName : "N/A";
                    string title = "- " + Convert.ToString(list[index].CONTENT) + "\n - Địa điểm: " + Convert.ToString(list[index].DEPARTMENT_NAME) + "\n - Trực cùng ca: " + str1;
                    calendarDto.title = title;
                    calendarDto.start = ToUnixTimespan(dateStart);
                    calendarDto.end = ToUnixTimespan(date);
                    calendarDto.url = string.Empty;
                    calendarDto.allDay = false;
                    calendarDtoList.Add(calendarDto);
                }
            }
            return Json(calendarDtoList, JsonRequestBehavior.AllowGet);
        }

        private string getDoctorName(int calendarDutyId, string userName, DateTime dateStart, int hour, List<SqlParameter> paramlist, BACHMAICRContext context)
        {
            paramlist = new List<SqlParameter>();
            paramlist.Add(new SqlParameter("@CALENDAR_DUTY_ID", calendarDutyId));
            paramlist.Add(new SqlParameter("@USERNAME", userName));
            paramlist.Add(new SqlParameter("@DATE_START", dateStart));
            paramlist.Add(new SqlParameter("@HOUR", hour));
            List<DOCTOR_NAME_LIST> list = context.Database.SqlQuery<DOCTOR_NAME_LIST>("exec sp_getDoctorName @CALENDAR_DUTY_ID, @USERNAME, @DATE_START, @HOUR", (object[])paramlist.ToArray()).ToList();
            string str = string.Empty;
            if (list.Count > 0)
            {
                str = list[0].DOCTOR_NAME;
            }
            paramlist = null;
            return str;
        }

        private long ToUnixTimespan(DateTime date)
        {
            return (long)Math.Truncate(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
        }
    }
}