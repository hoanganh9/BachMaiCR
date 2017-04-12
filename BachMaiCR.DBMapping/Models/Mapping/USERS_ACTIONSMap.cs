
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class USERS_ACTIONSMap : EntityTypeConfiguration<USERS_ACTIONS>
  {
    public USERS_ACTIONSMap() : base()
    {
      
      this.HasKey(t => new
      {
        ADMIN_USER_ID = t.ADMIN_USER_ID,
        WEBPAGES_ACTION_ID = t.WEBPAGES_ACTION_ID
      });
      this.Property(t => t.ADMIN_USER_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.WEBPAGES_ACTION_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.ToTable("USERS_ACTIONS");
      this.Property(t => t.ORDERS).HasColumnName("ORDERS");
      this.Property(t => t.IS_ACTIVE).HasColumnName("IS_ACTIVE");
      this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
      this.Property(t => t.UPDATE_DATE).HasColumnName("UPDATE_DATE");
      this.Property(t => t.ADMIN_USER_ID).HasColumnName("ADMIN_USER_ID");
      this.Property(t => t.WEBPAGES_ACTION_ID).HasColumnName("WEBPAGES_ACTION_ID");
      this.HasRequired(t => t.ADMIN_USER).WithMany(t => t.USERS_ACTIONS).HasForeignKey(d => d.ADMIN_USER_ID);
      this.HasRequired(t => t.WEBPAGES_ACTIONS).WithMany(t => t.USERS_ACTIONS).HasForeignKey(d => d.WEBPAGES_ACTION_ID);
    }
  }
}
