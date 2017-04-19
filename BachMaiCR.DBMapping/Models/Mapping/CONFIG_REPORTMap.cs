using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CONFIG_REPORTMap : EntityTypeConfiguration<CONFIG_REPORT>
  {
    public CONFIG_REPORTMap()
    {
      this.HasKey(t => t.CONFIG_REPORT_ID);
      this.Property(t => t.CONFIG_REPORT_NAME).HasMaxLength(50);
      this.Property(t => t.CONFIG_REPORT_EXCEL).HasMaxLength(50);
      this.Property(t => t.FUNCTION1).HasMaxLength(50);
      this.Property(t => t.COMMAND1).HasMaxLength(50);
      this.Property(t => t.SIGNATURE1).HasMaxLength(50);
      this.Property(t => t.FUNCTION2).HasMaxLength(50);
      this.Property(t => t.COMMAND2).HasMaxLength(50);
      this.Property(t => t.SIGNATURE2).HasMaxLength(50);
      this.ToTable("CONFIG_REPORT");
      this.Property(t => t.CONFIG_REPORT_ID).HasColumnName("CONFIG_REPORT_ID");
      this.Property(t => t.CONFIG_REPORT_NAME).HasColumnName("CONFIG_REPORT_NAME");
      this.Property(t => t.CONFIG_REPORT_EXCEL).HasColumnName("CONFIG_REPORT_EXCEL");
      this.Property(t => t.FUNCTION1).HasColumnName("FUNCTION1");
      this.Property(t => t.COMMAND1).HasColumnName("COMMAND1");
      this.Property(t => t.SIGNATURE1).HasColumnName("SIGNATURE1");
      this.Property(t => t.FUNCTION2).HasColumnName("FUNCTION2");
      this.Property(t => t.COMMAND2).HasColumnName("COMMAND2");
      this.Property(t => t.SIGNATURE2).HasColumnName("SIGNATURE2");
    }
  }
}
