using System;

namespace BachMaiCR.DBMapping.Models
{
    public class DoctorCalendarLeader
    {
        public string DOCTOR_NAME { get; set; }

        public DateTime? DATE_START { get; set; }

        public int CALENDAR_DUTY_ID { get; set; }

        public int DOCTORS_ID { get; set; }

        public string CALENDAR_NAME { get; set; }

        public string CALENDAR_CONTENT { get; set; }

        public string CALENDAR_PHONE { get; set; }

        public DateTime? DATE_END { get; set; }

        public int? CALENDAR_MONTH { get; set; }

        public int? CALENDAR_YEAR { get; set; }

        public int CALENDAR_DATA_ID { get; set; }

        public int? DUTY_TYPE { get; set; }

        public int LM_DEPARTMENT_ID { get; set; }

        public string POSITION_IDs { get; set; }

        public string CALENDAR_DUTY_NAME { get; set; }

        public string POSITION_NAMEs { get; set; }

        public string ABBREVIATION { get; set; }

        public int DOCTOR_GROUP_ID { get; set; }

        public int CALENDAR_DOCTOR_ID { get; set; }

        public int? CALENDAR_STATUS { get; set; }

        public int? TEMPLATES_ID { get; set; }

        public string PHONE { get; set; }

        public string LM_DEPARTMENT_IDs { get; set; }

        public string LM_DEPARTMENT_NAMEs { get; set; }
    }
}