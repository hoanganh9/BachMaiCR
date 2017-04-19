using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class ActionRepository : EFRepository<WEBPAGES_ACTIONS>, IActionRepository, IRepository<WEBPAGES_ACTIONS>
  {
    public ActionRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public WEBPAGES_ACTIONS GetByCode(string code)
    {
      return this.DbSet.FirstOrDefault(o => o.CODE == code);
    }

    public void AddOrUpdate(WEBPAGES_ACTIONS webpagesActions)
    {
      if (webpagesActions.WEBPAGES_ACTION_ID > 0)
        this.DbContext.Entry(webpagesActions).State = EntityState.Modified;
      else
        this.DbContext.Entry(webpagesActions).State = EntityState.Added;
      this.Save();
    }

    public IEnumerable<WEBPAGES_ACTIONS> GetNotInACodeList(List<string> codeList)
    {
      return this.DbSet.Where((o => !codeList.Contains(o.CODE)));
    }

    public IEnumerable<WEBPAGES_ACTIONS> GetInACodeList(List<string> codeList)
    {
      return this.DbSet.Where((o => codeList.Contains(o.CODE)));
    }

    public IEnumerable<WEBPAGES_ACTIONS> GetActiveActionsByUser(ADMIN_USER user)
    {
      List<WEBPAGES_ROLES> source = user.WEBPAGES_ROLES != null ? user.WEBPAGES_ROLES.ToList() : new List<WEBPAGES_ROLES>();
      List<int> actions = new List<int>();
      if (source.Any())
      {
        foreach (WEBPAGES_ROLES webpagesRoles in source)
        {
          if (webpagesRoles.WEBPAGES_ACTIONS != null && webpagesRoles.WEBPAGES_ACTIONS.Any())
            actions.AddRange(webpagesRoles.WEBPAGES_ACTIONS.Select(o => o.WEBPAGES_ACTION_ID).ToList());
        }
      }
      foreach (USERS_ACTIONS usersActions in user.USERS_ACTIONS.Where((o =>
      {
        bool? isActive = o.IS_ACTIVE;
        return isActive.GetValueOrDefault() && isActive.HasValue;
      })).ToList())
      {
        if (!actions.Contains(usersActions.WEBPAGES_ACTION_ID))
          actions.Add(usersActions.WEBPAGES_ACTION_ID);
      }
      foreach (USERS_ACTIONS usersActions in user.USERS_ACTIONS.Where((o =>
      {
        bool? isActive = o.IS_ACTIVE;
        return !isActive.GetValueOrDefault() && isActive.HasValue;
      })).ToList())
        actions.Remove(usersActions.WEBPAGES_ACTION_ID);
      return this.DbSet.Where(o => actions.Contains(o.WEBPAGES_ACTION_ID)).OrderBy(o => o.GROUP_NAME);
    }

    public IEnumerable<WEBPAGES_ACTIONS> GetAllActiveActions()
    {
      return this.DbSet.Where(o => o.IS_ACTIVE == true).OrderBy(o => o.GROUP_NAME).ToList();
    }

    public PagedList<WEBPAGES_ACTIONS> GetAll(int page, int size, string sort, string sortDir)
    {
      IQueryable<WEBPAGES_ACTIONS> source = this.DbSet.AsNoTracking().Select(obj => obj);
      if (sortDir.ToLower() == "asc")
        return source.OrderBy(sort).Paginate(page, size, 0);
      return source.OrderByDescending(sort).Paginate(page, size, 0);
    }
  }
}
