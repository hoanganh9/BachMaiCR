using System.Collections.Generic;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IDoctorGroupRepository : IRepository<DOCTOR_GROUPS>
  {
    List<SelectListItem> GetListItemBase();
  }
}
