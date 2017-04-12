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
    public class ConfigParametter
    {
        public ConfigParametter()
        {
            this.TIME_DISTANCE = 0;
            this.TIME_DISTANCE_OF_HOLIDAY = 0;
            this.NUMBER_DOCTOR_IN_DAY = 0;
            this.NORMS_DIRECT = 0;          
        }

        public int CONFIG_PARAMETES_ID { get; set; }
        [LocalizationDisplay("Template_Department")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? LM_DEPARTMENT_ID { get; set; }
        /// <summary>
        /// Khoảng cách thời gian giữa các lần trực
        /// </summary>
        [Digit]
        [LocalizationDisplay("LabelTimeDistance")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyTimeDistance", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int TIME_DISTANCE { get; set; }
        /// <summary>
        /// Không xếp lịch cố định 1 ngày trong tuần
        /// </summary>
        [LocalizationDisplay("LabelFixWeekend")]
        public bool IS_FIX_WEEKEND { get; set; }
        /// <summary>
        /// Thời gian trực tiếp sau thời gian nghỉ 
        /// </summary>
        [Digit]
        [LocalizationDisplay("LabelTimeDistanceOfHoliday")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyTimeDistanceOfHoliday", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? TIME_DISTANCE_OF_HOLIDAY { get; set; }
        /// <summary>
        /// Số lượng cán bộ trong 1 ngày 
        /// </summary>
        [Digit]
        [LocalizationDisplay("LabelNumberDoctorInDay")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyNumberDoctorInDay", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? NUMBER_DOCTOR_IN_DAY { get; set; }
        /// <summary>
        /// Định mức trực của cán bộ trong 1 năm
        /// </summary>
        [Digit]
        [LocalizationDisplay("LabelNormsDirect")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyNormsDirect", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? NORMS_DIRECT { get; set; }
        [LocalizationDisplay("LabelFemaleDirectAM")]
        public bool IS_FEMALE_DIRECT_AM { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        [LocalizationDisplay("LableDesriptionDisplay")]
        [StringLength(250, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string DESCRIPTION { get; set; }
        public int CONFIG_YEAR { get; set; }
        public int? CONFIG_TYPE { get; set; }
        public int? USER_CREATE_ID { get; set; }
        public DateTime? DATE_CREATE { get; set; }
        public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }
        public ConfigParametter(CONFIG_PARAMETES entity)
        {
            this.CONFIG_PARAMETES_ID = entity.CONFIG_PARAMETES_ID;
            this.LM_DEPARTMENT_ID = entity.LM_DEPARTMENT_ID;
            this.TIME_DISTANCE = entity.TIME_DISTANCE;
            this.IS_FIX_WEEKEND = entity.IS_FIX_WEEKEND;
            this.TIME_DISTANCE_OF_HOLIDAY = entity.TIME_DISTANCE_OF_HOLIDAY;
            this.NUMBER_DOCTOR_IN_DAY = entity.NUMBER_DOCTOR_IN_DAY;
            this.NORMS_DIRECT = entity.NORMS_DIRECT;

            this.IS_FEMALE_DIRECT_AM = entity.IS_FEMALE_DIRECT_AM;
            this.DESCRIPTION = entity.DESCRIPTION;
            this.CONFIG_YEAR = entity.CONFIG_YEAR;
            this.CONFIG_TYPE = entity.CONFIG_TYPE;
        }

        public CONFIG_PARAMETES GetConfigParametter()
        {
            var entity = new CONFIG_PARAMETES();
            entity.CONFIG_PARAMETES_ID = this.CONFIG_PARAMETES_ID;
            entity.LM_DEPARTMENT_ID = this.LM_DEPARTMENT_ID;
            entity.TIME_DISTANCE = this.TIME_DISTANCE;
            entity.IS_FIX_WEEKEND = this.IS_FIX_WEEKEND;
            entity.TIME_DISTANCE_OF_HOLIDAY = this.TIME_DISTANCE_OF_HOLIDAY;
            entity.NUMBER_DOCTOR_IN_DAY = this.NUMBER_DOCTOR_IN_DAY;
            entity.NORMS_DIRECT = this.NORMS_DIRECT;

            entity.IS_FEMALE_DIRECT_AM = this.IS_FEMALE_DIRECT_AM;
            entity.DESCRIPTION = this.DESCRIPTION;
            entity.CONFIG_YEAR = this.CONFIG_YEAR;
            entity.CONFIG_TYPE = this.CONFIG_TYPE;

            return entity;
        }
    }
}