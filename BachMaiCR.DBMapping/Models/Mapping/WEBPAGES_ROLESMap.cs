using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class WEBPAGES_ROLESMap : EntityTypeConfiguration<WEBPAGES_ROLES>
    {
        public WEBPAGES_ROLESMap()
        {
            this.HasKey(t => t.WEBPAGES_ROLE_ID);
            this.Property(t => t.ROLE_NAME).HasMaxLength(256);
            this.Property(t => t.ABBREVIATION).HasMaxLength(50);
            this.Property(t => t.DESCRIPTION).HasMaxLength(500);
            this.Property((Expression<Func<WEBPAGES_ROLES, byte[]>>)(t => t.UPDATE_DATE)).IsFixedLength().HasMaxLength(8).IsRowVersion();
            this.Property(t => t.ROLE_CODE).HasMaxLength(50);
            this.ToTable("WEBPAGES_ROLES");
            this.Property(t => t.WEBPAGES_ROLE_ID).HasColumnName("WEBPAGES_ROLE_ID");
            this.Property(t => t.ROLE_NAME).HasColumnName("ROLE_NAME");
            this.Property(t => t.ABBREVIATION).HasColumnName("ABBREVIATION");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
            this.Property(t => t.ISACTIVE).HasColumnName("ISACTIVE");
            this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
            this.Property((Expression<Func<WEBPAGES_ROLES, byte[]>>)(t => t.UPDATE_DATE)).HasColumnName("UPDATE_DATE");
            this.Property(t => t.ROLE_CODE).HasColumnName("ROLE_CODE");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.CREATE_USERID).HasColumnName("CREATE_USERID");
            this.HasMany<WEBPAGES_ACTIONS>(t => t.WEBPAGES_ACTIONS).WithMany(t => t.WEBPAGES_ROLES).Map((Action<ManyToManyAssociationMappingConfiguration>)(m =>
           {
               m.ToTable("ACTIONS_ROLES");
               m.MapLeftKey("WEBPAGES_ROLES_ID");
               m.MapRightKey("WEBPAGES_ACTION_ID");
           }));
            this.HasMany<ADMIN_USER>(t => t.ADMIN_USER).WithMany(t => t.WEBPAGES_ROLES).Map((Action<ManyToManyAssociationMappingConfiguration>)(m =>
           {
               m.ToTable("WEBPAGES_USERSINROLES");
               m.MapLeftKey("WEBPAGES_ROLES_ID");
               m.MapRightKey("ADMIN_USER_ID");
           }));
            this.HasOptional(t => t.LM_DEPARTMENT).WithMany(t => t.WEBPAGES_ROLES).HasForeignKey(d => d.LM_DEPARTMENT_ID);
        }
    }
}