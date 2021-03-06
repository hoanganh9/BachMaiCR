﻿using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace BachMaiCR.DataAccess.Repository
{
    public class ConfigDirectRepository : EFRepository<CONFIG_DIRECT>, IConfigDirectRepository, IRepository<CONFIG_DIRECT>
    {
        public ConfigDirectRepository(DbContext dbContext)
          : base(dbContext)
        {
        }

        public bool CheckCalendarDirect(int idDoctor, DateTime? dateCheck)
        {
            int num = this.DbSet.AsNoTracking().Where((obj => obj.DOCTORS_ID == idDoctor && SqlFunctions.DateDiff("dd", obj.DATE_BEGIN, dateCheck) >= (int?)0 && SqlFunctions.DateDiff("dd", obj.DATE_END, dateCheck) <= (int?)0)).ToList().Count();
            bool flag = false;
            if (num > 0)
                return true;
            return flag;
        }

        public PagedList<CONFIG_DIRECT> GetAll(ConfigHolidaySearch entity, int page, int size)
        {
            try
            {
                IQueryable<CONFIG_DIRECT> source = this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false);
                int? nullable;
                if (entity.SearchDeprtId.HasValue && entity.SearchDeprtId.Value >0)
                {
                    LM_DEPARTMENT deptPath = this.DbContext.LM_DEPARTMENT.AsNoTracking().FirstOrDefault((t => t.LM_DEPARTMENT_ID.Equals(entity.SearchDeprtId.Value)));
                    if (deptPath == null)
                        throw new Exception("ConfigDirect");
                    IQueryable<int> lstAllChildIds = this.DbContext.LM_DEPARTMENT.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(deptPath.DEPARTMENT_PATH))).Select(obj => obj.LM_DEPARTMENT_ID);
                    if (lstAllChildIds.Any())
                        source = source.Where((t => lstAllChildIds.Contains(t.LM_DEPARTMENT_ID)));
                }
                if (entity.SearchDoctorId.HasValue && entity.SearchDoctorId.Value > 0)
                    source = source.Where(t => t.DOCTORS_ID == entity.SearchDoctorId.Value);
                if (entity.SearchHolidayId.HasValue && entity.SearchHolidayId.Value>0)
                    source = source.Where(t => t.FEAST_ID == entity.SearchHolidayId.Value);
                if (!string.IsNullOrEmpty(entity.SearchDate) && entity.SearchDate.Trim() != "" && entity.SearchDate.Trim() != "__/__/____")
                {
                    DateTime? searchDate = new DateTime?();
                    searchDate = new DateTime?(DateTime.Parse(entity.SearchDate.Trim() + " 00:00"));
                    source = source.Where((t => SqlFunctions.DateDiff("dd", t.DATE_BEGIN, searchDate) >= (int?)0 && SqlFunctions.DateDiff("dd", t.DATE_END, searchDate) <= (int?)0));
                }
                return source.OrderBy(t => t.DOCTOR.DOCTOR_NAME).OrderBy(t => t.DATE_BEGIN).Paginate(page, size, 0);
            }
            catch
            {
                return new PagedList<CONFIG_DIRECT>();
            }
        }
    }
}