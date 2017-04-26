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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

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
            return View();
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
                LM_DEPARTMENT byId = unitOfWork.Departments.GetById(searchEntity.SearchDeprtId.Value);
                searchEntity.PathDepat = byId == null ? string.Empty : byId.DEPARTMENT_PATH;
            }
            else
            {
                List<LM_DEPARTMENT> deptCurrent = GetDeptCurrent();
                if (deptCurrent != null && deptCurrent.Count > 0)
                    searchEntity.SearchDeprtId = new int?(deptCurrent[0].LM_DEPARTMENT_ID);
            }
            PagedList<DOCTOR> all = unitOfWork.Doctors.GetAll(searchEntity, pageIndex, pagination.PageSize);
            pagination.TotalRows = all.TotalItemCount;
            pagination.CurrentRow = all.Count;
            ViewBag.Category = all;
            ViewBag.Pagination = pagination;
            List<string> actionCodesByUserName = GetActionCodesByUserName();
            ViewBag.Actions = actionCodesByUserName;
            return PartialView("_GetList");
        }

        [ActionDescription(ActionCode = "DOCTOR_VIEW", ActionName = "Xem hồ sơ cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
        [CustomAuthorize]
        public ActionResult OnDetail(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                DOCTOR byId = unitOfWork.Doctors.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                byId.DOCTOR_IMAGE = Utils.Utils.GetPathUserImage(byId.DOCTOR_IMAGE);
                return PartialView("_Detail", byId);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.View, "Xem hồ sơ cán bộ", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
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
                List<SelectListItem> listItemBase1 = unitOfWork.DoctorLevels.GetListItemBase();
                listItemBase1.Insert(0, selectListItem);
                ViewBag.DoctorLevel = listItemBase1;
                List<SelectListItem> listItemBase2 = unitOfWork.Categories.GetListItemBase(4);
                listItemBase2.Insert(0, selectListItem);
                ViewBag.ListEducation = listItemBase2;
                List<SelectListItem> listItemBase3 = unitOfWork.Departments.GetListItemBase();
                ViewBag.ListDepartment = listItemBase3;
                List<SelectListItem> listItemBase4 = unitOfWork.Categories.GetListItemBase(1);
                listItemBase4.Insert(0, selectListItem);
                ViewBag.ListProvince = listItemBase4;
                List<SelectListItem> listItemBase5 = unitOfWork.Categories.GetListItemBase(3);
                listItemBase5.Insert(0, selectListItem);
                ViewBag.ListPosition = listItemBase5;
                ViewBag.ActionUpdate = CheckPermistion("DOCTOR_UPDATE");
                ViewBag.ActionExport = CheckPermistion("DOCTOR_EXPORT");
                ViewBag.ActionImport = CheckPermistion("DOCTOR_IMPORT");
                List<LM_DEPARTMENT> deptCurrent = GetDeptCurrent();
                ViewBag.RootDepartment = deptCurrent;
                return PartialView("_Search", doctorSearch);
            }
            catch (Exception ex)
            {
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "DOCTOR_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
        public ActionResult OnInsert(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                IsAjaxRequest();
                ViewBag.DoctorLevel = unitOfWork.DoctorLevels.GetListItemBase();
                ViewBag.DoctorGroup = unitOfWork.DoctorGroups.GetListItemBase();
                ViewBag.ListEducation = unitOfWork.Categories.GetListItemBase(4);
                ViewBag.ListDepartment = unitOfWork.Departments.GetListItemBase();
                ViewBag.ListProvince = unitOfWork.Categories.GetListItemBase(1);
                ViewBag.ListPosition = unitOfWork.Categories.GetListItemBase(3);
                return PartialView("_Insert", new Doctor()
                {
                    AvatarUrl = Utils.Utils.GetPathUserImage("doctor_avatar_default.png")
                });
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [CustomAuthorize]
        [ActionDescription(ActionCode = "DOCTOR_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
        public ActionResult OnUpdate(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                DOCTOR byId = unitOfWork.Doctors.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                Doctor doctor = new Doctor(byId);
                ViewBag.DoctorGroup = unitOfWork.DoctorGroups.GetListItemBase();
                doctor.AvatarUrl = Utils.Utils.GetPathUserImage(doctor.AvatarUrl);
                ViewBag.DoctorLevel = unitOfWork.DoctorLevels.GetListItemBase();
                ViewBag.ListEducation = unitOfWork.Categories.GetListItemBase(4);
                ViewBag.ListDepartment = unitOfWork.Departments.GetListItemBase();
                ViewBag.ListProvince = unitOfWork.Categories.GetListItemBase(1);
                ViewBag.ListPosition = unitOfWork.Categories.GetListItemBase(3);
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", id, string.Empty, string.Empty);
                ViewBag.LM_DEPARTMENT_NAMEs = unitOfWork.Departments.GetListDepartment(byId.LM_DEPARTMENT_IDs);
                ViewBag.LM_DEPARTMENT_IDs = byId.LM_DEPARTMENT_IDs;
                return PartialView("_Insert", doctor);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [HttpPost]
        [ActionDescription(ActionCode = "DOCTOR_DELETE", ActionName = "Xóa thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
        public ActionResult OnDelete(int id)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                if (id <= 0)
                    throw new Exception(Localization.MsgItemInvalid);
                DOCTOR byId = unitOfWork.Doctors.GetById(id);
                if (byId == null)
                    throw new Exception(Localization.MsgItemNotExist);
                if (unitOfWork.Users.ExistReferenceUser(byId.DOCTORS_ID) || unitOfWork.CalendarDoctors.ExistReferenceUser(byId.DOCTORS_ID))
                    throw new Exception(Localization.MsgItemReference);
                byId.ISDELETE = true;
                unitOfWork.Doctors.Update(byId);
                unitOfWork.Doctors.Save();
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgDelSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionDescription(ActionCode = "DOCTOR_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ")]
        public ActionResult SubmitChange(Doctor entity)
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            try
            {
                bool flag = false;
                string path2 = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
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
                entity.EducationNames = unitOfWork.Categories.GetMutilName(entity.EducationIds);
                entity.PostionNames = unitOfWork.Categories.GetMutilName(entity.PostionIds);
                DOCTOR categoryModel = entity.GetCategoryModel();
                if (categoryModel.DOCTORS_ID.Equals(0))
                {
                    categoryModel.ISDELETE = false;
                    categoryModel.ISACTIVED = true;
                    if (flag)
                        categoryModel.DOCTOR_IMAGE = path2;
                    categoryModel.LM_DEPARTMENT_NAMEs = unitOfWork.Departments.GetListDepartment(categoryModel.LM_DEPARTMENT_IDs);
                    unitOfWork.Doctors.Add(categoryModel);
                    unitOfWork.Doctors.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Update, categoryModel.DOCTOR_NAME, Localization.MsgAddSuccess, "N/A", categoryModel.DOCTORS_ID, string.Empty, string.Empty);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                DOCTOR byId = unitOfWork.Doctors.GetById(categoryModel.DOCTORS_ID);
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
                byId.LM_DEPARTMENT_IDs = string.IsNullOrEmpty(categoryModel.LM_DEPARTMENT_IDs) ? string.Empty : categoryModel.LM_DEPARTMENT_IDs + ",";
                byId.LM_DEPARTMENT_NAMEs = unitOfWork.Departments.GetListDepartment(categoryModel.LM_DEPARTMENT_IDs);
                unitOfWork.Doctors.Update(byId);
                unitOfWork.Doctors.Save();
                WriteLog(enLogType.NomalLog, enActionType.Update, categoryModel.DOCTOR_NAME, Localization.MsgEditSuccess, "N/A", categoryModel.DOCTORS_ID, string.Empty, string.Empty);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                string str = !ex.ToString().Contains("Cannot insert duplicate") ? ex.Message : "Số chứng mình nhân dân đã tồn tại, vui lòng kiểm tra lại";
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", str, 0, string.Empty, string.Empty);
                return ReturnValue(str, false, "Index");
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
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xlsx");
                byte[] numArray = GenerateByte(fileName, searchEntity, 0);
                Response.BinaryWrite(numArray);
                Response.Flush();
                Response.Close();
                return new DownloadResult(numArray, fileName);
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.ExportExcel, "N/A", "Xuất danh sách nhân viên thất bại!", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "Index");
            }
        }

        private byte[] GenerateByte(string fileName, DoctorSearch searchEntity, int pageIndex = 0)
        {
            FileInfo fileInfo = new FileInfo(HttpContext.Server.MapPath("~/Views/Shared/ExcelTemplate/DoctorsTemplate.xlsx"));
            if (fileInfo.Exists.Equals(false))
                throw new Exception("Không tìm thấy tệp tin!");
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];
                excelWorksheet.Name = fileName;
                LM_DEPARTMENT lmDepartment = new LM_DEPARTMENT();
                if (searchEntity.SearchDeprtId.HasValue && searchEntity.SearchDeprtId.Value > 0)
                {
                    lmDepartment = unitOfWork.Departments.GetById(searchEntity.SearchDeprtId.Value);
                    searchEntity.PathDepat = lmDepartment == null ? string.Empty : lmDepartment.DEPARTMENT_PATH;
                }
                List<DOCTOR> all = unitOfWork.Doctors.GetAll(searchEntity);
                excelWorksheet.Cells["A2"].Value = (string.IsNullOrEmpty(searchEntity.PathDepat) ? string.Empty : lmDepartment.DEPARTMENT_NAME.ToUpper());
                int index = 1;
                int startRow = 6;
                foreach (DOCTOR doctor in all)
                {
                    int rowIndex = index + startRow;
                    excelWorksheet.Cells["B" + rowIndex].Value = doctor.DOCTOR_NAME;
                    excelWorksheet.Cells["C" + rowIndex].Value = (doctor.BIRTHDAY.HasValue ? doctor.BIRTHDAY.Value.ToString("dd/MM/yyyy") : "N/A");
                    excelWorksheet.Cells["D" + rowIndex].Value = doctor.GENDER.GetValueOrDefault() ? "Nam" : "Nữ";
                    excelWorksheet.Cells["E" + rowIndex].Value = doctor.PHONE;
                    excelWorksheet.Cells["F" + rowIndex].Value = doctor.IDENTITY_CARD;
                    string address = string.Empty;
                    if (!string.IsNullOrEmpty(doctor.PROVINCES))
                        address = doctor.PROVINCES;
                    if (!string.IsNullOrEmpty(doctor.DISTRICTS))
                        address = string.IsNullOrEmpty(address) ? doctor.DISTRICTS : address + ", " + doctor.DISTRICTS;
                    if (!string.IsNullOrEmpty(doctor.VILLAGE))
                        address = string.IsNullOrEmpty(address) ? doctor.VILLAGE : address + ", " + doctor.VILLAGE;
                    excelWorksheet.Cells["G" + rowIndex].Value = (string.IsNullOrEmpty(address) ? "N/A" : address);
                    excelWorksheet.Cells["H" + rowIndex].Value = doctor.POSITION_NAMEs;
                    excelWorksheet.Cells["I" + rowIndex].Value = doctor.DOCTOR_LEVEL.LEVEL_NAME;
                    excelWorksheet.Cells["J" + rowIndex].Value = doctor.EDUCATION_NAMEs;
                    excelWorksheet.Cells["K" + rowIndex].Value = string.IsNullOrEmpty(doctor.LM_DEPARTMENT_NAMEs) ? string.Empty : doctor.LM_DEPARTMENT_NAMEs;
                    excelWorksheet.Cells["L" + rowIndex].Value = doctor.DOCTORS_ID;
                    excelWorksheet.Cells["M" + rowIndex].Value = doctor.LM_DEPARTMENT_IDs;
                    index++;
                    excelWorksheet.Cells["A" + rowIndex].Value = index;
                }
                ExcelRange excelRange3 = excelWorksheet.Cells["A7:K" + (index + startRow - 1)];
                (excelRange3).Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                (excelRange3).Style.Border.Top.Style = ExcelBorderStyle.Hair;
                (excelRange3).Style.Border.Left.Style = ExcelBorderStyle.Hair;
                (excelRange3).Style.Border.Right.Style = ExcelBorderStyle.Hair;
                (excelRange3).Style.Border.BorderAround(ExcelBorderStyle.Thin);
                (excelRange3).Style.Numberformat.Format = ("@");
                (excelRange3).Style.WrapText = true;
                ExcelRange excelRange4 = excelWorksheet.Cells["A7:A" + (index + startRow - 1)];
                (excelRange4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                (excelRange4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                return excelPackage.GetAsByteArray();
            }
        }

        [HttpPost]
        public ActionResult AjaxUpload()
        {
            return Json(JsonResponse.Json200(Localization.MsgDelSuccess));
        }

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [CustomAuthorize]
        [ActionDescription(ActionCode = "DOCTOR_ACTIVE", ActionName = "Active/deactive cán bộ", GroupCode = "DOCTOR", GroupName = "Danh mục cán bộ", IsMenu = false)]
        public ActionResult ActiveChage(int doctorId, bool active)
        {
            if (doctorId <= 0)
                return Json(JsonResponse.Json400("Cán bộ không tồn tại "));
            DOCTOR byId = unitOfWork.Doctors.GetById(doctorId);
            if (byId == null)
                return Json(JsonResponse.Json200("Cập nhật thành công !"));
            byId.ISACTIVED = active;
            unitOfWork.Doctors.Update(byId);
            unitOfWork.Users.UpdateUserByDoctorId(doctorId, active);
            WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive cán bộ [" + byId.DOCTOR_NAME + "]", "N/A", "N/A", doctorId, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Cập nhật thành công !"));
        }
    }
}