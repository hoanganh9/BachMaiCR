
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DocTorDateMap : EntityTypeConfiguration<DocTorDate>
  {
    public DocTorDateMap() : base()
    {
      
      this.HasKey(t => new
      {
        TEMPLATE_COLUM_ID = t.TEMPLATE_COLUM_ID,
        CALENDAR_DUTY_ID = t.CALENDAR_DUTY_ID,
        DOCTORS_ID = t.DOCTORS_ID,
        CALENDAR_DATA_ID = t.CALENDAR_DATA_ID
      });
      this.Property(t => t.TEMPLATE_COLUM_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.COLUM_NAME).HasMaxLength(50);
      this.Property(t => t.CALENDAR_DUTY_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.DOCTORS_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.Property(t => t.CALENDAR_DATA_ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      this.ToTable("DocTorDates");
      this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
      this.Property(t => t.COLUM_NAME).HasColumnName("COLUM_NAME");
      this.Property(t => t.DATE_START).HasColumnName("DATE_START");
      this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
    }
  }
}
