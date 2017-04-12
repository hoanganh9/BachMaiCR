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
    public class ConfigDirect
    {
        public ConfigDirect()
        {

        }

        public int ConfigDirectId { get; set; }
        [LocalizationDisplay("LabelDepartment")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int DeparmentId { get; set; }
        public string DeparmentName { get; set; }

        [LocalizationDisplay("LabelDoctor")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int DoctorId { get; set; }      
        public DateTime? Date_Begin { get; set; }     
      
        public DateTime? Date_End { get; set; }

        [LocalizationDisplay("LableTypeOfHoliday")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? FeastId { get; set; }
        public bool? IsDeleted { get; set; }




        public ConfigDirect(CONFIG_DIRECT entity)
        {
            this.ConfigDirectId = entity.CONFIG_DIRECT_ID;
            this.DeparmentId = entity.LM_DEPARTMENT_ID;
            this.DoctorId = entity.DOCTORS_ID;
            this.Date_Begin = entity.DATE_BEGIN;
            this.Date_End = entity.DATE_END;
            this.FeastId = entity.FEAST_ID;
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

        public CONFIG_DIRECT GetConfigDirect()
        {
            var entity = new CONFIG_DIRECT();
            entity.CONFIG_DIRECT_ID = this.ConfigDirectId;
            entity.LM_DEPARTMENT_ID = this.DeparmentId;
            entity.DOCTORS_ID = this.DoctorId;
            entity.DATE_BEGIN = this.Date_Begin;
            entity.DATE_END = this.Date_End;
            entity.FEAST_ID = this.FeastId;
            entity.ISDELETE = this.IsDeleted;

            return entity;
        }
    }
}