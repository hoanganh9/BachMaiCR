using System;

namespace BachMaiCR.DBMapping.Models
{
  public class CONFIG_SMS_SEND
  {
    public int CONFIG_SMS_SEND_ID { get; set; }

    public int? CONFIG_SMS_ID { get; set; }

    public int? ALARM_SEND { get; set; }

    public int? ALARM_NUMBER { get; set; }

    public int? REPEAT_MINUTES { get; set; }

    public int? ALARM_TIME_NEXT { get; set; }

    public DateTime? DATE_SEND { get; set; }

    public virtual CONFIG_SMS CONFIG_SMS { get; set; }
  }
}
