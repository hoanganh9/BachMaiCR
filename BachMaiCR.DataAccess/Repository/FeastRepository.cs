using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            return this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false && obj.ISACTIVED == true).ToList();
        }

        public PagedList<FEAST> GetAllList(string name, int year, int page, int size, string sort, string sortDir)
        {
            IQueryable<FEAST> source = this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false && obj.FEAST_YEAR == year);
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
                source = source.Where((t => t.FEAST_TITLE.Contains(name.Trim())));
            if (sortDir.ToLower() == "asc")
                return source.OrderBy(sort).Paginate(page, size, 0);
            return source.OrderByDescending(sort).Paginate(page, size, 0);
        }
    }
}