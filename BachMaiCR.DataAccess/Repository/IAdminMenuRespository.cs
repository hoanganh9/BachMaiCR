using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IAdminMenuRespository : IRepository<ADMIN_MENU>
    {
        bool ExistChild(int id);

        List<ADMIN_MENU> GetAll_List();

        List<ADMIN_MENU> GetChildMenu(int parrentId);

        List<MENULIST> GetAll_Active();
    }
}