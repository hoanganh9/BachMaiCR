using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class ADMIN_LOGMap : EntityTypeConfiguration<ADMIN_LOG>
    {
        public ADMIN_LOGMap()
        {
            this.HasKey(t => t.LOG_ID);
            this.Property(t => t.SESSION_ID).HasMaxLength(50);
            this.Property(t => t.APP_CODE).HasMaxLength(50);
            this.Property(t => t.THREAD_ID).HasMaxLength(50);
            this.Property(t => t.USER_LOGIN).HasMaxLength(50);
            this.Property(t => t.USER_NAME).HasMaxLength(250);
            this.Property(t => t.IP_ADDRESS).HasMaxLength(50);
            this.Property(t => t.ACTION_CODE).HasMaxLength(250);
            this.Property(t => t.ACTION_NAME).HasMaxLength(250);
            this.Property(t => t.MENU_CODE).HasMaxLength(50);
            this.Property(t => t.MENU_NAME).HasMaxLength(250);
            this.Property(t => t.CONTENT).HasMaxLength(550);
            this.Property(t => t.DESCRIPTION).HasMaxLength(550);
            this.Property(t => t.ERROR_CODE).HasMaxLength(500);
            this.ToTable("ADMIN_LOG");
            this.Property(t => t.LOG_ID).HasColumnName("LOG_ID");
            this.Property(t => t.STATUS).HasColumnName("STATUS");
            this.Property(t => t.SESSION_ID).HasColumnName("SESSION_ID");
            this.Property(t => t.APP_CODE).HasColumnName("APP_CODE");
            this.Property(t => t.THREAD_ID).HasColumnName("THREAD_ID");
            this.Property(t => t.START_TIME).HasColumnName("START_TIME");
            this.Property(t => t.END_TIME).HasColumnName("END_TIME");
            this.Property(t => t.ADMIN_USER_ID).HasColumnName("ADMIN_USER_ID");
            this.Property(t => t.USER_LOGIN).HasColumnName("USER_LOGIN");
            this.Property(t => t.USER_NAME).HasColumnName("USER_NAME");
            this.Property(t => t.IP_ADDRESS).HasColumnName("IP_ADDRESS");
            this.Property(t => t.ACTION_CODE).HasColumnName("ACTION_CODE");
            this.Property(t => t.ACTION_NAME).HasColumnName("ACTION_NAME");
            this.Property(t => t.MENU_CODE).HasColumnName("MENU_CODE");
            this.Property(t => t.MENU_NAME).HasColumnName("MENU_NAME");
            this.Property(t => t.WEBPAGES_ACTION_ID).HasColumnName("WEBPAGES_ACTION_ID");
            this.Property(t => t.ACTION_TYPE).HasColumnName("ACTION_TYPE");
            this.Property(t => t.OBJECT_ID).HasColumnName("OBJECT_ID");
            this.Property(t => t.CONTENT).HasColumnName("CONTENT");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
            this.Property(t => t.LEVEL).HasColumnName("LEVEL");
            this.Property(t => t.ERROR_CODE).HasColumnName("ERROR_CODE");
            this.HasOptional(t => t.ADMIN_USER).WithMany(t => t.ADMIN_LOG).HasForeignKey(d => d.ADMIN_USER_ID);
            this.HasOptional(t => t.WEBPAGES_ACTIONS).WithMany(t => t.ADMIN_LOG).HasForeignKey(d => d.WEBPAGES_ACTION_ID);
        }
    }
}