using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            return DbSet.Where(o => o.MENU_PARENT_ID == id).Any();
        }

        public List<ADMIN_MENU> GetChildMenu(int parrentId)
        {
            return this.DbSet.Where(o => o.MENU_PARENT_ID == parrentId && o.ISACTIVE == true).OrderBy("MENU_ORDER").ToList();
        }

        public List<ADMIN_MENU> GetAll_List()
        {
            return this.DbSet.Where(o => o.ISACTIVE == true).OrderBy("MENU_ORDER").ToList();
        }

        public List<MENULIST> GetAll_Active()
        {
            IQueryable<MENULIST> source = this.DbSet.AsNoTracking().Where(obj => obj.MENU_URL != "#" && obj.ISACTIVE == true).OrderBy(obj => obj.MENU_NAME).Select((obj => new MENULIST()
            {
                MENU_NAME = obj.MENU_NAME,
                MENU_URL = obj.MENU_URL
            }));
            MENULIST menulist = new MENULIST();
            menulist.MENU_NAME = "Không chọn";
            menulist.MENU_URL = "";
            List<MENULIST> list = source.ToList();
            list.Insert(0, menulist);
            return list;
        }
    }
}