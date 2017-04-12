using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class CALENDAR_DUTY
  {
    public int CALENDAR_DUTY_ID { get; set; }

    public string CALENDAR_NAME { get; set; }

    public DateTime? DATE_CREATE { get; set; }

    public DateTime? DATE_APPROVED { get; set; }

    public int? USER_CREATE_ID { get; set; }

    public int? USER_APPROVED_ID { get; set; }

    public int? CALENDAR_STATUS { get; set; }

    public int? CALENDAR_MONTH { get; set; }

    public int? CALENDAR_YEAR { get; set; }

    public int? DUTY_TYPE { get; set; }

    public bool? ISDELETE { get; set; }

    public int? TEMPLATES_ID { get; set; }

    public int LM_DEPARTMENT_ID { get; set; }

    public string COMMENTS { get; set; }

    public int ISAPPROVED { get; set; }

    public string LM_DEPARTMENT_PARTS { get; set; }

    public int? USER_CANCEL_APPROVED { get; set; }

    public virtual ADMIN_USER ADMIN_USER { get; set; }

    public virtual ADMIN_USER ADMIN_USER1 { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE> CALENDAR_CHANGE { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DATA> CALENDAR_DATA { get; set; }

    public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }

    public virtual TEMPLATE TEMPLATE { get; set; }

    public CALENDAR_DUTY()
    {
      this.CALENDAR_CHANGE = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE>) new List<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE>();
      this.CALENDAR_DATA = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DATA>) new List<BachMaiCR.DBMapping.Models.CALENDAR_DATA>();
    }
  }
}
