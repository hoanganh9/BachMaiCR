using System;

namespace BachMaiCR.DBMapping.ModelsExt
{
    public class SearchCalendarDuty
    {
        public DateTime? DATE_CREATE { get; set; }

        public DateTime? DATE_APPROVED { get; set; }

        public string DATE_CREATES { get; set; }

        public string DATE_APPROVEDS { get; set; }

        public string ADMIN_USER_CREATE { get; set; }

        public string ADMIN_USER_APPROVED { get; set; }

        public int CALENDAR_STATUS { get; set; }

        public int DATE_MONTH { get; set; }

        public int DATE_YEAR { get; set; }

        public string FEAST { get; set; }

        public int TEMPLATES { get; set; }

        public string DEPARTMENTS { get; set; }
    }
}