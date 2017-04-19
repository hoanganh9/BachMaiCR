using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DOCTOR_LEVELMap : EntityTypeConfiguration<DOCTOR_LEVEL>
  {
    public DOCTOR_LEVELMap()
    {
      this.HasKey(t => t.DOCTOR_LEVEL_ID);
      this.Property(t => t.CODE).IsRequired().HasMaxLength(50);
      this.Property(t => t.LEVEL_NAME).HasMaxLength(250);
      this.ToTable("DOCTOR_LEVEL");
      this.Property(t => t.DOCTOR_LEVEL_ID).HasColumnName("DOCTOR_LEVEL_ID");
      this.Property(t => t.CODE).HasColumnName("CODE");
      this.Property(t => t.LEVEL_NAME).HasColumnName("LEVEL_NAME");
      this.Property(t => t.LEVEL_NUMBER).HasColumnName("LEVEL_NUMBER");
      this.Property(t => t.LEVEL_ORDER).HasColumnName("LEVEL_ORDER");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
    }
  }
}
