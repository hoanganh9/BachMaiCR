





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
  public class RoleRepository : EFRepository<WEBPAGES_ROLES>, IRoleRepository, IRepository<WEBPAGES_ROLES>
  {
    public RoleRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public PagedList<WEBPAGES_ROLES> GetAll(int page, int size, string sort, string sortDir, out int totalRow)
    {
      IQueryable<WEBPAGES_ROLES> source = this.DbSet.AsNoTracking().Select((obj => obj));
      totalRow = source.Count<WEBPAGES_ROLES>();
      if (sortDir.ToLower() == "asc")
        return this.DbSet.OrderBy<WEBPAGES_ROLES>(sort).Paginate<WEBPAGES_ROLES>(page, size, totalRow);
      return this.DbSet.OrderByDescending<WEBPAGES_ROLES>(sort).Paginate<WEBPAGES_ROLES>(page, size, totalRow);
    }

    public PagedList<WEBPAGES_ROLES> GetAll(int page, int size, string sort, string sortDir, string key, List<int> department)
    {
        var cDisplayClass0 = new
        {
            key = key,
            department = department
        };
      if (sortDir.ToLower() == "asc")
      {
        if (!string.IsNullOrEmpty(cDisplayClass0.key))
        {
          return this.DbSet.Where((o => (o.ROLE_NAME.Contains(cDisplayClass0.key.Trim()) || o.DESCRIPTION.Contains(cDisplayClass0.key.Trim())) && cDisplayClass0.department.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderBy<WEBPAGES_ROLES>(sort).Paginate<WEBPAGES_ROLES>(page, size, 0);
        }
        return this.DbSet.Where((o => cDisplayClass0.department.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderBy<WEBPAGES_ROLES>(sort).Paginate<WEBPAGES_ROLES>(page, size, 0);
      }
      if (!string.IsNullOrEmpty(cDisplayClass0.key))
      {
        return this.DbSet.Where((o => (o.ROLE_NAME.Contains(cDisplayClass0.key.Trim()) || o.DESCRIPTION.Contains(cDisplayClass0.key.Trim())) && cDisplayClass0.department.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderByDescending<WEBPAGES_ROLES>(sort).Paginate<WEBPAGES_ROLES>(page, size, 0);
      }
      return this.DbSet.Where((o => cDisplayClass0.department.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderByDescending<WEBPAGES_ROLES>(sort).Paginate<WEBPAGES_ROLES>(page, size, 0);
    }

    public List<WEBPAGES_ROLES> GetAll(List<int> department)
    {
        var cDisplayClass2 = new { department = department };
        return this.DbSet.Where((o => cDisplayClass2.department.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderBy<WEBPAGES_ROLES>("ROLE_NAME").ToList();
    }

    public void Delete(int id)
    {
      WEBPAGES_ROLES byId = this.GetById(id);
      if (byId == null)
        return;
      this.Delete(byId);
    }

    public WEBPAGES_ROLES GetByRoleName(string roleName)
    {
      return this.DbSet.FirstOrDefault((o => o.ROLE_NAME == roleName));
    }

    public bool IsExistRoleName(string roleName)
    {
      return this.DbSet.FirstOrDefault((o => o.ROLE_NAME == roleName)) != null;
    }

    public bool ExistReferenceDepartment(int deprtID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.LM_DEPARTMENT_ID.Value.Equals(deprtID))).Count<WEBPAGES_ROLES>() > 0;
    }
  }
}
