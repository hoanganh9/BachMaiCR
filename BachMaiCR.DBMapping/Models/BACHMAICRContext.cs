using BachMaiCR.DBMapping.Models.Mapping;
using System.Data.Entity;

namespace BachMaiCR.DBMapping.Models
{
    public class BACHMAICRContext : DbContext
    {
        public DbSet<ADMIN_LOG> ADMIN_LOG { get; set; }

        public DbSet<ADMIN_MENU> ADMIN_MENU { get; set; }

        public DbSet<ADMIN_USER> ADMIN_USER { get; set; }

        public DbSet<CALENDAR_AUTO> CALENDAR_AUTO { get; set; }

        public DbSet<CALENDAR_CHANGE> CALENDAR_CHANGE { get; set; }

        public DbSet<CALENDAR_DATA> CALENDAR_DATA { get; set; }

        public DbSet<CALENDAR_DOCTOR> CALENDAR_DOCTOR { get; set; }

        public DbSet<CALENDAR_DUTY> CALENDAR_DUTY { get; set; }

        public DbSet<CALENDAR_GROUP> CALENDAR_GROUP { get; set; }

        public DbSet<COLUM_LEVEL> COLUM_LEVEL { get; set; }

        public DbSet<CONFIG_DIRECT> CONFIG_DIRECT { get; set; }

        public DbSet<CONFIG_HOLIDAYS> CONFIG_HOLIDAYS { get; set; }

        public DbSet<CONFIG_PARAMETES> CONFIG_PARAMETES { get; set; }

        public DbSet<CONFIG_REPORT> CONFIG_REPORT { get; set; }

        public DbSet<CONFIG_SMS> CONFIG_SMS { get; set; }

        public DbSet<CONFIG_SMS_SEND> CONFIG_SMS_SEND { get; set; }

        public DbSet<CONFIG_TEMPLATE> CONFIG_TEMPLATE { get; set; }

        public DbSet<DOCTOR_GROUPS> DOCTOR_GROUPS { get; set; }

        public DbSet<DOCTOR_LEVEL> DOCTOR_LEVEL { get; set; }

        public DbSet<DOCTOR> DOCTORS { get; set; }

        public DbSet<FEAST> FEASTs { get; set; }

        public DbSet<LM_CATEGORY> LM_CATEGORY { get; set; }

        public DbSet<LM_DEPARTMENT> LM_DEPARTMENT { get; set; }

        public DbSet<REPORT> REPORTs { get; set; }

        public DbSet<sysdiagram> sysdiagrams { get; set; }

        public DbSet<TEMPLATE_COLUM> TEMPLATE_COLUM { get; set; }

        public DbSet<TEMPLATE> TEMPLATES { get; set; }

        public DbSet<USERS_ACTIONS> USERS_ACTIONS { get; set; }

        public DbSet<WEBPAGES_ACTIONS> WEBPAGES_ACTIONS { get; set; }

        public DbSet<WEBPAGES_FUNCTIONS> WEBPAGES_FUNCTIONS { get; set; }

        public DbSet<WEBPAGES_ROLES> WEBPAGES_ROLES { get; set; }

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
            Database.SetInitializer<BACHMAICRContext>((IDatabaseInitializer<BACHMAICRContext>)null);
        }

        public BACHMAICRContext()
          : base("Name=BACHMAICRContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add<ADMIN_LOG>(new ADMIN_LOGMap());
            modelBuilder.Configurations.Add<ADMIN_MENU>(new ADMIN_MENUMap());
            modelBuilder.Configurations.Add<ADMIN_USER>(new ADMIN_USERMap());
            modelBuilder.Configurations.Add<CALENDAR_AUTO>(new CALENDAR_AUTOMap());
            modelBuilder.Configurations.Add<CALENDAR_CHANGE>(new CALENDAR_CHANGEMap());
            modelBuilder.Configurations.Add<CALENDAR_DATA>(new CALENDAR_DATAMap());
            modelBuilder.Configurations.Add<CALENDAR_DOCTOR>(new CALENDAR_DOCTORMap());
            modelBuilder.Configurations.Add<CALENDAR_DUTY>(new CALENDAR_DUTYMap());
            modelBuilder.Configurations.Add<CALENDAR_GROUP>(new CALENDAR_GROUPMap());
            modelBuilder.Configurations.Add<COLUM_LEVEL>(new COLUM_LEVELMap());
            modelBuilder.Configurations.Add<CONFIG_DIRECT>(new CONFIG_DIRECTMap());
            modelBuilder.Configurations.Add<CONFIG_HOLIDAYS>(new CONFIG_HOLIDAYSMap());
            modelBuilder.Configurations.Add<CONFIG_PARAMETES>(new CONFIG_PARAMETESMap());
            modelBuilder.Configurations.Add<CONFIG_REPORT>(new CONFIG_REPORTMap());
            modelBuilder.Configurations.Add<CONFIG_SMS>(new CONFIG_SMSMap());
            modelBuilder.Configurations.Add<CONFIG_SMS_SEND>(new CONFIG_SMS_SENDMap());
            modelBuilder.Configurations.Add<CONFIG_TEMPLATE>(new CONFIG_TEMPLATEMap());
            modelBuilder.Configurations.Add<DOCTOR_GROUPS>(new DOCTOR_GROUPSMap());
            modelBuilder.Configurations.Add<DOCTOR_LEVEL>(new DOCTOR_LEVELMap());
            modelBuilder.Configurations.Add<DOCTOR>(new DOCTORMap());
            modelBuilder.Configurations.Add<FEAST>(new FEASTMap());
            modelBuilder.Configurations.Add<LM_CATEGORY>(new LM_CATEGORYMap());
            modelBuilder.Configurations.Add<LM_DEPARTMENT>(new LM_DEPARTMENTMap());
            modelBuilder.Configurations.Add<REPORT>(new REPORTMap());
            modelBuilder.Configurations.Add<sysdiagram>(new sysdiagramMap());
            modelBuilder.Configurations.Add<TEMPLATE_COLUM>(new TEMPLATE_COLUMMap());
            modelBuilder.Configurations.Add<TEMPLATE>(new TEMPLATEMap());
            modelBuilder.Configurations.Add<USERS_ACTIONS>(new USERS_ACTIONSMap());
            modelBuilder.Configurations.Add<WEBPAGES_ACTIONS>(new WEBPAGES_ACTIONSMap());
            modelBuilder.Configurations.Add<WEBPAGES_FUNCTIONS>(new WEBPAGES_FUNCTIONSMap());
            modelBuilder.Configurations.Add<WEBPAGES_ROLES>(new WEBPAGES_ROLESMap());
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