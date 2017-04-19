

namespace BachMaiCR.DBMapping.Models
{
  public class CONFIG_TEMPLATE
  {
    public int CONFIG_TEMPLATE_ID { get; set; }

    public int? TEMPLATES_ID { get; set; }

    public int? CONFIG_LEVEL { get; set; }

    public int? LM_DEPARTMENT_ID { get; set; }

    public bool? ISDELETE { get; set; }

    public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }

    public virtual TEMPLATE TEMPLATE { get; set; }
  }
}
