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
    public class ReportOfDay
    {
        public ReportOfDay()
        {
            this.StaffBusTrip = 0;
            this.StaffDirect = 0;
            this.StaffLeave = 0;
            this.StaffMaternity = 0;
            this.StaffSick = 0;
            this.StaffUnpaid = 0;
            this.StaffVacation = 0;
        }
        public int Id { get; set; }

        [LocalizationDisplay("LabelReportName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyNameReport", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(250, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Name { get; set; }

        [LocalizationDisplay("LabelDateCreate")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgContentEmptyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public DateTime DateCreate { get; set; }

        [LocalizationDisplay("LabelDateSend")]
        public DateTime? DateSent { get; set; }

        public int DeptId { get; set; }

        [LocalizationDisplay("LabelDepartment")]
        public string DeptName { get; set; }

        public int UserCreateId { get; set; }

        [LocalizationDisplay("LabelUserCreate")]
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string UserCreateName { get; set; }

        [LocalizationDisplay("LabelRecipient")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyUserRecipient", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(250, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string UserRecipientId { get; set; }

        [LocalizationDisplay("LabelRecipient")]
        [StringLength(500, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string UserRecipientName { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffVacation")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffVacation", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffVacation { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffLeave")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffLeave", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffLeave { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffSick")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffSick", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffSick { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffMaternity")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffMaternity", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffMaternity { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffUnpaid")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffUnpaid", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffUnpaid { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffBusinessTrip")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffBusinessTrip", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffBusTrip { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStatus")]
        [Range(0, 1000, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int Status { get; set; }

        [Digit]
        [LocalizationDisplay("LabelStaffDirect")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyStaffDirect", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberRangeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int StaffDirect { get; set; }

        public bool? IsDel { get; set; }

        public ReportOfDay(REPORT entity)
        {
            this.Id = entity.REPORT_ID;
            this.Name = string.IsNullOrEmpty(entity.REPORT_NAME) ? string.Empty : entity.REPORT_NAME.Trim();
            this.UserCreateName = string.IsNullOrEmpty(entity.USER_CREATE_NAME) ? string.Empty : entity.USER_CREATE_NAME.Trim();
            this.UserRecipientId = string.IsNullOrEmpty(entity.USER_RECIPIENTS_IDs) ? string.Empty : entity.USER_RECIPIENTS_IDs.Trim();
            this.UserRecipientName = string.IsNullOrEmpty(entity.USER_RECIPIENTS_NAMEs) ? string.Empty : entity.USER_RECIPIENTS_NAMEs.Trim();
            this.DateCreate = entity.DATE_CREATE;
            this.DateSent = entity.DATE_SENDED;
            this.UserCreateId = entity.USER_CREATE_ID;
            this.Status = entity.STATUS;
            this.StaffDirect = entity.NUMBER_STAFF_DIRECT;
            this.StaffLeave = entity.NUMBER_STAFF_LEAVE;
            this.StaffSick = entity.NUMBER_STAFF_SICK;
            this.StaffMaternity = entity.NUMBER_STAFF_MATERNITY;
            this.StaffUnpaid = entity.NUMBER_STAFF_UNPAID;
            this.StaffBusTrip = entity.NUMBER_STAFF_BUSINESS_TRIP;
            this.StaffVacation = entity.NUMBER_STAFF_VACATION;
            this.DeptId = entity.LM_DEPARTMENT_ID;
            this.DeptName = entity.LM_DEPARTMENT_NAME;

        }

        /// <summary>
        /// Get Model mapping from DataBase
        /// </summary>
        public REPORT GetModelMap
        {
            get
            {
                var entity = new REPORT();
                entity.REPORT_ID = this.Id;
                entity.REPORT_NAME = this.Name;
                entity.USER_CREATE_NAME = string.IsNullOrEmpty(this.UserCreateName) ? string.Empty : this.UserCreateName.Trim();
                entity.USER_RECIPIENTS_IDs = string.IsNullOrEmpty(this.UserRecipientId) ? string.Empty : this.UserRecipientId.Trim();
                entity.USER_RECIPIENTS_NAMEs = string.IsNullOrEmpty(this.UserRecipientName) ? string.Empty : this.UserRecipientName.Trim();
                entity.DATE_CREATE = this.DateCreate;
                entity.DATE_SENDED = this.DateSent;
                entity.USER_CREATE_ID = this.UserCreateId;
                entity.STATUS = this.Status;
                entity.NUMBER_STAFF_VACATION = this.StaffVacation;
                entity.NUMBER_STAFF_DIRECT = this.StaffDirect;
                entity.NUMBER_STAFF_LEAVE = this.StaffLeave;
                entity.NUMBER_STAFF_SICK = this.StaffSick;
                entity.NUMBER_STAFF_MATERNITY = this.StaffMaternity;
                entity.NUMBER_STAFF_UNPAID = this.StaffUnpaid;
                entity.NUMBER_STAFF_BUSINESS_TRIP = this.StaffBusTrip;
                entity.LM_DEPARTMENT_ID = this.DeptId;
                entity.LM_DEPARTMENT_NAME = string.IsNullOrEmpty(this.DeptName) ? string.Empty : this.DeptName.Trim();
                return entity;
            }
        }

        public static string GetStatus(object obj)
        {
            string status = Convert.ToString(obj);
            string vR = "";

            if (status == "0")
            {
                vR = Resources.Localization.ReportOfDayStatusCreate;
            }
            else if (status == "1")
            {
                vR = Resources.Localization.ReportOfDayStatusSend;
            }           
            return vR;
        }
    }
}