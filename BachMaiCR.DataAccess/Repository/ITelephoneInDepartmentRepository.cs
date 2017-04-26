using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface ITelephoneInDepartmentRepository : IRepository<TelephoneInDepartment>
    {
        List<TelephoneInDepartment> GetALLTelephone(int idCalendarDuty);
    }
}