using System;

namespace BachMaiCR.DBMapping.Models
{
    public class USERS_ACTIONS
    {
        public int? ORDERS { get; set; }

        public bool? IS_ACTIVE { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public DateTime UPDATE_DATE { get; set; }

        public int ADMIN_USER_ID { get; set; }

        public int WEBPAGES_ACTION_ID { get; set; }

        public virtual ADMIN_USER ADMIN_USER { get; set; }

        public virtual WEBPAGES_ACTIONS WEBPAGES_ACTIONS { get; set; }
    }
}