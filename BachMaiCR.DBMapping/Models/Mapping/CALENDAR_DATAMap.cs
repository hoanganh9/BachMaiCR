using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class CALENDAR_DATAMap : EntityTypeConfiguration<CALENDAR_DATA>
    {
        public CALENDAR_DATAMap()
        {
            this.HasKey(t => t.CALENDAR_DATA_ID);
            this.Property(t => t.CALENDAR_NAME).HasMaxLength(200);
            this.Property(t => t.CALENDAR_CONTENT).HasMaxLength(500);
            this.Property(t => t.CALENDAR_PHONE).HasMaxLength(15);
            this.Property(t => t.CALENDAR_NOTE).HasMaxLength(500);
            this.ToTable("CALENDAR_DATA");
            this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
            this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
            this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
            this.Property(t => t.CALENDAR_NAME).HasColumnName("CALENDAR_NAME");
            this.Property(t => t.CALENDAR_CONTENT).HasColumnName("CALENDAR_CONTENT");
            this.Property(t => t.CALENDAR_PHONE).HasColumnName("CALENDAR_PHONE");
            this.Property(t => t.CALENDAR_NOTE).HasColumnName("CALENDAR_NOTE");
            this.Property(t => t.DATE_START).HasColumnName("DATE_START");
            this.Property(t => t.DATE_END).HasColumnName("DATE_END");
            this.Property(t => t.CHANGE_STATUS).HasColumnName("CHANGE_STATUS");
            this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
            this.Property(t => t.CHIEF_VIEW).HasColumnName("CHIEF_VIEW");
            this.Property(t => t.CHIEF_ID).HasColumnName("CHIEF_ID");
            this.HasRequired(t => t.CALENDAR_DUTY).WithMany(t => t.CALENDAR_DATA).HasForeignKey(d => d.CALENDAR_DUTY_ID);
            this.HasOptional(t => t.TEMPLATE_COLUM).WithMany(t => t.CALENDAR_DATA).HasForeignKey(d => d.TEMPLATE_COLUM_ID);
        }
    }
}