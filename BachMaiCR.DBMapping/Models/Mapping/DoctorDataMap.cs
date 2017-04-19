using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DoctorDataMap : EntityTypeConfiguration<DoctorData>
  {
    public DoctorDataMap()
    {
      this.HasKey(t => new
      {
        CALENDAR_DOCTOR_ID = t.CALENDAR_DOCTOR_ID,
        CALENDAR_DATA_ID = t.CALENDAR_DATA_ID,
        DOCTORS_ID = t.DOCTORS_ID,
        CALENDAR_DUTY_ID = t.CALENDAR_DUTY_ID
      });
      this.Property(t => t.CALENDAR_DOCTOR_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.CALENDAR_DATA_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.CALENDAR_DUTY_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.ToTable("DoctorData");
      this.Property(t => t.CALENDAR_DOCTOR_ID).HasColumnName("CALENDAR_DOCTOR_ID");
      this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
      this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
      this.Property(t => t.DATE_START).HasColumnName("DATE_START");
      this.Property(t => t.DATE_END).HasColumnName("DATE_END");
    }
  }
}
