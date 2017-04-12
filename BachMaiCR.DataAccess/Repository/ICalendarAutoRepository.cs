





using System;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ICalendarAutoRepository : IRepository<CALENDAR_AUTO>
  {
    bool CheckCalendarDoctorAuto(int idDoctor, DateTime? dateCheck);

    bool CheckCalendarDoctor(int idDoctor, DateTime? dateCheck);
  }
}
