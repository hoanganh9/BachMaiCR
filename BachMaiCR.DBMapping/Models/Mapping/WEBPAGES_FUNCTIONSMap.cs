using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class WEBPAGES_FUNCTIONSMap : EntityTypeConfiguration<WEBPAGES_FUNCTIONS>
  {
    public WEBPAGES_FUNCTIONSMap()
    {
      this.HasKey(t => t.WEBPAGES_FUNCTIONS_ID);
      this.Property(t => t.UNIQUE_CODE).HasMaxLength(100);
      this.Property(t => t.ACTION_NAME).HasMaxLength(100);
      this.Property(t => t.CONTROLLER).HasMaxLength(100);
      this.Property(t => t.ACTION_TYPE).HasMaxLength(10);
      this.ToTable("WEBPAGES_FUNCTIONS");
      this.Property(t => t.WEBPAGES_FUNCTIONS_ID).HasColumnName("WEBPAGES_FUNCTIONS_ID");
      this.Property(t => t.UNIQUE_CODE).HasColumnName("UNIQUE_CODE");
      this.Property(t => t.ACTION_NAME).HasColumnName("ACTION_NAME");
      this.Property(t => t.CONTROLLER).HasColumnName("CONTROLLER");
      this.Property(t => t.ACTION_TYPE).HasColumnName("ACTION_TYPE");
      this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
      this.Property(t => t.UPDATE_DATE).HasColumnName("UPDATE_DATE");
    }
  }
}
