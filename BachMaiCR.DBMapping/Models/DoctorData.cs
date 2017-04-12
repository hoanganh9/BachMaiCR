using System;

namespace BachMaiCR.DBMapping.Models
{
  public class DoctorData
  {
    public int CALENDAR_DOCTOR_ID { get; set; }

    public int CALENDAR_DATA_ID { get; set; }

    public int DOCTORS_ID { get; set; }

    public int CALENDAR_DUTY_ID { get; set; }

    public int? TEMPLATE_COLUM_ID { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }
  }
}
