using System;

namespace BachMaiCR.DBMapping.Models
{
    public class ADMIN_LOG
    {
        public long LOG_ID { get; set; }

        public int STATUS { get; set; }

        public string SESSION_ID { get; set; }

        public string APP_CODE { get; set; }

        public string THREAD_ID { get; set; }

        public DateTime START_TIME { get; set; }

        public DateTime? END_TIME { get; set; }

        public int? ADMIN_USER_ID { get; set; }

        public string USER_LOGIN { get; set; }

        public string USER_NAME { get; set; }

        public string IP_ADDRESS { get; set; }

        public string ACTION_CODE { get; set; }

        public string ACTION_NAME { get; set; }

        public string MENU_CODE { get; set; }

        public string MENU_NAME { get; set; }

        public int? WEBPAGES_ACTION_ID { get; set; }

        public int ACTION_TYPE { get; set; }

        public int? OBJECT_ID { get; set; }

        public string CONTENT { get; set; }

        public string DESCRIPTION { get; set; }

        public int? LEVEL { get; set; }

        public string ERROR_CODE { get; set; }

        public virtual ADMIN_USER ADMIN_USER { get; set; }

        public virtual WEBPAGES_ACTIONS WEBPAGES_ACTIONS { get; set; }
    }
}