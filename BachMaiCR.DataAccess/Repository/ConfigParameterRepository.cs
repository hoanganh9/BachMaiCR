using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BachMaiCR.DataAccess.Repository
{
    public class ConfigParameterRepository : EFRepository<CONFIG_PARAMETES>, IConfigParameterRepository, IRepository<CONFIG_PARAMETES>
    {
        public ConfigParameterRepository(DbContext dbContext)
          : base(dbContext)
        {
        }

        public List<CONFIG_PARAMETES> GetAll(int? deparmentId, int iYear, int type)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.LM_DEPARTMENT_ID == deparmentId && obj.CONFIG_YEAR == iYear && obj.CONFIG_TYPE == type).ToList();
        }

        public List<CONFIG_PARAMETES> GetParameterLeader(int iYear, int type)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.CONFIG_YEAR == iYear && obj.CONFIG_TYPE == type).ToList();
        }
    }
}