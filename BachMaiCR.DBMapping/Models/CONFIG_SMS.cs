using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class CONFIG_SMS
  {
    public int CONFIG_SMS_ID { get; set; }

    public string CONFIG_SMS_NAME { get; set; }

    public int? ALARM_TIME_HOUR { get; set; }

    public int? ALARM_TIME_DAY { get; set; }

    public int? ALARM_COUNT { get; set; }

    public int? REPEAT_ALARM_HOUR { get; set; }

    public int? REPEAT_ALARM_MINUTES { get; set; }

    public int LM_DEPARTMENT_ID { get; set; }

    public int? TEMPLATES_ID { get; set; }

    public bool ISACTIVED { get; set; }

    public bool? ISDELETE { get; set; }

    public int? USER_CREATE_ID { get; set; }

    public DateTime? DATE_CREATE { get; set; }

    public DateTime? DATE_ACTIVED { get; set; }

    public int? USER_ACTIVED_ID { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_SMS_SEND> CONFIG_SMS_SEND { get; set; }

    public virtual TEMPLATE TEMPLATE { get; set; }

    public CONFIG_SMS()
    {
      this.CONFIG_SMS_SEND = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_SMS_SEND>) new List<BachMaiCR.DBMapping.Models.CONFIG_SMS_SEND>();
    }
  }
}
