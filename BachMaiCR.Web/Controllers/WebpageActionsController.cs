using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            WebPageActionEditModel pageActionEditModel = new WebPageActionEditModel(unitOfWork.Actions.GetById(id));
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Admin/_AddAction.cshtml", pageActionEditModel);
            RedirectToAction("ManageActions", "Admin");
            return null;
        }

        [HttpPost]
        [ActionDescription(ActionCode = "WebpageActions_AddAction", ActionName = "Sửa chức năng", GroupCode = "ACTIONS_GROUP_CODE", GroupName = "Quản lý chức năng", IsMenu = false)]
        [ValidateInput(false)]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AddAction(WebPageActionEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.WEBPAGES_ACTION_ID > 0)
                    {
                        WEBPAGES_ACTIONS byId = unitOfWork.Actions.GetById(model.WEBPAGES_ACTION_ID);
                        if (byId != null)
                        {
                            byId.DESCRIPTION = model.DESCRIPTION;
                            byId.MENU_NAME = model.MENUNAME;
                            byId.MANUAL_EDITED = true;
                            byId.GROUP_ORDER = model.GROUP_ORDER;
                            byId.MENU_ORDER = model.MENU_ORDER;
                            byId.IS_ACTIVE = model.IS_ACTIVE;
                            WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", byId.WEBPAGES_ACTION_ID, string.Empty, string.Empty);
                            unitOfWork.Actions.Update(byId);
                        }
                    }
                    if (Request.IsAjaxRequest())
                        return Json(JsonResponse.Json200("Cập nhập thành công"));
                    return null;
                }
                if (Request.IsAjaxRequest())
                    return Json(JsonResponse.Json500("Phải nhập đủ thông tin"));
                return null;
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
            WEBPAGES_ACTIONS byId = unitOfWork.Actions.GetById(id);
            if (byId == null)
                return Json(JsonResponse.Json404("Action not found"));
            byId.IS_ACTIVE = active;
            unitOfWork.Actions.Update(byId);
            WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", "N/A", byId.WEBPAGES_ACTION_ID, string.Empty, string.Empty);
            return Json(JsonResponse.Json200("Cập nhật thành công"));
        }

        [HttpGet]
        public ActionResult GetActionInRole(int roleId)
        {
            List<WEBPAGES_ACTIONS> list = unitOfWork.Actions.GetAll().ToList();
            List<int> selectedActionIds = unitOfWork.Roles.GetById(roleId).WEBPAGES_ACTIONS.Select((o => o.WEBPAGES_ACTION_ID)).ToList();
            return Json(JsonResponse.Json200(list.Select((o => new KeyTextItem()
            {
                Text = o.DESCRIPTION,
                Id = o.WEBPAGES_ACTION_ID.ToString(),
                Selected = selectedActionIds.Contains(o.WEBPAGES_ACTION_ID)
            })).ToList()), JsonRequestBehavior.AllowGet);
        }
    }
}