using System.Data.Entity.ModelConfiguration;

namespace BachMaiCR.DBMapping.Models.Mapping
{
    public class ADMIN_USERMap : EntityTypeConfiguration<ADMIN_USER>
    {
        public ADMIN_USERMap()
        {
            this.HasKey(t => t.ADMIN_USER_ID);
            this.Property(t => t.USERNAME).HasMaxLength(50);
            this.Property(t => t.USERCODE).HasMaxLength(50);
            this.Property(t => t.FULLNAME).HasMaxLength(50);
            this.Property(t => t.PHONE).HasMaxLength(15);
            this.Property(t => t.MAIL).HasMaxLength(50);
            this.Property(t => t.PASSWORD).HasMaxLength(100);
            this.Property(t => t.SALT).HasMaxLength(100);
            this.Property(t => t.ACTIVE_URL).HasMaxLength(500);
            this.Property(t => t.SESSION_TOKEN).HasMaxLength(500);
            this.ToTable("ADMIN_USER");
            this.Property(t => t.ADMIN_USER_ID).HasColumnName("ADMIN_USER_ID");
            this.Property(t => t.USERNAME).HasColumnName("USERNAME");
            this.Property(t => t.USERCODE).HasColumnName("USERCODE");
            this.Property(t => t.FULLNAME).HasColumnName("FULLNAME");
            this.Property(t => t.PHONE).HasColumnName("PHONE");
            this.Property(t => t.MAIL).HasColumnName("MAIL");
            this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
            this.Property(t => t.UPDATE_DATE).HasColumnName("UPDATE_DATE");
            this.Property(t => t.ISACTIVED).HasColumnName("ISACTIVED");
            this.Property(t => t.ISDELETE).HasColumnName("ISDELETE");
            this.Property(t => t.PASSWORD).HasColumnName("PASSWORD");
            this.Property(t => t.SALT).HasColumnName("SALT");
            this.Property(t => t.RESET_PASSWORD_DATE).HasColumnName("RESET_PASSWORD_DATE");
            this.Property(t => t.REQIURE_CHANGE_PASS).HasColumnName("REQIURE_CHANGE_PASS");
            this.Property(t => t.ACTIVE_URL).HasColumnName("ACTIVE_URL");
            this.Property(t => t.ACTIVE_ENDDATE).HasColumnName("ACTIVE_ENDDATE");
            this.Property(t => t.STEP).HasColumnName("STEP");
            this.Property(t => t.LM_DEPARTMENT_ID).HasColumnName("LM_DEPARTMENT_ID");
            this.Property(t => t.DOCTORS_ID).HasColumnName("DOCTORS_ID");
            this.Property(t => t.SESSION_TOKEN).HasColumnName("SESSION_TOKEN");
            this.Property(t => t.IS_ADMIN).HasColumnName("IS_ADMIN");
            this.HasOptional(t => t.LM_DEPARTMENT).WithMany(t => t.ADMIN_USER).HasForeignKey(d => d.LM_DEPARTMENT_ID);
        }
    }
}