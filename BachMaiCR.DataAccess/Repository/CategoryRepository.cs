﻿using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.DataAccess.Repository
{
    public class CategoryRepository : EFRepository<LM_CATEGORY>, ICategoryRepository, IRepository<LM_CATEGORY>
    {
        public CategoryRepository(DbContext dbContext)
          : base(dbContext)
        {
        }

        public bool ExistCode(LM_CATEGORY entity)
        {
            if (entity == null)
                return false;
            IQueryable<LM_CATEGORY> source = this.DbSet.AsNoTracking().Where((obj => obj.CODE.Equals(entity.CODE.Trim()) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (entity.CATEGORY_STYLE.HasValue.Equals(false) || obj.CATEGORY_STYLE.Value.Equals(entity.CATEGORY_STYLE.Value))));
            if (entity.LM_CATEGORY_ID <= 0)
                return source.Count() > 0;
            return source.Where(obj => obj.LM_CATEGORY_ID != entity.LM_CATEGORY_ID).Any();
        }

        public PagedList<LM_CATEGORY> GetAll(int type, string name, int page, int size, string sort, string sortDir)
        {
            IQueryable<LM_CATEGORY> source = this.DbSet.AsNoTracking().Where((obj => obj.CATEGORY_STYLE.Value.Equals(type) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (obj.CATEGORY_NAME.Contains(name) || obj.CODE.Contains(name))));
            if (sortDir.ToLower() == "asc")
                return source.OrderBy(sort).Paginate(page, size, 0);
            return source.OrderByDescending(sort).Paginate(page, size, 0);
        }

        public List<SelectListItem> GetListItemBase(int type)
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.CATEGORY_STYLE.Value.Equals(type) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).OrderBy(obj => obj.CATEGORY_NAME).Select((obj => new SelectListItem()
            {
                Value = obj.LM_CATEGORY_ID.ToString(),
                Text = obj.CATEGORY_NAME
            })).ToList();
        }

        public string GetMutilName(string lstId)
        {
            if (string.IsNullOrEmpty(lstId))
                return string.Empty;
            IQueryable<string> source = this.DbSet.AsNoTracking().Where((obj => lstId.Contains("," + obj.LM_CATEGORY_ID.ToString() + ","))).Select(obj => obj.CATEGORY_NAME);
            if (lstId.Any())
                return string.Join(", ", source.ToArray()).Trim();
            return string.Empty;
        }
    }
}