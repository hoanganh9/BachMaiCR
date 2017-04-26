using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class LM_DEPARTMENTMap : EntityTypeConfiguration<LM_DEPARTMENT>
    {
        public LM_DEPARTMENTMap()
        {
            this.HasKey(t => t.LM_DEPARTMENT_ID);
            this.Property(t => t.DEPARTMENT_NAME).HasMaxLength(150);
            this.Property(t => t.DEPARTMENT_CODE).HasMaxLength(50);
            this.Property(t => t.DEPARTMENT_ADDRESS).HasMaxLength(250);
            this.Property(t => t.DEPARTMENT_PHONE).HasMaxLength(20);
            this.Property(t => t.DEPARTMENT_FAX).HasMaxLength(20);
            this.Property(t => t.DESCRIPTION).HasMaxLength(500);
            this.Property(t => t.NOTES).HasMaxLength(300);
            this.Property(t => t.DEPARTMENT_PATH).HasMaxLength(250);
            this.ToTable("LM_DEPARTMENT");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.DEPARTMENT_PARENT_ID).HasColumnName("DEPARTMENT_PARENT_ID");
            this.Property(t => t.DEPARTMENT_NAME).HasColumnName("DEPARTMENT_NAME");
            this.Property(t => t.DEPARTMENT_CODE).HasColumnName("DEPARTMENT_CODE");
            this.Property(t => t.DEPARTMENT_ADDRESS).HasColumnName("DEPARTMENT_ADDRESS");
            this.Property(t => t.DEPARTMENT_PHONE).HasColumnName("DEPARTMENT_PHONE");
            this.Property(t => t.DEPARTMENT_FAX).HasColumnName("DEPARTMENT_FAX");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
            this.Property(t => t.LEVELS).HasColumnName("LEVELS");
            this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
            this.Property(t => t.NOTES).HasColumnName("NOTES");
            this.Property(t => t.DEPARTMENT_PATH).HasColumnName("DEPARTMENT_PATH");
        }
    }
}