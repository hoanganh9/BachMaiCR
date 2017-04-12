using System;
using System.Collections.Generic;

namespace BachMaiCR.Web.Models
{
    public partial class CalendarChangeModel
    {
      
        public int CALENDAR_CHANGE_ID { get; set; }
        public Nullable<int> CALENDAR_DUTY_ID { get; set; }
        public Nullable<int> TEMPLATE_COLUMN_ID { get; set; }
        public Nullable<System.DateTime> DATE_START { get; set; }
        public Nullable<System.DateTime> DATE_CHANGE_START { get; set; }
        public Nullable<int> DOCTORS_ID { get; set; }
        public string DOCTORS_NAME { get; set; }
        public Nullable<int> DOCTORS_CHANGE_ID { get; set; }
        public string DOCTORS_CHANGE_NAME { get; set; }
        public Nullable<System.DateTime> DATE_CHANGE { get; set; }
        public Nullable<int> ADMIN_USER_ID { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<int> STATUS_APPROVED { get; set; }
        public int CALENDAR_DELETE { get; set; }
        public Nullable<int> USER_NOT_APPROVED { get; set; }
        public Nullable<int> COLUMN_CHANGE_ID { get; set; }
    }
}
