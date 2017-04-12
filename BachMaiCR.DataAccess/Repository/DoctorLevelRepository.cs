





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class DoctorLevelRepository : EFRepository<DOCTOR_LEVEL>, IDoctorLevelRepository, IRepository<DOCTOR_LEVEL>
  {
    public DoctorLevelRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public bool ExistCode(DOCTOR_LEVEL entity)
    {
      if (entity == null)
        return false;
      IQueryable<DOCTOR_LEVEL> source = this.DbSet.AsNoTracking().Where((obj => obj.CODE.Equals(entity.CODE.Trim()) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))));
      if (entity.DOCTOR_LEVEL_ID <= 0)
        return source.Count<DOCTOR_LEVEL>() > 0;
      return source.Where((obj => obj.DOCTOR_LEVEL_ID != entity.DOCTOR_LEVEL_ID)).Count<DOCTOR_LEVEL>() > 0;
    }

    public PagedList<DOCTOR_LEVEL> GetAll(int type, string name, int page, int size, string sort, string sortDir)
    {
      IQueryable<DOCTOR_LEVEL> source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && (obj.LEVEL_NAME.Contains(name) || obj.CODE.Contains(name))));
      if (sortDir.ToLower() == "asc")
        return source.OrderBy<DOCTOR_LEVEL>(sort).Paginate<DOCTOR_LEVEL>(page, size, 0);
      return source.OrderByDescending<DOCTOR_LEVEL>(sort).Paginate<DOCTOR_LEVEL>(page, size, 0);
    }

    public List<DOCTOR_LEVEL> GetAll_List()
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy<DOCTOR_LEVEL, int?>((Expression<Func<DOCTOR_LEVEL, int?>>) (t => t.LEVEL_NUMBER)).ToList<DOCTOR_LEVEL>();
    }

    public List<SelectListItem> GetListItemBase()
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy<DOCTOR_LEVEL, int?>((Expression<Func<DOCTOR_LEVEL, int?>>) (obj => obj.LEVEL_ORDER)).Select((obj => new SelectListItem()
      {
        Value = obj.DOCTOR_LEVEL_ID.ToString(),
        Text = obj.LEVEL_NAME
      })).ToList<SelectListItem>();
    }

    public DoctorLevelView GetDoctorLevelByIdDoctor(int idDoctor)
    {
      return ((IQueryable<DoctorLevelView>) this.DbContext.DoctorLevelViews).FirstOrDefault<DoctorLevelView>((o => o.DOCTORS_ID == idDoctor));
    }
  }
}
