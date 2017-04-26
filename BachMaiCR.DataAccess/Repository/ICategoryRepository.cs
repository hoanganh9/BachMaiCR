using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BachMaiCR.DataAccess.Repository
{
    public interface ICategoryRepository : IRepository<LM_CATEGORY>
    {
        bool ExistCode(LM_CATEGORY entity);

        PagedList<LM_CATEGORY> GetAll(int type, string name, int page, int size, string sort, string sortDir);

        List<SelectListItem> GetListItemBase(int type);

        string GetMutilName(string lstId);
    }
}