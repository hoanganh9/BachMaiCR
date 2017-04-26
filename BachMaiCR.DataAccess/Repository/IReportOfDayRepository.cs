using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IReportOfDayRepository : IRepository<REPORT>
    {
        PagedList<REPORT> GetAll(ItemSearch entity, int page, int size, int usrId = 0, int? doctorId = 0);

        bool CheckExistDateCreate(DateTime dtime, int deptID);
    }
}