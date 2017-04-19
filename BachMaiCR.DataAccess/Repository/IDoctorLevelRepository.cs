using System.Collections.Generic;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IDoctorLevelRepository : IRepository<DOCTOR_LEVEL>
  {
    bool ExistCode(DOCTOR_LEVEL entity);

    PagedList<DOCTOR_LEVEL> GetAll(int type, string name, int page, int size, string sort, string sortDir);

    List<DOCTOR_LEVEL> GetAll_List();

    List<SelectListItem> GetListItemBase();

    DoctorLevelView GetDoctorLevelByIdDoctor(int idDoctor);
  }
}
