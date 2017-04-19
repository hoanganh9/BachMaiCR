using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class ConfigReportRepository : EFRepository<CONFIG_REPORT>, IConfigReportRepository, IRepository<CONFIG_REPORT>
  {
    public ConfigReportRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public List<CONFIG_REPORT> GetAllByExcelName(string excelName)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.CONFIG_REPORT_EXCEL == excelName.Trim())).ToList();
    }
  }
}
