using System.Collections.Generic;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess.Repository
{
  public interface ITelephoneInDepartmentRepository : IRepository<TelephoneInDepartment>
  {
    List<TelephoneInDepartment> GetALLTelephone(int idCalendarDuty);
  }
}
