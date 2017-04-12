using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class UserModel
    {
        public int ADMIN_USER_ID { get; set; }

        [LocalizationDisplay("UserModelDisplayNameUserName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserModelUserNameRequired",
            ErrorMessageResourceType = typeof (Resources.Localization))]
        [Remote("CheckExistUserName", "Users", ErrorMessageResourceName = "UserModelUserNameExist",
            ErrorMessageResourceType = typeof(Resources.Localization), HttpMethod = "POST", AdditionalFields = "Id")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa là 50 ký tự")]
        public string USERNAME { get; set; }

        [LocalizationDisplay("UserModelDisplayNameCode")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa là 50 ký tự")]
        public string USERCODE { get; set; }

        [LocalizationDisplay("UserModelDisplayNameFullName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserModelFullNameRequired",
            ErrorMessageResourceType = typeof (Resources.Localization))]
        [StringLength(50, ErrorMessage = "Độ dài tối đa là 50 ký tự")]
        public string FULLNAME { get; set; }

        [LocalizationDisplay("UserModelDisplayNameSMSPhone")]
        [PhoneNumber]
        [StringLength(15, ErrorMessage = "Số điện thoại không hợp lệ", MinimumLength = 8)]
        public string PHONE { get; set; }

        [LocalizationDisplay("UserModelDisplayNameEmail")]
        //[EmailAddress]
        [StringLength(50, ErrorMessage = "Độ dài tối đa là 50 ký tự")]
        public string MAIL { get; set; }

        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }

        [LocalizationDisplay("UserModelDisplayNameIsActive")]
        public Nullable<bool> ISACTIVED { get; set; }

        public Nullable<bool> ISDELETE { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AdminUser_Validate_Password",
            ErrorMessageResourceType = typeof (Resources.Localization))]
        [StringLength(100, ErrorMessage = "Độ dài tối đa là 100 ký tự và tối thiểu là 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [StrongPassword]
        [LocalizationDisplay("AdminUser_Label_Password")]
        public string PASSWORD { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AdminUser_Validate_ConfirmPassword",
            ErrorMessageResourceType = typeof (Resources.Localization))]
        [DataType(DataType.Password)]
        [LocalizationDisplay("AdminUser_Label_Confirm_password")]
        [System.ComponentModel.DataAnnotations.Compare("PASSWORD", ErrorMessage = "Mật khẩu và Mật khẩu nhập lại không khớp nhau.")]
        public string ConfirmPassword { get; set; }


        public string SALT { get; set; }
        public Nullable<System.DateTime> RESET_PASSWORD_DATE { get; set; }
        public Nullable<bool> REQIURE_CHANGE_PASS { get; set; }
          [LocalizationDisplay("Trang mặc định")]
        public string ACTIVE_URL { get; set; }
        public Nullable<System.DateTime> ACTIVE_ENDDATE { get; set; }
        public Nullable<byte> STEP { get; set; }

        [LocalizationDisplay("UserModelDisplayNameDepartment")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phải nhập phòng ban")]
        public int LM_DEPARTMENT_ID { get; set; }

        public string LM_DEPARTMENT_NAME { get; set; }

        [LocalizationDisplay("Cán bộ")]
        public int DOCTORS_ID { get; set; }      
      
        public UserModel()
        {

        }

        public UserModel(ADMIN_USER u)
        {
            ADMIN_USER_ID = u.ADMIN_USER_ID;
            USERNAME = u.USERNAME;
            FULLNAME = u.FULLNAME;
            PHONE = u.PHONE;
            MAIL = u.MAIL;
            CREATE_DATE = u.CREATE_DATE;
            USERCODE = u.USERCODE;
            ISACTIVED = u.ISACTIVED ?? false;
            REQIURE_CHANGE_PASS = u.REQIURE_CHANGE_PASS;
            ISDELETE = u.ISDELETE;
            PASSWORD = u.PASSWORD;
            ACTIVE_URL = u.ACTIVE_URL;
            ACTIVE_ENDDATE = u.ACTIVE_ENDDATE;
            SALT = u.SALT;
            LM_DEPARTMENT_ID =(int) u.LM_DEPARTMENT_ID;
            DOCTORS_ID = u.DOCTORS_ID == null ? 0 : (int)u.DOCTORS_ID;
            if (u.LM_DEPARTMENT != null)
            {
                LM_DEPARTMENT_NAME = u.LM_DEPARTMENT.DEPARTMENT_NAME;
            }
            else
            {
                LM_DEPARTMENT_NAME = "";
            }
        }
    }

    public class UserResetPasswordModel
    {
        public int ADMIN_USER_ID { get; set; }

        public string USERNAME { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phải nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự")]
        [DataType(DataType.Password)]
        [LocalizationDisplay("Mật khẩu mới")]
        [StrongPassword]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizationDisplay("Xác nhận mật khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage="Mật khẩu xác nhận không đúng")]
        public string ConfirmPassword { get; set; }

        public UserResetPasswordModel()
        {

        }

        public UserResetPasswordModel(ADMIN_USER u)
        {
            ADMIN_USER_ID = u.ADMIN_USER_ID;
            USERNAME = u.USERNAME;
        }
    }
}