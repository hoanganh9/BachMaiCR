using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface ITemplateRepository : IRepository<TEMPLATE>
    {
        PagedList<TEMPLATE> GetAll(SearchTemplate calendarSearch, int page, int size, string sort, string sortDir, int types, out int totalRow);

        List<TEMPLATE> GetListByDate(int DepartmentID, DateTime date, int status);

        bool ExistReferenceDepartment(int deprtID);
    }
}