





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class CalendarDutyRepository : EFRepository<CALENDAR_DUTY>, ICalendarDutyRepository, IRepository<CALENDAR_DUTY>
  {
    public CalendarDutyRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public List<DoctorHospital> GetDoctorHospital(int iMonth = 0, int iYear = 0)
    {
      return ((IQueryable<DoctorHospital>) this.DbContext.DoctorHospitals).Where((o => o.CALENDAR_MONTH == (int?) iMonth && o.CALENDAR_YEAR == (int?) iYear)).ToList<DoctorHospital>();
    }

    public List<DoctorHospital> GetDoctorHospitalLeader(int iMonth = 0, int iYear = 0)
    {
      return ((IQueryable<DoctorHospital>) this.DbContext.DoctorHospitals).Where((o => o.CALENDAR_MONTH == (int?) iMonth && o.CALENDAR_YEAR == (int?) iYear && o.DUTY_TYPE == (int?) 1)).ToList<DoctorHospital>();
    }

    public List<DoctorHospital> GetDoctorHospitalByDepartment(int iMonth = 0, int iYear = 0, int idDepartment = 0)
    {
      return ((IQueryable<DoctorHospital>) this.DbContext.DoctorHospitals).Where((o => o.CALENDAR_MONTH == (int?) iMonth && o.CALENDAR_YEAR == (int?) iYear && o.LM_DEPARTMENT_ID == idDepartment && (o.LEVEL_NUMBER == (int?) 1 || o.LEVEL_NUMBER == (int?) 2))).ToList<DoctorHospital>();
    }

    public List<CALENDAR_DUTY> GetByApproved(int month = 0, int year = 0, int dutyType = 3, int isApproved = 0)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == (int?) month && obj.CALENDAR_YEAR == (int?) year && obj.DUTY_TYPE == (int?) dutyType && obj.ISAPPROVED == isApproved && obj.ISDELETE == false)).ToList<CALENDAR_DUTY>();
    }

    public CALENDAR_DUTY CheckCalendarHospital(int calendarMonth, int calendarYear, int DutyType)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == (int?) calendarMonth && obj.CALENDAR_YEAR == (int?) calendarYear && obj.DUTY_TYPE == (int?) DutyType && obj.ISDELETE == false)).FirstOrDefault<CALENDAR_DUTY>();
    }

    public int CheckCalendarDuty(int calendarMonth, int calendarYear, int idTemplate, int idDepartment)
    {
      List<CALENDAR_DUTY> list = this.DbSet.AsNoTracking().Where((obj => obj.CALENDAR_MONTH == (int?) calendarMonth && obj.CALENDAR_YEAR == (int?) calendarYear && obj.TEMPLATES_ID == (int?) idTemplate && obj.ISDELETE == false)).ToList<CALENDAR_DUTY>();
      return list.Count<CALENDAR_DUTY>() <= 0 ? 0 : list[0].CALENDAR_DUTY_ID;
    }

    public bool checkCalendar(int calendarMonth = 0, int calendarYear = 0, int dutyType = 1, int idDepartment = 0, int isDefault = 0)
    {
      IQueryable<CALENDAR_DUTY> source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.CALENDAR_MONTH == (int?) calendarMonth && obj.CALENDAR_YEAR == (int?) calendarYear && obj.DUTY_TYPE == (int?) dutyType));
      if (isDefault == 1)
        source = source.Where((o => o.TEMPLATES_ID == new int?()));
      return source.Count<CALENDAR_DUTY>() > 0;
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarLeader(int idCalendar = 0)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.CALENDAR_DUTY_ID == idCalendar)).OrderBy((o => o.DOCTORS_ID)).ToList<DoctorCalendarLeader>();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarByDeparment(int deparmentId = 0)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.LM_DEPARTMENT_ID == deparmentId)).OrderBy((o => o.DOCTORS_ID)).ToList<DoctorCalendarLeader>();
    }

    public List<CALENDAR_DUTY> GetCalendarDirector(int iMonth = 0, int iYear = 0, int duty_type = 1)
    {
      return ((IQueryable<CALENDAR_DUTY>) this.DbContext.CALENDAR_DUTY).Where((o => o.DUTY_TYPE == (int?) duty_type)).ToList<CALENDAR_DUTY>();
    }

    public List<CALENDAR_DUTY> GetCalendarByDeparment(int iMonth = 0, int iYear = 0, int duty_type = 3, int? idDepartment = 0)
    {
      return ((IQueryable<CALENDAR_DUTY>) this.DbContext.CALENDAR_DUTY).Where((o => (int?) o.LM_DEPARTMENT_ID == idDepartment && o.TEMPLATES_ID == new int?())).ToList<CALENDAR_DUTY>();
    }

    public int GetCalendarDutyId(int iMonth = 0, int iYear = 0, int duty_type = 3, int idDepartment = 0, int isDefault = 0)
    {
      var source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.CALENDAR_MONTH == (int?) iMonth && obj.CALENDAR_YEAR == (int?) iYear && obj.DUTY_TYPE == (int?) duty_type && obj.LM_DEPARTMENT_ID == idDepartment)).Select(obj => new
      {
        CALENDAR_DUTY_ID = obj.CALENDAR_DUTY_ID,
        TEMPLATES_ID = obj.TEMPLATES_ID
      });
      if (isDefault == 1)
        source = source.Where(o => o.TEMPLATES_ID == new int?());
      int num = 0;
      var list = source.ToList();
      if (list.Count > 0)
        num = list[0].CALENDAR_DUTY_ID;
      return num;
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarDirector(int iMonth = 0, int iYear = 0, int duty_type = 1)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.DUTY_TYPE == (int?) duty_type)).ToList<DoctorCalendarLeader>();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarPesonal(int iMonth = 0, int iYear = 0, int doctorId = 0)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.DOCTORS_ID == doctorId)).ToList<DoctorCalendarLeader>();
    }

    public List<DoctorCalendarLeader> GetDoctorByCalendarDutyId(int calendarDutyId)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.CALENDAR_DUTY_ID == calendarDutyId)).ToList<DoctorCalendarLeader>();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarDefault(int iMonth = 0, int iYear = 0, int duty_type = 3, int? deparmentId = 0)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.TEMPLATES_ID == new int?())).ToList<DoctorCalendarLeader>();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarHospital(int iMonth = 0, int iYear = 0)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((o => o.CALENDAR_YEAR == (int?) iYear)).ToList<DoctorCalendarLeader>();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarHospital(int iMonth = 0, int iYear = 0, DateTime? timeStart = null, DateTime? timeEnd = null)
    {
      List<DoctorCalendarLeader> doctorCalendarLeaderList = new List<DoctorCalendarLeader>();
      IQueryable<DoctorCalendarLeader> source = ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((t => t.DATE_START <= (DateTime?) timeEnd.Value && t.DATE_START >= (DateTime?) timeStart.Value && t.CALENDAR_STATUS == (int?) 3));
      if (!timeStart.HasValue)
        ;
      if (!timeEnd.HasValue)
        ;
      return source.ToList<DoctorCalendarLeader>();
    }

    public bool ExistTemplateId(int id)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.TEMPLATES_ID.Value.Equals(id) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count<CALENDAR_DUTY>() > 0;
    }

    public PagedList<CALENDAR_DUTY> GetAll(SearchCalendarDuty calendarSearch, int page, int size, string sort, string sortDir, int types, string idDepartment, out int totalRow)
    {
      totalRow = 0;
      IQueryable<CALENDAR_DUTY> source = this.DbSet.AsNoTracking().Where((obj => (calendarSearch.DATE_CREATE == new DateTime?() || obj.DATE_CREATE.Value.Year == calendarSearch.DATE_CREATE.Value.Year && obj.DATE_CREATE.Value.Month == calendarSearch.DATE_CREATE.Value.Month && obj.DATE_CREATE.Value.Day == calendarSearch.DATE_CREATE.Value.Day) && (calendarSearch.DATE_APPROVED == new DateTime?() || obj.DATE_APPROVED.Value.Year == calendarSearch.DATE_APPROVED.Value.Year && obj.DATE_APPROVED.Value.Month == calendarSearch.DATE_APPROVED.Value.Month && obj.DATE_APPROVED.Value.Day == calendarSearch.DATE_APPROVED.Value.Day) && (calendarSearch.ADMIN_USER_CREATE == (object) null || calendarSearch.ADMIN_USER_CREATE == "" || obj.ADMIN_USER.FULLNAME.Contains(calendarSearch.ADMIN_USER_CREATE)) && (calendarSearch.ADMIN_USER_APPROVED == (object) null || calendarSearch.ADMIN_USER_APPROVED == "" || obj.ADMIN_USER1.FULLNAME.Contains(calendarSearch.ADMIN_USER_APPROVED)) && (calendarSearch.CALENDAR_STATUS == 0 || obj.CALENDAR_STATUS == (int?) calendarSearch.CALENDAR_STATUS) && (calendarSearch.DATE_MONTH == 0 || obj.CALENDAR_MONTH == (int?) calendarSearch.DATE_MONTH) && (calendarSearch.DATE_YEAR == 0 || obj.CALENDAR_YEAR == (int?) calendarSearch.DATE_YEAR) && (calendarSearch.DEPARTMENTS == (object) null || calendarSearch.DEPARTMENTS == "" || obj.LM_DEPARTMENT.DEPARTMENT_NAME.Contains(calendarSearch.DEPARTMENTS)) && (types == 1 || types == 2 || types == 4 || obj.LM_DEPARTMENT_PARTS.Contains("," + idDepartment.ToString() + ",")) && obj.ISDELETE == false && obj.DUTY_TYPE == (int?) types));
      totalRow = source.Count<CALENDAR_DUTY>();
      return source.OrderByDescending<CALENDAR_DUTY>(sort).Paginate<CALENDAR_DUTY>(page, size, totalRow);
    }

    public List<DoctorCalendar> GetDoctorCalendar(int idCalendar = 0)
    {
      return ((IQueryable<DoctorCalendar>) this.DbContext.DoctorCalendars).Where((o => o.CALENDAR_DUTY_ID == idCalendar)).ToList<DoctorCalendar>();
    }

    public bool ExistReferenceDepartment(int deprtID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.LM_DEPARTMENT_ID.Equals(deprtID) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count<CALENDAR_DUTY>() > 0;
    }

    public PagedList<DoctorCalendarLeader> GetDoctorCalendarHoliday(DoctorSearch entity, int page, int size)
    {
      try
      {
        List<DoctorCalendarLeader> doctorCalendarLeaderList = new List<DoctorCalendarLeader>();
        IQueryable<DoctorCalendarLeader> source1 = ((IQueryable<DoctorCalendarLeader>) ((DbQuery<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).AsNoTracking()).Select((obj => obj));
        IQueryable<FEAST> source2 = ((IQueryable<FEAST>) ((DbQuery<FEAST>) this.DbContext.FEASTs).AsNoTracking()).Where((obj => obj.ISDELETE == false && obj.ISACTIVED == true && obj.FEAST_YEAR == (int?) DateTime.Now.Year));
        if (!source2.Any<FEAST>())
          return new PagedList<DoctorCalendarLeader>();
        List<FEASTSEARCH> feastsearchList = new List<FEASTSEARCH>();
        FEASTSEARCH feastsearch1;
        if (entity.SearchFeastId.HasValue && entity.SearchFeastId.Value > 0)
        {
          IQueryable<FEAST> source3 = source2.Where((t => t.FEAST_ID.Equals(entity.SearchFeastId.Value)));
          if (!source3.Any<FEAST>())
            return new PagedList<DoctorCalendarLeader>();
          List<FEAST> list = source3.ToList<FEAST>();
          for (int index = 0; index < list.Count; ++index)
          {
            feastsearchList.Add(new FEASTSEARCH()
            {
              DATE_BEGIN = list[index].DATE_BEGIN,
              DATE_END = list[index].DATE_END
            });
            feastsearch1 = (FEASTSEARCH) null;
          }
        }
        else
        {
          List<FEAST> list = source2.ToList<FEAST>();
          for (int index = 0; index < list.Count; ++index)
          {
            feastsearchList.Add(new FEASTSEARCH()
            {
              DATE_BEGIN = list[index].DATE_BEGIN,
              DATE_END = list[index].DATE_END
            });
            feastsearch1 = (FEASTSEARCH) null;
          }
        }
        Expression<Func<DoctorCalendarLeader, bool>> expression = null;
        foreach (FEASTSEARCH feastsearch2 in feastsearchList)
        {
          FEASTSEARCH dept = feastsearch2;
          Expression<Func<DoctorCalendarLeader, bool>> second = (t => SqlFunctions.DateDiff("dd", dept.DATE_BEGIN, t.DATE_START) >= (int?) 0 && SqlFunctions.DateDiff("dd", dept.DATE_END, t.DATE_START) <= (int?) 0);
          expression = expression != null ? expression.Or<DoctorCalendarLeader>(second) : second;
        }
        IQueryable<DoctorCalendarLeader> source4 = source1.Where(expression);
        int? nullable = entity.CalendarYear;
        if (nullable.HasValue)
          source4 = source4.Where((t => t.CALENDAR_YEAR == (int?) entity.CalendarYear.Value));
        if (!string.IsNullOrEmpty(entity.SearchName))
          source4 = source4.Where((t => t.DOCTOR_NAME.Contains(entity.SearchName.Trim())));
        nullable = entity.SearchDeprtId;
        int num1;
        if (nullable.HasValue)
        {
          nullable = entity.SearchDeprtId;
          num1 = nullable.Value <= 0 ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 == 0)
        {
          LM_DEPARTMENT deptPath = ((IQueryable<LM_DEPARTMENT>) ((DbQuery<LM_DEPARTMENT>) this.DbContext.LM_DEPARTMENT).AsNoTracking()).FirstOrDefault<LM_DEPARTMENT>((t => t.LM_DEPARTMENT_ID.Equals(entity.SearchDeprtId.Value)));
          if (deptPath == null)
            throw new Exception("Holiday");
          IQueryable<int> lstAllChildIds = ((IQueryable<LM_DEPARTMENT>) ((DbQuery<LM_DEPARTMENT>) this.DbContext.LM_DEPARTMENT).AsNoTracking()).Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(deptPath.DEPARTMENT_PATH))).Select((obj => obj.LM_DEPARTMENT_ID));
          if (lstAllChildIds.Any<int>())
            source4 = source4.Where((t => lstAllChildIds.Contains<int>(t.LM_DEPARTMENT_ID)));
        }
        nullable = entity.SearchPositionId;
        int num2;
        if (nullable.HasValue)
        {
          nullable = entity.SearchPositionId;
          num2 = nullable.Value <= 0 ? 1 : 0;
        }
        else
          num2 = 1;
        if (num2 == 0)
          source4 = source4.Where((t => t.POSITION_IDs.Contains("," + entity.SearchPositionId.Value.ToString() + ",")));
        return source4.Where((t => t.CALENDAR_STATUS == (int?) 3)).OrderBy((t => t.DOCTOR_NAME)).Paginate<DoctorCalendarLeader>(page, size, 0);
      }
      catch
      {
        return new PagedList<DoctorCalendarLeader>();
      }
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarDirector(List<int> doctorIds, int calendarDutyId)
    {
      return ((IQueryable<DoctorCalendarLeader>) this.DbContext.DoctorCalendarLeaders).Where((t => doctorIds.Contains(t.DOCTORS_ID) && t.CALENDAR_DUTY_ID.Equals(calendarDutyId))).ToList<DoctorCalendarLeader>();
    }
  }
}
