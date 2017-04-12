
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace BachMaiCR.DBMapping.Models.Mapping
{
  public class sysdiagramMap : EntityTypeConfiguration<sysdiagram>
  {
    public sysdiagramMap() : base()
    {
      
      this.HasKey(t => t.diagram_id);
      this.Property(t => t.name).IsRequired().HasMaxLength(128);
      this.ToTable("sysdiagrams");
      this.Property(t => t.name).HasColumnName("name");
      this.Property(t => t.principal_id).HasColumnName("principal_id");
      this.Property(t => t.diagram_id).HasColumnName("diagram_id");
      this.Property(t => t.version).HasColumnName("version");
      this.Property(t => t.definition).HasColumnName("definition");
    }
  }
}
