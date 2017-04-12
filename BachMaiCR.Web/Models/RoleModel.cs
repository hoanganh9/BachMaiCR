using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DataAccess;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class RoleModel
    {
        public int WEBPAGES_ROLES_ID { get; set; }

        [LocalizationDisplay("RoleModelDisplayName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhóm người dùng không được để trống")]
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string ROLES_NAME { get; set; }

        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string ABBREVIATION { get; set; }


        [StringLength(200, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string DESCRIPTION { get; set; }

        [LocalizationDisplay("RoleModelDisplayIsActive")]
        public bool ISACTIVE { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public Nullable<int> CREATE_USERID { get; set; }

        [LocalizationDisplay("UserModelDisplayNameDepartment")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phải nhập phòng ban")]
        public int LM_DEPARTMENT_ID { get; set; }

        public string LM_DEPARTMENT_NAME { get; set; }

        public RoleModel(WEBPAGES_ROLES role)
        {
            WEBPAGES_ROLES_ID = role.WEBPAGES_ROLE_ID;
            ROLES_NAME = role.ROLE_NAME;
            DESCRIPTION = role.DESCRIPTION;
            ISACTIVE = role.ISACTIVE ?? false;
            CREATE_DATE = role.CREATE_DATE;
            CREATE_USERID = role.CREATE_USERID == null ? 0 : (int)role.CREATE_USERID;
            LM_DEPARTMENT_ID = role.LM_DEPARTMENT_ID == null ? 0 : (int)role.LM_DEPARTMENT_ID;
            ABBREVIATION = role.ABBREVIATION;
            LM_DEPARTMENT_ID = role.LM_DEPARTMENT_ID==null?0:(int)role.LM_DEPARTMENT_ID;
            IUnitOfWork unitOfWork = new UnitOfWork();
            LM_DEPARTMENT department = unitOfWork.Departments.GetById(LM_DEPARTMENT_ID);
            if (department != null)
            {
                LM_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
            }
            else
            {
                LM_DEPARTMENT_NAME = "";
            }
        }

        public RoleModel()
        {
        }

        //public int? LM_DEPARTMENT_ID { get; set; }
    }
}