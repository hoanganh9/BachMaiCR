using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IConfigHolidaysRepository : IRepository<CONFIG_HOLIDAYS>
    {
        PagedList<CONFIG_HOLIDAYS> GetAll(ConfigHolidaySearch entity, int page, int size);

        bool CheckCalendarHoliday(int idDoctor, DateTime? dateCheck);
    }
}