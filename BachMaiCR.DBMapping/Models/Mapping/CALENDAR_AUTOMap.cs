using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CALENDAR_AUTOMap : EntityTypeConfiguration<CALENDAR_AUTO>
  {
    public CALENDAR_AUTOMap()
    {
      this.HasKey(t => t.CALENDAR_AUTO_ID);
      this.ToTable("CALENDAR_AUTO");
      this.Property(t => t.CALENDAR_AUTO_ID).HasColumnName("CALENDAR_AUTO_ID");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
      this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
    }
  }
}
