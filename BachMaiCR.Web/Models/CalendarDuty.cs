using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class CalendarDutyModel
    {
      
        public int CALENDAR_DUTY_ID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyCalendarHospital", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(300, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string CALENDAR_NAME { get; set; }
        public Nullable<System.DateTime> DATE_CREATE { get; set; }
        public Nullable<System.DateTime> DATE_APPROVED { get; set; }
        public Nullable<int> USER_CREATE_ID { get; set; }
        public Nullable<int> USER_APPROVED_ID { get; set; }
        public Nullable<int> CALENDAR_STATUS { get; set; }
        public Nullable<int> CALENDAR_MONTH { get; set; }
        public Nullable<int> CALENDAR_YEAR { get; set; }
        public Nullable<int> DUTY_TYPE { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public Nullable<int> TEMPLATES_ID { get; set; }
        public int LM_DEPARTMENT_ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyContentCalendar", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(300, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string COMMENTS { get; set; }
        public virtual ADMIN_USER ADMIN_USER { get; set; }
        public virtual ADMIN_USER ADMIN_USER1 { get; set; }
        public virtual ICollection<CALENDAR_DATA> CALENDAR_DATA { get; set; }
        public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }
        public virtual TEMPLATE TEMPLATE { get; set; }
    }
}
