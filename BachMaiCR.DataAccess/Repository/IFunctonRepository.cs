using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IFunctonRepository : IRepository<WEBPAGES_FUNCTIONS>
  {
    WEBPAGES_FUNCTIONS GetByUniqueCode(string uniqueCode);

    IEnumerable<string> GetActionCodeListByUniqueCode(string uniqueCode);
  }
}
