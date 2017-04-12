// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.WebpageActionsController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class WebpageActionsController : BaseController
  {
    public WebpageActionsController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [HttpGet]
    [ActionDescription(ActionCode = "WebpageActions_AddAction", ActionName = "Sửa chức năng", GroupCode = "ACTIONS_GROUP_CODE", GroupName = "Quản lý chức năng", IsMenu = false)]
    public ActionResult AddAction(int id)
    {
      WebPageActionEditModel pageActionEditModel = new WebPageActionEditModel(this.unitOfWork.Actions.GetById(id));
      if (this.Request.IsAjaxRequest())
        return (ActionResult) this.PartialView("~/Views/Admin/_AddAction.cshtml", (object) pageActionEditModel);
      this.RedirectToAction("ManageActions", "Admin");
      return (ActionResult) null;
    }

    [HttpPost]
    [ActionDescription(ActionCode = "WebpageActions_AddAction", ActionName = "Sửa chức năng", GroupCode = "ACTIONS_GROUP_CODE", GroupName = "Quản lý chức năng", IsMenu = false)]
    [ValidateInput(false)]
    [ValidateJsonAntiForgeryToken]
    public ActionResult AddAction(WebPageActionEditModel model)
    {
      try
      {
        if (this.ModelState.IsValid)
        {
          if (model.WEBPAGES_ACTION_ID > 0)
          {
            WEBPAGES_ACTIONS byId = this.unitOfWork.Actions.GetById(model.WEBPAGES_ACTION_ID);
            if (byId != null)
            {
              byId.DESCRIPTION = model.DESCRIPTION;
              byId.MENU_NAME = model.MENUNAME;
              byId.MANUAL_EDITED = true;
              byId.GROUP_ORDER = model.GROUP_ORDER;
              byId.MENU_ORDER = model.MENU_ORDER;
              byId.IS_ACTIVE = new bool?(model.IS_ACTIVE);
              this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", byId.WEBPAGES_ACTION_ID, "", "");
              this.unitOfWork.Actions.Update(byId);
            }
          }
          if (this.Request.IsAjaxRequest())
            return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhập thành công"));
          return (ActionResult) null;
        }
        if (this.Request.IsAjaxRequest())
          return (ActionResult) this.Json(JsonResponse.Json500((object) "Phải nhập đủ thông tin"));
        return (ActionResult) null;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    [ValidateJsonAntiForgeryToken]
    [HttpPost]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "WebpageActions_UpdateActive", ActionName = "Active/deactive chức năng", GroupCode = "ACTIONS_GROUP_CODE", GroupName = "Quản lý chức năng", IsMenu = false)]
    public JsonResult UpdateActive(int id, bool active)
    {
      WEBPAGES_ACTIONS byId = this.unitOfWork.Actions.GetById(id);
      if (byId == null)
        return this.Json(JsonResponse.Json404((object) "Action not found"));
      byId.IS_ACTIVE = new bool?(active);
      this.unitOfWork.Actions.Update(byId);
      this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", byId.WEBPAGES_ACTION_ID, "", "");
      return this.Json(JsonResponse.Json200((object) "Cập nhật thành công"));
    }

    [HttpGet]
    public ActionResult GetActionInRole(int roleId)
    {
      List<WEBPAGES_ACTIONS> list = this.unitOfWork.Actions.GetAll().ToList<WEBPAGES_ACTIONS>();
      List<int> selectedActionIds = this.unitOfWork.Roles.GetById(roleId).WEBPAGES_ACTIONS.Select<WEBPAGES_ACTIONS, int>((Func<WEBPAGES_ACTIONS, int>) (o => o.WEBPAGES_ACTION_ID)).ToList<int>();
      return (ActionResult) this.Json(JsonResponse.Json200((object) list.Select<WEBPAGES_ACTIONS, KeyTextItem>((Func<WEBPAGES_ACTIONS, KeyTextItem>) (o => new KeyTextItem()
      {
        Text = o.DESCRIPTION,
        Id = o.WEBPAGES_ACTION_ID.ToString(),
        Selected = selectedActionIds.Contains(o.WEBPAGES_ACTION_ID)
      })).ToList<KeyTextItem>()), JsonRequestBehavior.AllowGet);
    }
  }
}
