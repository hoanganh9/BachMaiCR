using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class TEMPLATE
  {
    public int TEMPLATES_ID { get; set; }

    public string TEMPLATE_NAME { get; set; }

    public string ABBREVIATION { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public int? STATUS { get; set; }

    public DateTime? DATE_CREATE { get; set; }

    public int? USER_CREATE_ID { get; set; }

    public string USER_CREATE { get; set; }

    public DateTime? DATE_APPROVED { get; set; }

    public int? USER_APPROVED_ID { get; set; }

    public string USER_APPROVED { get; set; }

    public string DESCRIPTION { get; set; }

    public bool? ISDELETE { get; set; }

    public int LM_DEPARTMENT_ID { get; set; }

    public string LM_DEPARTMENT_PATH { get; set; }

    public virtual ADMIN_USER ADMIN_USER { get; set; }

    public virtual ADMIN_USER ADMIN_USER1 { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY> CALENDAR_DUTY { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_SMS> CONFIG_SMS { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE> CONFIG_TEMPLATE { get; set; }

    public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.TEMPLATE_COLUM> TEMPLATE_COLUM { get; set; }

    public TEMPLATE()
    {
      this.CALENDAR_DUTY = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>) new List<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>();
      this.CONFIG_SMS = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_SMS>) new List<BachMaiCR.DBMapping.Models.CONFIG_SMS>();
      this.CONFIG_TEMPLATE = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE>) new List<BachMaiCR.DBMapping.Models.CONFIG_TEMPLATE>();
      this.TEMPLATE_COLUM = (ICollection<BachMaiCR.DBMapping.Models.TEMPLATE_COLUM>) new List<BachMaiCR.DBMapping.Models.TEMPLATE_COLUM>();
    }
  }
}
