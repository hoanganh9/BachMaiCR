





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class CalendarAutoRepository : EFRepository<CALENDAR_AUTO>, ICalendarAutoRepository, IRepository<CALENDAR_AUTO>
  {
    public CalendarAutoRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public bool CheckCalendarDoctorAuto(int idDoctor, DateTime? dateCheck)
    {
      int num = this.DbSet.AsNoTracking().Where((obj => obj.DOCTORS_ID == (int?) idDoctor && SqlFunctions.DateDiff("dd", obj.DATE_CREATE, dateCheck) == (int?) 0)).ToList().Count<CALENDAR_AUTO>();
      bool flag = false;
      if (num > 0)
        return true;
      return flag;
    }

    public bool CheckCalendarDoctor(int idDoctor, DateTime? dateCheck)
    {
      int count = ((IQueryable<CALENDAR_DUTY>) this.DbContext.CALENDAR_DUTY).Join((IEnumerable<CALENDAR_DATA>) this.DbContext.CALENDAR_DATA, (Expression<Func<CALENDAR_DUTY, int>>) (calendar => calendar.CALENDAR_DUTY_ID), (Expression<Func<CALENDAR_DATA, int>>) (data => data.CALENDAR_DUTY_ID), (calendar, data) => new
      {
        calendar = calendar,
        data = data
      }).Join((IEnumerable<CALENDAR_DOCTOR>) this.DbContext.CALENDAR_DOCTOR, data => data.data.CALENDAR_DATA_ID, (Expression<Func<CALENDAR_DOCTOR, int>>) (doc => doc.CALENDAR_DATA_ID), (data, doc) => new
      {
       TransparentIdentifier2 = data,
        doc = doc
      }).Where(data => data.TransparentIdentifier2.calendar.ISDELETE == false && data.TransparentIdentifier2.data.ISDELETE == false && data.doc.DOCTORS_ID == idDoctor && SqlFunctions.DateDiff("dd", data.TransparentIdentifier2.data.DATE_START, dateCheck) == (int?) 0).Select(data => new
      {
        DOCTORS_ID = data.doc.DOCTORS_ID
      }).ToList().Count;
      bool flag = false;
      if (count > 0)
        return true;
      return flag;
    }
  }
}
