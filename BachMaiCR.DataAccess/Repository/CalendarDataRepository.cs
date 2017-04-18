





using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class CalendarDataRepository : EFRepository<CALENDAR_DATA>, ICalendarDataRepository, IRepository<CALENDAR_DATA>
  {
    public CalendarDataRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn, DateTime startDate, DateTime endDate)
    {
      DbQuery<CALENDAR_DATA> dbQuery = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DATA, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.TEMPLATE_COLUM_ID == (int?) idColumn && obj.DATE_START >= (DateTime?) startDate && obj.DATE_START <= (DateTime?) endDate);
      foreach (CALENDAR_DATA entity in dbQuery.Where(predicate).ToList())
        this.Delete(entity);
    }

    public void DeleteByIDCalendarDuty(int idCalendarDuty, int idColumn)
    {
      DbQuery<CALENDAR_DATA> dbQuery = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DATA, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.TEMPLATE_COLUM_ID == (int?) idColumn);
      foreach (CALENDAR_DATA entity in dbQuery.Where(predicate).ToList())
        this.Delete(entity);
    }

    public void DeleteCalendarDataById(int idCalendarDuty)
    {
      DbQuery<CALENDAR_DATA> dbQuery = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_DATA, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty);
      foreach (CALENDAR_DATA entity in dbQuery.Where(predicate).ToList())
        this.Delete(entity);
    }
  }
}
