// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.PersonalCalendarController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

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
      return this.View(this.sViewPath + "PersonalCalendar.cshtml");
    }

    [CustomAuthorize]
    public PartialViewResult LoadCalendarPersonal()
    {
      return this.PartialView(this.sViewPath + "PersonalCalendar.cshtml");
    }

    [ActionDescription(ActionCode = "PersonalCalendar_View", ActionName = "Xem lịch cá nhân", GroupCode = "PersonalCalendar", GroupName = "Lịch cá nhân", IsMenu = false)]
    [CustomAuthorize]
    public JsonResult GetEvents(string imonth, string iyear)
    {
      string name = this.User.Identity.Name;
      DateTime dateTime1 = BachMaiCR.Web.Utils.Utils.ToDateTime(("15/" + imonth + "/" + iyear), "dd/mm/yyyy");
      List<SqlParameter> sqlParameterList = new List<SqlParameter>();
      sqlParameterList.Add(new SqlParameter("@USERNAME", name));
      sqlParameterList.Add(new SqlParameter("@DATE_START", dateTime1));
      BACHMAICRContext context = new BACHMAICRContext();
      List<CALENDAR> list = ((IEnumerable<CALENDAR>) context.Database.SqlQuery<CALENDAR>("exec sp_getEvents @USERNAME, @DATE_START ", (object[]) sqlParameterList.ToArray())).ToList();
      List<SqlParameter> paramlist = (List<SqlParameter>) null;
      int hashCode1 = DayShifts.StartHour.GetHashCode();
      int hashCode2 = DayShifts.StartMinute.GetHashCode();
      int hashCode3 = DayShifts.EndHour.GetHashCode();
      int hashCode4 = DayShifts.StartMinute.GetHashCode();
      IList<CalendarDTO> calendarDtoList = (IList<CalendarDTO>) new List<CalendarDTO>();
      if (list != null && list.Count > 0)
      {
        for (int index = 0; index < list.Count; ++index)
        {
          CalendarDTO calendarDto = new CalendarDTO();
          DateTime dateTime2 = list[index].DATE_START;
          DateTime? dateEnd = list[index].DATE_END;
          int dutyType = list[index].DUTY_TYPE;
          int hour1 = dateTime2.Hour;
          dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode3, hashCode4, 0);
          DateTime date = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode1, hashCode2, 0);
          if (dutyType == DutyType.Directors.GetHashCode())
          {
            if (dateEnd.HasValue)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              DateTime dateTime3 = dateEnd.Value;
              int year = dateTime3.Year;
              dateTime3 = dateEnd.Value;
              int month = dateTime3.Month;
              dateTime3 = dateEnd.Value;
              int day = dateTime3.Day;
              int hour2 = hashCode3;
              int minute = hashCode4;
              int second = 0;
              // ISSUE: explicit reference operation
              date = new DateTime(year, month, day, hour2, minute, second);
            }
            else
              date = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode3, hashCode4, 0);
          }
          if (dutyType == DutyType.Leader.GetHashCode())
          {
            if (hour1 == DayType.Day.GetHashCode())
            {
              dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode1, hashCode2, 0);
              date = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode3, hashCode4, 0);
            }
            else
            {
              dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode3, hashCode4, 0);
              date = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, hashCode1, hashCode2, 0);
            }
          }
          calendarDto.id = index;
          string doctorName = this.getDoctorName(list[index].CALENDAR_DUTY_ID, name, dateTime2, hour1, paramlist, context);
          string str1 = !string.IsNullOrEmpty(doctorName) ? doctorName : "N/A";
          string str2 = "- " + Convert.ToString(list[index].CONTENT) + "\n - Địa điểm: " + Convert.ToString(list[index].DEPARTMENT_NAME) + "\n - Trực cùng ca: " + str1;
          calendarDto.title = str2;
          calendarDto.start = this.ToUnixTimespan(dateTime2);
          calendarDto.end = this.ToUnixTimespan(date);
          calendarDto.url = "";
          calendarDto.allDay = false;
          calendarDtoList.Add(calendarDto);
        }
      }
      return this.Json(calendarDtoList, JsonRequestBehavior.AllowGet);
    }

    private string getDoctorName(int calendarDutyId, string userName, DateTime dateStart, int hour, List<SqlParameter> paramlist, BACHMAICRContext context)
    {
      paramlist = new List<SqlParameter>();
      paramlist.Add(new SqlParameter("@CALENDAR_DUTY_ID", calendarDutyId));
      paramlist.Add(new SqlParameter("@USERNAME", userName));
      paramlist.Add(new SqlParameter("@DATE_START", dateStart));
      paramlist.Add(new SqlParameter("@HOUR", hour));
      List<DOCTOR_NAME_LIST> list = ((IEnumerable<DOCTOR_NAME_LIST>) context.Database.SqlQuery<DOCTOR_NAME_LIST>("exec sp_getDoctorName @CALENDAR_DUTY_ID, @USERNAME, @DATE_START, @HOUR", (object[]) paramlist.ToArray())).ToList();
      string str = "";
      if (list != null && list.Count > 0)
        str = list[0].DOCTOR_NAME;
      paramlist = (List<SqlParameter>) null;
      return str;
    }

    private long ToUnixTimespan(DateTime date)
    {
      return (long) Math.Truncate(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
    }
  }
}
