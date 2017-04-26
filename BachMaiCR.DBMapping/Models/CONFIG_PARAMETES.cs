using System;

namespace BachMaiCR.DBMapping.Models
{
    public class CONFIG_PARAMETES
    {
        public int CONFIG_PARAMETES_ID { get; set; }

        public int? LM_DEPARTMENT_ID { get; set; }

        public int TIME_DISTANCE { get; set; }

        public bool IS_FIX_WEEKEND { get; set; }

        public int? TIME_DISTANCE_OF_HOLIDAY { get; set; }

        public int? NUMBER_DOCTOR_IN_DAY { get; set; }

        public int? NORMS_DIRECT { get; set; }

        public bool IS_FEMALE_DIRECT_AM { get; set; }

        public string DESCRIPTION { get; set; }

        public int CONFIG_YEAR { get; set; }

        public int? CONFIG_TYPE { get; set; }

        public int? USER_CREATE_ID { get; set; }

        public DateTime? DATE_CREATE { get; set; }
    }
}