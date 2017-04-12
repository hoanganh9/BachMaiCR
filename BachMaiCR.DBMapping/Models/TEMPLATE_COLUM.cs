using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class TEMPLATE_COLUM
  {
    public int TEMPLATE_COLUM_ID { get; set; }

    public string COLUM_NAME { get; set; }

    public int? COLUM_ORDER { get; set; }

    public int TEMPLATES_ID { get; set; }

    public int? LM_DEPARTMENT_ID { get; set; }

    public string LM_DEPARTMENT_PATH { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DATA> CALENDAR_DATA { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.COLUM_LEVEL> COLUM_LEVEL { get; set; }

    public virtual TEMPLATE TEMPLATE { get; set; }

    public TEMPLATE_COLUM()
    {
      this.CALENDAR_DATA = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DATA>) new List<BachMaiCR.DBMapping.Models.CALENDAR_DATA>();
      this.COLUM_LEVEL = (ICollection<BachMaiCR.DBMapping.Models.COLUM_LEVEL>) new List<BachMaiCR.DBMapping.Models.COLUM_LEVEL>();
    }
  }
}
