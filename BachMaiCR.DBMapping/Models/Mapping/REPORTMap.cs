using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class REPORTMap : EntityTypeConfiguration<REPORT>
  {
    public REPORTMap()
    {
      this.HasKey(t => t.REPORT_ID);
      this.Property(t => t.REPORT_NAME).IsRequired().HasMaxLength(250);
      this.Property(t => t.LM_DEPARTMENT_NAME).HasMaxLength(150);
      this.Property(t => t.USER_CREATE_NAME).HasMaxLength(50);
      this.Property(t => t.USER_RECIPIENTS_IDs).HasMaxLength(500);
      this.Property(t => t.USER_RECIPIENTS_NAMEs).HasMaxLength(1000);
      this.ToTable("REPORT");
      this.Property(t => t.REPORT_ID).HasColumnName("REPORT_ID");
      this.Property(t => t.REPORT_NAME).HasColumnName("REPORT_NAME");
      this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
      this.Property(t => t.DATE_SENDED).HasColumnName("DATE_SENDED");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.LM_DEPARTMENT_NAME).HasColumnName("LM_DEPARTMENT_NAME");
      this.Property(t => t.USER_CREATE_ID).HasColumnName("USER_CREATE_ID");
      this.Property(t => t.USER_CREATE_NAME).HasColumnName("USER_CREATE_NAME");
      this.Property(t => t.USER_RECIPIENTS_IDs).HasColumnName("USER_RECIPIENTS_IDs");
      this.Property(t => t.USER_RECIPIENTS_NAMEs).HasColumnName("USER_RECIPIENTS_NAMEs");
      this.Property(t => t.STATUS).HasColumnName("STATUS");
      this.Property(t => t.NUMBER_STAFF_DIRECT).HasColumnName("NUMBER_STAFF_DIRECT");
      this.Property(t => t.NUMBER_STAFF_LEAVE).HasColumnName("NUMBER_STAFF_LEAVE");
      this.Property(t => t.NUMBER_STAFF_VACATION).HasColumnName("NUMBER_STAFF_VACATION");
      this.Property(t => t.NUMBER_STAFF_SICK).HasColumnName("NUMBER_STAFF_SICK");
      this.Property(t => t.NUMBER_STAFF_MATERNITY).HasColumnName("NUMBER_STAFF_MATERNITY");
      this.Property(t => t.NUMBER_STAFF_UNPAID).HasColumnName("NUMBER_STAFF_UNPAID");
      this.Property(t => t.NUMBER_STAFF_BUSINESS_TRIP).HasColumnName("NUMBER_STAFF_BUSINESS_TRIP");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
    }
  }
}
