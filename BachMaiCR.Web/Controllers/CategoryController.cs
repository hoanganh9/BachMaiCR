// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.CategoryController
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
  public class CategoryController : BaseController
  {
    public CategoryController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CATEGORY_SAVE", ActionName = "Cập nhật danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    public ActionResult OnInsert(int type)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        Category category = new Category();
        category.Type = new int?(type);
        if (this.Request.IsAjaxRequest())
          return this.PartialView("~/Views/Category/AddCategory.cshtml", category);
        return null;
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
      }
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CATEGORY_SAVE", ActionName = "Cập nhật danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemNotExist);
        LM_CATEGORY byId = this.unitOfWork.Categories.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        return this.PartialView("~/Views/Category/AddCategory.cshtml", new Category(byId));
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
      }
    }

    [ActionDescription(ActionCode = "CATEGORY_DELETE", ActionName = "Xóa danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    [HttpPost]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        LM_CATEGORY byId = this.unitOfWork.Categories.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        if (this.unitOfWork.Doctors.ExistReferenceCategory(byId.LM_CATEGORY_ID))
          throw new Exception(Localization.MsgItemReference);
        byId.ISDELETE = true;
        this.unitOfWork.Categories.Update(byId);
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
      }
    }

    [HttpPost]
    [ValidateInput(false)]
    [ActionDescription(ActionCode = "CATEGORY_SAVE", ActionName = "Cập nhật danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    public ActionResult SubmitChange(Category entity)
    {
      try
      {
        LM_CATEGORY categoryModel = entity.GetCategoryModel();
        if (this.unitOfWork.Categories.ExistCode(categoryModel))
          throw new Exception(Localization.MsgCodeExist);
        entity.IsDel = new bool?(false);
        if (entity.Id.Equals(0))
        {
          categoryModel.ISDELETE = new bool?(false);
          this.unitOfWork.Categories.Add(categoryModel);
          this.unitOfWork.Categories.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", Localization.MsgAddSuccess, "N/A", categoryModel.LM_CATEGORY_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        LM_CATEGORY byId = this.unitOfWork.Categories.GetById(categoryModel.LM_CATEGORY_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.CATEGORY_NAME = categoryModel.CATEGORY_NAME;
        byId.CODE = categoryModel.CODE;
        byId.CATEGORY_DESCRIPTION = categoryModel.CATEGORY_DESCRIPTION;
        this.unitOfWork.Categories.Update(byId);
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", categoryModel.LM_CATEGORY_ID, "", "");
        return this.ReturnValue(Localization.MsgEditSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "~/Views/Category/Index.cshtml");
      }
    }

    [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    [CustomAuthorize]
    public ActionResult ProvinceIndex()
    {
ViewBag.Type = enCategoryType.Province;
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
ViewBag.ActionUpdate = list.Any<string>() && list.Contains("CATEGORY_SAVE");
      return this.View("Index");
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    public ActionResult PositionIndex()
    {
ViewBag.Type = enCategoryType.Position;
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
ViewBag.ActionUpdate = list.Any<string>() && list.Contains("CATEGORY_SAVE");
      return this.View("Index");
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    public ActionResult EducationIndex()
    {
ViewBag.Type = enCategoryType.EducationIndex;
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
ViewBag.ActionUpdate = list.Any<string>() && list.Contains("CATEGORY_SAVE");
      return this.View("Index");
    }

    [CustomAuthorize]
    [ActionDescription(ActionCode = "CATEGORY_VIEW", ActionName = "Xem thông tin danh mục", GroupCode = "CATEGORY", GroupName = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ")]
    public ActionResult HolidaysIndex()
    {
ViewBag.Type = enCategoryType.TypeOfHolidays;
      List<string> list = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
ViewBag.ActionUpdate = list.Any<string>() && list.Contains("CATEGORY_SAVE");
      return this.View("Index");
    }

    [ValidateInput(false)]
    [CustomAuthorize]
    public PartialViewResult GetCategoryList(int type, string name, string sortFiled, string sortDir, int pageIndex = 0)
    {
      pageIndex = pageIndex <= 0 ? 0 : pageIndex;
      Pagination pagination = new Pagination()
      {
        ActionName = "GetCategoryList",
        ModelName = "Category",
        MaxPages = 7,
        PageSize = 10,
        CurrentPage = pageIndex,
        InputSearchId = "txt_search_form",
        TagetReLoadId = "cat_list_render",
        CtgType = type
      };
      name = string.IsNullOrEmpty(name) ? "" : name.Trim();
      sortFiled = string.IsNullOrEmpty(sortFiled) ? "CODE" : sortFiled;
      sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
      PagedList<LM_CATEGORY> all = this.unitOfWork.Categories.GetAll(type, name, pageIndex, pagination.PageSize, sortFiled, sortDir);
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
ViewBag.ActionUpdate = this.CheckPermistion("CATEGORY_SAVE");
ViewBag.ActionDelete = this.CheckPermistion("CATEGORY_DELETE");
      return this.PartialView("_PartialCategoryList");
    }
  }
}
