





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class TemplateColumRepository : EFRepository<TEMPLATE_COLUM>, ITemplateColumRepository, IRepository<TEMPLATE_COLUM>
  {
    public TemplateColumRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public List<TEMPLATE_COLUM> GetColumnByIDTemplate(int idTemplate, int idDepartment)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.TEMPLATES_ID == idTemplate && obj.LM_DEPARTMENT_PATH.Contains(idDepartment.ToString()))).OrderBy<TEMPLATE_COLUM, int?>((Expression<Func<TEMPLATE_COLUM, int?>>) (obj => obj.COLUM_ORDER)).ToList();
    }
  }
}
