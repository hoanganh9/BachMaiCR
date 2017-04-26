using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class TEMPLATE_COLUMMap : EntityTypeConfiguration<TEMPLATE_COLUM>
    {
        public TEMPLATE_COLUMMap()
        {
            this.HasKey(t => t.TEMPLATE_COLUM_ID);
            this.Property(t => t.COLUM_NAME).HasMaxLength(50);
            this.Property(t => t.LM_DEPARTMENT_PATH).HasMaxLength(50);
            this.ToTable("TEMPLATE_COLUM");
            this.Property(t => t.TEMPLATE_COLUM_ID).HasColumnName("TEMPLATE_COLUM_ID");
            this.Property(t => t.COLUM_NAME).HasColumnName("COLUM_NAME");
            this.Property(t => t.COLUM_ORDER).HasColumnName("COLUM_ORDER");
            this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.LM_DEPARTMENT_PATH).HasColumnName("LM_DEPARTMENT_PATH");
            this.HasRequired(t => t.TEMPLATE).WithMany(t => t.TEMPLATE_COLUM).HasForeignKey(d => d.TEMPLATES_ID);
        }
    }
}