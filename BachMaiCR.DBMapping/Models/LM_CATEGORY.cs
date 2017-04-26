using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
    public class LM_CATEGORY
    {
        public int LM_CATEGORY_ID { get; set; }

        public string CATEGORY_NAME { get; set; }

        public string CATEGORY_DESCRIPTION { get; set; }

        public string CODE { get; set; }

        public int? CATEGORY_ORDER { get; set; }

        public int? CATEGORY_STYLE { get; set; }

        public int? CATEGORY_PARENT { get; set; }

        public int? CATEGORY_STATUS { get; set; }

        public string NOTES { get; set; }

        public bool? ISDELETE { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS> CONFIG_HOLIDAYS { get; set; }

        public LM_CATEGORY()
        {
            this.CONFIG_HOLIDAYS = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>)new List<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>();
        }
    }
}