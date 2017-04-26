using BachMaiCR.DBMapping.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BachMaiCR.DataAccess.Repository
{
    public class TelephoneInDepartmentRepository : EFRepository<TelephoneInDepartment>, ITelephoneInDepartmentRepository, IRepository<TelephoneInDepartment>
    {
        public TelephoneInDepartmentRepository(DbContext dbContext)
          : base(dbContext)
        {
        }

        public List<TelephoneInDepartment> GetALLTelephone(int idCalendarDuty)
        {
            return this.DbContext.TelephoneInDepartments.Where(o => o.CALENDAR_ID == idCalendarDuty).ToList();
        }
    }
}