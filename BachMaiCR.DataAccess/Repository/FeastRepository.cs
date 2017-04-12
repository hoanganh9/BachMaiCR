





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class FeastRepository : EFRepository<FEAST>, IFeastRepository, IRepository<FEAST>
  {
    public FeastRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public List<FEAST> GetAll()
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.ISACTIVED == true)).ToList<FEAST>();
    }

    public PagedList<FEAST> GetAllList(string name, int year, int page, int size, string sort, string sortDir)
    {
      IQueryable<FEAST> source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.FEAST_YEAR == (int?) year));
      if (!string.IsNullOrEmpty(name) && name.Trim() != "")
        source = source.Where((t => t.FEAST_TITLE.Contains(name.Trim())));
      if (sortDir.ToLower() == "asc")
        return source.OrderBy<FEAST>(sort).Paginate<FEAST>(page, size, 0);
      return source.OrderByDescending<FEAST>(sort).Paginate<FEAST>(page, size, 0);
    }
  }
}
