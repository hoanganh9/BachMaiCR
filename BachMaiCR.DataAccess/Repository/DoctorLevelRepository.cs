using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

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
                return source.Any();
            return source.Where(obj => obj.DOCTOR_LEVEL_ID != entity.DOCTOR_LEVEL_ID).Any();
        }

        public PagedList<DOCTOR_LEVEL> GetAll(int type, string name, int page, int size, string sort, string sortDir)
        {
            IQueryable<DOCTOR_LEVEL> source = this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE != true && (obj.LEVEL_NAME.Contains(name) || obj.CODE.Contains(name)));
            if (sortDir.ToLower() == "asc")
                return source.OrderBy(sort).Paginate(page, size, 0);
            return source.OrderByDescending(sort).Paginate(page, size, 0);
        }

        public List<DOCTOR_LEVEL> GetAll_List()
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE != true).OrderBy(t => t.LEVEL_NUMBER).ToList();
        }

        public List<SelectListItem> GetListItemBase()
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE != true).OrderBy(obj => obj.LEVEL_ORDER).Select((obj => new SelectListItem()
            {
                Value = obj.DOCTOR_LEVEL_ID.ToString(),
                Text = obj.LEVEL_NAME
            })).ToList();
        }

        public DoctorLevelView GetDoctorLevelByIdDoctor(int idDoctor)
        {
            return this.DbContext.DoctorLevelViews.FirstOrDefault(o => o.DOCTORS_ID == idDoctor);
        }
    }
}