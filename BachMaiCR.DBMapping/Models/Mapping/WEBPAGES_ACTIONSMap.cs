using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class WEBPAGES_ACTIONSMap : EntityTypeConfiguration<WEBPAGES_ACTIONS>
    {
        public WEBPAGES_ACTIONSMap()
        {
            this.HasKey(t => t.WEBPAGES_ACTION_ID);
            this.Property(t => t.DESCRIPTION).HasMaxLength(500);
            this.Property(t => t.MENU_NAME).HasMaxLength(200);
            this.Property(t => t.CODE).HasMaxLength(50);
            this.Property(t => t.GROUP_CODE).HasMaxLength(200);
            this.Property(t => t.GROUP_NAME).HasMaxLength(200);
            this.Property(t => t.GROUP_CLASSCSS).HasMaxLength(100);
            this.Property(t => t.ACTION_CLASSCSS).HasMaxLength(100);
            this.Property(t => t.GROUP_CHILD_CODE).HasMaxLength(200);
            this.Property(t => t.GROUP_CHILD_NAME).HasMaxLength(200);
            this.ToTable("WEBPAGES_ACTIONS");
            this.Property(t => t.WEBPAGES_ACTION_ID).HasColumnName("WEBPAGES_ACTION_ID");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
            this.Property(t => t.MENU_NAME).HasColumnName("MENU_NAME");
            this.Property(t => t.CODE).HasColumnName("CODE");
            this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
            this.Property(t => t.UPDATE_DATE).HasColumnName("UPDATE_DATE");
            this.Property(t => t.IS_ACTIVE).HasColumnName("IS_ACTIVE");
            this.Property(t => t.GROUP_ORDER).HasColumnName("GROUP_ORDER");
            this.Property(t => t.MENU_ORDER).HasColumnName("MENU_ORDER");
            this.Property(t => t.GROUP_CODE).HasColumnName("GROUP_CODE");
            this.Property(t => t.GROUP_NAME).HasColumnName("GROUP_NAME");
            this.Property(t => t.IS_MENU).HasColumnName("IS_MENU");
            this.Property(t => t.MANUAL_EDITED).HasColumnName("MANUAL_EDITED");
            this.Property(t => t.GROUP_CLASSCSS).HasColumnName("GROUP_CLASSCSS");
            this.Property(t => t.ACTION_CLASSCSS).HasColumnName("ACTION_CLASSCSS");
            this.Property(t => t.GROUP_CHILD_CODE).HasColumnName("GROUP_CHILD_CODE");
            this.Property(t => t.GROUP_CHILD_NAME).HasColumnName("GROUP_CHILD_NAME");
            this.HasMany<WEBPAGES_FUNCTIONS>(t => t.WEBPAGES_FUNCTIONS).WithMany(t => t.WEBPAGES_ACTIONS).Map((Action<ManyToManyAssociationMappingConfiguration>)(m =>
           {
               m.ToTable("FUNCTIONS_ACTIONS");
               m.MapLeftKey("WEBPAGES_ACTION_ID");
               m.MapRightKey("WEBPAGES_FUNCTIONS_ID");
           }));
        }
    }
}