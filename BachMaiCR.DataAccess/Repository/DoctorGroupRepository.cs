





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public class DoctorGroupRepository : EFRepository<DOCTOR_GROUPS>, IDoctorGroupRepository, IRepository<DOCTOR_GROUPS>
  {
    public DoctorGroupRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public List<SelectListItem> GetListItemBase()
    {
      return this.DbSet.AsNoTracking().OrderByDescending((obj => obj.DOCTOR_GROUP_ID)).Select((obj => new SelectListItem()
      {
        Value = obj.DOCTOR_GROUP_ID.ToString(),
        Text = obj.DOCTOR_GROUP_NAME
      })).ToList();
    }
  }
}
