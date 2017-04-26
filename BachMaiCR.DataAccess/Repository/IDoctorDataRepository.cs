using BachMaiCR.DBMapping.Models;
using System;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IDoctorDataRepository : IRepository<DoctorData>
    {
        DoctorData CheckDoctorData(int idCalendarDuty, int idDoctor, int idTemplate_column, DateTime DateStart);
    }
}