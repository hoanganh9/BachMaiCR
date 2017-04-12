using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class CalendarChangeRepository : EFRepository<CALENDAR_CHANGE>, ICalendarChangeRepository, IRepository<CALENDAR_CHANGE>
  {
    public CalendarChangeRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public int CheckChangeCaledar(CALENDAR_CHANGE objCalendar, int status = 0)
    {
      List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendar.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == objCalendar.TEMPLATE_COLUMN_ID && obj.DATE_START == objCalendar.DATE_START && obj.DOCTORS_ID == objCalendar.DOCTORS_ID && obj.DOCTORS_CHANGE_ID == objCalendar.DOCTORS_CHANGE_ID && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
      return list.Count<CALENDAR_CHANGE>() <= 0 ? 0 : list[0].CALENDAR_CHANGE_ID;
    }

    public int CheckChangeCaledarDefault(CALENDAR_CHANGE objCalendar, int status = 0)
    {
      List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendar.CALENDAR_DUTY_ID && obj.DATE_START == objCalendar.DATE_START && obj.DOCTORS_ID == objCalendar.DOCTORS_ID && obj.DOCTORS_CHANGE_ID == objCalendar.DOCTORS_CHANGE_ID && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
      return list.Count<CALENDAR_CHANGE>() <= 0 ? 0 : list[0].CALENDAR_CHANGE_ID;
    }

    public List<CALENDAR_CHANGE> GetListByIdCalendar(int idCalendarDuty, int statusApproved)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty && obj.STATUS_APPROVED == (int?) statusApproved)).ToList<CALENDAR_CHANGE>();
    }

    public List<CALENDAR_CHANGE> GetListByDate(int month, int year)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.DATE_START.Value.Month == month && obj.DATE_START.Value.Year == year && obj.STATUS_APPROVED == (int?) 2)).ToList<CALENDAR_CHANGE>();
    }

    public List<CALENDAR_CHANGE> GetListChangeCalendar(int idCalendarDuty)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
    }

    public List<CALENDAR_CHANGE> ListNew(CALENDAR_CHANGE objCalendarChange, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == objCalendarChange.TEMPLATE_COLUMN_ID && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
    }

    public List<CALENDAR_CHANGE> ListNewChange(CALENDAR_CHANGE objCalendarChange, int status, int type)
    {
      IQueryable<CALENDAR_CHANGE> source = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2)));
      if (type == 1)
        source = source.Where((o => o.DOCTORS_ID == objCalendarChange.DOCTORS_ID && o.DATE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_START.Value.Year == objCalendarChange.DATE_START.Value.Year));
      else if (type == 2)
        source = source.Where((o => o.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_ID && o.DATE_CHANGE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_CHANGE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_CHANGE_START.Value.Year == objCalendarChange.DATE_START.Value.Year));
      else if (type == 0)
        source = source.Where((o => o.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_ID && o.DATE_CHANGE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_CHANGE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_CHANGE_START.Value.Year == objCalendarChange.DATE_START.Value.Year || o.DOCTORS_ID == objCalendarChange.DOCTORS_ID && o.DATE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_START.Value.Year == objCalendarChange.DATE_START.Value.Year));
      return source.ToList<CALENDAR_CHANGE>();
    }

    public void DeleteCalendarChangeByStatus(int idCalendarDuty, int status)
    {
      DbSet<CALENDAR_CHANGE> calendarChange = this.DbContext.CALENDAR_CHANGE;
      Expression<Func<CALENDAR_CHANGE, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty && obj.STATUS == (int?) status && obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2);
      foreach (CALENDAR_CHANGE entity in ((IQueryable<CALENDAR_CHANGE>) calendarChange).Where(predicate).ToList<CALENDAR_CHANGE>())
        this.Delete(entity);
    }

    public CALENDAR_CHANGE GetInforHospital(CALENDAR_CHANGE objCalendarChange, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == new int?() && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_CHANGE_ID && obj.DATE_CHANGE_START == objCalendarChange.DATE_CHANGE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).FirstOrDefault<CALENDAR_CHANGE>();
    }

    public CALENDAR_CHANGE GetChangeByIdDuty(int IdcalendaDuty)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DELETE == (int?) IdcalendaDuty)).FirstOrDefault<CALENDAR_CHANGE>();
    }

    public CALENDAR_CHANGE GetInfor(CALENDAR_CHANGE objCalendarChange, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && (objCalendarChange.TEMPLATE_COLUMN_ID == (int?) 0 || obj.TEMPLATE_COLUMN_ID == new int?() || obj.TEMPLATE_COLUMN_ID == objCalendarChange.TEMPLATE_COLUMN_ID) && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).FirstOrDefault<CALENDAR_CHANGE>();
    }

    public CALENDAR_CHANGE GetInforCh(CALENDAR_CHANGE objCalendarChange, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.CALENDAR_DELETE == objCalendarChange.CALENDAR_DELETE && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).FirstOrDefault<CALENDAR_CHANGE>();
    }

    public CALENDAR_CHANGE GetInforDoctorChange(CALENDAR_CHANGE objCalendarChange, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_CHANGE_ID && obj.DATE_CHANGE_START == objCalendarChange.DATE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).FirstOrDefault<CALENDAR_CHANGE>();
    }

    public CALENDAR_CHANGE GetInforDefault(CALENDAR_CHANGE objCalendarChange, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).First<CALENDAR_CHANGE>();
    }

    public CALENDAR_CHANGE isExistCalendarDuty(CALENDAR_CHANGE objCalendarChange, int status)
    {
      List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == objCalendarChange.TEMPLATE_COLUMN_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DOCTORS_CHANGE_ID != objCalendarChange.DOCTORS_CHANGE_ID && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
      return list.Count<CALENDAR_CHANGE>() <= 0 ? (CALENDAR_CHANGE) null : list[0];
    }

    public CALENDAR_CHANGE isExistCalendarDutyDefault(CALENDAR_CHANGE objCalendarChange, int status)
    {
      List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DOCTORS_CHANGE_ID != objCalendarChange.DOCTORS_CHANGE_ID && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
      return list.Count<CALENDAR_CHANGE>() <= 0 ? (CALENDAR_CHANGE) null : list[0];
    }

    public int DeleteCalendarByID(int idCalendarDuty = 0)
    {
      List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DELETE == (int?) idCalendarDuty && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
      int num;
      if (list.Count<CALENDAR_CHANGE>() > 0)
      {
        num = list[0].CALENDAR_CHANGE_ID;
        this.Delete(list[0]);
      }
      else
        num = 0;
      return num;
    }

    public int DeleteCalendarByIDAndDay(int idCalendarDuty = 0, int month = 0)
    {
      int num = 0;
      List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty && obj.DATE_START.Value.Month == month && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).ToList<CALENDAR_CHANGE>();
      if (list.Count<CALENDAR_CHANGE>() > 0)
      {
        foreach (CALENDAR_CHANGE entity in list)
        {
          num = list.Count<CALENDAR_CHANGE>();
          this.Delete(entity);
        }
      }
      else
        num = 0;
      return num;
    }

    public void UpdateStatus(int idCalendarDuty, int status)
    {
      DbQuery<CALENDAR_CHANGE> dbQuery = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_CHANGE, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty && obj.STATUS_APPROVED == (int?) 1);
      foreach (CALENDAR_CHANGE calendarChange in (IEnumerable<CALENDAR_CHANGE>) dbQuery.Where(predicate))
      {
        calendarChange.STATUS_APPROVED = new int?(status);
        ((DbEntityEntry<CALENDAR_CHANGE>) this.DbContext.Entry<CALENDAR_CHANGE>(calendarChange)).State = EntityState.Modified;
      }
      this.Save();
    }

    public void DeleteCalendarChangeById(int idCalendarDuty)
    {
      DbQuery<CALENDAR_CHANGE> dbQuery = this.DbSet.AsNoTracking();
      Expression<Func<CALENDAR_CHANGE, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty);
      foreach (CALENDAR_CHANGE entity in dbQuery.Where(predicate).ToList<CALENDAR_CHANGE>())
        this.Delete(entity);
    }

    public bool ExistChangeCaledar(CALENDAR_CHANGE objCalendar, int status = 0)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendar.CALENDAR_DUTY_ID && obj.DATE_START == objCalendar.DATE_START && obj.DOCTORS_ID == objCalendar.DOCTORS_ID && obj.DOCTORS_CHANGE_ID == objCalendar.DOCTORS_CHANGE_ID && obj.DATE_CHANGE_START == objCalendar.DATE_CHANGE_START && obj.STATUS == (int?) status && (obj.STATUS_APPROVED == (int?) 1 || obj.STATUS_APPROVED == (int?) 2))).FirstOrDefault<CALENDAR_CHANGE>() != null;
    }

    public void AddRange(List<CALENDAR_CHANGE> lstCalendarChange)
    {
      foreach (var m0 in lstCalendarChange)
        ((DbEntityEntry<CALENDAR_CHANGE>) this.DbContext.Entry<CALENDAR_CHANGE>(m0)).State = EntityState.Added;
      this.Save();
    }

    public List<CALENDAR_CHANGE> ListCalendarChange(int idCalendarDuty)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == (int?) idCalendarDuty)).OrderBy<CALENDAR_CHANGE, DateTime?>((Expression<Func<CALENDAR_CHANGE, DateTime?>>) (obj => obj.DATE_START)).ThenBy<CALENDAR_CHANGE, int?>((Expression<Func<CALENDAR_CHANGE, int?>>) (obj => obj.STATUS)).ToList<CALENDAR_CHANGE>();
    }
  }
}
