using System.Collections.Generic;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IDepartmentRepository : IRepository<LM_DEPARTMENT>
  {
    bool ExistCode(LM_DEPARTMENT entity);

    PagedList<LM_DEPARTMENT> GetAll(int type, string name, int page, int size, string sort, string sortDir);

    bool ExistChild(int id);

    List<LM_DEPARTMENT> GetAll_List();

    List<SelectListItem> GetListItemBase();

    List<ItemBase> GetListItemBase(List<int> ids);

    List<LM_DEPARTMENT> GetChildDepartment(int parrentId);

    List<LM_DEPARTMENT> GetChildDepartment(string parrentId);

    List<int> GetAllByDepartmentID(string idDepartment);

    List<DEPARTMENTLIST> GetAllDepartmentByLevel(int Level);

    List<DEPARTMENTLIST> GetAllDepartmentByLevelVT(int Level);

    List<DEPARTMENTLIST> GetAllDepartmentByParent(string departmentPart);

    List<DEPARTMENTLIST> GetAllDepartmentById(int? DeparmentId);

    bool ExistDeparmentLevel0(int? DeparmentId);

    string DeparmentNameById(int? DeparmentId);

    string GetListDepartment(string Ids);

    string GetListDepartmentCode(string Ids);
  }
}
