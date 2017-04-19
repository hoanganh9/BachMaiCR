using System;
using System.Collections.Generic;
using System.Data.Entity;
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
      return this.DbContext.DoctorHospitals.Where(o => o.CALENDAR_MONTH == iMonth && o.CALENDAR_YEAR == iYear).ToList();
    }

    public List<DoctorHospital> GetDoctorHospitalLeader(int iMonth = 0, int iYear = 0)
    {
      return this.DbContext.DoctorHospitals.Where(o => o.CALENDAR_MONTH == iMonth && o.CALENDAR_YEAR == iYear && o.DUTY_TYPE == 1).ToList();
    }

    public List<DoctorHospital> GetDoctorHospitalByDepartment(int iMonth = 0, int iYear = 0, int idDepartment = 0)
    {
      return this.DbContext.DoctorHospitals.Where((o => o.CALENDAR_MONTH == iMonth && o.CALENDAR_YEAR == iYear && o.LM_DEPARTMENT_ID == idDepartment && (o.LEVEL_NUMBER == 1 || o.LEVEL_NUMBER == 2))).ToList();
    }

    public List<CALENDAR_DUTY> GetByApproved(int month = 0, int year = 0, int dutyType = 3, int isApproved = 0)
    {
      return this.DbSet.AsNoTracking().Where(obj => obj.CALENDAR_MONTH == month && obj.CALENDAR_YEAR == year && obj.DUTY_TYPE == dutyType && obj.ISAPPROVED == isApproved && obj.ISDELETE == false).ToList();
    }

    public CALENDAR_DUTY CheckCalendarHospital(int calendarMonth, int calendarYear, int DutyType)
    {
      return this.DbSet.AsNoTracking().Where(obj => obj.CALENDAR_MONTH == calendarMonth && obj.CALENDAR_YEAR == calendarYear && obj.DUTY_TYPE == DutyType && obj.ISDELETE == false).FirstOrDefault();
    }

    public int CheckCalendarDuty(int calendarMonth, int calendarYear, int idTemplate, int idDepartment)
    {
      List<CALENDAR_DUTY> list = this.DbSet.AsNoTracking().Where(obj => obj.CALENDAR_MONTH == calendarMonth && obj.CALENDAR_YEAR == calendarYear && obj.TEMPLATES_ID == idTemplate && obj.ISDELETE == false).ToList();
      return list.Count() <= 0 ? 0 : list[0].CALENDAR_DUTY_ID;
    }

    public bool checkCalendar(int calendarMonth = 0, int calendarYear = 0, int dutyType = 1, int idDepartment = 0, int isDefault = 0)
    {
      IQueryable<CALENDAR_DUTY> source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.CALENDAR_MONTH == calendarMonth && obj.CALENDAR_YEAR == calendarYear && obj.DUTY_TYPE == dutyType));
      if (isDefault == 1)
        source = source.Where(o => o.LM_DEPARTMENT_ID == idDepartment).Where((o => o.TEMPLATES_ID == new int?()));
      return source.Count() > 0;
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarLeader(int idCalendar = 0)
    {
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_DUTY_ID == idCalendar).OrderBy(o => o.DOCTORS_ID).ToList();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarByDeparment(int deparmentId = 0)
    {
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.LM_DEPARTMENT_ID == deparmentId).OrderBy(o => o.DOCTORS_ID).ToList();
    }

    public List<CALENDAR_DUTY> GetCalendarDirector(int iMonth = 0, int iYear = 0, int duty_type = 1)
    {
      return this.DbContext.CALENDAR_DUTY.Where(o => o.CALENDAR_MONTH == iMonth).Where(o => o.CALENDAR_YEAR == iYear).Where(o => o.ISDELETE == false).Where(o => o.DUTY_TYPE == duty_type).ToList();
    }

    public List<CALENDAR_DUTY> GetCalendarByDeparment(int iMonth = 0, int iYear = 0, int duty_type = 3, int? idDepartment = 0)
    {
      return this.DbContext.CALENDAR_DUTY.Where(o => o.CALENDAR_MONTH == iMonth).Where(o => o.CALENDAR_YEAR == iYear).Where(o => o.ISDELETE == false).Where(o => o.DUTY_TYPE == duty_type).Where((o => (int?) o.LM_DEPARTMENT_ID == idDepartment && o.TEMPLATES_ID == new int?())).ToList();
    }

    public int GetCalendarDutyId(int iMonth = 0, int iYear = 0, int duty_type = 3, int idDepartment = 0, int isDefault = 0)
    {
      var source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.CALENDAR_MONTH == iMonth && obj.CALENDAR_YEAR == iYear && obj.DUTY_TYPE == duty_type && obj.LM_DEPARTMENT_ID == idDepartment)).Select(obj => new
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
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_MONTH == iMonth).Where(o => o.CALENDAR_YEAR == iYear).Where(o => o.DUTY_TYPE == duty_type).ToList();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarPesonal(int iMonth = 0, int iYear = 0, int doctorId = 0)
    {
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_MONTH == iMonth).Where(o => o.CALENDAR_YEAR == iYear).Where(o => o.DOCTORS_ID == doctorId).ToList();
    }

    public List<DoctorCalendarLeader> GetDoctorByCalendarDutyId(int calendarDutyId)
    {
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_DUTY_ID == calendarDutyId).ToList();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarDefault(int iMonth = 0, int iYear = 0, int duty_type = 3, int? deparmentId = 0)
    {
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_MONTH == iMonth).Where(o => o.CALENDAR_YEAR == iYear).Where(o => o.DUTY_TYPE == duty_type).Where((o => (int?) o.LM_DEPARTMENT_ID == deparmentId)).Where((o => o.TEMPLATES_ID == new int?())).ToList();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarHospital(int iMonth = 0, int iYear = 0)
    {
      return this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_MONTH == iMonth).Where(o => o.CALENDAR_YEAR == iYear).ToList();
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarHospital(int iMonth = 0, int iYear = 0, DateTime? timeStart = null, DateTime? timeEnd = null)
    {
      List<DoctorCalendarLeader> doctorCalendarLeaderList = new List<DoctorCalendarLeader>();
      IQueryable<DoctorCalendarLeader> source = this.DbContext.DoctorCalendarLeaders.Where(o => o.CALENDAR_MONTH == iMonth && o.CALENDAR_YEAR == iYear).Where((t => t.DATE_START <= (DateTime?) timeEnd.Value && t.DATE_START >= (DateTime?) timeStart.Value && t.CALENDAR_STATUS == 3));
        if (!timeStart.HasValue)
            source.Where(o => o.DATE_START == timeStart);
      if (!timeEnd.HasValue)
          source.Where(o => o.DATE_END == timeEnd);
        return source.ToList();
    }

    public bool ExistTemplateId(int id)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.TEMPLATES_ID.Value.Equals(id) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count() > 0;
    }

    public PagedList<CALENDAR_DUTY> GetAll(SearchCalendarDuty calendarSearch, int page, int size, string sort, string sortDir, int types, string idDepartment, out int totalRow)
    {
      totalRow = 0;
      IQueryable<CALENDAR_DUTY> source = this.DbSet.AsNoTracking().Where((obj => (calendarSearch.DATE_CREATE == new DateTime?() || obj.DATE_CREATE.Value.Year == calendarSearch.DATE_CREATE.Value.Year && obj.DATE_CREATE.Value.Month == calendarSearch.DATE_CREATE.Value.Month && obj.DATE_CREATE.Value.Day == calendarSearch.DATE_CREATE.Value.Day) && (calendarSearch.DATE_APPROVED == new DateTime?() || obj.DATE_APPROVED.Value.Year == calendarSearch.DATE_APPROVED.Value.Year && obj.DATE_APPROVED.Value.Month == calendarSearch.DATE_APPROVED.Value.Month && obj.DATE_APPROVED.Value.Day == calendarSearch.DATE_APPROVED.Value.Day) && (calendarSearch.ADMIN_USER_CREATE == (object) null || calendarSearch.ADMIN_USER_CREATE == "" || obj.ADMIN_USER.FULLNAME.Contains(calendarSearch.ADMIN_USER_CREATE)) && (calendarSearch.ADMIN_USER_APPROVED == (object) null || calendarSearch.ADMIN_USER_APPROVED == "" || obj.ADMIN_USER1.FULLNAME.Contains(calendarSearch.ADMIN_USER_APPROVED)) && (calendarSearch.CALENDAR_STATUS == 0 || obj.CALENDAR_STATUS == calendarSearch.CALENDAR_STATUS) && (calendarSearch.DATE_MONTH == 0 || obj.CALENDAR_MONTH == calendarSearch.DATE_MONTH) && (calendarSearch.DATE_YEAR == 0 || obj.CALENDAR_YEAR == calendarSearch.DATE_YEAR) && (calendarSearch.DEPARTMENTS == (object) null || calendarSearch.DEPARTMENTS == "" || obj.LM_DEPARTMENT.DEPARTMENT_NAME.Contains(calendarSearch.DEPARTMENTS)) && (types == 1 || types == 2 || types == 4 || obj.LM_DEPARTMENT_PARTS.Contains("," + idDepartment.ToString() + ",")) && obj.ISDELETE == false && obj.DUTY_TYPE == types));
      totalRow = source.Count();
      return source.OrderByDescending(sort).Paginate(page, size, totalRow);
    }

    public List<DoctorCalendar> GetDoctorCalendar(int idCalendar = 0)
    {
      return this.DbContext.DoctorCalendars.Where(o => o.CALENDAR_DUTY_ID == idCalendar).ToList();
    }

    public bool ExistReferenceDepartment(int deprtID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.LM_DEPARTMENT_ID.Equals(deprtID) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count() > 0;
    }

    public PagedList<DoctorCalendarLeader> GetDoctorCalendarHoliday(DoctorSearch entity, int page, int size)
    {
      try
      {
        List<DoctorCalendarLeader> doctorCalendarLeaderList = new List<DoctorCalendarLeader>();
        IQueryable<DoctorCalendarLeader> source1 = this.DbContext.DoctorCalendarLeaders.AsNoTracking().Select(obj => obj);
        IQueryable<FEAST> source2 = this.DbContext.FEASTs.AsNoTracking().Where(obj => obj.ISDELETE == false && obj.ISACTIVED == true && obj.FEAST_YEAR == DateTime.Now.Year);
        if (!source2.Any())
          return new PagedList<DoctorCalendarLeader>();
        List<FEASTSEARCH> feastsearchList = new List<FEASTSEARCH>();
        FEASTSEARCH feastsearch1;
        if (entity.SearchFeastId.HasValue && entity.SearchFeastId.Value > 0)
        {
          IQueryable<FEAST> source3 = source2.Where((t => t.FEAST_ID.Equals(entity.SearchFeastId.Value)));
          if (!source3.Any())
            return new PagedList<DoctorCalendarLeader>();
          List<FEAST> list = source3.ToList();
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
          List<FEAST> list = source2.ToList();
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
          expression = expression != null ? expression.Or(second) : second;
        }
        IQueryable<DoctorCalendarLeader> source4 = source1.Where(expression);
        int? nullable = entity.CalendarYear;
        if (nullable.HasValue)
          source4 = source4.Where(t => t.CALENDAR_YEAR == entity.CalendarYear.Value);
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
          LM_DEPARTMENT deptPath = this.DbContext.LM_DEPARTMENT.AsNoTracking().FirstOrDefault((t => t.LM_DEPARTMENT_ID.Equals(entity.SearchDeprtId.Value)));
          if (deptPath == null)
            throw new Exception("Holiday");
          IQueryable<int> lstAllChildIds = this.DbContext.LM_DEPARTMENT.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(deptPath.DEPARTMENT_PATH))).Select(obj => obj.LM_DEPARTMENT_ID);
          if (lstAllChildIds.Any())
            source4 = source4.Where((t => lstAllChildIds.Contains(t.LM_DEPARTMENT_ID)));
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
        return source4.Where(t => t.CALENDAR_STATUS == 3).OrderBy(t => t.DOCTOR_NAME).Paginate(page, size, 0);
      }
      catch
      {
        return new PagedList<DoctorCalendarLeader>();
      }
    }

    public List<DoctorCalendarLeader> GetDoctorCalendarDirector(List<int> doctorIds, int calendarDutyId)
    {
      return this.DbContext.DoctorCalendarLeaders.Where((t => doctorIds.Contains(t.DOCTORS_ID) && t.CALENDAR_DUTY_ID.Equals(calendarDutyId))).ToList();
    }
  }
}
