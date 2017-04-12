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
    public class ConfigHoliday
    {
        public ConfigHoliday()
        {

        }

        public int ConfigHolidayId { get; set; }
        [LocalizationDisplay("LabelDepartment")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int DeparmentId { get; set; }
        public string DeparmentName { get; set; }

        [LocalizationDisplay("LabelDoctor")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int DoctorId { get; set; }
        //[UIHint("Date")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyFeastFromDate", ErrorMessageResourceType = typeof(Resources.Localization))]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = " {0:dd/MM/yyyy}")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date_Begin { get; set; }
        //[UIHint("Date")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyFeastToDate", ErrorMessageResourceType = typeof(Resources.Localization))]
        //[DisplayFormat(NullDisplayText = "", DataFormatString = " {0:dd/MM/yyyy}")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date_End { get; set; }

        [LocalizationDisplay("LableTypeOfHoliday")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? HolidayId { get; set; }
        public bool? IsDeleted { get; set; }




        public ConfigHoliday(CONFIG_HOLIDAYS entity)
        {
            this.ConfigHolidayId = entity.CONFIG_HOLIDAY_ID;
            this.DeparmentId = entity.LM_DEPARTMENT_ID;
            this.DoctorId = entity.DOCTORS_ID;
            this.Date_Begin = entity.DATE_BEGIN;
            this.Date_End = entity.DATE_END;
            this.HolidayId = entity.HOLIDAYS_ID;
            this.IsDeleted = entity.ISDELETE;

            if (entity.LM_DEPARTMENT_ID != null)
            {
                DeparmentName = entity.LM_DEPARTMENT.DEPARTMENT_NAME;
            }
            else
            {
                DeparmentName = "";
            }
        }

        public CONFIG_HOLIDAYS GetConfigHoliday()
        {
            var entity = new CONFIG_HOLIDAYS();
            entity.CONFIG_HOLIDAY_ID = this.ConfigHolidayId;
            entity.LM_DEPARTMENT_ID = this.DeparmentId;
            entity.DOCTORS_ID = this.DoctorId;
            entity.DATE_BEGIN = this.Date_Begin;
            entity.DATE_END = this.Date_End;
            entity.HOLIDAYS_ID = this.HolidayId;
            entity.ISDELETE = this.IsDeleted;

            return entity;
        }
    }
}