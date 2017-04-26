using BachMaiCR.DBMapping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

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
            var list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendar.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == objCalendar.TEMPLATE_COLUMN_ID && obj.DATE_START == objCalendar.DATE_START && obj.DOCTORS_ID == objCalendar.DOCTORS_ID && obj.DOCTORS_CHANGE_ID == objCalendar.DOCTORS_CHANGE_ID && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
            return list.Count() <= 0 ? 0 : list[0].CALENDAR_CHANGE_ID;
        }

        public int CheckChangeCaledarDefault(CALENDAR_CHANGE objCalendar, int status = 0)
        {
            List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendar.CALENDAR_DUTY_ID && obj.DATE_START == objCalendar.DATE_START && obj.DOCTORS_ID == objCalendar.DOCTORS_ID && obj.DOCTORS_CHANGE_ID == objCalendar.DOCTORS_CHANGE_ID && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
            return list.Count() <= 0 ? 0 : list[0].CALENDAR_CHANGE_ID;
        }

        public List<CALENDAR_CHANGE> GetListByIdCalendar(int idCalendarDuty, int statusApproved)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.STATUS_APPROVED == statusApproved).ToList();
        }

        public List<CALENDAR_CHANGE> GetListByDate(int month, int year)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.DATE_START.Value.Month == month && obj.DATE_START.Value.Year == year && obj.STATUS_APPROVED == 2).ToList();
        }

        public List<CALENDAR_CHANGE> GetListChangeCalendar(int idCalendarDuty)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
        }

        public List<CALENDAR_CHANGE> ListNew(CALENDAR_CHANGE objCalendarChange, int status)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == objCalendarChange.TEMPLATE_COLUMN_ID && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
        }

        public List<CALENDAR_CHANGE> ListNewChange(CALENDAR_CHANGE objCalendarChange, int status, int type)
        {
            IQueryable<CALENDAR_CHANGE> source = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2)));
            if (type == 1)
                source = source.Where(o => o.DOCTORS_ID == objCalendarChange.DOCTORS_ID && o.DATE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_START.Value.Year == objCalendarChange.DATE_START.Value.Year);
            else if (type == 2)
                source = source.Where(o => o.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_ID && o.DATE_CHANGE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_CHANGE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_CHANGE_START.Value.Year == objCalendarChange.DATE_START.Value.Year);
            else if (type == 0)
                source = source.Where((o => o.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_ID && o.DATE_CHANGE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_CHANGE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_CHANGE_START.Value.Year == objCalendarChange.DATE_START.Value.Year || o.DOCTORS_ID == objCalendarChange.DOCTORS_ID && o.DATE_START.Value.Day == objCalendarChange.DATE_START.Value.Day && o.DATE_START.Value.Month == objCalendarChange.DATE_START.Value.Month && o.DATE_START.Value.Year == objCalendarChange.DATE_START.Value.Year));
            return source.ToList();
        }

        public void DeleteCalendarChangeByStatus(int idCalendarDuty, int status)
        {
            DbSet<CALENDAR_CHANGE> calendarChange = this.DbContext.CALENDAR_CHANGE;
            Expression<Func<CALENDAR_CHANGE, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.STATUS == status && obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2);
            foreach (CALENDAR_CHANGE entity in calendarChange.Where(predicate).ToList())
                this.Delete(entity);
        }

        public CALENDAR_CHANGE GetInforHospital(CALENDAR_CHANGE objCalendarChange, int status)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == new int?() && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_CHANGE_ID && obj.DATE_CHANGE_START == objCalendarChange.DATE_CHANGE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).FirstOrDefault();
        }

        public CALENDAR_CHANGE GetChangeByIdDuty(int IdcalendaDuty)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.CALENDAR_DELETE == IdcalendaDuty).FirstOrDefault();
        }

        public CALENDAR_CHANGE GetInfor(CALENDAR_CHANGE objCalendarChange, int status)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && (objCalendarChange.TEMPLATE_COLUMN_ID == 0 || obj.TEMPLATE_COLUMN_ID == new int?() || obj.TEMPLATE_COLUMN_ID == objCalendarChange.TEMPLATE_COLUMN_ID) && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).FirstOrDefault();
        }

        public CALENDAR_CHANGE GetInforCh(CALENDAR_CHANGE objCalendarChange, int status)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.CALENDAR_DELETE == objCalendarChange.CALENDAR_DELETE && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).FirstOrDefault();
        }

        public CALENDAR_CHANGE GetInforDoctorChange(CALENDAR_CHANGE objCalendarChange, int status)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.DOCTORS_CHANGE_ID == objCalendarChange.DOCTORS_CHANGE_ID && obj.DATE_CHANGE_START == objCalendarChange.DATE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).FirstOrDefault();
        }

        public CALENDAR_CHANGE GetInforDefault(CALENDAR_CHANGE objCalendarChange, int status)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).First();
        }

        public CALENDAR_CHANGE isExistCalendarDuty(CALENDAR_CHANGE objCalendarChange, int status)
        {
            List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.TEMPLATE_COLUMN_ID == objCalendarChange.TEMPLATE_COLUMN_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DOCTORS_CHANGE_ID != objCalendarChange.DOCTORS_CHANGE_ID && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
            return list.Count() <= 0 ? (CALENDAR_CHANGE)null : list[0];
        }

        public CALENDAR_CHANGE isExistCalendarDutyDefault(CALENDAR_CHANGE objCalendarChange, int status)
        {
            List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendarChange.CALENDAR_DUTY_ID && obj.DATE_START == objCalendarChange.DATE_START && obj.DOCTORS_ID == objCalendarChange.DOCTORS_ID && obj.DOCTORS_CHANGE_ID != objCalendarChange.DOCTORS_CHANGE_ID && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
            return list.Count() <= 0 ? (CALENDAR_CHANGE)null : list[0];
        }

        public int DeleteCalendarByID(int idCalendarDuty = 0)
        {
            List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DELETE == idCalendarDuty && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
            int num;
            if (list.Count() > 0)
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
            List<CALENDAR_CHANGE> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.DATE_START.Value.Month == month && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).ToList();
            if (list.Count() > 0)
            {
                foreach (CALENDAR_CHANGE entity in list)
                {
                    num = list.Count();
                    this.Delete(entity);
                }
            }
            else
                num = 0;
            return num;
        }

        public void UpdateStatus(int idCalendarDuty, int status)
        {
            DbQuery<CALENDAR_CHANGE> source = this.DbSet.AsNoTracking();
            Expression<Func<CALENDAR_CHANGE, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty && obj.STATUS_APPROVED == 1);
            foreach (CALENDAR_CHANGE entity in source.Where(predicate))
            {
                entity.STATUS_APPROVED = status;
                this.DbContext.Entry(entity).State = EntityState.Modified;
            }
            this.Save();
        }

        public void DeleteCalendarChangeById(int idCalendarDuty)
        {
            DbQuery<CALENDAR_CHANGE> source = this.DbSet.AsNoTracking();
            Expression<Func<CALENDAR_CHANGE, bool>> predicate = (obj => obj.CALENDAR_DUTY_ID == idCalendarDuty);
            foreach (CALENDAR_CHANGE entity in source.Where(predicate).ToList())
                this.Delete(entity);
        }

        public bool ExistChangeCaledar(CALENDAR_CHANGE objCalendar, int status = 0)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_DUTY_ID == objCalendar.CALENDAR_DUTY_ID && obj.DATE_START == objCalendar.DATE_START && obj.DOCTORS_ID == objCalendar.DOCTORS_ID && obj.DOCTORS_CHANGE_ID == objCalendar.DOCTORS_CHANGE_ID && obj.DATE_CHANGE_START == objCalendar.DATE_CHANGE_START && obj.STATUS == status && (obj.STATUS_APPROVED == 1 || obj.STATUS_APPROVED == 2))).FirstOrDefault() != null;
        }

        public void AddRange(List<CALENDAR_CHANGE> lstCalendarChange)
        {
            foreach (CALENDAR_CHANGE entity in lstCalendarChange)
                this.DbContext.Entry(entity).State = EntityState.Added;
            this.Save();
        }

        public List<CALENDAR_CHANGE> ListCalendarChange(int idCalendarDuty)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.CALENDAR_DUTY_ID == idCalendarDuty).OrderBy(obj => obj.DATE_START).ThenBy<CALENDAR_CHANGE, int?>(obj => obj.STATUS).ToList();
        }
    }
}