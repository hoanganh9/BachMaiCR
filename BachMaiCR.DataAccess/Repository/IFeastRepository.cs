using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IFeastRepository : IRepository<FEAST>
    {
        List<FEAST> GetAll();

        PagedList<FEAST> GetAllList(string name, int year, int page, int size, string sort, string sortDir);
    }
}