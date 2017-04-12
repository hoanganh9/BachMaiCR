using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
  public class WEBPAGES_FUNCTIONS
  {
    public int WEBPAGES_FUNCTIONS_ID { get; set; }

    public string UNIQUE_CODE { get; set; }

    public string ACTION_NAME { get; set; }

    public string CONTROLLER { get; set; }

    public string ACTION_TYPE { get; set; }

    public DateTime? CREATE_DATE { get; set; }

    public DateTime? UPDATE_DATE { get; set; }

    public virtual ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS> WEBPAGES_ACTIONS { get; set; }

    public WEBPAGES_FUNCTIONS()
    {
      this.WEBPAGES_ACTIONS = (ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS>) new List<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS>();
    }
  }
}
