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
    public class Category
    {
        public Category()
        {

        }

        //[]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgContentEmptyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LableCodeDisplay")]
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Code { get; set; }

        public int Id { get; set; }
        //
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgContentEmptyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(500, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Name { get; set; }




        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? Type { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? Parent { get; set; }


        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int Index { get; set; }

        [LocalizationDisplay("LableDesriptionDisplay")]
        [StringLength(500, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Description { get; set; }
        public bool? IsDel { get; set; }
        public int? Status { get; set; }


        public Category(LM_CATEGORY entity)
        {
            this.Id = entity.LM_CATEGORY_ID;
            this.Code = entity.CODE;
            this.Name = entity.CATEGORY_NAME;
            this.Type = entity.CATEGORY_STYLE;
            this.Parent = entity.CATEGORY_PARENT;
            this.Description = entity.CATEGORY_DESCRIPTION;
            this.IsDel = entity.ISDELETE;
            this.Status = entity.CATEGORY_STATUS;
            //i
        }

        public LM_CATEGORY GetCategoryModel()
        {
            var entity = new LM_CATEGORY();
            entity.LM_CATEGORY_ID = this.Id;
            entity.CODE = string.IsNullOrEmpty(this.Code) ? string.Empty : this.Code.Trim();
            entity.CATEGORY_NAME = string.IsNullOrEmpty(this.Name) ? string.Empty : this.Name.Trim();
            entity.CATEGORY_STYLE = this.Type;
            entity.CATEGORY_PARENT = this.Parent;
            entity.CATEGORY_DESCRIPTION = string.IsNullOrEmpty(this.Description) ? string.Empty : this.Description.Trim();
            entity.ISDELETE = this.IsDel;
            entity.CATEGORY_STATUS = this.Status;
            return entity;
        }
    }
}