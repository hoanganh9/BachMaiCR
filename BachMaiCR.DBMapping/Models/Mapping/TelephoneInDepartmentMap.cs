using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class TelephoneInDepartmentMap : EntityTypeConfiguration<TelephoneInDepartment>
  {
    public TelephoneInDepartmentMap()
    {
      this.HasKey(t => new
      {
        LM_DEPARTMENT_ID = t.LM_DEPARTMENT_ID,
        CALENDAR_GROUP_ID = t.CALENDAR_GROUP_ID
      });
      this.Property(t => t.LM_DEPARTMENT_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.DEPARTMENT_NAME).HasMaxLength(150);
      this.Property(t => t.DEPARTMENT_CODE).HasMaxLength(50);
      this.Property(t => t.DEPARTMENT_ADDRESS).HasMaxLength(250);
      this.Property(t => t.DEPARTMENT_PHONE).HasMaxLength(20);
      this.Property(t => t.CALENDAR_GROUP_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.ToTable("TelephoneInDepartment");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.DEPARTMENT_NAME).HasColumnName("DEPARTMENT_NAME");
      this.Property(t => t.DEPARTMENT_CODE).HasColumnName("DEPARTMENT_CODE");
      this.Property(t => t.DEPARTMENT_ADDRESS).HasColumnName("DEPARTMENT_ADDRESS");
      this.Property(t => t.DEPARTMENT_PHONE).HasColumnName("DEPARTMENT_PHONE");
      this.Property(t => t.LEVELS).HasColumnName("LEVELS");
      this.Property(t => t.CALENDAR_GROUP_ID).HasColumnName("CALENDAR_GROUP_ID");
      this.Property(t => t.CALENDAR_MONTH).HasColumnName("CALENDAR_MONTH");
      this.Property(t => t.CALENDAR_YEAR).HasColumnName("CALENDAR_YEAR");
      this.Property(t => t.CALENDAR_PARENT_ID).HasColumnName("CALENDAR_PARENT_ID");
      this.Property(t => t.CALENDAR_ID).HasColumnName("CALENDAR_ID");
    }
  }
}
