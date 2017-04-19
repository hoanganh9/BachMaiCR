using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DoctorCalendarLeaderMap : EntityTypeConfiguration<DoctorCalendarLeader>
  {
    public DoctorCalendarLeaderMap()
    {
      this.HasKey(t => new
      {
        CALENDAR_DUTY_ID = t.CALENDAR_DUTY_ID,
        DOCTORS_ID = t.DOCTORS_ID,
        CALENDAR_DATA_ID = t.CALENDAR_DATA_ID,
        LM_DEPARTMENT_ID = t.LM_DEPARTMENT_ID,
        DOCTOR_GROUP_ID = t.DOCTOR_GROUP_ID,
        CALENDAR_DOCTOR_ID = t.CALENDAR_DOCTOR_ID
      });
      this.Property(t => t.DOCTOR_NAME).HasMaxLength(100);
      this.Property(t => t.CALENDAR_DUTY_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.CALENDAR_NAME).HasMaxLength(200);
      this.Property(t => t.CALENDAR_CONTENT).HasMaxLength(500);
      this.Property(t => t.CALENDAR_PHONE).HasMaxLength(15);
      this.Property(t => t.CALENDAR_DATA_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.LM_DEPARTMENT_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.POSITION_IDs).HasMaxLength(250);
      this.Property(t => t.CALENDAR_DUTY_NAME).HasMaxLength(500);
      this.Property(t => t.POSITION_NAMEs).HasMaxLength(500);
      this.Property(t => t.ABBREVIATION).HasMaxLength(25);
      this.Property(t => t.DOCTOR_GROUP_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.CALENDAR_DOCTOR_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.PHONE).HasMaxLength(15);
      this.Property(t => t.LM_DEPARTMENT_IDs).HasMaxLength(250);
      this.Property(t => t.LM_DEPARTMENT_NAMEs).HasMaxLength(500);
      this.ToTable("DoctorCalendarLeader");
      this.Property(t => t.DOCTOR_NAME).HasColumnName("DOCTOR_NAME");
      this.Property(t => t.DATE_START).HasColumnName("DATE_START");
      this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.CALENDAR_NAME).HasColumnName("CALENDAR_NAME");
      this.Property(t => t.CALENDAR_CONTENT).HasColumnName("CALENDAR_CONTENT");
      this.Property(t => t.CALENDAR_PHONE).HasColumnName("CALENDAR_PHONE");
      this.Property(t => t.DATE_END).HasColumnName("DATE_END");
      this.Property(t => t.CALENDAR_MONTH).HasColumnName("CALENDAR_MONTH");
      this.Property(t => t.CALENDAR_YEAR).HasColumnName("CALENDAR_YEAR");
      this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
      this.Property(t => t.DUTY_TYPE).HasColumnName("DUTY_TYPE");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.POSITION_IDs).HasColumnName("POSITION_IDs");
      this.Property(t => t.CALENDAR_DUTY_NAME).HasColumnName("CALENDAR_DUTY_NAME");
      this.Property(t => t.POSITION_NAMEs).HasColumnName("POSITION_NAMEs");
      this.Property(t => t.ABBREVIATION).HasColumnName("ABBREVIATION");
      this.Property(t => t.DOCTOR_GROUP_ID).HasColumnName("DOCTOR_GROUP_ID");
      this.Property(t => t.CALENDAR_DOCTOR_ID).HasColumnName("CALENDAR_DOCTOR_ID");
      this.Property(t => t.CALENDAR_STATUS).HasColumnName("CALENDAR_STATUS");
      this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
      this.Property(t => t.PHONE).HasColumnName("PHONE");
      this.Property(t => t.LM_DEPARTMENT_IDs).HasColumnName("LM_DEPARTMENT_IDs");
      this.Property(t => t.LM_DEPARTMENT_NAMEs).HasColumnName("LM_DEPARTMENT_NAMEs");
    }
  }
}
