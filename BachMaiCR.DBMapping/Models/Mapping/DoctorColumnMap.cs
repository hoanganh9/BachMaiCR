using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DoctorColumnMap : EntityTypeConfiguration<DoctorColumn>
  {
    public DoctorColumnMap()
    {
      this.HasKey(t => new
      {
        DOCTORS_ID = t.DOCTORS_ID,
        TEMPLATE_COLUM_ID = t.TEMPLATE_COLUM_ID
      });
      this.Property(t => t.COLUM_NAME).HasMaxLength(50);
      this.Property(t => t.LEVEL_NAME).HasMaxLength(250);
      this.Property(t => t.DOCTOR_NAME).HasMaxLength(100);
      this.Property(t => t.TEMPLATE_NAME).HasMaxLength(300);
      this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.TEMPLATE_COLUM_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.ToTable("DoctorColumn");
      this.Property(t => t.COLUM_NAME).HasColumnName("COLUM_NAME");
      this.Property(t => t.LEVEL_NAME).HasColumnName("LEVEL_NAME");
      this.Property(t => t.DOCTOR_NAME).HasColumnName("DOCTOR_NAME");
      this.Property(t => t.TEMPLATE_NAME).HasColumnName("TEMPLATE_NAME");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
    }
  }
}
