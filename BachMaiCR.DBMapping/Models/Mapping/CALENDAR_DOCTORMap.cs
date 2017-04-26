using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class CALENDAR_DOCTORMap : EntityTypeConfiguration<CALENDAR_DOCTOR>
    {
        public CALENDAR_DOCTORMap()
        {
            this.HasKey(t => t.CALENDAR_DOCTOR_ID);
            this.ToTable("CALENDAR_DOCTOR");
            this.Property(t => t.CALENDAR_DOCTOR_ID).HasColumnName("CALENDAR_DOCTOR_ID");
            this.Property(t => t.CALENDAR_DATA_ID).HasColumnName("CALENDAR_DATA_ID");
            this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
            this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
            this.Property(t => t.COLUMN_LEVEL_ID).HasColumnName("COLUMN_LEVEL_ID");
            this.HasRequired(t => t.CALENDAR_DATA).WithMany(t => t.CALENDAR_DOCTOR).HasForeignKey(d => d.CALENDAR_DATA_ID);
            this.HasRequired(t => t.DOCTOR).WithMany(t => t.CALENDAR_DOCTOR).HasForeignKey(d => d.DOCTORS_ID);
        }
    }
}