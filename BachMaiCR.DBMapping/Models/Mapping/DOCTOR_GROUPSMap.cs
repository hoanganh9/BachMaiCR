
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DOCTOR_GROUPSMap : EntityTypeConfiguration<DOCTOR_GROUPS>
  {
    public DOCTOR_GROUPSMap() : base()
    {
      
      this.HasKey(t => t.DOCTOR_GROUP_ID);
      this.Property(t => t.DOCTOR_GROUP_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.DOCTOR_GROUP_NAME).IsRequired().HasMaxLength(250);
      this.Property(t => t.DESCRIPTION).HasMaxLength(250);
      this.ToTable("DOCTOR_GROUPS");
      this.Property(t => t.DOCTOR_GROUP_ID).HasColumnName("DOCTOR_GROUP_ID");
      this.Property(t => t.DOCTOR_GROUP_NAME).HasColumnName("DOCTOR_GROUP_NAME");
      this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
    }
  }
}
