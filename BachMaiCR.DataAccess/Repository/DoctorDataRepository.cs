





using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class DoctorDataRepository : EFRepository<DoctorData>, IDoctorDataRepository, IRepository<DoctorData>
  {
    public DoctorDataRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public DoctorData CheckDoctorData(int idCalendarDuty, int idDoctor, int idTemplate_column, DateTime DateStart)
    {
      return this.DbSet.FirstOrDefault((o => o.CALENDAR_DUTY_ID == idCalendarDuty && o.DOCTORS_ID == idDoctor && (idTemplate_column == 0 || o.TEMPLATE_COLUM_ID == (int?) idTemplate_column) && (o.DATE_START.Value.Year == DateStart.Year && o.DATE_START.Value.Month == DateStart.Month && o.DATE_START.Value.Day == DateStart.Day)));
    }
  }
}
