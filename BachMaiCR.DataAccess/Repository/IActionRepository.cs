





using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IActionRepository : IRepository<WEBPAGES_ACTIONS>
  {
    WEBPAGES_ACTIONS GetByCode(string code);

    void AddOrUpdate(WEBPAGES_ACTIONS webpagesActions);

    IEnumerable<WEBPAGES_ACTIONS> GetNotInACodeList(List<string> codeList);

    IEnumerable<WEBPAGES_ACTIONS> GetInACodeList(List<string> codeList);

    IEnumerable<WEBPAGES_ACTIONS> GetActiveActionsByUser(ADMIN_USER user);

    IEnumerable<WEBPAGES_ACTIONS> GetAllActiveActions();

    PagedList<WEBPAGES_ACTIONS> GetAll(int page, int size, string sort, string sortDir);
  }
}
