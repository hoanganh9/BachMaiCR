





using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class UsersActionsRepository : EFRepository<USERS_ACTIONS>, IUsersInActionsRepository, IRepository<USERS_ACTIONS>
  {
    public UsersActionsRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public USERS_ACTIONS GetByKey(int actionId, int userId)
    {
      return this.DbSet.FirstOrDefault((o => o.WEBPAGES_ACTION_ID == actionId && o.ADMIN_USER_ID == userId));
    }

    public void AddOrUpdate(USERS_ACTIONS userInAction)
    {
      if (userInAction == null)
        return;
      USERS_ACTIONS byKey = this.GetByKey(userInAction.WEBPAGES_ACTION_ID, userInAction.ADMIN_USER_ID);
      if (byKey == null)
      {
        this.Add(userInAction);
      }
      else
      {
        byKey.IS_ACTIVE = userInAction.IS_ACTIVE;
        byKey.UPDATE_DATE = userInAction.UPDATE_DATE;
        this.Update(byKey);
      }
      this.Save();
    }
  }
}
