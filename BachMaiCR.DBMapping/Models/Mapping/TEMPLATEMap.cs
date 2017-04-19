using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class TEMPLATEMap : EntityTypeConfiguration<TEMPLATE>
  {
    public TEMPLATEMap()
    {
      this.HasKey(t => t.TEMPLATES_ID);
      this.Property(t => t.TEMPLATE_NAME).HasMaxLength(300);
      this.Property(t => t.ABBREVIATION).HasMaxLength(50);
      this.Property(t => t.USER_CREATE).HasMaxLength(25);
      this.Property(t => t.USER_APPROVED).HasMaxLength(25);
      this.Property(t => t.DESCRIPTION).HasMaxLength(500);
      this.Property(t => t.LM_DEPARTMENT_PATH).HasMaxLength(50);
      this.ToTable("TEMPLATES");
      this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
      this.Property(t => t.TEMPLATE_NAME).HasColumnName("TEMPLATE_NAME");
      this.Property(t => t.ABBREVIATION).HasColumnName("ABBREVIATION");
      this.Property(t => t.DATE_START).HasColumnName("DATE_START");
      this.Property(t => t.DATE_END).HasColumnName("DATE_END");
      this.Property(t => t.STATUS).HasColumnName("STATUS");
      this.Property(t => t.DATE_CREATE).HasColumnName("DATE_CREATE");
      this.Property(t => t.USER_CREATE_ID).HasColumnName("USER_CREATE_ID");
      this.Property(t => t.USER_CREATE).HasColumnName("USER_CREATE");
      this.Property(t => t.DATE_APPROVED).HasColumnName("DATE_APPROVED");
      this.Property(t => t.USER_APPROVED_ID).HasColumnName("USER_APPROVED_ID");
      this.Property(t => t.USER_APPROVED).HasColumnName("USER_APPROVED");
      this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
      this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
      this.Property(t => t.LM_DEPARTMENT_PATH).HasColumnName("LM_DEPARTMENT_PATH");
      this.HasOptional(t => t.ADMIN_USER).WithMany(t => t.TEMPLATES).HasForeignKey(d => d.USER_CREATE_ID);
      this.HasOptional(t => t.ADMIN_USER1).WithMany(t => t.TEMPLATES1).HasForeignKey(d => d.USER_APPROVED_ID);
      this.HasRequired(t => t.LM_DEPARTMENT).WithMany(t => t.TEMPLATES).HasForeignKey(d => d.LM_DEPARTMENT_ID);
    }
  }
}
