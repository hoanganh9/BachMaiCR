using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class DOCTOR_LEVEL
  {
    public int DOCTOR_LEVEL_ID { get; set; }

    public string CODE { get; set; }

    public string LEVEL_NAME { get; set; }

    public int? LEVEL_NUMBER { get; set; }

    public int? LEVEL_ORDER { get; set; }

    public bool? ISDELETE { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.COLUM_LEVEL> COLUM_LEVEL { get; set; }

    public virtual ICollection<DOCTOR> DOCTORS { get; set; }

    public DOCTOR_LEVEL()
    {
      this.COLUM_LEVEL = (ICollection<BachMaiCR.DBMapping.Models.COLUM_LEVEL>) new List<BachMaiCR.DBMapping.Models.COLUM_LEVEL>();
      this.DOCTORS = (ICollection<DOCTOR>) new List<DOCTOR>();
    }
  }
}
