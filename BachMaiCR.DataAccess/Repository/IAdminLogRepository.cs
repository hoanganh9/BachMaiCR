





using System.Collections.Generic;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IAdminLogRepository : IRepository<ADMIN_LOG>
  {
    PagedList<ADMIN_LOG> GetAll(LogSearch entity, int page, int size);

    bool OnDeleteListId(string lstId);

    List<SelectListItem> GetListMenuName();
  }
}
