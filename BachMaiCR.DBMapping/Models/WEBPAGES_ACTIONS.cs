using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
    public class WEBPAGES_ACTIONS
    {
        public int WEBPAGES_ACTION_ID { get; set; }

        public string DESCRIPTION { get; set; }

        public string MENU_NAME { get; set; }

        public string CODE { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public DateTime? UPDATE_DATE { get; set; }

        public bool? IS_ACTIVE { get; set; }

        public int? GROUP_ORDER { get; set; }

        public int? MENU_ORDER { get; set; }

        public string GROUP_CODE { get; set; }

        public string GROUP_NAME { get; set; }

        public bool? IS_MENU { get; set; }

        public bool? MANUAL_EDITED { get; set; }

        public string GROUP_CLASSCSS { get; set; }

        public string ACTION_CLASSCSS { get; set; }

        public string GROUP_CHILD_CODE { get; set; }

        public string GROUP_CHILD_NAME { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.ADMIN_LOG> ADMIN_LOG { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.USERS_ACTIONS> USERS_ACTIONS { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES> WEBPAGES_ROLES { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_FUNCTIONS> WEBPAGES_FUNCTIONS { get; set; }

        public WEBPAGES_ACTIONS()
        {
            this.ADMIN_LOG = (ICollection<BachMaiCR.DBMapping.Models.ADMIN_LOG>)new List<BachMaiCR.DBMapping.Models.ADMIN_LOG>();
            this.USERS_ACTIONS = (ICollection<BachMaiCR.DBMapping.Models.USERS_ACTIONS>)new List<BachMaiCR.DBMapping.Models.USERS_ACTIONS>();
            this.WEBPAGES_ROLES = (ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>)new List<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>();
            this.WEBPAGES_FUNCTIONS = (ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_FUNCTIONS>)new List<BachMaiCR.DBMapping.Models.WEBPAGES_FUNCTIONS>();
        }
    }
}