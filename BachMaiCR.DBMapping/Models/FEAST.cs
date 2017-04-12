using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class FEAST
  {
    public int FEAST_ID { get; set; }

    public string FEAST_TITLE { get; set; }

    public DateTime? DATE_BEGIN { get; set; }

    public DateTime? DATE_END { get; set; }

    public int? FEAST_YEAR { get; set; }

    public int? FEAST_TYPE { get; set; }

    public bool ISDELETE { get; set; }

    public bool ISACTIVED { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_DIRECT> CONFIG_DIRECT { get; set; }

    public FEAST()
    {
      this.CONFIG_DIRECT = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>) new List<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>();
    }
  }
}
