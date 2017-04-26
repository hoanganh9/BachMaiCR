using System;
using System.Collections.Generic;

namespace BachMaiCR.DBMapping.Models
{
    public class DOCTOR
    {
        public int DOCTORS_ID { get; set; }

        public string DOCTOR_NAME { get; set; }

        public string CODE_STAFF { get; set; }

        public int DOCTOR_GROUP_ID { get; set; }

        public string ABBREVIATION { get; set; }

        public bool? GENDER { get; set; }

        public DateTime? BIRTHDAY { get; set; }

        public string INSURANCE_NUMBER { get; set; }

        public string INSURANCE_REGISTER { get; set; }

        public string IDENTITY_CARD { get; set; }

        public string IDENTITY_PLACE { get; set; }

        public DateTime? IDENTITY_DATE { get; set; }

        public string NATION { get; set; }

        public string RELIGION { get; set; }

        public string PLACE_BIRTH { get; set; }

        public int? DOCTOR_ORDER { get; set; }

        public string PROVINCES { get; set; }

        public string DISTRICTS { get; set; }

        public string VILLAGE { get; set; }

        public string ADDRESS { get; set; }

        public string PHONE { get; set; }

        public string EMAIL { get; set; }

        public string DOCTOR_IMAGE { get; set; }

        public DateTime? DATE_START { get; set; }

        public int DOCTOR_LEVEL_ID { get; set; }

        public int? PRESENT_WORK_ID { get; set; }

        public string EDUCATION_IDs { get; set; }

        public string EDUCATION_NAMEs { get; set; }

        public string POSITION_IDs { get; set; }

        public string POSITION_NAMEs { get; set; }

        public string LM_DEPARTMENT_IDs { get; set; }

        public string LM_DEPARTMENT_NAMEs { get; set; }

        public bool? ISDELETE { get; set; }

        public bool? ISACTIVED { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE> CALENDAR_CHANGE { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR> CALENDAR_DOCTOR { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_DIRECT> CONFIG_DIRECT { get; set; }

        public virtual ICollection<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS> CONFIG_HOLIDAYS { get; set; }

        public virtual DOCTOR_GROUPS DOCTOR_GROUPS { get; set; }

        public virtual DOCTOR_LEVEL DOCTOR_LEVEL { get; set; }

        public DOCTOR()
        {
            this.CALENDAR_CHANGE = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE>)new List<BachMaiCR.DBMapping.Models.CALENDAR_CHANGE>();
            this.CALENDAR_DOCTOR = (ICollection<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR>)new List<BachMaiCR.DBMapping.Models.CALENDAR_DOCTOR>();
            this.CONFIG_DIRECT = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>)new List<BachMaiCR.DBMapping.Models.CONFIG_DIRECT>();
            this.CONFIG_HOLIDAYS = (ICollection<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>)new List<BachMaiCR.DBMapping.Models.CONFIG_HOLIDAYS>();
        }
    }
}