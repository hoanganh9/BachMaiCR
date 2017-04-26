using BachMaiCR.DataAccess.Repository;
using System;

namespace BachMaiCR.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        IActionRepository Actions { get; }

        IFunctonRepository Functons { get; }

        IRoleRepository Roles { get; }

        ICategoryRepository Categories { get; }

        IDoctorLevelRepository DoctorLevels { get; }

        IDepartmentRepository Departments { get; }

        ICalendarDutyRepository CalendarDuty { get; }

        IUsersInActionsRepository UsersInActions { get; }

        IDoctorRepository Doctors { get; }

        ITemplateColumRepository TemplatesColumn { get; }

        ITemplateRepository Templates { get; }

        ICalendarDoctorRepository CalendarDoctors { get; }

        ICalendarDataRepository CalendarDatas { get; }

        IAdminLogRepository AdminLogs { get; }

        IReportOfDayRepository ReportOfDays { get; }

        IAdminMenuRespository AdminMenu { get; }

        ICalendarChangeRepository CalendarChanges { get; }

        IDoctorGroupRepository DoctorGroups { get; }

        ICalendarGroupRepository CalendarGroups { get; }

        IFeastRepository Feasts { get; }

        IDoctorDataRepository DoctorDatas { get; }

        ITelephoneInDepartmentRepository TelephoneInDepartments { get; }

        IConfigReportRepository ConfigReports { get; }

        IConfigHolidaysRepository ConfigHolidays { get; }

        IConfigDirectRepository ConfigDirect { get; }

        IConfigParameterRepository ConfigParameter { get; }

        IConfigSMSRepository ConfigSMS { get; }

        ICalendarAutoRepository CalendarAuto { get; }

        void Save();

        ITransaction BeginTransaction();

        void EndTransaction(ITransaction transaction);
    }
}