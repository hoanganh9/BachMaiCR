using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class CONFIG_SMSMap : EntityTypeConfiguration<CONFIG_SMS>
    {
        public CONFIG_SMSMap()
        {
            this.HasKey(t => t.CONFIG_SMS_ID);
            this.Property(t => t.CONFIG_SMS_NAME).HasMaxLength(150);
            this.ToTable("CONFIG_SMS");
            this.Property(t => t.CONFIG_SMS_ID).HasColumnName("CONFIG_SMS_ID");
            this.Property(t => t.CONFIG_SMS_NAME).HasColumnName("CONFIG_SMS_NAME");
            this.Property(t => t.ALARM_TIME_HOUR).HasColumnName("ALARM_TIME_HOUR");
            this.Property(t => t.ALARM_TIME_DAY).HasColumnName("ALARM_TIME_DAY");
            this.Property(t => t.ALARM_COUNT).HasColumnName("ALARM_COUNT");
            this.Property(t => t.REPEAT_ALARM_HOUR).HasColumnName("REPEAT_ALARM_HOUR");
            this.Property(t => t.REPEAT_ALARM_MINUTES).HasColumnName("REPEAT_ALARM_MINUTES");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
            this.Property(t => t.ISACTIVED).HasColumnName("ISACTIVED");
            this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
            this.Property(t => t.USER_CREATE_ID).HasColumnName("USER_CREATE_ID");
            this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
            this.Property(t => t.DATE_ACTIVED).HasColumnName("DATE_ACTIVED");
            this.Property(t => t.USER_ACTIVED_ID).HasColumnName("USER_ACTIVED_ID");
            this.Property(t => t.DATE_START).HasColumnName("DATE_START");
            this.Property(t => t.DATE_END).HasColumnName("DATE_END");
            this.HasRequired(t => t.LM_DEPARTMENT).WithMany(t => t.CONFIG_SMS).HasForeignKey(d => d.LM_DEPARTMENT_ID);
            this.HasOptional(t => t.TEMPLATE).WithMany(t => t.CONFIG_SMS).HasForeignKey(d => d.TEMPLATES_ID);
        }
    }
}