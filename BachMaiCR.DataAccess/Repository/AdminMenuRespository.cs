





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
  public class AdminMenuRespository : EFRepository<ADMIN_MENU>, IAdminMenuRespository, IRepository<ADMIN_MENU>
  {
    public AdminMenuRespository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public bool ExistChild(int id)
    {
      return this.DbSet.Where((o => o.MENU_PARENT_ID == (int?) id)).Count<ADMIN_MENU>() > 0;
    }

    public List<ADMIN_MENU> GetChildMenu(int parrentId)
    {
      return this.DbSet.Where((o => o.MENU_PARENT_ID == (int?) parrentId && o.ISACTIVE == true)).OrderBy<ADMIN_MENU>("MENU_ORDER").ToList<ADMIN_MENU>();
    }

    public List<ADMIN_MENU> GetAll_List()
    {
      return this.DbSet.Where((o => o.ISACTIVE == true)).OrderBy<ADMIN_MENU>("MENU_ORDER").ToList<ADMIN_MENU>();
    }

    public List<MENULIST> GetAll_Active()
    {
      IQueryable<MENULIST> source = this.DbSet.AsNoTracking().Where((obj => obj.MENU_URL != "#" && obj.ISACTIVE == true)).OrderBy((obj => obj.MENU_NAME)).Select((obj => new MENULIST()
      {
        MENU_NAME = obj.MENU_NAME,
        MENU_URL = obj.MENU_URL
      }));
      MENULIST menulist = new MENULIST();
      menulist.MENU_NAME = "Không chọn";
      menulist.MENU_URL = "";
      List<MENULIST> list = source.ToList<MENULIST>();
      list.Insert(0, menulist);
      return list;
    }
  }
}
