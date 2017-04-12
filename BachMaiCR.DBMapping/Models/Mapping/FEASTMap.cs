
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class FEASTMap : EntityTypeConfiguration<FEAST>
  {
    public FEASTMap() : base()
    {
      
      this.HasKey(t => t.FEAST_ID);
      this.Property(t => t.FEAST_TITLE).IsRequired().HasMaxLength(150);
      this.ToTable("FEAST");
      this.Property(t => t.FEAST_ID).HasColumnName("FEAST_ID");
      this.Property(t => t.FEAST_TITLE).HasColumnName("FEAST_TITLE");
      this.Property(t => t.DATE_BEGIN).HasColumnName("DATE_BEGIN");
      this.Property(t => t.DATE_END).HasColumnName("DATE_END");
      this.Property(t => t.FEAST_YEAR).HasColumnName("FEAST_YEAR");
      this.Property(t => t.FEAST_TYPE).HasColumnName("FEAST_TYPE");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
      this.Property(t => t.ISACTIVED).HasColumnName("ISACTIVED");
    }
  }
}
