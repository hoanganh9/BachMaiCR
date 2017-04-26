using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IConfigParameterRepository : IRepository<CONFIG_PARAMETES>
    {
        List<CONFIG_PARAMETES> GetAll(int? deparmentId, int iYear, int type);

        List<CONFIG_PARAMETES> GetParameterLeader(int iYear, int type);
    }
}