using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface ITemplateColumRepository : IRepository<TEMPLATE_COLUM>
    {
        List<TEMPLATE_COLUM> GetColumnByIDTemplate(int idTemplate, int idDepartment);
    }
}