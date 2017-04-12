
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class AdminUserRepository : EFRepository<ADMIN_USER>, IUserRepository, IRepository<ADMIN_USER>
  {
    public AdminUserRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public int Count()
    {
      return this.DbSet.Count<ADMIN_USER>();
    }

    public IEnumerable<string> GetActionCodesByUserId(int userId)
    {
      return this.GetActionCodesByUser(this.GetById(userId));
    }

    public IEnumerable<string> GetActionCodesByUserName(string userName)
    {
      return this.GetActionCodesByUser(this.GetByUserName(userName));
    }

    public IEnumerable<string> GetActionCodesByUser(ADMIN_USER user)
    {
      if (user == null)
        return (IEnumerable<string>) null;
      List<WEBPAGES_ACTIONS> source = new List<WEBPAGES_ACTIONS>();
      ICollection<WEBPAGES_ROLES> webpagesRoles1 = user.WEBPAGES_ROLES;
      if (webpagesRoles1 != null)
      {
        foreach (WEBPAGES_ROLES webpagesRoles2 in (IEnumerable<WEBPAGES_ROLES>) webpagesRoles1)
        {
          bool? isactive = webpagesRoles2.ISACTIVE;
          if ((!isactive.GetValueOrDefault() ? 0 : (isactive.HasValue ? 1 : 0)) != 0 && webpagesRoles2.WEBPAGES_ACTIONS != null)
            source.AddRange((IEnumerable<WEBPAGES_ACTIONS>) webpagesRoles2.WEBPAGES_ACTIONS.ToList<WEBPAGES_ACTIONS>());
        }
      }
      List<int> list = source.Select( (o => o.WEBPAGES_ACTION_ID)).ToList<int>();
      foreach (USERS_ACTIONS usersActions in user.USERS_ACTIONS.Where((o =>
      {
        bool? isActive = o.IS_ACTIVE;
        return isActive.GetValueOrDefault() && isActive.HasValue;
      })).ToList<USERS_ACTIONS>())
      {
        USERS_ACTIONS usersInAction = usersActions;
        if (!list.Contains(usersInAction.WEBPAGES_ACTION_ID))
        {
          WEBPAGES_ACTIONS webpagesActions = ((IQueryable<WEBPAGES_ACTIONS>) this.DbContext.WEBPAGES_ACTIONS).FirstOrDefault<WEBPAGES_ACTIONS>((o => o.WEBPAGES_ACTION_ID == usersInAction.WEBPAGES_ACTION_ID));
          if (webpagesActions != null)
            source.Add(webpagesActions);
        }
      }
      foreach (USERS_ACTIONS usersActions in user.USERS_ACTIONS.Where((o =>
      {
        bool? isActive = o.IS_ACTIVE;
        return !isActive.GetValueOrDefault() && isActive.HasValue;
      })).ToList<USERS_ACTIONS>())
      {
        USERS_ACTIONS usersInAction = usersActions;
        WEBPAGES_ACTIONS webpagesActions = source.FirstOrDefault<WEBPAGES_ACTIONS>((o => o.WEBPAGES_ACTION_ID == usersInAction.WEBPAGES_ACTION_ID));
        if (webpagesActions != null)
          source.Remove(webpagesActions);
      }
      return source.Select( (o => o.CODE));
    }

    public PagedList<ADMIN_USER> GetAll(int page, int size, string sort, string sortDir, bool isdelete)
    {
      if (sortDir.ToLower() == "asc")
        return this.DbSet.Where((o => o.ISDELETE == isdelete && o.IS_ADMIN !=  true)).OrderBy<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
      return this.DbSet.Where((o => o.ISDELETE == isdelete && o.IS_ADMIN !=  true)).OrderByDescending<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
    }

    public List<ADMIN_USER> GetAll(List<int> departments, string key, int currentUserId, int createdUserId)
    {
      if (!string.IsNullOrEmpty(key))
      {
        return this.DbSet.Where((o => o.ISDELETE == false && o.IS_ADMIN !=  true && o.ADMIN_USER_ID != currentUserId && o.ADMIN_USER_ID != createdUserId && departments.Contains<int>(o.LM_DEPARTMENT_ID ?? 0) && (o.FULLNAME.Contains(key.Trim()) || o.USERNAME.Contains(key.Trim())))).OrderBy<ADMIN_USER>("USERNAME").ToList<ADMIN_USER>();
      }
      return this.DbSet.Where((o => o.ISDELETE == false && o.IS_ADMIN !=  true && o.ADMIN_USER_ID != currentUserId && o.ADMIN_USER_ID != createdUserId && departments.Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderBy<ADMIN_USER>("USERNAME").ToList<ADMIN_USER>();
    }

    public PagedList<ADMIN_USER> GetAll(int page, int size, string sort, string sortDir, bool isdelete, List<int> departments)
    {
      if (sortDir.ToLower() == "asc")
      {
        return this.DbSet.Where((o => o.ISDELETE == isdelete && o.IS_ADMIN !=  true && departments.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderBy<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
      }
      return this.DbSet.Where((o => o.ISDELETE == isdelete && o.IS_ADMIN !=  true && departments.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderByDescending<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
    }

    public ADMIN_USER GetByUserName(string userName)
    {
      if (userName == null)
        return (ADMIN_USER) null;
      return this.DbSet.AsNoTracking().FirstOrDefault<ADMIN_USER>((o => o.USERNAME == userName));
    }

    public ADMIN_USER GetByUserCode(string userCode)
    {
      return this.DbSet.FirstOrDefault<ADMIN_USER>((o => o.USERCODE == userCode));
    }

    public List<ADMIN_USER> GetAll(string sort, string sortDir)
    {
      if (sortDir.ToLower() == "asc")
        return this.DbSet.Where((o => o.ISDELETE == false && o.ISACTIVED == true && o.IS_ADMIN !=  true)).OrderBy<ADMIN_USER>(sort).ToList<ADMIN_USER>();
      return this.DbSet.Where((o => o.ISDELETE == false && o.ISACTIVED == true && o.IS_ADMIN !=  true)).OrderByDescending<ADMIN_USER>(sort).ToList<ADMIN_USER>();
    }

    public PagedList<ADMIN_USER> SearchAllByUserName(string key, int page, int size, string sort, string sortDir)
    {
      if (sortDir.ToLower() == "asc")
        return this.DbSet.Where((o => o.USERNAME.Contains(key) && o.IS_ADMIN !=  true)).OrderBy<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
      return this.DbSet.Where((o => o.USERNAME.Contains(key) && o.IS_ADMIN !=  true)).OrderByDescending<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
    }

    public PagedList<ADMIN_USER> SearchAllByUserName(string key, int page, int size, string sort, string sortDir, List<int> departments)
    {
      var cDisplayClass1a = new { key = key, departments = departments };
      if (sortDir.ToLower() == "asc")
      {
        return this.DbSet.Where((o => (o.USERNAME.Contains(cDisplayClass1a.key) || o.FULLNAME.Contains(cDisplayClass1a.key)) && o.IS_ADMIN !=  true && o.ISDELETE == false && cDisplayClass1a.departments.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderBy<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
      }
      return this.DbSet.Where((o => (o.USERNAME.Contains(cDisplayClass1a.key) || o.FULLNAME.Contains(cDisplayClass1a.key)) && o.IS_ADMIN !=  true && o.ISDELETE == false && cDisplayClass1a.departments.Select( (d => d)).Contains<int>(o.LM_DEPARTMENT_ID ?? 0))).OrderByDescending<ADMIN_USER>(sort).Paginate<ADMIN_USER>(page, size, 0);
    }

    public bool CheckUserExit(string userName)
    {
      return this.DbSet.FirstOrDefault<ADMIN_USER>((o => o.USERNAME == userName)) != null;
    }

    public string GetUrlActive(string UserName)
    {
      IQueryable<URL_ACTIVE> source = this.DbSet.AsNoTracking().Where((obj => obj.USERNAME == UserName.Trim())).OrderBy((obj => obj.USERNAME)).Select((obj => new URL_ACTIVE()
      {
        ACTIVE_URL = obj.ACTIVE_URL
      }));
      string str = "";
      if (source.Count<URL_ACTIVE>() > 0)
        str = source.ToList<URL_ACTIVE>()[0].ACTIVE_URL;
      return str;
    }

    public bool ExistReferenceDepartment(int deprtID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.LM_DEPARTMENT_ID.Value.Equals(deprtID) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count<ADMIN_USER>() > 0;
    }

    public bool ExistReferenceUser(int usrID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.DOCTORS_ID.Value.Equals(usrID) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count<ADMIN_USER>() > 0;
    }

    public void UpdateUserByDoctorId(int doctorId, bool active)
    {
      DbQuery<ADMIN_USER> dbQuery = this.DbSet.AsNoTracking();
      Expression<Func<ADMIN_USER, bool>> predicate = (obj => obj.DOCTORS_ID == (int?) doctorId && obj.ISDELETE == false && obj.ISACTIVED == !active);
      foreach (ADMIN_USER entity in dbQuery.Where(predicate).ToList<ADMIN_USER>())
      {
        entity.ISACTIVED = new bool?(active);
        this.Update(entity);
      }
    }
  }
}
