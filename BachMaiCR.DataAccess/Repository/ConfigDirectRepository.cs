





using System;
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
  public class ConfigDirectRepository : EFRepository<CONFIG_DIRECT>, IConfigDirectRepository, IRepository<CONFIG_DIRECT>
  {
    public ConfigDirectRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public bool CheckCalendarDirect(int idDoctor, DateTime? dateCheck)
    {
      int num = this.DbSet.AsNoTracking().Where((obj => obj.DOCTORS_ID == idDoctor && SqlFunctions.DateDiff("dd", obj.DATE_BEGIN, dateCheck) >= (int?) 0 && SqlFunctions.DateDiff("dd", obj.DATE_END, dateCheck) <= (int?) 0)).ToList().Count<CONFIG_DIRECT>();
      bool flag = false;
      if (num > 0)
        return true;
      return flag;
    }

    public PagedList<CONFIG_DIRECT> GetAll(ConfigHolidaySearch entity, int page, int size)
    {
      try
      {
        IQueryable<CONFIG_DIRECT> source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false));
        int? nullable;
        int num1;
        if (entity.SearchDeprtId.HasValue)
        {
          nullable = entity.SearchDeprtId;
          num1 = nullable.Value <= 0 ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 == 0)
        {
          LM_DEPARTMENT deptPath = ((IQueryable<LM_DEPARTMENT>) ((DbQuery<LM_DEPARTMENT>) this.DbContext.LM_DEPARTMENT).AsNoTracking()).FirstOrDefault((t => t.LM_DEPARTMENT_ID.Equals(entity.SearchDeprtId.Value)));
          if (deptPath == null)
            throw new Exception("ConfigDirect");
          IQueryable<int> lstAllChildIds = ((IQueryable<LM_DEPARTMENT>) ((DbQuery<LM_DEPARTMENT>) this.DbContext.LM_DEPARTMENT).AsNoTracking()).Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(deptPath.DEPARTMENT_PATH))).Select((obj => obj.LM_DEPARTMENT_ID));
          if (lstAllChildIds.Any<int>())
            source = source.Where((t => lstAllChildIds.Contains<int>(t.LM_DEPARTMENT_ID)));
        }
        nullable = entity.SearchDoctorId;
        int num2;
        if (nullable.HasValue)
        {
          nullable = entity.SearchDoctorId;
          if (nullable.Value > 0)
          {
            nullable = entity.SearchDoctorId;
            num2 = nullable.Value == 0 ? 1 : 0;
            goto label_14;
          }
        }
        num2 = 1;
label_14:
        if (num2 == 0)
          source = source.Where((t => t.DOCTORS_ID == entity.SearchDoctorId.Value));
        nullable = entity.SearchHolidayId;
        int num3;
        if (nullable.HasValue)
        {
          nullable = entity.SearchHolidayId;
          if (nullable.Value > 0)
          {
            nullable = entity.SearchHolidayId;
            num3 = nullable.Value == 0 ? 1 : 0;
            goto label_20;
          }
        }
        num3 = 1;
label_20:
        if (num3 == 0)
          source = source.Where((t => t.FEAST_ID == (int?) entity.SearchHolidayId.Value));
        if (!string.IsNullOrEmpty(entity.SearchDate) && entity.SearchDate.Trim() != "" && entity.SearchDate.Trim() != "__/__/____")
        {
          DateTime? searchDate = new DateTime?();
          searchDate = new DateTime?(DateTime.Parse(entity.SearchDate.Trim() + " 00:00"));
          source = source.Where((t => SqlFunctions.DateDiff("dd", t.DATE_BEGIN, searchDate) >= (int?) 0 && SqlFunctions.DateDiff("dd", t.DATE_END, searchDate) <= (int?) 0));
        }
        return source.OrderBy((t => t.DOCTOR.DOCTOR_NAME)).OrderBy<CONFIG_DIRECT, DateTime?>((Expression<Func<CONFIG_DIRECT, DateTime?>>) (t => t.DATE_BEGIN)).Paginate<CONFIG_DIRECT>(page, size, 0);
      }
      catch
      {
        return new PagedList<CONFIG_DIRECT>();
      }
    }
  }
}
