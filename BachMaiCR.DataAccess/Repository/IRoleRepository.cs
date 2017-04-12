





using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IRoleRepository : IRepository<WEBPAGES_ROLES>
  {
    PagedList<WEBPAGES_ROLES> GetAll(int page, int size, string sort, string sortDir, string key, List<int> department);

    PagedList<WEBPAGES_ROLES> GetAll(int page, int size, string sort, string sortDir, out int totalRow);

    List<WEBPAGES_ROLES> GetAll(List<int> department);

    void Delete(int id);

    WEBPAGES_ROLES GetByRoleName(string roleName);

    bool ExistReferenceDepartment(int deprtID);

    bool IsExistRoleName(string roleName);
  }
}
