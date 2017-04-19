using System;
using System.Collections.Generic;
using System.Data.Entity;
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
      return this.DbSet.AsNoTracking().Where(obj => obj.DOCTORS_ID == idDoctor && SqlFunctions.DateDiff("dd", obj.DATE_CREATE, dateCheck) == 0).Any();
    }

    public bool CheckCalendarDoctor(int idDoctor, DateTime? dateCheck)
    {
        return (from c in DbContext.CALENDAR_DUTY
            join data in DbContext.CALENDAR_DATA on c.CALENDAR_DUTY_ID equals data.CALENDAR_DUTY_ID
            join doc in DbContext.CALENDAR_DOCTOR on data.CALENDAR_DATA_ID equals doc.CALENDAR_DATA_ID
            where c.ISDELETE == false && data.ISDELETE == false && doc.DOCTORS_ID == idDoctor &&
                  SqlFunctions.DateDiff("dd", data.DATE_START, dateCheck) == 0
            select doc.DOCTORS_ID).Any();
      
    }
  }
}
