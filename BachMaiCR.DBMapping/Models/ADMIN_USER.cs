using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
    public class ADMIN_USER
    {
        public int ADMIN_USER_ID { get; set; }

        public string USERNAME { get; set; }

        public string USERCODE { get; set; }

        public string FULLNAME { get; set; }

        public string PHONE { get; set; }

        public string MAIL { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public DateTime? UPDATE_DATE { get; set; }

        public bool? ISACTIVED { get; set; }

        public bool? ISDELETE { get; set; }

        public string PASSWORD { get; set; }

        public string SALT { get; set; }

        public DateTime? RESET_PASSWORD_DATE { get; set; }

        public bool? REQIURE_CHANGE_PASS { get; set; }

        public string ACTIVE_URL { get; set; }

        public DateTime? ACTIVE_ENDDATE { get; set; }

        public byte? STEP { get; set; }

        public int? LM_DEPARTMENT_ID { get; set; }

        public int? DOCTORS_ID { get; set; }

        public string SESSION_TOKEN { get; set; }

        public bool? IS_ADMIN { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.ADMIN_LOG> ADMIN_LOG { get; set; }

        public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY> CALENDAR_DUTY { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY> CALENDAR_DUTY1 { get; set; }

        public virtual ICollection<TEMPLATE> TEMPLATES { get; set; }

        public virtual ICollection<TEMPLATE> TEMPLATES1 { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.USERS_ACTIONS> USERS_ACTIONS { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES> WEBPAGES_ROLES { get; set; }

        public ADMIN_USER()
        {
            this.ADMIN_LOG = (ICollection<BachMaiCR.DBMapping.Models.ADMIN_LOG>)new List<BachMaiCR.DBMapping.Models.ADMIN_LOG>();
            this.CALENDAR_DUTY = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>)new List<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>();
            this.CALENDAR_DUTY1 = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>)new List<BachMaiCR.DBMapping.Models.CALENDAR_DUTY>();
            this.TEMPLATES = (ICollection<TEMPLATE>)new List<TEMPLATE>();
            this.TEMPLATES1 = (ICollection<TEMPLATE>)new List<TEMPLATE>();
            this.USERS_ACTIONS = (ICollection<BachMaiCR.DBMapping.Models.USERS_ACTIONS>)new List<BachMaiCR.DBMapping.Models.USERS_ACTIONS>();
            this.WEBPAGES_ROLES = (ICollection<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>)new List<BachMaiCR.DBMapping.Models.WEBPAGES_ROLES>();
        }
    }
}