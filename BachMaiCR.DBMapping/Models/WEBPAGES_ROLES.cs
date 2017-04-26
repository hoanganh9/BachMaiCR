using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
    public class WEBPAGES_ROLES
    {
        public int WEBPAGES_ROLE_ID { get; set; }

        public string ROLE_NAME { get; set; }

        public string ABBREVIATION { get; set; }

        public string DESCRIPTION { get; set; }

        public bool? ISACTIVE { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public byte[] UPDATE_DATE { get; set; }

        public string ROLE_CODE { get; set; }

        public int? LM_DEPARTMENT_ID { get; set; }

        public int? CREATE_USERID { get; set; }

        public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS> WEBPAGES_ACTIONS { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.ADMIN_USER> ADMIN_USER { get; set; }

        public WEBPAGES_ROLES()
        {
            this.WEBPAGES_ACTIONS = (ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS>)new List<BachMaiCR.DBMapping.Models.WEBPAGES_ACTIONS>();
            this.ADMIN_USER = (ICollection<BachMaiCR.DBMapping.Models.ADMIN_USER>)new List<BachMaiCR.DBMapping.Models.ADMIN_USER>();
        }
    }
}