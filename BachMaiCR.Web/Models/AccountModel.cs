using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{

    public class LocalPasswordModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "LocalPasswordModelOldPasswordRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [DataType(DataType.Password)]
        [LocalizationDisplay("LocalPasswordModelOldPassword")]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "LocalPasswordModelNewPasswordRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(100, ErrorMessageResourceName = "LocalPasswordModelNewPasswordStringLength", MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Localization))]
        [DataType(DataType.Password)]
        [LocalizationDisplay("LocalPasswordModelNewPassword")]
        [StrongPassword]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizationDisplay("LocalPasswordModelConfirmPassword")]
        [Compare("NewPassword", ErrorMessageResourceName = "LocalPasswordModelNewPasswordCompare", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string ConfirmPassword { get; set; }

    }

    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "LoginModelUserNameRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LoginModelDisplayUserName")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "LoginModelPasswordRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [DataType(DataType.Password)]
        [LocalizationDisplay("LoginModelDisplayPassword")]
        public string Password { get; set; }

        [LocalizationDisplay("LoginModelDisplayRemember")]
        public bool RememberMe { get; set; }

        [DataType(DataType.Password)]
        [LocalizationDisplay("LocalCaptchaModelConfirmCaptcha")]
        public string ConfirmCaptcha { get; set; }
    }

    public class RegisterModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RegisterModelUserNameRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("RegisterModelUserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
    