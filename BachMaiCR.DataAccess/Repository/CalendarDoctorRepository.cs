using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class CalendarDoctorRepository : EFRepository<CALENDAR_DOCTOR>, ICalendarDoctorRepository, IRepository<CALENDAR_DOCTOR>
  {
    public CalendarDoctorRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn, DateTime startDate, DateTime endDate)
    {
      DbQuery<CALENDAR_DOCTOR> source = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DOCTOR, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.COLUMN_LEVEL_ID == idColumn && obj.CALENDAR_DATA.DATE_START >= (DateTime?) startDate && obj.CALENDAR_DATA.DATE_START <= (DateTime?) endDate);
      foreach (CALENDAR_DOCTOR entity in source.Where(predicate).ToList())
        this.Delete(entity);
    }

    public void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn)
    {
      DbQuery<CALENDAR_DOCTOR> source = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DOCTOR, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.COLUMN_LEVEL_ID == idColumn);
      foreach (CALENDAR_DOCTOR entity in source.Where(predicate).ToList())
        this.Delete(entity);
    }

    public void DeleteCalendarDoctorById(int idCalendarDuty)
    {
      DbQuery<CALENDAR_DOCTOR> source = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DOCTOR, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty);
      foreach (CALENDAR_DOCTOR entity in source.Where(predicate).ToList())
        this.Delete(entity);
    }

    public void DeleteByDoctorId(int idCalendarDuty, int idDoctor, DateTime date)
    {
      DbQuery<CALENDAR_DOCTOR> source = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DOCTOR, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.DOCTORS_ID == idDoctor && SqlFunctions.DateDiff("dd", (DateTime?) date, obj.CALENDAR_DATA.DATE_START) == 0);
      foreach (CALENDAR_DOCTOR entity in source.Where(predicate).ToList())
        this.Delete(entity);
    }

    public bool ExistReferenceUser(int usrID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.DOCTORS_ID.Equals(usrID))).Count() > 0;
    }
  }
}
