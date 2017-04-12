using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class CALENDAR_DATA
  {
    public int CALENDAR_DATA_ID { get; set; }

    public int CALENDAR_DUTY_ID { get; set; }

    public int? TEMPLATE_COLUM_ID { get; set; }

    public string CALENDAR_NAME { get; set; }

    public string CALENDAR_CONTENT { get; set; }

    public string CALENDAR_PHONE { get; set; }

    public string CALENDAR_NOTE { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public int? CHANGE_STATUS { get; set; }

    public bool? ISDELETE { get; set; }

    public bool? CHIEF_VIEW { get; set; }

    public int CHIEF_ID { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR> CALENDAR_DOCTOR { get; set; }

    public virtual CALENDAR_DUTY CALENDAR_DUTY { get; set; }

    public virtual TEMPLATE_COLUM TEMPLATE_COLUM { get; set; }

    public CALENDAR_DATA()
    {
      this.CALENDAR_DOCTOR = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR>) new List<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR>();
    }
  }
}
