

namespace BachMaiCR.DBMapping.Models
{
  public class TelephoneInDepartment
  {
    public int LM_DEPARTMENT_ID { get; set; }

    public string DEPARTMENT_NAME { get; set; }

    public string DEPARTMENT_CODE { get; set; }

    public string DEPARTMENT_ADDRESS { get; set; }

    public string DEPARTMENT_PHONE { get; set; }

    public int? LEVELS { get; set; }

    public int CALENDAR_GROUP_ID { get; set; }

    public int? CALENDAR_MONTH { get; set; }

    public int? CALENDAR_YEAR { get; set; }

    public int? CALENDAR_PARENT_ID { get; set; }

    public int? CALENDAR_ID { get; set; }
  }
}
