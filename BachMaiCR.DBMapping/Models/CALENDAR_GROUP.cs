namespace BachMaiCR.DBMapping.Models
{
    public class CALENDAR_GROUP
    {
        public int CALENDAR_GROUP_ID { get; set; }

        public int CALENDAR_ID { get; set; }

        public int CALENDAR_PARENT_ID { get; set; }

        public int? LM_DEPARTMENT_ID { get; set; }

        public int? CALENDAR_MONTH { get; set; }

        public int? CALENDAR_YEAR { get; set; }

        public int? CALENDAR_TYPE { get; set; }

        public int? CALENDAR_STATUS { get; set; }
    }
}