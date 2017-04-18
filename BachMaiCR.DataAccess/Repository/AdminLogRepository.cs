

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class AdminLogRepository : EFRepository<ADMIN_LOG>, IAdminLogRepository, IRepository<ADMIN_LOG>
  {
    public AdminLogRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public PagedList<ADMIN_LOG> GetAll(LogSearch entity, int page, int size)
    {
      try
      {
        IQueryable<ADMIN_LOG> source = this.DbSet.AsNoTracking().Where((obj => (string.IsNullOrEmpty(entity.SearchName) || obj.CONTENT.Contains(entity.SearchName.Trim()) || obj.DESCRIPTION.Contains(entity.SearchName.Trim()) || obj.ERROR_CODE.Contains(entity.SearchName.Trim())) && (string.IsNullOrEmpty(entity.SearchMenuCode) || obj.MENU_CODE.Contains(entity.SearchMenuCode.Trim())) && (string.IsNullOrEmpty(entity.SearchIpAddress) || obj.IP_ADDRESS.Contains(entity.SearchIpAddress.Trim())) && (string.IsNullOrEmpty(entity.SearchUserName) || obj.USER_NAME.Contains(entity.SearchUserName.Trim()))));
        if (entity.SearchDateStart.HasValue)
          source = source.Where((t => SqlFunctions.DateDiff("dd", (DateTime?) entity.SearchDateStart.Value, (DateTime?) t.START_TIME) >= (int?) 0));
        if (entity.SearchDateEnd.HasValue)
          source = source.Where((t => SqlFunctions.DateDiff("dd", (DateTime?) entity.SearchDateEnd.Value, (DateTime?) t.START_TIME) <= (int?) 0));
        int? nullable = entity.SearchStatus;
        int num1;
        if (nullable.HasValue)
        {
          nullable = entity.SearchStatus;
          num1 = nullable.Value < 0 ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 == 0)
          source = source.Where((t => t.STATUS.Equals(entity.SearchStatus.Value)));
        nullable = entity.SearchAction;
        int num2;
        if (nullable.HasValue)
        {
          nullable = entity.SearchAction;
          num2 = nullable.Value < 0 ? 1 : 0;
        }
        else
          num2 = 1;
        if (num2 == 0)
          source = source.Where((t => t.ACTION_TYPE.Equals(entity.SearchAction.Value)));
        return source.OrderByDescending((t => t.START_TIME)).Paginate<ADMIN_LOG>(page, size, 0);
      }
      catch
      {
        return new PagedList<ADMIN_LOG>();
      }
    }

    public List<SelectListItem> GetListMenuName()
    {
      try
      { 
        return this.DbSet.AsNoTracking().OrderBy((obj => obj.MENU_NAME)).Select((obj => new SelectListItem()
        {
          Text = obj.MENU_NAME,
          Value = obj.MENU_CODE
        })).Distinct<SelectListItem>().ToList();
      }
      catch
      {
        return new List<SelectListItem>();
      }
    }

    public bool OnDeleteListId(string lstId)
    {
      if (string.IsNullOrEmpty(lstId))
        return false;
      IQueryable<ADMIN_LOG> source = this.DbSet.AsNoTracking().Where((obj => lstId.Contains("," + obj.LOG_ID.ToString() + ",")));
      if (!source.Any<ADMIN_LOG>())
        return false;
      foreach (ADMIN_LOG entity in source.ToList())
        this.Delete(entity);
      this.Save();
      return true;
    }
  }
}
