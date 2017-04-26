using System;

namespace BachMaiCR.DBMapping.ModelsExt
{
    public class DoctorInCalendar
    {
        public int DOCTORS_ID { get; set; }

        public int CALENDAR_DATA_ID { get; set; }

        public int CALENDAR_DUTY_ID { get; set; }

        public DateTime? DATE_START { get; set; }

        public int? CALENDAR_STATUS { get; set; }

        public string CALENDAR_NAME { get; set; }

        public int? TEMPLATES_ID { get; set; }

        public int ISAPPROVED { get; set; }
    }
}