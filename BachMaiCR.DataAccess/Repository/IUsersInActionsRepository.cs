using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IUsersInActionsRepository : IRepository<USERS_ACTIONS>
  {
    USERS_ACTIONS GetByKey(int actionId, int userId);

    void AddOrUpdate(USERS_ACTIONS userInAction);
  }
}
