using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IDoctorGroupRepository : IRepository<DOCTOR_GROUPS>
    {
        List<SelectListItem> GetListItemBase();
    }
}