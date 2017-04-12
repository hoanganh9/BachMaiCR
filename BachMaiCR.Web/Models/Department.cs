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
    public class Department
    {
        public Department()
        {

        }

        //[]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyCodeDepartment", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelDepartmentCode")]
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Code { get; set; }

        public int Id { get; set; }
        //

        [LocalizationDisplay("LabelDepartmentName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyNameDepartment", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(150, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Name { get; set; }

        [LocalizationDisplay("LabelAddress")]
        [StringLength(250, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Address { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyPhoneDepartment", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(20, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessageResourceName = "MsgPhoneInvalid", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Phone { get; set; }

        [StringLength(20, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string FaxNum { get; set; }


        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? Parent { get; set; }


        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int Index { get; set; }

        [LocalizationDisplay("LableDesriptionDisplay")]
        [StringLength(500, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Description { get; set; }

        [StringLength(300, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Note { get; set; }
        public bool? IsDel { get; set; }
        public int? Status { get; set; }


        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? Level { get; set; }
        public Department(LM_DEPARTMENT entity)
        {
            this.Id = entity.LM_DEPARTMENT_ID;
            this.Code = string.IsNullOrEmpty(entity.DEPARTMENT_CODE) ? string.Empty : entity.DEPARTMENT_CODE.Trim();
            this.Name = string.IsNullOrEmpty(entity.DEPARTMENT_NAME) ? string.Empty : entity.DEPARTMENT_NAME.Trim();
            this.Phone = string.IsNullOrEmpty(entity.DEPARTMENT_PHONE) ? string.Empty : entity.DEPARTMENT_PHONE.Trim();
            this.Address = string.IsNullOrEmpty(entity.DEPARTMENT_ADDRESS) ? string.Empty : entity.DEPARTMENT_ADDRESS.Trim();
            this.FaxNum = string.IsNullOrEmpty(entity.DEPARTMENT_FAX) ? string.Empty : entity.DEPARTMENT_FAX.Trim();
            this.Description = string.IsNullOrEmpty(entity.DESCRIPTION) ? string.Empty : entity.DESCRIPTION.Trim();
            this.Path = string.IsNullOrEmpty(entity.DEPARTMENT_PATH) ? string.Empty : entity.DEPARTMENT_PATH.Trim();
            this.Parent = entity.DEPARTMENT_PARENT_ID;
            this.IsDel = entity.ISDELETE;
            //i
        }

        /// <summary>
        /// Hàm chuyển đổi từ Model sang Model Mapping
        /// </summary>
        /// <returns></returns>
        public LM_DEPARTMENT GetCategoryModel()
        {
            var entity = new LM_DEPARTMENT();
            entity.LM_DEPARTMENT_ID = this.Id;
            entity.DEPARTMENT_NAME = string.IsNullOrEmpty(this.Name) ? string.Empty : this.Name.Trim();
            entity.DEPARTMENT_CODE = string.IsNullOrEmpty(this.Code) ? string.Empty : this.Code.Trim();
            entity.DEPARTMENT_ADDRESS = string.IsNullOrEmpty(this.Address) ? string.Empty : this.Address.Trim();
            entity.DEPARTMENT_PHONE = string.IsNullOrEmpty(this.Phone) ? string.Empty : this.Phone.Trim();
            entity.DEPARTMENT_FAX = string.IsNullOrEmpty(this.FaxNum) ? string.Empty : this.FaxNum.Trim();
            entity.DEPARTMENT_PATH = string.IsNullOrEmpty(this.Path) ? string.Empty : this.Path.Trim();
            entity.DESCRIPTION = string.IsNullOrEmpty(this.Description) ? string.Empty : this.Description.Trim();
            entity.LEVELS = this.Level;
            entity.ISDELETE = this.IsDel;
            entity.DEPARTMENT_PARENT_ID = this.Parent;
            return entity;
        }

        public string Path { get; set; }
    }
}