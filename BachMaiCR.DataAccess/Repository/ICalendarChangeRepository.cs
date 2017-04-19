using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ICalendarChangeRepository : IRepository<CALENDAR_CHANGE>
  {
    void DeleteCalendarChangeById(int idCalendarDuty);

    int CheckChangeCaledar(CALENDAR_CHANGE objCalendar, int status = 0);

    int CheckChangeCaledarDefault(CALENDAR_CHANGE objCalendar, int status = 0);

    int DeleteCalendarByID(int idCalendarDuty = 0);

    List<CALENDAR_CHANGE> GetListChangeCalendar(int idCalendarDuty);

    List<CALENDAR_CHANGE> GetListByIdCalendar(int idCalendarDuty, int statusApproved);

    List<CALENDAR_CHANGE> GetListByDate(int month, int year);

    bool ExistChangeCaledar(CALENDAR_CHANGE objCalendar, int status = 0);

    List<CALENDAR_CHANGE> ListNew(CALENDAR_CHANGE objCalendarChange, int status);

    List<CALENDAR_CHANGE> ListNewChange(CALENDAR_CHANGE objCalendarChange, int status, int type);

    CALENDAR_CHANGE GetInfor(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE GetInforCh(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE GetInforHospital(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE GetInforDoctorChange(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE GetInforDefault(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE isExistCalendarDuty(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE isExistCalendarDutyDefault(CALENDAR_CHANGE objCalendarChange, int status);

    CALENDAR_CHANGE GetChangeByIdDuty(int IdcalendaDuty);

    void UpdateStatus(int idCalendarDuty, int status);

    void AddRange(List<CALENDAR_CHANGE> lstCalendarChange);

    int DeleteCalendarByIDAndDay(int idCalendarDuty = 0, int month = 0);

    void DeleteCalendarChangeByStatus(int idCalendarDuty, int status);

    List<CALENDAR_CHANGE> ListCalendarChange(int idCalendarDuty);
  }
}
