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
    public class Doctor
    {
        public Doctor()
        {

        }
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên viết tắt
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorCodeRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelShortName")]
        [StringLength(25, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Code { get; set; }

        /// <summary>
        /// Tên bác sĩ
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorNameRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(100, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelDoctorName")]
        public string Name { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(200, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelAddress")]
        public string Address { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorPhoneRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [StringLength(15, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessageResourceName = "MsgPhoneInvalid", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelPhone")]
        public string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// 
        [EmailAddress]
        [StringLength(60, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelEmail")]
        public string Email { get; set; }

        /// <summary>
        /// Thứ tự
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MsgNumberOnlyRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int? Index { get; set; }

        /// <summary>
        /// Tỉnh thành
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelProvince")]
        public string Province { get; set; }

        /// <summary>
        /// Quận huyện
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelDistricts")]
        public string District { get; set; }

        /// <summary>
        /// Đường dẫn Avatar
        /// </summary>
        [StringLength(300, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
       // [LocalizationDisplay("LabelDepartment")]
       // [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgDepartmentInvalid", ErrorMessageResourceType = typeof(Resources.Localization))]
       // [Range(1, int.MaxValue, ErrorMessageResourceName = "MsgDepartmentInvalid", ErrorMessageResourceType = typeof(Resources.Localization))]
       // public int DepartmentId { get; set; }

        /// <summary>
        /// Danh sách mã phòng ban
        /// </summary>
        [LocalizationDisplay("LabelDepartment")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgDepartmentInvalid", ErrorMessageResourceType = typeof(Resources.Localization))]
        //[Range(1, int.MaxValue, ErrorMessageResourceName = "MsgDepartmentInvalid", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string DepartmentIds { get; set; }

        public string DepartmentNames { get; set; }

        /// <summary>
        /// Vị trí
        /// </summary>
        [LocalizationDisplay("LabelLevel")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorLevelRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public int LevelID { get; set; }
        public bool? IsDel { get; set; }

        /// <summary>
        /// Ngáy sinh
        /// </summary>
        [LocalizationDisplay("LabelBirthday")]
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// Học hàm/học vị
        /// </summary>
        [LocalizationDisplay("LabelEducation")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorEducationRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string EducationIds { get; set; }

        /// <summary>
        /// Học hàm/học vị
        /// </summary>
        [LocalizationDisplay("LabelEducation")]
        public string EducationNames { get; set; }

        /// <summary>
        /// Chức danh
        /// </summary>
        [LocalizationDisplay("LabelPosition")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "DoctorJobTitleRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string PostionIds { get; set; }

        /// <summary>
        /// Chức danh
        /// </summary>
        [LocalizationDisplay("LabelPosition")]
        public string PostionNames { get; set; }


        /// <summary>
        /// Số hiệu nhân viên, mã nhân viên
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelCodeStaff")]
        public string CodeStaff { get; set; }

        /// <summary>
        /// Số sổ BHXH
        /// </summary>
        [StringLength(30, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelInsuranceNumber")]
        public string InsuranceNumber { get; set; }

        /// <summary>
        /// Số đăng ký sổ BHXH
        /// </summary>
        [StringLength(30, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelInsuranceRegister")]
        public string InsuranceRegister { get; set; }
        //LabelIndentity

        /// <summary>
        /// Số CMND
        /// </summary>
        [StringLength(20, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]     
        [LocalizationDisplay("LabelIdentity")]
        public string IdentityCard { get; set; }

        /// <summary>
        /// Nơi cấp CMND
        /// </summary>      
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelIdentityPlace")]
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>      
        [LocalizationDisplay("LabelIdentityDate")]
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        [LocalizationDisplay("LabelDateStart")]
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// Dân tộc
        /// </summary>
        [StringLength(20, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelNation")]
        public string Nation { get; set; }

        /// <summary>
        /// Tôn giáo
        /// </summary>
        [StringLength(20, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelReligion")]
        public string Religion { get; set; }


        /// <summary>
        /// Nơi sinh
        /// </summary>
        [StringLength(100, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelPlaceBirth")]
        public string PlaceBirth { get; set; }

        /// <summary>
        /// Xã phường
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelVillage")]
        public string Village { get; set; }

      

        /// <summary>
        /// Giới tính
        /// </summary>
        public bool? Gender { get; set; }

        [LocalizationDisplay("LabelDoctorGroup")]
        public int DoctorGroupId { get; set; }


        public Doctor(DOCTOR entity)
        {
            this.Id = entity.DOCTORS_ID;
            this.Name = string.IsNullOrEmpty(entity.DOCTOR_NAME) ? string.Empty : entity.DOCTOR_NAME.Trim();
            this.Code = string.IsNullOrEmpty(entity.ABBREVIATION) ? string.Empty : entity.ABBREVIATION.Trim();
            this.Address = string.IsNullOrEmpty(entity.ADDRESS) ? string.Empty : entity.ADDRESS.Trim();
            this.Phone = string.IsNullOrEmpty(entity.PHONE) ? string.Empty : entity.PHONE.Trim();
            this.Index = entity.DOCTOR_ORDER;
            this.Province = string.IsNullOrEmpty(entity.PROVINCES) ? string.Empty : entity.PROVINCES.Trim();
            this.BirthDay = entity.BIRTHDAY;
            this.AvatarUrl = string.IsNullOrEmpty(entity.DOCTOR_IMAGE) ? string.Empty : entity.DOCTOR_IMAGE.Trim();
            this.DepartmentIds = entity.LM_DEPARTMENT_IDs;
            this.LevelID = entity.DOCTOR_LEVEL_ID;
            this.IsDel = entity.ISDELETE;
            this.Gender = entity.GENDER;
            /*--------------------------------
             */
            this.CodeStaff = string.IsNullOrEmpty(entity.CODE_STAFF) ? string.Empty : entity.CODE_STAFF.Trim();
            this.InsuranceNumber = string.IsNullOrEmpty(entity.INSURANCE_NUMBER) ? string.Empty : entity.INSURANCE_NUMBER.Trim();
            this.InsuranceRegister = string.IsNullOrEmpty(entity.INSURANCE_REGISTER) ? string.Empty : entity.INSURANCE_REGISTER.Trim();
            this.IdentityCard = string.IsNullOrEmpty(entity.IDENTITY_CARD) ? string.Empty : entity.IDENTITY_CARD.Trim();
            this.IdentityPlace = string.IsNullOrEmpty(entity.IDENTITY_PLACE) ? string.Empty : entity.IDENTITY_PLACE.Trim();
            this.IdentityDate = entity.IDENTITY_DATE;
            //
            this.Nation = string.IsNullOrEmpty(entity.NATION) ? string.Empty : entity.NATION.Trim();
            this.Religion = string.IsNullOrEmpty(entity.RELIGION) ? string.Empty : entity.RELIGION.Trim();
            this.PlaceBirth = string.IsNullOrEmpty(entity.PLACE_BIRTH) ? string.Empty : entity.PLACE_BIRTH.Trim();
            this.District = string.IsNullOrEmpty(entity.DISTRICTS) ? string.Empty : entity.DISTRICTS.Trim();
            this.Village = string.IsNullOrEmpty(entity.VILLAGE) ? string.Empty : entity.VILLAGE.Trim();
            this.Email = string.IsNullOrEmpty(entity.EMAIL) ? string.Empty : entity.EMAIL.Trim();

            this.EducationIds = string.IsNullOrEmpty(entity.EDUCATION_IDs) ? string.Empty : entity.EDUCATION_IDs.Trim();
            this.EducationNames = string.IsNullOrEmpty(entity.EDUCATION_NAMEs) ? string.Empty : entity.EDUCATION_NAMEs.Trim();
            this.PostionIds = string.IsNullOrEmpty(entity.POSITION_IDs) ? string.Empty : entity.POSITION_IDs.Trim();
            this.PostionNames = string.IsNullOrEmpty(entity.POSITION_NAMEs) ? string.Empty : entity.POSITION_NAMEs.Trim();

            this.DoctorGroupId = entity.DOCTOR_GROUP_ID;
            this.DateStart = entity.DATE_START;
            this.Gender = entity.GENDER;
        }

        /// <summary>
        /// Hàm chuyển đổi từ Model sang Model Mapping
        /// </summary>
        /// <returns></returns>
        public DOCTOR GetCategoryModel()
        {
            var entity = new DOCTOR();
            entity.DOCTORS_ID = this.Id;
            entity.DOCTOR_NAME = string.IsNullOrEmpty(this.Name) ? string.Empty : this.Name.Trim();
            entity.ABBREVIATION = string.IsNullOrEmpty(this.Code) ? string.Empty : this.Code.Trim();
            entity.ADDRESS = string.IsNullOrEmpty(this.Address) ? string.Empty : this.Address.Trim();
            entity.PHONE = string.IsNullOrEmpty(this.Phone) ? string.Empty : this.Phone.Trim();
            entity.EMAIL = string.IsNullOrEmpty(this.Email) ? string.Empty : this.Email.Trim();
            entity.DATE_START = this.DateStart;
            entity.PROVINCES = string.IsNullOrEmpty(this.Province) ? string.Empty : this.Province.Trim();
            entity.DOCTOR_IMAGE = string.IsNullOrEmpty(this.AvatarUrl) ? string.Empty : this.AvatarUrl.Trim();
            entity.DOCTOR_ORDER = this.Index;
            entity.BIRTHDAY = this.BirthDay;
           // entity.LM_DEPARTMENT_ID = this.DepartmentId;
            entity.LM_DEPARTMENT_IDs = string.IsNullOrEmpty(this.DepartmentIds) ? "" : this.DepartmentIds + ",";
            entity.LM_DEPARTMENT_NAMEs = this.DepartmentNames;
            entity.DOCTOR_LEVEL_ID = this.LevelID;
            entity.ISDELETE = this.IsDel;
            entity.GENDER = this.Gender;
            //
            /*--------------------------------
            */
            entity.CODE_STAFF = string.IsNullOrEmpty(this.CodeStaff) ? string.Empty : this.CodeStaff.Trim();
            entity.INSURANCE_NUMBER = string.IsNullOrEmpty(this.InsuranceNumber) ? string.Empty : this.InsuranceNumber.Trim();
            entity.INSURANCE_REGISTER = string.IsNullOrEmpty(this.InsuranceRegister) ? string.Empty : this.InsuranceRegister.Trim();
            entity.IDENTITY_CARD = string.IsNullOrEmpty(this.IdentityCard) ? string.Empty : this.IdentityCard.Trim();
            entity.IDENTITY_PLACE = string.IsNullOrEmpty(this.IdentityPlace) ? string.Empty : this.IdentityPlace.Trim();
            entity.IDENTITY_DATE = this.IdentityDate;
            //
            entity.NATION = string.IsNullOrEmpty(this.Nation) ? string.Empty : this.Nation.Trim();
            entity.RELIGION = string.IsNullOrEmpty(this.Religion) ? string.Empty : this.Religion.Trim();
            entity.PLACE_BIRTH = string.IsNullOrEmpty(this.PlaceBirth) ? string.Empty : this.PlaceBirth.Trim();
            entity.DISTRICTS = string.IsNullOrEmpty(this.District) ? string.Empty : this.District.Trim();
            entity.VILLAGE = string.IsNullOrEmpty(this.Village) ? string.Empty : this.Village.Trim();

            entity.EDUCATION_IDs = string.IsNullOrEmpty(this.EducationIds) ? string.Empty : this.EducationIds.Trim();
            entity.EDUCATION_NAMEs = string.IsNullOrEmpty(this.EducationNames) ? string.Empty : this.EducationNames.Trim();
            entity.POSITION_IDs = string.IsNullOrEmpty(this.PostionIds) ? string.Empty : this.PostionIds.Trim();
            entity.POSITION_NAMEs = string.IsNullOrEmpty(this.PostionNames) ? string.Empty : this.PostionNames.Trim();
            entity.DOCTOR_GROUP_ID = this.DoctorGroupId;

            return entity;
        }


    }
}