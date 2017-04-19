using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class TemplateRepository : EFRepository<TEMPLATE>, ITemplateRepository, IRepository<TEMPLATE>
  {
    public TemplateRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public PagedList<TEMPLATE> GetAll(SearchTemplate calendarSearch, int page, int size, string sort, string sortDir, int types, out int totalRow)
    {
      DateTime? toDate = new DateTime?();
      DateTime? fromDate = new DateTime?();
      if (!string.IsNullOrEmpty(calendarSearch.DATE_APPROVEDS))
      {
        toDate = new DateTime?(DateTime.Parse(calendarSearch.DATE_APPROVEDS.Trim() + " 00:00"));
        fromDate = new DateTime?(DateTime.Parse(calendarSearch.DATE_APPROVEDS.Trim() + " 23:59"));
      }
      int deparmentId = -1;
      if (!string.IsNullOrEmpty(calendarSearch.DEPARTMENTS))
        deparmentId = int.Parse(calendarSearch.DEPARTMENTS);
      totalRow = 0;
      IQueryable<TEMPLATE> source = this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false);
      if (!string.IsNullOrEmpty(calendarSearch.TEMPLATE_NAME) && calendarSearch.TEMPLATE_NAME.Trim() != "")
        source = source.Where((t => t.TEMPLATE_NAME.Contains(calendarSearch.TEMPLATE_NAME.Trim())));
      if (!string.IsNullOrEmpty(calendarSearch.ABBREVIATION) && calendarSearch.ABBREVIATION.Trim() != "")
        source = source.Where((t => t.ABBREVIATION.Contains(calendarSearch.ABBREVIATION.Trim())));
      if (calendarSearch.STATUS != 0)
        source = source.Where(t => t.STATUS == calendarSearch.STATUS);
      if (!string.IsNullOrEmpty(calendarSearch.DATE_APPROVEDS) && calendarSearch.DATE_APPROVEDS.Trim() != "")
        source = source.Where(t => t.DATE_APPROVED >= toDate).Where((t => t.DATE_APPROVED <= fromDate));
      if (!string.IsNullOrEmpty(calendarSearch.ADMIN_USER_CREATE) && calendarSearch.ADMIN_USER_CREATE.Trim() != "")
        source = source.Where((t => t.ADMIN_USER.FULLNAME.Contains(calendarSearch.ADMIN_USER_CREATE.Trim())));
      if (!string.IsNullOrEmpty(calendarSearch.ADMIN_USER_APPROVED) && calendarSearch.ADMIN_USER_APPROVED.Trim() != "")
        source = source.Where((t => t.ADMIN_USER1.FULLNAME.Contains(calendarSearch.ADMIN_USER_APPROVED.Trim())));
      if (deparmentId != -1)
        source = source.Where(t => t.LM_DEPARTMENT_ID == deparmentId);
      totalRow = source.Count();
      if (sortDir.ToLower() == "asc")
        return source.OrderBy(sort).OrderBy("TEMPLATE_NAME").Paginate(page, size, totalRow);
      return source.OrderByDescending(sort).OrderBy("TEMPLATE_NAME").Paginate(page, size, totalRow);
    }

    public List<TEMPLATE> GetListByDate(int DepartmentID, DateTime date, int status)
    {
      return this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DATE_START <= (DateTime?) date && obj.DATE_END >= (DateTime?) date && obj.STATUS == status && obj.LM_DEPARTMENT_PATH.Contains(DepartmentID.ToString()))).OrderBy(obj => obj.TEMPLATE_NAME).ToList();
    }

    public bool ExistReferenceDepartment(int deprtID)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.LM_DEPARTMENT_ID.Equals(deprtID) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count() > 0;
    }
  }
}
