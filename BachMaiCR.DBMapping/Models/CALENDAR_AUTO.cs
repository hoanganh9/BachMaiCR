using System;

namespace BachMaiCR.DBMapping.Models
{
    public class CALENDAR_AUTO
    {
        public int CALENDAR_AUTO_ID { get; set; }

        public int? LM_DEPARTMENT_ID { get; set; }

        public int? TEMPLATES_ID { get; set; }

        public int? TEMPLATE_COLUM_ID { get; set; }

        public int? DOCTORS_ID { get; set; }

        public DateTime? DATE_CREATE { get; set; }
    }
}