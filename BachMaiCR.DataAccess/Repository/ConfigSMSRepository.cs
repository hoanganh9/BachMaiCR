





using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class ConfigSMSRepository : EFRepository<CONFIG_SMS>, IConfigSMSRepository, IRepository<CONFIG_SMS>
  {
    public ConfigSMSRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public PagedList<CONFIG_SMS> GetAll(ConfigSMSSearch entity, int page, int size)
    {
      try
      {
        IQueryable<CONFIG_SMS> source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false));
        if (entity.SearchDeprtId.HasValue && entity.SearchDeprtId.Value > 0)
        {
          LM_DEPARTMENT deptPath = ((IQueryable<LM_DEPARTMENT>) ((DbQuery<LM_DEPARTMENT>) this.DbContext.LM_DEPARTMENT).AsNoTracking()).FirstOrDefault((t => t.LM_DEPARTMENT_ID.Equals(entity.SearchDeprtId.Value)));
          if (deptPath == null)
            throw new Exception("ConfigSMS");
          IQueryable<int> lstAllChildIds = ((IQueryable<LM_DEPARTMENT>) ((DbQuery<LM_DEPARTMENT>) this.DbContext.LM_DEPARTMENT).AsNoTracking()).Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(deptPath.DEPARTMENT_PATH))).Select((obj => obj.LM_DEPARTMENT_ID));
          if (lstAllChildIds.Any<int>())
            source = source.Where((t => lstAllChildIds.Contains<int>(t.LM_DEPARTMENT_ID)));
        }
        if (!string.IsNullOrEmpty(entity.SearchName) && entity.SearchName.Trim() != "")
          source = source.Where((t => t.CONFIG_SMS_NAME.Contains(entity.SearchName.Trim())));
        return source.OrderBy((t => t.CONFIG_SMS_NAME)).OrderBy((t => t.CONFIG_SMS_NAME)).Paginate<CONFIG_SMS>(page, size, 0);
      }
      catch
      {
        return new PagedList<CONFIG_SMS>();
      }
    }
  }
}
