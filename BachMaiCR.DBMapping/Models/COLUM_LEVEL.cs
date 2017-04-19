

namespace BachMaiCR.DBMapping.Models
{
  public class COLUM_LEVEL
  {
    public int COLUM_LEVEL_ID { get; set; }

    public int TEMPLATE_COLUM_ID { get; set; }

    public int DOCTOR_LEVEL_ID { get; set; }

    public int? TEMPLATES_ID { get; set; }

    public virtual DOCTOR_LEVEL DOCTOR_LEVEL { get; set; }

    public virtual TEMPLATE_COLUM TEMPLATE_COLUM { get; set; }
  }
}
