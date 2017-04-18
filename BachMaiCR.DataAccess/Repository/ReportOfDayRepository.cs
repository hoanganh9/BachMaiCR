





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
  public class ReportOfDayRepository : EFRepository<REPORT>, IReportOfDayRepository, IRepository<REPORT>
  {
    public ReportOfDayRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public PagedList<REPORT> GetAll(ItemSearch entity, int page, int size, int usrId = 0, int? doctorId = 0)
    {
      try
      {
        IQueryable<REPORT> source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (string.IsNullOrEmpty(entity.SearchUserCreate) || obj.USER_CREATE_NAME.Contains(entity.SearchUserCreate.Trim())) && (string.IsNullOrEmpty(entity.SearchUserReceive) || obj.USER_RECIPIENTS_NAMEs.Contains(entity.SearchUserReceive.Trim())) && (string.IsNullOrEmpty(entity.SearchName) || obj.REPORT_NAME.Contains(entity.SearchName.Trim()))));
        if (entity.SearchDateStart.HasValue)
          source = source.Where((t => SqlFunctions.DateDiff("dd", (DateTime?) entity.SearchDateStart.Value, (DateTime?) t.DATE_CREATE) >= (int?) 0));
        DateTime? nullable = entity.SearchDateEnd;
        if (nullable.HasValue)
          source = source.Where((t => SqlFunctions.DateDiff("dd", (DateTime?) entity.SearchDateEnd.Value, (DateTime?) t.DATE_CREATE) <= (int?) 0));
        nullable = entity.SearchDateSent;
        if (nullable.HasValue)
          source = source.Where((t => SqlFunctions.DateDiff("dd", (DateTime?) entity.SearchDateSent.Value, (DateTime?) t.DATE_SENDED.Value) == (int?) 0));
        int? searchStatus = entity.SearchStatus;
        int num;
        if (searchStatus.HasValue)
        {
          searchStatus = entity.SearchStatus;
          num = searchStatus.Value < 0 ? 1 : 0;
        }
        else
          num = 1;
        if (num == 0)
          source = source.Where((t => t.STATUS.Equals(entity.SearchStatus.Value)));
        if (usrId > 0)
          source = source.Where((t => t.USER_CREATE_ID == usrId || t.USER_RECIPIENTS_IDs.Contains("," + doctorId.ToString() + ",") && t.STATUS == 1));
        return source.OrderByDescending((t => t.DATE_CREATE)).Paginate<REPORT>(page, size, 0);
      }
      catch
      {
        return new PagedList<REPORT>();
      }
    }

    public bool CheckExistDateCreate(DateTime dtime, int deptID)
    {
      return this.DbSet.AsNoTracking().FirstOrDefault((t => SqlFunctions.DateDiff("dd", (DateTime?) t.DATE_CREATE, (DateTime?) dtime) == (int?) 0 && deptID.Equals(t.LM_DEPARTMENT_ID) && t.ISDELETE == false)) != null;
    }
  }
}
