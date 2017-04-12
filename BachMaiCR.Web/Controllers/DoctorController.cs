// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.DoctorController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Common.CustomActionResult;
using BachMaiCR.Web.Models;
using BachMaiCR.Web.Utils;

namespace BachMaiCR.Web.Controllers
{
  public class DoctorController : BaseController
  {
    public DoctorController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [ActionDescription(ActionCode = "DOCTOR_VIEW", ActionName = "Xem hồ sơ cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    [CustomAuthorize]
    public ActionResult Index()
    {
ViewBag.Title = "Danh mục cán bộ";
      return (ActionResult) this.View();
    }

    [ActionDescription(ActionCode = "DOCTOR_VIEW", ActionName = "Xem hồ sơ cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    [ValidateInput(false)]
    [CustomAuthorize]
    public PartialViewResult GetList(DoctorSearch searchEntity, int pageIndex = 0)
    {
      if (searchEntity == null)
        searchEntity = new DoctorSearch();
      pageIndex = pageIndex <= 0 ? 0 : pageIndex;
      Pagination pagination = new Pagination()
      {
        ActionName = "GetList",
        ModelName = "Doctor",
        MaxPages = 7,
        PageSize = 10,
        CurrentPage = pageIndex,
        InputSearchId = "txt_search_form",
        TagetReLoadId = "cat_list_render"
      };
      if (searchEntity.SearchDeprtId.HasValue && searchEntity.SearchDeprtId.Value > 0)
      {
        LM_DEPARTMENT byId = this.unitOfWork.Departments.GetById(searchEntity.SearchDeprtId.Value);
        searchEntity.PathDepat = byId == null ? "" : byId.DEPARTMENT_PATH;
      }
      else
      {
        List<LM_DEPARTMENT> deptCurrent = this.GetDeptCurrent();
        if (deptCurrent != null && deptCurrent.Count > 0)
          searchEntity.SearchDeprtId = new int?(deptCurrent[0].LM_DEPARTMENT_ID);
      }
      PagedList<DOCTOR> all = this.unitOfWork.Doctors.GetAll(searchEntity, pageIndex, pagination.PageSize);
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
      List<string> actionCodesByUserName = this.GetActionCodesByUserName();
ViewBag.Actions = actionCodesByUserName;
      return this.PartialView("_GetList");
    }

    [ActionDescription(ActionCode = "DOCTOR_VIEW", ActionName = "Xem hồ sơ cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    [CustomAuthorize]
    public ActionResult OnDetail(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        DOCTOR byId = this.unitOfWork.Doctors.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.DOCTOR_IMAGE = BachMaiCR.Web.Utils.Utils.GetPathUserImage(byId.DOCTOR_IMAGE);
        return (ActionResult) this.PartialView("_Detail", (object) byId);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.View, "Xem hồ sơ cán bộ", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "DOCTOR_VIEW", ActionName = "Xem hồ sơ cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    [CustomAuthorize]
    public ActionResult OnSearch()
    {
      try
      {
        SelectListItem selectListItem = new SelectListItem()
        {
          Value = "0",
          Text = Localization.LabelSearchAll
        };
        DoctorSearch doctorSearch = new DoctorSearch();
        List<SelectListItem> listItemBase1 = this.unitOfWork.DoctorLevels.GetListItemBase();
        listItemBase1.Insert(0, selectListItem);
ViewBag.DoctorLevel = listItemBase1;
        List<SelectListItem> listItemBase2 = this.unitOfWork.Categories.GetListItemBase(4);
        listItemBase2.Insert(0, selectListItem);
ViewBag.ListEducation = listItemBase2;
        List<SelectListItem> listItemBase3 = this.unitOfWork.Departments.GetListItemBase();
ViewBag.ListDepartment = listItemBase3;
        List<SelectListItem> listItemBase4 = this.unitOfWork.Categories.GetListItemBase(1);
        listItemBase4.Insert(0, selectListItem);
ViewBag.ListProvince = listItemBase4;
        List<SelectListItem> listItemBase5 = this.unitOfWork.Categories.GetListItemBase(3);
        listItemBase5.Insert(0, selectListItem);
ViewBag.ListPosition = listItemBase5;
ViewBag.ActionUpdate = this.CheckPermistion("DOCTOR_UPDATE");
ViewBag.ActionExport = this.CheckPermistion("DOCTOR_EXPORT");
ViewBag.ActionImport = this.CheckPermistion("DOCTOR_IMPORT");
        List<LM_DEPARTMENT> deptCurrent = this.GetDeptCurrent();
ViewBag.RootDepartment = deptCurrent;
        return (ActionResult) this.PartialView("_Search", (object) doctorSearch);
      }
      catch (Exception ex)
      {
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "DOCTOR_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    public ActionResult OnInsert(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        this.IsAjaxRequest();
ViewBag.DoctorLevel = this.unitOfWork.DoctorLevels.GetListItemBase();
ViewBag.DoctorGroup = this.unitOfWork.DoctorGroups.GetListItemBase();
ViewBag.ListEducation = this.unitOfWork.Categories.GetListItemBase(4);
ViewBag.ListDepartment = this.unitOfWork.Departments.GetListItemBase();
ViewBag.ListProvince = this.unitOfWork.Categories.GetListItemBase(1);
ViewBag.ListPosition = this.unitOfWork.Categories.GetListItemBase(3);
        return (ActionResult) this.PartialView("_Insert", (object) new Doctor()
        {
          AvatarUrl = BachMaiCR.Web.Utils.Utils.GetPathUserImage("doctor_avatar_default.png")
        });
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "DOCTOR_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        DOCTOR byId = this.unitOfWork.Doctors.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        Doctor doctor = new Doctor(byId);
ViewBag.DoctorGroup = this.unitOfWork.DoctorGroups.GetListItemBase();
        doctor.AvatarUrl = BachMaiCR.Web.Utils.Utils.GetPathUserImage(doctor.AvatarUrl);
ViewBag.DoctorLevel = this.unitOfWork.DoctorLevels.GetListItemBase();
ViewBag.ListEducation = this.unitOfWork.Categories.GetListItemBase(4);
ViewBag.ListDepartment = this.unitOfWork.Departments.GetListItemBase();
ViewBag.ListProvince = this.unitOfWork.Categories.GetListItemBase(1);
ViewBag.ListPosition = this.unitOfWork.Categories.GetListItemBase(3);
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", id, "", "");
ViewBag.LM_DEPARTMENT_NAMEs = this.unitOfWork.Departments.GetListDepartment(byId.LM_DEPARTMENT_IDs);
ViewBag.LM_DEPARTMENT_IDs = byId.LM_DEPARTMENT_IDs;
        return (ActionResult) this.PartialView("_Insert", (object) doctor);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [HttpPost]
    [ActionDescription(ActionCode = "DOCTOR_DELETE", ActionName = "Xóa thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        DOCTOR byId = this.unitOfWork.Doctors.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        if (this.unitOfWork.Users.ExistReferenceUser(byId.DOCTORS_ID) || this.unitOfWork.CalendarDoctors.ExistReferenceUser(byId.DOCTORS_ID))
          throw new Exception(Localization.MsgItemReference);
        byId.ISDELETE = true;
        this.unitOfWork.Doctors.Update(byId);
        this.unitOfWork.Doctors.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    [HttpPost]
    [ActionDescription(ActionCode = "DOCTOR_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    public ActionResult SubmitChange(Doctor entity)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        bool flag = false;
        string path2 = string.Empty;
        if (this.Request.Files.Count > 0)
        {
          HttpPostedFileBase file = this.Request.Files[0];
          if (!file.IsImage())
            throw new Exception("Định dạng ảnh không hợp lệ!");
          if (file.ContentLength <= 0 || file.ContentLength >= 2097152)
            throw new Exception("Tệp ảnh không hợp lệ hoặc lớn hơn 2Mb!");
          path2 = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
          string str = Path.Combine(ConfigurationManager.AppSettings["RootContentFolder"], "Images/UserAvartars");
          DirectoryInfo directoryInfo = new DirectoryInfo(str);
          if (!directoryInfo.Exists)
            directoryInfo.Create();
          file.SaveAs(Path.Combine(str, path2));
          flag = true;
        }
        entity.EducationNames = this.unitOfWork.Categories.GetMutilName(entity.EducationIds);
        entity.PostionNames = this.unitOfWork.Categories.GetMutilName(entity.PostionIds);
        DOCTOR categoryModel = entity.GetCategoryModel();
        if (categoryModel.DOCTORS_ID.Equals(0))
        {
          categoryModel.ISDELETE = new bool?(false);
          categoryModel.ISACTIVED = true;
          if (flag)
            categoryModel.DOCTOR_IMAGE = path2;
          categoryModel.LM_DEPARTMENT_NAMEs = this.unitOfWork.Departments.GetListDepartment(categoryModel.LM_DEPARTMENT_IDs);
          this.unitOfWork.Doctors.Add(categoryModel);
          this.unitOfWork.Doctors.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Update, categoryModel.DOCTOR_NAME, Localization.MsgAddSuccess, "N/A", categoryModel.DOCTORS_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        DOCTOR byId = this.unitOfWork.Doctors.GetById(categoryModel.DOCTORS_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.DOCTORS_ID = categoryModel.DOCTORS_ID;
        byId.DOCTOR_NAME = categoryModel.DOCTOR_NAME;
        byId.ABBREVIATION = categoryModel.ABBREVIATION;
        byId.ADDRESS = categoryModel.ADDRESS;
        byId.PHONE = categoryModel.PHONE;
        byId.DOCTOR_ORDER = categoryModel.DOCTOR_ORDER;
        byId.PROVINCES = categoryModel.PROVINCES;
        byId.BIRTHDAY = categoryModel.BIRTHDAY;
        if (flag)
          byId.DOCTOR_IMAGE = path2;
        byId.DOCTORS_ID = categoryModel.DOCTORS_ID;
        byId.DOCTOR_LEVEL_ID = categoryModel.DOCTOR_LEVEL_ID;
        byId.EDUCATION_IDs = categoryModel.EDUCATION_IDs;
        byId.EDUCATION_NAMEs = categoryModel.EDUCATION_NAMEs;
        byId.POSITION_IDs = categoryModel.POSITION_IDs;
        byId.POSITION_NAMEs = categoryModel.POSITION_NAMEs;
        byId.DATE_START = categoryModel.DATE_START;
        byId.DISTRICTS = categoryModel.DISTRICTS;
        byId.INSURANCE_NUMBER = categoryModel.INSURANCE_NUMBER;
        byId.INSURANCE_REGISTER = categoryModel.INSURANCE_REGISTER;
        byId.RELIGION = categoryModel.RELIGION;
        byId.NATION = categoryModel.NATION;
        byId.IDENTITY_DATE = categoryModel.IDENTITY_DATE;
        byId.IDENTITY_CARD = categoryModel.IDENTITY_CARD;
        byId.IDENTITY_PLACE = categoryModel.IDENTITY_PLACE;
        byId.CODE_STAFF = categoryModel.CODE_STAFF;
        byId.PLACE_BIRTH = categoryModel.PLACE_BIRTH;
        byId.EMAIL = categoryModel.EMAIL;
        byId.GENDER = categoryModel.GENDER;
        byId.DOCTOR_GROUP_ID = categoryModel.DOCTOR_GROUP_ID;
        byId.VILLAGE = categoryModel.VILLAGE;
        byId.LM_DEPARTMENT_IDs = string.IsNullOrEmpty(categoryModel.LM_DEPARTMENT_IDs) ? "" : categoryModel.LM_DEPARTMENT_IDs + ",";
        byId.LM_DEPARTMENT_NAMEs = this.unitOfWork.Departments.GetListDepartment(categoryModel.LM_DEPARTMENT_IDs);
        this.unitOfWork.Doctors.Update(byId);
        this.unitOfWork.Doctors.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, categoryModel.DOCTOR_NAME, Localization.MsgEditSuccess, "N/A", categoryModel.DOCTORS_ID, "", "");
        return this.ReturnValue(Localization.MsgEditSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        string str = !ex.ToString().Contains("Cannot insert duplicate") ? ex.Message : "Số chứng mình nhân dân đã tồn tại, vui lòng kiểm tra lại";
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", str, 0, "", "");
        return this.ReturnValue(str, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "DOCTOR_EXPORT", ActionName = "Xuất dữ liệu Excel", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
    public ActionResult Export(DoctorSearch searchEntity, int pageIndex = 0)
    {
      try
      {
        if (searchEntity == null)
          searchEntity = new DoctorSearch();
        string fileName = "DS-CAN-BO_Khoa-Tim-Mach";
        this.Response.Clear();
        this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xlsx");
        byte[] numArray = this.GenerateByte(fileName, searchEntity, 0);
        this.Response.BinaryWrite(numArray);
        this.Response.Flush();
        this.Response.Close();
        return (ActionResult) new DownloadResult(numArray, fileName);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.ExportExcel, "N/A", "Xuất danh sách nhân viên thất bại!", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    private byte[] GenerateByte(string fileName, DoctorSearch searchEntity, int pageIndex = 0)
    {
      FileInfo fileInfo = new FileInfo(this.HttpContext.Server.MapPath("~/Views/Shared/ExcelTemplate/DoctorsTemplate.xlsx"));
      if (fileInfo.Exists.Equals(false))
        throw new Exception("Không tìm thấy tệp tin!");
      using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
      {
        ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
        excelWorksheet.Name = fileName;
        LM_DEPARTMENT lmDepartment = new LM_DEPARTMENT();
        if (searchEntity.SearchDeprtId.HasValue && searchEntity.SearchDeprtId.Value > 0)
        {
          lmDepartment = this.unitOfWork.Departments.GetById(searchEntity.SearchDeprtId.Value);
          searchEntity.PathDepat = lmDepartment == null ? "" : lmDepartment.DEPARTMENT_PATH;
        }
        List<DOCTOR> all = this.unitOfWork.Doctors.GetAll(searchEntity);
        excelWorksheet.Cells["A2"].Value = string.IsNullOrEmpty(searchEntity.PathDepat) ? (object) "" : (object) lmDepartment.DEPARTMENT_NAME.ToUpper();
        int num1 = 1;
        int num2 = 6;
        foreach (DOCTOR doctor in all)
        {
          ExcelRange cells1 = excelWorksheet.Cells;
          string str1 = "B";
          int num3 = num1 + num2;
          string str2 = num3.ToString();
          string str3 = str1 + str2;
          cells1[str3].Value =  doctor.DOCTOR_NAME;
          ExcelRange cells2 = excelWorksheet.Cells;
          string str4 = "C";
          num3 = num1 + num2;
          string str5 = num3.ToString();
          string str6 = str4 + str5;
          cells2[str6].Value = doctor.BIRTHDAY.HasValue ? (object) doctor.BIRTHDAY.Value.ToString("dd/MM/yyyy") : (object) "N/A";
          string str7 = "D";
          num3 = num1 + num2;
          string str8 = num3.ToString();
          string str9 = str7 + str8;
          ExcelRange excelRange1 = excelWorksheet.Cells[str9];
          bool? gender = doctor.GENDER;
          string str10;
          if (gender.HasValue)
          {
            gender = doctor.GENDER;
            if (gender.Value)
            {
              str10 = "Nam";
              goto label_11;
            }
          }
          str10 = "Nữ";
label_11:
          excelRange1.Value = str10;
          string str11 = "E";
          num3 = num1 + num2;
          string str12 = num3.ToString();
          string str13 = str11 + str12;
                    excelWorksheet.Cells[str13].Value =  doctor.PHONE;
          string str14 = "F";
          num3 = num1 + num2;
          string str15 = num3.ToString();
          string str16 = str14 + str15;
                    excelWorksheet.Cells[str16].Value =  doctor.IDENTITY_CARD;
          string str17 = "";
          if (!string.IsNullOrEmpty(doctor.PROVINCES))
            str17 = doctor.PROVINCES;
          if (!string.IsNullOrEmpty(doctor.DISTRICTS))
            str17 = string.IsNullOrEmpty(str17) ? doctor.DISTRICTS : str17 + ", " + doctor.DISTRICTS;
          if (!string.IsNullOrEmpty(doctor.VILLAGE))
            str17 = string.IsNullOrEmpty(str17) ? doctor.VILLAGE : str17 + ", " + doctor.VILLAGE;
                    string str18 = "G";
          num3 = num1 + num2;
          string str19 = num3.ToString();
          string str20 = str18 + str19;
                    excelWorksheet.Cells[str20].Value = string.IsNullOrEmpty(str17) ? (object) "N/A" : (object) str17;
          
          string str21 = "H";
          num3 = num1 + num2;
          string str22 = num3.ToString();
          string str23 = str21 + str22;
                    excelWorksheet.Cells[str23].Value =  doctor.POSITION_NAMEs;
          string str24 = "I";
          num3 = num1 + num2;
          string str25 = num3.ToString();
          string str26 = str24 + str25;
                    excelWorksheet.Cells[str26].Value =  doctor.DOCTOR_LEVEL.LEVEL_NAME;
          string str27 = "J";
          num3 = num1 + num2;
          string str28 = num3.ToString();
          string str29 = str27 + str28;
                    excelWorksheet.Cells[str29].Value =  doctor.EDUCATION_NAMEs;
          string str30 = "K";
          num3 = num1 + num2;
          string str31 = num3.ToString();
          string str32 = str30 + str31;
                    excelWorksheet.Cells[str32].Value = string.IsNullOrEmpty(doctor.LM_DEPARTMENT_NAMEs) ? (object) string.Empty : (object) doctor.LM_DEPARTMENT_NAMEs;
          
          string str33 = "A";
          num3 = num1 + num2;
          string str34 = num3.ToString();
          string str35 = str33 + str34;
          ExcelRange excelRange2 = excelWorksheet.Cells[str35];
          num3 = num1++;
          string str36 = num3.ToString();
          excelRange2.Value = str36;
          string str37 = "L";
          num3 = num1 + num2;
          string str38 = num3.ToString();
          string str39 = str37 + str38;
                    excelWorksheet.Cells[str39].Value =  doctor.DOCTORS_ID;
          string str40 = "M";
          num3 = num1 + num2;
          string str41 = num3.ToString();
          string str42 = str40 + str41;
                    excelWorksheet.Cells[str42].Value =  doctor.LM_DEPARTMENT_IDs;
        }
        ExcelRange excelRange3 = excelWorksheet.Cells["A7:K" + (num1 + num2 - 1).ToString()];
        excelRange3.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
        excelRange3.Style.Border.Top.Style = ExcelBorderStyle.Hair;
        excelRange3.Style.Border.Left.Style = ExcelBorderStyle.Hair;
        excelRange3.Style.Border.Right.Style = ExcelBorderStyle.Hair;
        excelRange3.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        excelRange3.Style.Numberformat.Format="@";
        excelRange3.Style.WrapText = true;
        ExcelRange excelRange4 = excelWorksheet.Cells["A7:A" + (num1 + num2 - 1).ToString()];
        excelRange4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        excelRange4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        return excelPackage.GetAsByteArray();
      }
    }

    [HttpPost]
    public ActionResult AjaxUpload()
    {
      return (ActionResult) this.Json(JsonResponse.Json200((object) Localization.MsgDelSuccess));
    }

    [ValidateJsonAntiForgeryToken]
    [HttpPost]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "DOCTOR_ACTIVE", ActionName = "Active/deactive cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ", IsMenu = false)]
    public ActionResult ActiveChage(int doctorId, bool active)
    {
      if (doctorId <= 0)
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Cán bộ không tồn tại "));
      DOCTOR byId = this.unitOfWork.Doctors.GetById(doctorId);
      if (byId == null)
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật thành công !"));
      byId.ISACTIVED = new bool?(active);
      this.unitOfWork.Doctors.Update(byId);
      this.unitOfWork.Users.UpdateUserByDoctorId(doctorId, active);
      this.WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive cán bộ [" + byId.DOCTOR_NAME + "]", "N/A", "N/A", doctorId, "", "");
      return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật thành công !"));
    }
  }
}
