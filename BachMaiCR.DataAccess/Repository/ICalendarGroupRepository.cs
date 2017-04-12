





using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ICalendarGroupRepository : IRepository<CALENDAR_GROUP>
  {
    string DerparmentCalendarGroup(int monthx, int yearx);

    CALENDAR_GROUP CheckIsExist(int idCalendar, int idCalendarParent, int month, int year);

    List<CALENDAR_GROUP> ListCalendarGroup(int monthx, int yearx);

    List<CALENDAR_GROUP> GetAllByDate(CALENDAR_GROUP objCalendar);

    List<CALENDAR_GROUP> GetAllByDateIsApproved(int monthx, int yearx);

    bool CheckIsCalendarApproved(int idCalendar);
  }
}
