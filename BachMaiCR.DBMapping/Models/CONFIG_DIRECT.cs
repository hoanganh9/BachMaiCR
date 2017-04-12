using System;

namespace BachMaiCR.DBMapping.Models
{
  public class CONFIG_DIRECT
  {
    public int CONFIG_DIRECT_ID { get; set; }

    public int LM_DEPARTMENT_ID { get; set; }

    public int DOCTORS_ID { get; set; }

    public DateTime? DATE_BEGIN { get; set; }

    public DateTime? DATE_END { get; set; }

    public int? FEAST_ID { get; set; }

    public int? USER_CREATE_ID { get; set; }

    public DateTime? DATE_CREATE { get; set; }

    public bool? ISDELETE { get; set; }

    public virtual DOCTOR DOCTOR { get; set; }

    public virtual FEAST FEAST { get; set; }

    public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }
  }
}
