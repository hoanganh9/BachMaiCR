using BachMaiCR.DBMapping.Models;
using System;

namespace BachMaiCR.DataAccess.Repository
{
    public interface ICalendarDataRepository : IRepository<CALENDAR_DATA>
    {
        void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn, DateTime startDate, DateTime endDate);

        void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn);

        void DeleteCalendarDataById(int idCalendarDuty);
    }
}