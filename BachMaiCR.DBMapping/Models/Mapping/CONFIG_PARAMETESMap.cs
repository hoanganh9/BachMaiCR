using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CONFIG_PARAMETESMap : EntityTypeConfiguration<CONFIG_PARAMETES>
  {
    public CONFIG_PARAMETESMap()
    {
      this.HasKey(t => t.CONFIG_PARAMETES_ID);
      this.Property(t => t.DESCRIPTION).HasMaxLength(250);
      this.ToTable("CONFIG_PARAMETES");
      this.Property(t => t.CONFIG_PARAMETES_ID).HasColumnName("CONFIG_PARAMETES_ID");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.TIME_DISTANCE).HasColumnName("TIME_DISTANCE");
      this.Property(t => t.IS_FIX_WEEKEND).HasColumnName("IS_FIX_WEEKEND");
      this.Property(t => t.TIME_DISTANCE_OF_HOLIDAY).HasColumnName("TIME_DISTANCE_OF_HOLIDAY");
      this.Property(t => t.NUMBER_DOCTOR_IN_DAY).HasColumnName("NUMBER_DOCTOR_IN_DAY");
      this.Property(t => t.NORMS_DIRECT).HasColumnName("NORMS_DIRECT");
      this.Property(t => t.IS_FEMALE_DIRECT_AM).HasColumnName("IS_FEMALE_DIRECT_AM");
      this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
      this.Property(t => t.CONFIG_YEAR).HasColumnName("CONFIG_YEAR");
      this.Property(t => t.CONFIG_TYPE).HasColumnName("CONFIG_TYPE");
      this.Property(t => t.USER_CREATE_ID).HasColumnName("USER_CREATE_ID");
      this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
    }
  }
}
