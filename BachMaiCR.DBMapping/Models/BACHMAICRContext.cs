using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using BachMaiCR.DBMapping.Models.Mapping;

namespace BachMaiCR.DBMapping.Models
{
  public class BACHMAICRContext : DbContext
  {
    public DbSet<BachMaiCR.DBMapping.Models.ADMIN_LOG> ADMIN_LOG { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.ADMIN_MENU> ADMIN_MENU { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.ADMIN_USER> ADMIN_USER { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CALENDAR_AUTO> CALENDAR_AUTO { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE> CALENDAR_CHANGE { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CALENDAR_DATA> CALENDAR_DATA { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR> CALENDAR_DOCTOR { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CALENDAR_DUTY> CALENDAR_DUTY { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CALENDAR_GROUP> CALENDAR_GROUP { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.COLUM_LEVEL> COLUM_LEVEL { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_DIRECT> CONFIG_DIRECT { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS> CONFIG_HOLIDAYS { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_PARAMETES> CONFIG_PARAMETES { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_REPORT> CONFIG_REPORT { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_SMS> CONFIG_SMS { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_SMS_SEND> CONFIG_SMS_SEND { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE> CONFIG_TEMPLATE { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.DOCTOR_GROUPS> DOCTOR_GROUPS { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.DOCTOR_LEVEL> DOCTOR_LEVEL { get; set; }

    public DbSet<DOCTOR> DOCTORS { get; set; }

    public DbSet<FEAST> FEASTs { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.LM_CATEGORY> LM_CATEGORY { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.LM_DEPARTMENT> LM_DEPARTMENT { get; set; }

    public DbSet<REPORT> REPORTs { get; set; }

    public DbSet<sysdiagram> sysdiagrams { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.TEMPLATE_COLUM> TEMPLATE_COLUM { get; set; }

    public DbSet<TEMPLATE> TEMPLATES { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.USERS_ACTIONS> USERS_ACTIONS { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS> WEBPAGES_ACTIONS { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.WEBPAGES_FUNCTIONS> WEBPAGES_FUNCTIONS { get; set; }

    public DbSet<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES> WEBPAGES_ROLES { get; set; }

    public DbSet<DoctorCalendar> DoctorCalendars { get; set; }

    public DbSet<DoctorCalendarLeader> DoctorCalendarLeaders { get; set; }

    public DbSet<DoctorColumn> DoctorColumns { get; set; }

    public DbSet<DoctorData> DoctorDatas { get; set; }

    public DbSet<DocTorDate> DocTorDates { get; set; }

    public DbSet<DoctorHospital> DoctorHospitals { get; set; }

    public DbSet<DoctorLevelView> DoctorLevelViews { get; set; }

    public DbSet<TelephoneInDepartment> TelephoneInDepartments { get; set; }

    static BACHMAICRContext()
    {
      Database.SetInitializer<BACHMAICRContext>((IDatabaseInitializer<BACHMAICRContext>) null);
    }

    public BACHMAICRContext() : base("Name=BACHMAICRContext")
    {
    }

    protected virtual void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.ADMIN_LOG>(new ADMIN_LOGMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.ADMIN_MENU>(new ADMIN_MENUMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.ADMIN_USER>(new ADMIN_USERMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CALENDAR_AUTO>(new CALENDAR_AUTOMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE>(new CALENDAR_CHANGEMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CALENDAR_DATA>(new CALENDAR_DATAMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR>(new CALENDAR_DOCTORMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>(new CALENDAR_DUTYMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CALENDAR_GROUP>(new CALENDAR_GROUPMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.COLUM_LEVEL>(new COLUM_LEVELMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>(new CONFIG_DIRECTMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>(new CONFIG_HOLIDAYSMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_PARAMETES>(new CONFIG_PARAMETESMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_REPORT>(new CONFIG_REPORTMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_SMS>(new CONFIG_SMSMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_SMS_SEND>(new CONFIG_SMS_SENDMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE>(new CONFIG_TEMPLATEMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.DOCTOR_GROUPS>(new DOCTOR_GROUPSMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.DOCTOR_LEVEL>(new DOCTOR_LEVELMap());
      modelBuilder.Configurations.Add<DOCTOR>(new DOCTORMap());
      modelBuilder.Configurations.Add<FEAST>(new FEASTMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.LM_CATEGORY>(new LM_CATEGORYMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.LM_DEPARTMENT>(new LM_DEPARTMENTMap());
      modelBuilder.Configurations.Add<REPORT>(new REPORTMap());
      modelBuilder.Configurations.Add<sysdiagram>(new sysdiagramMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.TEMPLATE_COLUM>(new TEMPLATE_COLUMMap());
      modelBuilder.Configurations.Add<TEMPLATE>(new TEMPLATEMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.USERS_ACTIONS>(new USERS_ACTIONSMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS>(new WEBPAGES_ACTIONSMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.WEBPAGES_FUNCTIONS>(new WEBPAGES_FUNCTIONSMap());
      modelBuilder.Configurations.Add<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>(new WEBPAGES_ROLESMap());
      modelBuilder.Configurations.Add<DoctorCalendar>(new DoctorCalendarMap());
      modelBuilder.Configurations.Add<DoctorCalendarLeader>(new DoctorCalendarLeaderMap());
      modelBuilder.Configurations.Add<DoctorColumn>(new DoctorColumnMap());
      modelBuilder.Configurations.Add<DoctorData>(new DoctorDataMap());
      modelBuilder.Configurations.Add<DocTorDate>(new DocTorDateMap());
      modelBuilder.Configurations.Add<DoctorHospital>(new DoctorHospitalMap());
      modelBuilder.Configurations.Add<DoctorLevelView>(new DoctorLevelViewMap());
      modelBuilder.Configurations.Add<TelephoneInDepartment>(new TelephoneInDepartmentMap());
    }
  }
}
