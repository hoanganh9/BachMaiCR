using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CONFIG_DIRECTMap : EntityTypeConfiguration<CONFIG_DIRECT>
  {
    public CONFIG_DIRECTMap()
    {
      this.HasKey(t => t.CONFIG_DIRECT_ID);
      this.ToTable("CONFIG_DIRECT");
      this.Property(t => t.CONFIG_DIRECT_ID).HasColumnName("CONFIG_DIRECT_ID");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.DATE_BEGIN).HasColumnName("DATE_BEGIN");
      this.Property(t => t.DATE_END).HasColumnName("DATE_END");
      this.Property(t => t.FEAST_ID).HasColumnName("FEAST_ID");
      this.Property(t => t.USER_CREATE_ID).HasColumnName("USER_CREATE_ID");
      this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
      this.HasRequired(t => t.DOCTOR).WithMany(t => t.CONFIG_DIRECT).HasForeignKey(d => d.DOCTORS_ID);
      this.HasOptional(t => t.FEAST).WithMany(t => t.CONFIG_DIRECT).HasForeignKey(d => d.FEAST_ID);
      this.HasRequired(t => t.LM_DEPARTMENT).WithMany(t => t.CONFIG_DIRECT).HasForeignKey(d => d.LM_DEPARTMENT_ID);
    }
  }
}
