using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class DOCTORMap : EntityTypeConfiguration<DOCTOR>
  {
    public DOCTORMap()
    {
      this.HasKey(t => t.DOCTORS_ID);
      this.Property(t => t.DOCTOR_NAME).HasMaxLength(100);
      this.Property(t => t.CODE_STAFF).HasMaxLength(50);
      this.Property(t => t.ABBREVIATION).HasMaxLength(25);
      this.Property(t => t.INSURANCE_NUMBER).HasMaxLength(30);
      this.Property(t => t.INSURANCE_REGISTER).HasMaxLength(30);
      this.Property(t => t.IDENTITY_CARD).HasMaxLength(20);
      this.Property(t => t.IDENTITY_PLACE).HasMaxLength(50);
      this.Property(t => t.NATION).HasMaxLength(20);
      this.Property(t => t.RELIGION).HasMaxLength(20);
      this.Property(t => t.PLACE_BIRTH).HasMaxLength(100);
      this.Property(t => t.PROVINCES).HasMaxLength(50);
      this.Property(t => t.DISTRICTS).HasMaxLength(50);
      this.Property(t => t.VILLAGE).HasMaxLength(50);
      this.Property(t => t.ADDRESS).HasMaxLength(200);
      this.Property(t => t.PHONE).HasMaxLength(15);
      this.Property(t => t.EMAIL).HasMaxLength(60);
      this.Property(t => t.DOCTOR_IMAGE).HasMaxLength(300);
      this.Property(t => t.EDUCATION_IDs).HasMaxLength(250);
      this.Property(t => t.EDUCATION_NAMEs).HasMaxLength(500);
      this.Property(t => t.POSITION_IDs).HasMaxLength(250);
      this.Property(t => t.POSITION_NAMEs).HasMaxLength(500);
      this.Property(t => t.LM_DEPARTMENT_IDs).HasMaxLength(250);
      this.Property(t => t.LM_DEPARTMENT_NAMEs).HasMaxLength(500);
      this.ToTable("DOCTORS");
      this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
      this.Property(t => t.DOCTOR_NAME).HasColumnName("DOCTOR_NAME");
      this.Property(t => t.CODE_STAFF).HasColumnName("CODE_STAFF");
      this.Property(t => t.DOCTOR_GROUP_ID).HasColumnName("DOCTOR_GROUP_ID");
      this.Property(t => t.ABBREVIATION).HasColumnName("ABBREVIATION");
      this.Property(t => t.GENDER).HasColumnName("GENDER");
      this.Property(t => t.BIRTHDAY).HasColumnName("BIRTHDAY");
      this.Property(t => t.INSURANCE_NUMBER).HasColumnName("INSURANCE_NUMBER");
      this.Property(t => t.INSURANCE_REGISTER).HasColumnName("INSURANCE_REGISTER");
      this.Property(t => t.IDENTITY_CARD).HasColumnName("IDENTITY_CARD");
      this.Property(t => t.IDENTITY_PLACE).HasColumnName("IDENTITY_PLACE");
      this.Property(t => t.IDENTITY_DATE).HasColumnName("IDENTITY_DATE");
      this.Property(t => t.NATION).HasColumnName("NATION");
      this.Property(t => t.RELIGION).HasColumnName("RELIGION");
      this.Property(t => t.PLACE_BIRTH).HasColumnName("PLACE_BIRTH");
      this.Property(t => t.DOCTOR_ORDER).HasColumnName("DOCTOR_ORDER");
      this.Property(t => t.PROVINCES).HasColumnName("PROVINCES");
      this.Property(t => t.DISTRICTS).HasColumnName("DISTRICTS");
      this.Property(t => t.VILLAGE).HasColumnName("VILLAGE");
      this.Property(t => t.ADDRESS).HasColumnName("ADDRESS");
      this.Property(t => t.PHONE).HasColumnName("PHONE");
      this.Property(t => t.EMAIL).HasColumnName("EMAIL");
      this.Property(t => t.DOCTOR_IMAGE).HasColumnName("DOCTOR_IMAGE");
      this.Property(t => t.DATE_START).HasColumnName("DATE_START");
      this.Property(t => t.DOCTOR_LEVEL_ID).HasColumnName("DOCTOR_LEVEL_ID");
      this.Property(t => t.PRESENT_WORK_ID).HasColumnName("PRESENT_WORK_ID");
      this.Property(t => t.EDUCATION_IDs).HasColumnName("EDUCATION_IDs");
      this.Property(t => t.EDUCATION_NAMEs).HasColumnName("EDUCATION_NAMEs");
      this.Property(t => t.POSITION_IDs).HasColumnName("POSITION_IDs");
      this.Property(t => t.POSITION_NAMEs).HasColumnName("POSITION_NAMEs");
      this.Property(t => t.LM_DEPARTMENT_IDs).HasColumnName("LM_DEPARTMENT_IDs");
      this.Property(t => t.LM_DEPARTMENT_NAMEs).HasColumnName("LM_DEPARTMENT_NAMEs");
      this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
      this.Property(t => t.ISACTIVED).HasColumnName("ISACTIVED");
      this.HasRequired(t => t.DOCTOR_GROUPS).WithMany(t => t.DOCTORS).HasForeignKey(d => d.DOCTOR_GROUP_ID);
      this.HasRequired(t => t.DOCTOR_LEVEL).WithMany(t => t.DOCTORS).HasForeignKey(d => d.DOCTOR_LEVEL_ID);
    }
  }
}
