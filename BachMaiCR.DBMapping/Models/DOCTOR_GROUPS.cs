using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class DOCTOR_GROUPS
  {
    public int DOCTOR_GROUP_ID { get; set; }

    public string DOCTOR_GROUP_NAME { get; set; }

    public string DESCRIPTION { get; set; }

    public virtual ICollection<DOCTOR> DOCTORS { get; set; }

    public DOCTOR_GROUPS()
    {
      this.DOCTORS = (ICollection<DOCTOR>) new List<DOCTOR>();
    }
  }
}
