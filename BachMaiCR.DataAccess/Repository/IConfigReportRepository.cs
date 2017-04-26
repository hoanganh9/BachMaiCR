using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IConfigReportRepository : IRepository<CONFIG_REPORT>
    {
        List<CONFIG_REPORT> GetAllByExcelName(string excelName);
    }
}