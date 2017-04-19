using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class LM_CATEGORYMap : EntityTypeConfiguration<LM_CATEGORY>
  {
    public LM_CATEGORYMap()
    {
      this.HasKey(t => t.LM_CATEGORY_ID);
      this.Property(t => t.CATEGORY_NAME).HasMaxLength(500);
      this.Property(t => t.CATEGORY_DESCRIPTION).HasMaxLength(500);
      this.Property(t => t.CODE).HasMaxLength(50);
      this.Property(t => t.NOTES).HasMaxLength(500);
      this.ToTable("LM_CATEGORY");
      this.Property(t => t.LM_CATEGORY_ID).HasColumnName("LM_CATEGORY_ID");
      this.Property(t => t.CATEGORY_NAME).HasColumnName("CATEGORY_NAME");
      this.Property(t => t.CATEGORY_DESCRIPTION).HasColumnName("CATEGORY_DESCRIPTION");
      this.Property(t => t.CODE).HasColumnName("CODE");
      this.Property(t => t.CATEGORY_ORDER).HasColumnName("CATEGORY_ORDER");
      this.Property(t => t.CATEGORY_STYLE).HasColumnName("CATEGORY_STYLE");
      this.Property(t => t.CATEGORY_PARENT).HasColumnName("CATEGORY_PARENT");
      this.Property(t => t.CATEGORY_STATUS).HasColumnName("CATEGORY_STATUS");
      this.Property(t => t.NOTES).HasColumnName("NOTES");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
    }
  }
}
