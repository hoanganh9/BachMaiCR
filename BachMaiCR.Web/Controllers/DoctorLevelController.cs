// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.DoctorLevelController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Entity;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class DoctorLevelController : BaseController
  {
    public DoctorLevelController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [ActionDescription(ActionCode = "DOCTORLEVEL_VIEW", ActionName = "Xem thông tin vị trí", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
    [CustomAuthorize]
    public ActionResult Index()
    {
ViewBag.Title = "Danh mục vị trí cán bộ";
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
ViewBag.ActionUpdate = list.Any<string>() && list.Contains("DOCTORLEVEL_SAVE");
      return this.View();
    }

    [ActionDescription(ActionCode = "DOCTORLEVEL_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
    [CustomAuthorize]
    public ActionResult OnInsert(int type)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        DoctorLevel doctorLevel = new DoctorLevel();
        if (this.Request.IsAjaxRequest())
          return this.PartialView("_AddDoctorLevel", doctorLevel);
        return null;
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "DOCTORLEVEL_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        DOCTOR_LEVEL byId = this.unitOfWork.DoctorLevels.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        DoctorLevel doctorLevel = new DoctorLevel(byId);
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", id, "", "");
        return this.PartialView("_AddDoctorLevel", doctorLevel);
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "DOCTORLEVEL_DELETE", ActionName = "Xóa thông tin vị trí", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
    [CustomAuthorize]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        DOCTOR_LEVEL byId = this.unitOfWork.DoctorLevels.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.ISDELETE = true;
        this.unitOfWork.DoctorLevels.Update(byId);
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, byId.CODE + "-" + byId.LEVEL_NAME, Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "DOCTORLEVEL_SAVE", ActionName = "Cập nhật thông tin", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
    [HttpPost]
    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitChange(DoctorLevel entity)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        DOCTOR_LEVEL categoryModel = entity.GetCategoryModel();
        if (this.unitOfWork.DoctorLevels.ExistCode(categoryModel))
          throw new Exception(string.Format("Mã kí hiệu đã tồn tại, hãy chọn mã kí hiệu khác", entity.Code));
        entity.IsDel = new bool?(false);
        if (entity.Id.Equals(0))
        {
          categoryModel.ISDELETE = new bool?(false);
          this.unitOfWork.DoctorLevels.Add(categoryModel);
          this.unitOfWork.DoctorLevels.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, categoryModel.CODE + "-" + categoryModel.LEVEL_NAME, Localization.MsgAddSuccess, "N/A", categoryModel.DOCTOR_LEVEL_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        DOCTOR_LEVEL byId = this.unitOfWork.DoctorLevels.GetById(categoryModel.DOCTOR_LEVEL_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.LEVEL_NAME = categoryModel.LEVEL_NAME;
        byId.CODE = categoryModel.CODE;
        byId.LEVEL_NUMBER = categoryModel.LEVEL_NUMBER;
        this.unitOfWork.DoctorLevels.Update(byId);
        this.WriteLog(enLogType.NomalLog, enActionType.Update, categoryModel.CODE + "-" + categoryModel.LEVEL_NAME, Localization.MsgEditSuccess, "N/A", categoryModel.DOCTOR_LEVEL_ID, "", "");
        return this.ReturnValue(Localization.MsgEditSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ValidateInput(false)]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "DOCTORLEVEL_VIEW", ActionName = "Xem thông tin vị trí", GroupCode = "DOCTORLEVEL", GroupName = "Danh mục vị trí cán bộ")]
    public PartialViewResult DoctorLevelList(int type, string name, string sortFiled, string sortDir, int pageIndex = 0)
    {
      pageIndex = pageIndex <= 0 ? 0 : pageIndex;
      Pagination pagination = new Pagination()
      {
        ActionName = "DoctorLevelList",
        ModelName = "DoctorLevel",
        MaxPages = 7,
        PageSize = 10,
        CurrentPage = pageIndex,
        InputSearchId = "txt_search_form",
        TagetReLoadId = "cat_list_render",
        CtgType = type
      };
      name = string.IsNullOrEmpty(name) ? "" : name.Trim();
      sortFiled = string.IsNullOrEmpty(sortFiled) ? "LEVEL_NUMBER" : sortFiled;
      sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
      PagedList<DOCTOR_LEVEL> all = this.unitOfWork.DoctorLevels.GetAll(type, name, pageIndex, pagination.PageSize, sortFiled, sortDir);
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
ViewBag.ActionUpdate = this.CheckPermistion("DOCTORLEVEL_SAVE");
ViewBag.ActionDelete = this.CheckPermistion("DOCTORLEVEL_DELETE");
      return this.PartialView("_DoctorLevelList");
    }
  }
}
