using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class CALENDAR_CHANGEMap : EntityTypeConfiguration<CALENDAR_CHANGE>
    {
        public CALENDAR_CHANGEMap()
        {
            this.HasKey(t => t.CALENDAR_CHANGE_ID);
            this.Property(t => t.DOCTORS_NAME).HasMaxLength(50);
            this.Property(t => t.DOCTORS_CHANGE_NAME).HasMaxLength(50);
            this.ToTable("CALENDAR_CHANGE");
            this.Property(t => t.CALENDAR_CHANGE_ID).HasColumnName("CALENDAR_CHANGE_ID");
            this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
            this.Property(t => t.TEMPLATE_COLUMN_ID).HasColumnName("TEMPLATE_COLUMN_ID");
            this.Property(t => t.DATE_START).HasColumnName("DATE_START");
            this.Property(t => t.DATE_CHANGE_START).HasColumnName("DATE_CHANGE_START");
            this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
            this.Property(t => t.DOCTORS_NAME).HasColumnName("DOCTORS_NAME");
            this.Property(t => t.DOCTORS_CHANGE_ID).HasColumnName("DOCTORS_CHANGE_ID");
            this.Property(t => t.DOCTORS_CHANGE_NAME).HasColumnName("DOCTORS_CHANGE_NAME");
            this.Property(t => t.DATE_CHANGE).HasColumnName("DATE_CHANGE");
            this.Property(t => t.ADMIN_USER_ID).HasColumnName("ADMIN_USER_ID");
            this.Property(t => t.STATUS).HasColumnName("STATUS");
            this.Property(t => t.STATUS_APPROVED).HasColumnName("STATUS_APPROVED");
            this.Property(t => t.CALENDAR_DELETE).HasColumnName("CALENDAR_DELETE");
            this.Property(t => t.USER_NOT_APPROVED).HasColumnName("USER_NOT_APPROVED");
            this.Property(t => t.COLUMN_CHANGE_ID).HasColumnName("COLUMN_CHANGE_ID");
            this.HasOptional(t => t.CALENDAR_DUTY).WithMany(t => t.CALENDAR_CHANGE).HasForeignKey(d => d.CALENDAR_DUTY_ID);
            this.HasOptional(t => t.DOCTOR).WithMany(t => t.CALENDAR_CHANGE).HasForeignKey(d => d.DOCTORS_ID);
        }
    }
}