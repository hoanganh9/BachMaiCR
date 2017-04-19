using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class ADMIN_MENUMap : EntityTypeConfiguration<ADMIN_MENU>
  {
    public ADMIN_MENUMap()
    {
      this.HasKey(t => t.MENU_ID);
      this.Property(t => t.MENU_NAME).IsRequired().HasMaxLength(250);
      this.Property(t => t.MENU_IMAGE_CSS).HasMaxLength(250);
      this.Property(t => t.MENU_CODE).HasMaxLength(150);
      this.Property(t => t.DESCRIPTION).HasMaxLength(250);
      this.Property(t => t.ACTIONCODE).HasMaxLength(300);
      this.Property(t => t.MENU_CLASS).HasMaxLength(50);
      this.Property(t => t.MENU_URL).HasMaxLength(500);
      this.ToTable("ADMIN_MENU");
      this.Property(t => t.MENU_ID).HasColumnName("MENU_ID");
      this.Property(t => t.MENU_PARENT_ID).HasColumnName("MENU_PARENT_ID");
      this.Property(t => t.MENU_NAME).HasColumnName("MENU_NAME");
      this.Property(t => t.MENU_IMAGE_CSS).HasColumnName("MENU_IMAGE_CSS");
      this.Property(t => t.MENU_CODE).HasColumnName("MENU_CODE");
      this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
      this.Property(t => t.ACTIONCODE).HasColumnName("ACTIONCODE");
      this.Property(t => t.MENU_CLASS).HasColumnName("MENU_CLASS");
      this.Property(t => t.MENU_URL).HasColumnName("MENU_URL");
      this.Property(t => t.MENU_ORDER).HasColumnName("MENU_ORDER");
      this.Property(t => t.MENU_LEVEL).HasColumnName("MENU_LEVEL");
      this.Property(t => t.ISACTIVE).HasColumnName("ISACTIVE");
    }
  }
}
