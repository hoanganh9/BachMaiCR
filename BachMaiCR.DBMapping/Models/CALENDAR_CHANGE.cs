using System;

namespace BachMaiCR.DBMapping.Models
{
    public class CALENDAR_CHANGE
    {
        public int CALENDAR_CHANGE_ID { get; set; }

        public int? CALENDAR_DUTY_ID { get; set; }

        public int? TEMPLATE_COLUMN_ID { get; set; }

        public DateTime? DATE_START { get; set; }

        public DateTime? DATE_CHANGE_START { get; set; }

        public int? DOCTORS_ID { get; set; }

        public string DOCTORS_NAME { get; set; }

        public int? DOCTORS_CHANGE_ID { get; set; }

        public string DOCTORS_CHANGE_NAME { get; set; }

        public DateTime? DATE_CHANGE { get; set; }

        public int? ADMIN_USER_ID { get; set; }

        public int? STATUS { get; set; }

        public int? STATUS_APPROVED { get; set; }

        public int? CALENDAR_DELETE { get; set; }

        public int? USER_NOT_APPROVED { get; set; }

        public int? COLUMN_CHANGE_ID { get; set; }

        public virtual CALENDAR_DUTY CALENDAR_DUTY { get; set; }

        public virtual DOCTOR DOCTOR { get; set; }
    }
}