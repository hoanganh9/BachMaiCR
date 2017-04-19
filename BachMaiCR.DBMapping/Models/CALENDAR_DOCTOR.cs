

namespace BachMaiCR.DBMapping.Models
{
  public class CALENDAR_DOCTOR
  {
    public int CALENDAR_DOCTOR_ID { get; set; }

    public int CALENDAR_DATA_ID { get; set; }

    public int DOCTORS_ID { get; set; }

    public int? CALENDAR_DUTY_ID { get; set; }

    public int? COLUMN_LEVEL_ID { get; set; }

    public virtual CALENDAR_DATA CALENDAR_DATA { get; set; }

    public virtual DOCTOR DOCTOR { get; set; }
  }
}
