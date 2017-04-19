using System;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IConfigDirectRepository : IRepository<CONFIG_DIRECT>
  {
    PagedList<CONFIG_DIRECT> GetAll(ConfigHolidaySearch entity, int page, int size);

    bool CheckCalendarDirect(int idDoctor, DateTime? dateCheck);
  }
}
