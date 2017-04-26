using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class DoctorCalendarMap : EntityTypeConfiguration<DoctorCalendar>
    {
        public DoctorCalendarMap()
        {
            this.HasKey(t => new
            {
                CALENDAR_DUTY_ID = t.CALENDAR_DUTY_ID,
                TEMPLATE_COLUM_ID = t.TEMPLATE_COLUM_ID,
                DOCTORS_ID = t.DOCTORS_ID,
                CALENDAR_DATA_ID = t.CALENDAR_DATA_ID
            });
            this.Property(t => t.DOCTOR_NAME).HasMaxLength(100);
            this.Property(t => t.CALENDAR_DUTY_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.TEMPLATE_COLUM_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.CALENDAR_DATA_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.ABBREVIATION).HasMaxLength(25);
            this.ToTable("DoctorCalendar");
            this.Property(t => t.DOCTOR_NAME).HasColumnName("DOCTOR_NAME");
            this.Property(t => t.DATE_START).HasColumnName("DATE_START");
            this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
            this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
            this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
            this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
            this.Property(t => t.ABBREVIATION).HasColumnName("ABBREVIATION");
        }
    }
}