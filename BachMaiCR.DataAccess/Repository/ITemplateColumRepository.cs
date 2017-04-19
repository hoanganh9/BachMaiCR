using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ITemplateColumRepository : IRepository<TEMPLATE_COLUM>
  {
    List<TEMPLATE_COLUM> GetColumnByIDTemplate(int idTemplate, int idDepartment);
  }
}
