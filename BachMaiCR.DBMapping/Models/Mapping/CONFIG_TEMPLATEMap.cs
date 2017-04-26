using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class CONFIG_TEMPLATEMap : EntityTypeConfiguration<CONFIG_TEMPLATE>
    {
        public CONFIG_TEMPLATEMap()
        {
            this.HasKey(t => t.CONFIG_TEMPLATE_ID);
            this.ToTable("CONFIG_TEMPLATE");
            this.Property(t => t.CONFIG_TEMPLATE_ID).HasColumnName("CONFIG_TEMPLATE_ID");
            this.Property(t => t.TEMPLATES_ID).HasColumnName("TEMPLATES_ID");
            this.Property(t => t.CONFIG_LEVEL).HasColumnName("CONFIG_LEVEL");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
            this.HasOptional(t => t.LM_DEPARTMENT).WithMany(t => t.CONFIG_TEMPLATE).HasForeignKey(d => d.LM_DEPARTMENT_ID);
            this.HasOptional(t => t.TEMPLATE).WithMany(t => t.CONFIG_TEMPLATE).HasForeignKey(d => d.TEMPLATES_ID);
        }
    }
}