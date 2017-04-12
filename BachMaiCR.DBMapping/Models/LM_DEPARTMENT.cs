using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class LM_DEPARTMENT
  {
    public int LM_DEPARTMENT_ID { get; set; }

    public int? DEPARTMENT_PARENT_ID { get; set; }

    public string DEPARTMENT_NAME { get; set; }

    public string DEPARTMENT_CODE { get; set; }

    public string DEPARTMENT_ADDRESS { get; set; }

    public string DEPARTMENT_PHONE { get; set; }

    public string DEPARTMENT_FAX { get; set; }

    public string DESCRIPTION { get; set; }

    public int? LEVELS { get; set; }

    public bool? ISDELETE { get; set; }

    public string NOTES { get; set; }

    public string DEPARTMENT_PATH { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.ADMIN_USER> ADMIN_USER { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY> CALENDAR_DUTY { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_DIRECT> CONFIG_DIRECT { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS> CONFIG_HOLIDAYS { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_SMS> CONFIG_SMS { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE> CONFIG_TEMPLATE { get; set; }

    public virtual ICollection<TEMPLATE> TEMPLATES { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES> WEBPAGES_ROLES { get; set; }

    public LM_DEPARTMENT()
    {
      this.ADMIN_USER = (ICollection<BachMaiCR.DBMapping.Models.ADMIN_USER>) new List<BachMaiCR.DBMapping.Models.ADMIN_USER>();
      this.CALENDAR_DUTY = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>) new List<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>();
      this.CONFIG_DIRECT = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>) new List<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>();
      this.CONFIG_HOLIDAYS = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>) new List<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>();
      this.CONFIG_SMS = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_SMS>) new List<BachMaiCR.DBMapping.Models.CONFIG_SMS>();
      this.CONFIG_TEMPLATE = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE>) new List<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE>();
      this.TEMPLATES = (ICollection<TEMPLATE>) new List<TEMPLATE>();
      this.WEBPAGES_ROLES = (ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>) new List<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>();
    }
  }
}
