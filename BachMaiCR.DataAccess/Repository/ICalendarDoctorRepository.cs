





using System;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ICalendarDoctorRepository : IRepository<CALENDAR_DOCTOR>
  {
    void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn, DateTime startDate, DateTime endDate);

    void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn);

    void DeleteCalendarDoctorById(int idCalendarDuty);

    bool ExistReferenceUser(int usrID);

    void DeleteByDoctorId(int idCalendarDuty, int idDoctor, DateTime date);
  }
}
