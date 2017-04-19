using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IConfigReportRepository : IRepository<CONFIG_REPORT>
  {
    List<CONFIG_REPORT> GetAllByExcelName(string excelName);
  }
}
