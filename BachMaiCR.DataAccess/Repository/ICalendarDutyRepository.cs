using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface ICalendarDutyRepository : IRepository<CALENDAR_DUTY>
    {
        PagedList<CALENDAR_DUTY> GetAll(SearchCalendarDuty calendarSearch, int page, int size, string sort, string sortDir, int types, string idDepartment, out int totalRow);

        int CheckCalendarDuty(int calendarMonth, int calendarYear, int idTemplate, int idDepartment);

        List<DoctorHospital> GetDoctorHospital(int iMonth = 0, int iYear = 0);

        List<DoctorHospital> GetDoctorHospitalLeader(int iMonth = 0, int iYear = 0);

        List<DoctorHospital> GetDoctorHospitalByDepartment(int iMonth = 0, int iYear = 0, int idDepartment = 0);

        CALENDAR_DUTY CheckCalendarHospital(int calendarMonth, int calendarYear, int DutyType);

        List<CALENDAR_DUTY> GetByApproved(int month = 0, int year = 0, int dutyType = 3, int isApproved = 0);

        bool checkCalendar(int calendarMonth = 0, int calendarYear = 0, int dutyType = 1, int idDepartment = 0, int isDefault = 0);

        List<DoctorCalendar> GetDoctorCalendar(int idCalendar = 0);

        List<DoctorCalendarLeader> GetDoctorCalendarLeader(int idCalendar = 0);

        List<CALENDAR_DUTY> GetCalendarDirector(int iMonth = 0, int iYear = 0, int duty_type = 1);

        List<CALENDAR_DUTY> GetCalendarByDeparment(int iMonth = 0, int iYear = 0, int duty_type = 3, int? idDepartment = 0);

        int GetCalendarDutyId(int iMonth = 0, int iYear = 0, int duty_type = 3, int idDepartment = 0, int isDefault = 0);

        List<DoctorCalendarLeader> GetDoctorCalendarDirector(List<int> doctorIds, int calendarDutyId);

        List<DoctorCalendarLeader> GetDoctorCalendarDirector(int iMonth = 0, int iYear = 0, int duty_type = 1);

        List<DoctorCalendarLeader> GetDoctorByCalendarDutyId(int calendarDutyId);

        List<DoctorCalendarLeader> GetDoctorCalendarDefault(int iMonth = 0, int iYear = 0, int duty_type = 3, int? deparmentId = 0);

        List<DoctorCalendarLeader> GetDoctorCalendarHospital(int iMonth = 0, int iYear = 0);

        List<DoctorCalendarLeader> GetDoctorCalendarHospital(int iMonth = 0, int iYear = 0, DateTime? timeStart = null, DateTime? timeEnd = null);

        PagedList<DoctorCalendarLeader> GetDoctorCalendarHoliday(DoctorSearch entity, int page, int size);

        bool ExistReferenceDepartment(int deprtID);

        bool ExistTemplateId(int id);

        List<DoctorCalendarLeader> GetDoctorCalendarByDeparment(int deparmentId = 0);

        List<DoctorCalendarLeader> GetDoctorCalendarPesonal(int iMonth = 0, int iYear = 0, int doctorId = 0);
    }
}