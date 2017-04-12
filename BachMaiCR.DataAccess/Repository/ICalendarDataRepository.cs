





using System;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ICalendarDataRepository : IRepository<CALENDAR_DATA>
  {
    void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn, DateTime startDate, DateTime endDate);

    void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn);

    void DeleteCalendarDataById(int idCalendarDuty);
  }
}
