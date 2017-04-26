using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IFunctonRepository : IRepository<WEBPAGES_FUNCTIONS>
    {
        WEBPAGES_FUNCTIONS GetByUniqueCode(string uniqueCode);

        IEnumerable<string> GetActionCodeListByUniqueCode(string uniqueCode);
    }
}