using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class DoctorHospitalMap : EntityTypeConfiguration<DoctorHospital>
    {
        public DoctorHospitalMap()
        {
            this.HasKey(t => new
            {
                CALENDAR_DUTY_ID = t.CALENDAR_DUTY_ID,
                DOCTORS_ID = t.DOCTORS_ID,
                ISAPPROVED = t.ISAPPROVED,
                LM_DEPARTMENT_ID = t.LM_DEPARTMENT_ID,
                CALENDAR_DATA_ID = t.CALENDAR_DATA_ID
            });
            this.Property(t => t.CALENDAR_DUTY_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.DOCTOR_NAME).HasMaxLength(100);
            this.Property(t => t.ABBREVIATION).HasMaxLength(25);
            this.Property(t => t.PHONE).HasMaxLength(15);
            this.Property(t => t.EMAIL).HasMaxLength(60);
            this.Property(t => t.ISAPPROVED).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.LM_DEPARTMENT_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.CALENDAR_DATA_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.LM_DEPARTMENT_IDs).HasMaxLength(250);
            this.ToTable("DoctorHospital");
            this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
            this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
            this.Property(t => t.DOCTOR_NAME).HasColumnName("DOCTOR_NAME");
            this.Property(t => t.ABBREVIATION).HasColumnName("ABBREVIATION");
            this.Property(t => t.PHONE).HasColumnName("PHONE");
            this.Property(t => t.EMAIL).HasColumnName("EMAIL");
            this.Property(t => t.LEVEL_NUMBER).HasColumnName("LEVEL_NUMBER");
            this.Property(t => t.CALENDAR_MONTH).HasColumnName("CALENDAR_MONTH");
            this.Property(t => t.CALENDAR_YEAR).HasColumnName("CALENDAR_YEAR");
            this.Property(t => t.DATE_START).HasColumnName("DATE_START");
            this.Property(t => t.ISAPPROVED).HasColumnName("ISAPPROVED");
            this.Property(t => t.CALENDAR_STATUS).HasColumnName("CALENDAR_STATUS");
            this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
            this.Property(t => t.DUTY_TYPE).HasColumnName("DUTY_TYPE");
            this.Property(t => t.LM_DEPARTMENT_IDs).HasColumnName("LM_DEPARTMENT_IDs");
        }
    }
}