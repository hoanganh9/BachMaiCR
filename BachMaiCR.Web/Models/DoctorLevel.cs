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
    public class DoctorLevel
    {
        public DoctorLevel()
        {

        }

        public int Id { get; set; }

        //[]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgContentEmptyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LableCodeDisplay")]
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Code { get; set; }

        //
        [LocalizationDisplay("LableDoctorLevelName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgContentEmptyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(250, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Name { get; set; }

        [Digit]
        [LocalizationDisplay("LableDoctorLevel")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgContentEmptyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [Range(0, 100, ErrorMessage = "Giá trị lớn hơn hoặc bằng 0 và nhỏ hơn hoặc bằng 100")]
        public int? Level { get; set; }

        [LocalizationDisplay("MsgNumberOnlyRequired")]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? Index { get; set; }

        [LocalizationDisplay("LableDesriptionDisplay")]
        public bool? IsDel { get; set; }


        public DoctorLevel(DOCTOR_LEVEL entity)
        {
            this.Id = entity.DOCTOR_LEVEL_ID;
            this.Code = entity.CODE;
            this.Name = entity.LEVEL_NAME;
            this.Level = entity.LEVEL_NUMBER;
            this.Index = entity.LEVEL_ORDER;
            this.IsDel = entity.ISDELETE;
        }

        public DOCTOR_LEVEL GetCategoryModel()
        {
            var entity = new DOCTOR_LEVEL();
            entity.DOCTOR_LEVEL_ID = this.Id;
            entity.LEVEL_NAME = string.IsNullOrEmpty(this.Name) ? string.Empty : this.Name.Trim();
            entity.CODE = string.IsNullOrEmpty(this.Code) ? string.Empty : this.Code.Trim();
            entity.LEVEL_ORDER = this.Index;
            entity.LEVEL_NUMBER = this.Level;
            entity.ISDELETE = this.IsDel;
            return entity;
        }
    }
}