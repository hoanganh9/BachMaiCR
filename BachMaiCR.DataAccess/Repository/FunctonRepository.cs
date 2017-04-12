





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class FunctonRepository : EFRepository<WEBPAGES_FUNCTIONS>, IFunctonRepository, IRepository<WEBPAGES_FUNCTIONS>
  {
    public FunctonRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public WEBPAGES_FUNCTIONS GetByUniqueCode(string uniqueCode)
    {
      return this.DbSet.FirstOrDefault<WEBPAGES_FUNCTIONS>((o => o.UNIQUE_CODE == uniqueCode));
    }

    public IEnumerable<string> GetActionCodeListByUniqueCode(string uniqueCode)
    {
      WEBPAGES_FUNCTIONS byUniqueCode = this.GetByUniqueCode(uniqueCode);
      if (byUniqueCode != null)
        return byUniqueCode.WEBPAGES_ACTIONS.Select( (o => o.CODE));
      return (IEnumerable<string>) new string[0];
    }
  }
}
