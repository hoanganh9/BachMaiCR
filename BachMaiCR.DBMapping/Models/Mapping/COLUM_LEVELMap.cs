using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class COLUM_LEVELMap : EntityTypeConfiguration<COLUM_LEVEL>
  {
    public COLUM_LEVELMap()
    {
      this.HasKey(t => t.COLUM_LEVEL_ID);
      this.ToTable("COLUM_LEVEL");
      this.Property(t => t.COLUM_LEVEL_ID).HasColumnName("COLUM_LEVEL_ID");
      this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
      this.Property(t => t.DOCTOR_LEVEL_ID).HasColumnName("DOCTOR_LEVEL_ID");
      this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
      this.HasRequired(t => t.DOCTOR_LEVEL).WithMany(t => t.COLUM_LEVEL).HasForeignKey(d => d.DOCTOR_LEVEL_ID);
      this.HasRequired(t => t.TEMPLATE_COLUM).WithMany(t => t.COLUM_LEVEL).HasForeignKey(d => d.TEMPLATE_COLUM_ID);
    }
  }
}
