using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class ConfigSMS
    {
        public ConfigSMS()
        {
            this.ISACTIVED = true;
            this.DATE_START = DateTime.Now;
        }
        public int CONFIG_SMS_ID { get; set; }
        [LocalizationDisplay("LabelSMSName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptySMSName", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(150, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string CONFIG_SMS_NAME { get; set; }

        [LocalizationDisplay("LabelDepartment")]
        public int? ALARM_TIME_HOUR { get; set; }
        public int? ALARM_TIME_DAY { get; set; }

        public int? ALARM_COUNT { get; set; }
        [LocalizationDisplay("LabelDepartment")]
        public int? REPEAT_ALARM_HOUR { get; set; }
        [LocalizationDisplay("LabelDepartment")]
        public int? REPEAT_ALARM_MINUTES { get; set; }
        [LocalizationDisplay("LabelSMSDeparmentApply")]
        public int LM_DEPARTMENT_ID { get; set; }

        public string LM_DEPARTMENT_NAME { get; set; }
        [LocalizationDisplay("LabelSMSTemplateApply")]
        public int? TEMPLATES_ID { get; set; }
        public bool ISACTIVED { get; set; }
        public bool? ISDELETE { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyFeastFromDate", ErrorMessageResourceType = typeof(Resources.Localization))]
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }


        public ConfigSMS(CONFIG_SMS entity)
        {
            this.CONFIG_SMS_ID = entity.CONFIG_SMS_ID;
            this.CONFIG_SMS_NAME = entity.CONFIG_SMS_NAME;
            this.ALARM_TIME_HOUR = entity.ALARM_TIME_HOUR;
            this.ALARM_TIME_DAY = entity.ALARM_TIME_DAY;
            this.ALARM_COUNT = entity.ALARM_COUNT;
            this.REPEAT_ALARM_HOUR = entity.REPEAT_ALARM_HOUR;
            this.REPEAT_ALARM_MINUTES = entity.REPEAT_ALARM_MINUTES;
            this.LM_DEPARTMENT_ID = entity.LM_DEPARTMENT_ID;
            this.TEMPLATES_ID = entity.TEMPLATES_ID;
            this.ISACTIVED = entity.ISACTIVED;
            this.ISDELETE = entity.ISDELETE;
            this.DATE_START = entity.DATE_START;
            this.DATE_END = entity.DATE_END;
            if (entity.LM_DEPARTMENT_ID != null)
            {
                LM_DEPARTMENT_NAME = entity.LM_DEPARTMENT.DEPARTMENT_NAME;
            }
            else
            {
                LM_DEPARTMENT_NAME = "";
            }
        }

        public CONFIG_SMS GetConfigSMS()
        {
            var entity = new CONFIG_SMS();
            entity.CONFIG_SMS_ID = this.CONFIG_SMS_ID;
            entity.CONFIG_SMS_NAME = this.CONFIG_SMS_NAME;
            entity.ALARM_TIME_HOUR = this.ALARM_TIME_HOUR;
            entity.ALARM_TIME_DAY = this.ALARM_TIME_DAY;
            entity.ALARM_COUNT = this.ALARM_COUNT;
            entity.REPEAT_ALARM_HOUR = this.REPEAT_ALARM_HOUR;
            entity.REPEAT_ALARM_MINUTES = this.REPEAT_ALARM_MINUTES;
            entity.LM_DEPARTMENT_ID = this.LM_DEPARTMENT_ID;
            entity.TEMPLATES_ID = this.TEMPLATES_ID;
            entity.ISACTIVED = this.ISACTIVED;
            entity.ISDELETE = this.ISDELETE;
            entity.DATE_START = this.DATE_START;
            entity.DATE_END = this.DATE_END;

            return entity;
        }
    }
}