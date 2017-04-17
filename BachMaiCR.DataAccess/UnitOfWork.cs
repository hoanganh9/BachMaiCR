using System;
using System.Data.Entity;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.DataAccess
{
  public class UnitOfWork : IUnitOfWork, IDisposable
  {
    private bool _disposed = false;
    private BACHMAICRContext _context;

    public BACHMAICRContext Context
    {
      get
      {
        return this._context;
      }
      set
      {
        this._context = value;
      }
    }

    public IUserRepository Users
    {
      get
      {
        return (IUserRepository) new AdminUserRepository((DbContext) this._context);
      }
    }

    public IActionRepository Actions
    {
      get
      {
        return (IActionRepository) new ActionRepository((DbContext) this._context);
      }
    }

    public IFunctonRepository Functons
    {
      get
      {
        return (IFunctonRepository) new FunctonRepository((DbContext) this._context);
      }
    }

    public IRoleRepository Roles
    {
      get
      {
        return (IRoleRepository) new RoleRepository((DbContext) this._context);
      }
    }

    public ICategoryRepository Categories
    {
      get
      {
        return (ICategoryRepository) new CategoryRepository((DbContext) this._context);
      }
    }

    public ICalendarDutyRepository CalendarDuty
    {
      get
      {
        return (ICalendarDutyRepository) new CalendarDutyRepository((DbContext) this._context);
      }
    }

    public IDoctorLevelRepository DoctorLevels
    {
      get
      {
        return (IDoctorLevelRepository) new DoctorLevelRepository((DbContext) this._context);
      }
    }

    public IDepartmentRepository Departments
    {
      get
      {
        return (IDepartmentRepository) new DepartmentRepository((DbContext) this._context);
      }
    }

    public IDoctorRepository Doctors
    {
      get
      {
        return (IDoctorRepository) new DoctorRepository((DbContext) this._context);
      }
    }

    public ITemplateColumRepository TemplatesColumn
    {
      get
      {
        return (ITemplateColumRepository) new TemplateColumRepository((DbContext) this._context);
      }
    }

    public ICalendarDoctorRepository CalendarDoctors
    {
      get
      {
        return (ICalendarDoctorRepository) new CalendarDoctorRepository((DbContext) this._context);
      }
    }

    public ICalendarDataRepository CalendarDatas
    {
      get
      {
        return (ICalendarDataRepository) new CalendarDataRepository((DbContext) this._context);
      }
    }

    public IAdminLogRepository AdminLogs
    {
      get
      {
        return (IAdminLogRepository) new AdminLogRepository((DbContext) this._context);
      }
    }

    public IAdminMenuRespository AdminMenu
    {
      get
      {
        return (IAdminMenuRespository) new AdminMenuRespository((DbContext) this._context);
      }
    }

    public IDoctorGroupRepository DoctorGroups
    {
      get
      {
        return (IDoctorGroupRepository) new DoctorGroupRepository((DbContext) this._context);
      }
    }

    public IReportOfDayRepository ReportOfDays
    {
      get
      {
        return (IReportOfDayRepository) new ReportOfDayRepository((DbContext) this._context);
      }
    }

    public IUsersInActionsRepository UsersInActions
    {
      get
      {
        return (IUsersInActionsRepository) new UsersActionsRepository((DbContext) this._context);
      }
    }

    public ICalendarGroupRepository CalendarGroups
    {
      get
      {
        return (ICalendarGroupRepository) new CalendarGroupRepository((DbContext) this._context);
      }
    }

    public ITemplateRepository Templates
    {
      get
      {
        return (ITemplateRepository) new TemplateRepository((DbContext) this._context);
      }
    }

    public ICalendarChangeRepository CalendarChanges
    {
      get
      {
        return (ICalendarChangeRepository) new CalendarChangeRepository((DbContext) this._context);
      }
    }

    public IFeastRepository Feasts
    {
      get
      {
        return (IFeastRepository) new FeastRepository((DbContext) this._context);
      }
    }

    public IDoctorDataRepository DoctorDatas
    {
      get
      {
        return (IDoctorDataRepository) new DoctorDataRepository((DbContext) this._context);
      }
    }

    public IConfigReportRepository ConfigReports
    {
      get
      {
        return (IConfigReportRepository) new ConfigReportRepository((DbContext) this._context);
      }
    }

    public IConfigHolidaysRepository ConfigHolidays
    {
      get
      {
        return (IConfigHolidaysRepository) new ConfigHolidaysRepository((DbContext) this._context);
      }
    }

    public IConfigDirectRepository ConfigDirect
    {
      get
      {
        return (IConfigDirectRepository) new ConfigDirectRepository((DbContext) this._context);
      }
    }

    public IConfigParameterRepository ConfigParameter
    {
      get
      {
        return (IConfigParameterRepository) new ConfigParameterRepository((DbContext) this._context);
      }
    }

    public IConfigSMSRepository ConfigSMS
    {
      get
      {
        return (IConfigSMSRepository) new ConfigSMSRepository((DbContext) this._context);
      }
    }

    public ICalendarAutoRepository CalendarAuto
    {
      get
      {
        return (ICalendarAutoRepository) new CalendarAutoRepository((DbContext) this._context);
      }
    }

    public ITelephoneInDepartmentRepository TelephoneInDepartments
    {
      get
      {
        return (ITelephoneInDepartmentRepository) new TelephoneInDepartmentRepository((DbContext) this._context);
      }
    }

    public UnitOfWork()
    {
      this.InitializeContext();
    }

    protected void InitializeContext()
    {
      this._context = new BACHMAICRContext();
      this._context.Configuration.LazyLoadingEnabled= true;
    }

    public ITransaction BeginTransaction()
    {
      return (ITransaction) new Transaction(this);
    }

    public void EndTransaction(ITransaction transaction)
    {
      if (transaction == null)
        return;
      transaction.Dispose();
      transaction = null;
    }

    public void Save()
    {
      this._context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this._disposed && disposing)
        this._context.Dispose();
      this._disposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
