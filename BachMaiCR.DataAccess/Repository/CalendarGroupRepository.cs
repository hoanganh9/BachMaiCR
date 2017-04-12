





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class CalendarGroupRepository : EFRepository<CALENDAR_GROUP>, ICalendarGroupRepository, IRepository<CALENDAR_GROUP>
  {
    public CalendarGroupRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public string DerparmentCalendarGroup(int monthx, int yearx)
    {
      IQueryable<CALENDAR_GROUP> source = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == (int?) monthx && obj.CALENDAR_YEAR == (int?) yearx));
      string str = "";
      List<CALENDAR_GROUP> list = source.ToList<CALENDAR_GROUP>();
      for (int index = 0; index < list.Count; ++index)
        str = str + "," + (object) list[index].LM_DEPARTMENT_ID;
      return str;
    }

    public List<CALENDAR_GROUP> ListCalendarGroup(int monthx, int yearx)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == (int?) monthx && obj.CALENDAR_YEAR == (int?) yearx)).ToList<CALENDAR_GROUP>();
    }

    public CALENDAR_GROUP CheckIsExist(int idCalendar, int idCalendarParent, int month, int year)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_ID == idCalendar && obj.CALENDAR_PARENT_ID == idCalendarParent && obj.CALENDAR_MONTH == (int?) month && obj.CALENDAR_YEAR == (int?) year)).FirstOrDefault<CALENDAR_GROUP>();
    }

    public List<CALENDAR_GROUP> GetAllByDate(CALENDAR_GROUP objCalendar)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == objCalendar.CALENDAR_MONTH && obj.CALENDAR_YEAR == objCalendar.CALENDAR_YEAR && obj.CALENDAR_STATUS == (int?) 0)).ToList<CALENDAR_GROUP>();
    }

    public List<CALENDAR_GROUP> GetAllByDateIsApproved(int monthx, int yearx)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == (int?) monthx && obj.CALENDAR_YEAR == (int?) yearx && obj.CALENDAR_STATUS == (int?) 1)).ToList<CALENDAR_GROUP>();
    }

    public bool CheckIsCalendarApproved(int idCalendar)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_PARENT_ID == idCalendar && obj.CALENDAR_STATUS == (int?) 1)).Select(obj => new
      {
        CALENDAR_GROUP_ID = obj.CALENDAR_GROUP_ID
      }).ToList().Count() > 0;
    }
  }
}
