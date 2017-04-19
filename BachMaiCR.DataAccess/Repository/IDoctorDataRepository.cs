using System;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IDoctorDataRepository : IRepository<DoctorData>
  {
    DoctorData CheckDoctorData(int idCalendarDuty, int idDoctor, int idTemplate_column, DateTime DateStart);
  }
}
