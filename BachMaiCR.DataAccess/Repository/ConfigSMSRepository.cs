﻿using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;
using System.Data.Entity;
using System.Linq;

namespace BachMaiCR.DataAccess.Repository
{
    public class ConfigSMSRepository : EFRepository<CONFIG_SMS>, IConfigSMSRepository, IRepository<CONFIG_SMS>
    {
        public ConfigSMSRepository(DbContext dbContext)
          : base(dbContext)
        {
        }

        public PagedList<CONFIG_SMS> GetAll(ConfigSMSSearch entity, int page, int size)
        {
            try
            {
                IQueryable<CONFIG_SMS> source = this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false);
                if (entity.SearchDeprtId.HasValue && entity.SearchDeprtId.Value > 0)
                {
                    LM_DEPARTMENT deptPath = this.DbContext.LM_DEPARTMENT.AsNoTracking().FirstOrDefault((t => t.LM_DEPARTMENT_ID.Equals(entity.SearchDeprtId.Value)));
                    if (deptPath == null)
                        throw new Exception("ConfigSMS");
                    IQueryable<int> lstAllChildIds = this.DbContext.LM_DEPARTMENT.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(deptPath.DEPARTMENT_PATH))).Select(obj => obj.LM_DEPARTMENT_ID);
                    if (lstAllChildIds.Any())
                        source = source.Where((t => lstAllChildIds.Contains(t.LM_DEPARTMENT_ID)));
                }
                if (!string.IsNullOrEmpty(entity.SearchName) && entity.SearchName.Trim() != "")
                    source = source.Where((t => t.CONFIG_SMS_NAME.Contains(entity.SearchName.Trim())));
                return source.OrderBy(t => t.CONFIG_SMS_NAME).OrderBy(t => t.CONFIG_SMS_NAME).Paginate(page, size, 0);
            }
            catch
            {
                return new PagedList<CONFIG_SMS>();
            }
        }
    }
}