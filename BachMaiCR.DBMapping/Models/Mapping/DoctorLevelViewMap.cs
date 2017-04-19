using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DoctorLevelViewMap : EntityTypeConfiguration<DoctorLevelView>
  {
    public DoctorLevelViewMap()
    {
      this.HasKey(t => t.DOCTORS_ID);
      this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.LEVEL_NAME).HasMaxLength(250);
      this.ToTable("DoctorLevelView");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.LEVEL_NAME).HasColumnName("LEVEL_NAME");
      this.Property(t => t.LEVEL_NUMBER).HasColumnName("LEVEL_NUMBER");
    }
  }
}
