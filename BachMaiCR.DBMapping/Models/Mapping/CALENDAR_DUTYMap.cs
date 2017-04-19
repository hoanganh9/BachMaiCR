using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class CALENDAR_DUTYMap : EntityTypeConfiguration<CALENDAR_DUTY>
  {
    public CALENDAR_DUTYMap()
    {
      this.HasKey(t => t.CALENDAR_DUTY_ID);
      this.Property(t => t.CALENDAR_NAME).HasMaxLength(500);
      this.Property(t => t.COMMENTS).HasMaxLength(300);
      this.Property(t => t.LM_DEPARTMENT_PARTS).HasMaxLength(400);
      this.ToTable("CALENDAR_DUTY");
      this.Property(t => t.CALENDAR_DUTY_ID).HasColumnName("CALENDAR_DUTY_ID");
      this.Property(t => t.CALENDAR_NAME).HasColumnName("CALENDAR_NAME");
      this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
      this.Property(t => t.DATE_APPROVED).HasColumnName("DATE_APPROVED");
      this.Property(t => t.USER_CREATE_ID).HasColumnName("USER_CREATE_ID");
      this.Property(t => t.USER_APPROVED_ID).HasColumnName("USER_APPROVED_ID");
      this.Property(t => t.CALENDAR_STATUS).HasColumnName("CALENDAR_STATUS");
      this.Property(t => t.CALENDAR_MONTH).HasColumnName("CALENDAR_MONTH");
      this.Property(t => t.CALENDAR_YEAR).HasColumnName("CALENDAR_YEAR");
      this.Property(t => t.DUTY_TYPE).HasColumnName("DUTY_TYPE");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
      this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.COMMENTS).HasColumnName("COMMENTS");
      this.Property(t => t.ISAPPROVED).HasColumnName("ISAPPROVED");
      this.Property(t => t.LM_DEPARTMENT_PARTS).HasColumnName("LM_DEPARTMENT_PARTS");
      this.Property(t => t.USER_CANCEL_APPROVED).HasColumnName("USER_CANCEL_APPROVED");
      this.HasOptional(t => t.ADMIN_USER).WithMany(t => t.CALENDAR_DUTY).HasForeignKey(d => d.USER_CREATE_ID);
      this.HasOptional(t => t.ADMIN_USER1).WithMany(t => t.CALENDAR_DUTY1).HasForeignKey(d => d.USER_APPROVED_ID);
      this.HasRequired(t => t.LM_DEPARTMENT).WithMany(t => t.CALENDAR_DUTY).HasForeignKey(d => d.LM_DEPARTMENT_ID);
      this.HasOptional(t => t.TEMPLATE).WithMany(t => t.CALENDAR_DUTY).HasForeignKey(d => d.TEMPLATES_ID);
    }
  }
}
