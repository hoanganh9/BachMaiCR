using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CONFIG_SMS_SENDMap : EntityTypeConfiguration<CONFIG_SMS_SEND>
  {
    public CONFIG_SMS_SENDMap()
    {
      this.HasKey(t => t.CONFIG_SMS_SEND_ID);
      this.ToTable("CONFIG_SMS_SEND");
      this.Property(t => t.CONFIG_SMS_SEND_ID).HasColumnName("CONFIG_SMS_SEND_ID");
      this.Property(t => t.CONFIG_SMS_ID).HasColumnName("CONFIG_SMS_ID");
      this.Property(t => t.ALARM_SEND).HasColumnName("ALARM_SEND");
      this.Property(t => t.ALARM_NUMBER).HasColumnName("ALARM_NUMBER");
      this.Property(t => t.REPEAT_MINUTES).HasColumnName("REPEAT_MINUTES");
      this.Property(t => t.ALARM_TIME_NEXT).HasColumnName("ALARM_TIME_NEXT");
      this.Property(t => t.DATE_SEND).HasColumnName("DATE_SEND");
      this.HasOptional(t => t.CONFIG_SMS).WithMany(t => t.CONFIG_SMS_SEND).HasForeignKey(d => d.CONFIG_SMS_ID);
    }
  }
}
