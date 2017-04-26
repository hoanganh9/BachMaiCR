using System;

namespace BachMaiCR.DBMapping.Models
{
    public class DoctorHospital
    {
        public int CALENDAR_DUTY_ID { get; set; }

        public int DOCTORS_ID { get; set; }

        public string DOCTOR_NAME { get; set; }

        public string ABBREVIATION { get; set; }

        public string PHONE { get; set; }

        public string EMAIL { get; set; }

        public int? LEVEL_NUMBER { get; set; }

        public int? CALENDAR_MONTH { get; set; }

        public int? CALENDAR_YEAR { get; set; }

        public DateTime? DATE_START { get; set; }

        public int ISAPPROVED { get; set; }

        public int? CALENDAR_STATUS { get; set; }

        public int? TEMPLATE_COLUM_ID { get; set; }

        public int LM_DEPARTMENT_ID { get; set; }

        public int CALENDAR_DATA_ID { get; set; }

        public int? DUTY_TYPE { get; set; }

        public string LM_DEPARTMENT_IDs { get; set; }
    }
}