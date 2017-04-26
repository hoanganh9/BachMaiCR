using System;

namespace BachMaiCR.DBMapping.Models
{
    public class REPORT
    {
        public int REPORT_ID { get; set; }

        public string REPORT_NAME { get; set; }

        public DateTime DATE_CREATE { get; set; }

        public DateTime? DATE_SENDED { get; set; }

        public int LM_DEPARTMENT_ID { get; set; }

        public string LM_DEPARTMENT_NAME { get; set; }

        public int USER_CREATE_ID { get; set; }

        public string USER_CREATE_NAME { get; set; }

        public string USER_RECIPIENTS_IDs { get; set; }

        public string USER_RECIPIENTS_NAMEs { get; set; }

        public int STATUS { get; set; }

        public int NUMBER_STAFF_DIRECT { get; set; }

        public int NUMBER_STAFF_LEAVE { get; set; }

        public int NUMBER_STAFF_VACATION { get; set; }

        public int NUMBER_STAFF_SICK { get; set; }

        public int NUMBER_STAFF_MATERNITY { get; set; }

        public int NUMBER_STAFF_UNPAID { get; set; }

        public int NUMBER_STAFF_BUSINESS_TRIP { get; set; }

        public bool? ISDELETE { get; set; }
    }
}