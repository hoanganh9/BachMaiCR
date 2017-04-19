using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CALENDAR_GROUPMap : EntityTypeConfiguration<CALENDAR_GROUP>
  {
    public CALENDAR_GROUPMap()
    {
      this.HasKey(t => t.CALENDAR_GROUP_ID);
      this.ToTable("CALENDAR_GROUP");
      this.Property(t => t.CALENDAR_GROUP_ID).HasColumnName("CALENDAR_GROUP_ID");
      this.Property(t => t.CALENDAR_ID).HasColumnName("CALENDAR_ID");
      this.Property(t => t.CALENDAR_PARENT_ID).HasColumnName("CALENDAR_PARENT_ID");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.CALENDAR_MONTH).HasColumnName("CALENDAR_MONTH");
      this.Property(t => t.CALENDAR_YEAR).HasColumnName("CALENDAR_YEAR");
      this.Property(t => t.CALENDAR_TYPE).HasColumnName("CALENDAR_TYPE");
      this.Property(t => t.CALENDAR_STATUS).HasColumnName("CALENDAR_STATUS");
    }
  }
}
