// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ConfigSMSController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using Resources;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class ConfigSMSController : BaseController
  {
    public ConfigSMSController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [ActionDescription(ActionCode = "CONFIG_SMS_VIEW", ActionName = "Xem danh sách", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    [CustomAuthorize]
    public ActionResult Index()
    {
ViewBag.Title = "Cấu hình gửi SMS";
      return (ActionResult) this.View();
    }

    [CustomAuthorize]
    [ValidateInput(false)]
    [ActionDescription(ActionCode = "CONFIG_SMS_VIEW", ActionName = "Xem danh sách", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    public PartialViewResult GetList(ConfigSMSSearch searchEntity, int pageIndex = 0)
    {
      if (searchEntity == null)
        searchEntity = new ConfigSMSSearch();
      pageIndex = pageIndex <= 0 ? 0 : pageIndex;
      Pagination pagination = new Pagination()
      {
        ActionName = "GetList",
        ModelName = "ConfigSMS",
        MaxPages = 7,
        PageSize = 10,
        CurrentPage = pageIndex,
        InputSearchId = "txt_search_form",
        TagetReLoadId = "cat_list_render"
      };
      PagedList<CONFIG_SMS> all = this.unitOfWork.ConfigSMS.GetAll(searchEntity, pageIndex, pagination.PageSize);
      pagination.TotalRows = all.TotalItemCount;
      pagination.CurrentRow = all.Count;
ViewBag.Category = all;
ViewBag.Pagination = pagination;
      List<string> actionCodesByUserName = this.GetActionCodesByUserName();
ViewBag.Actions = actionCodesByUserName;
      return this.PartialView("_GetList");
    }

    [ActionDescription(ActionCode = "CONFIG_SMS_VIEW", ActionName = "Xem danh sách", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
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
        ConfigSMSSearch configSmsSearch = new ConfigSMSSearch();
ViewBag.ActionUpdate = this.CheckPermistion("CONFIG_SMS_UPDATE");
        return (ActionResult) this.PartialView("_Search", (object) configSmsSearch);
      }
      catch (Exception ex)
      {
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_SMS_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    [CustomAuthorize]
    public ActionResult OnInsert(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        return (ActionResult) this.PartialView("_Insert", (object) new ConfigSMS());
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_SMS_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    [CustomAuthorize]
    public ActionResult OnUpdate(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        CONFIG_SMS byId = this.unitOfWork.ConfigSMS.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        return (ActionResult) this.PartialView("_Insert", (object) new ConfigSMS(byId));
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_SMS_DELETE", ActionName = "Xóa thông tin", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    [HttpPost]
    public ActionResult OnDelete(int id)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        if (id <= 0)
          throw new Exception(Localization.MsgItemInvalid);
        CONFIG_SMS byId = this.unitOfWork.ConfigSMS.GetById(id);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.ISDELETE = true;
        this.unitOfWork.ConfigSMS.Update(byId);
        this.unitOfWork.ConfigSMS.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelSuccess, "N/A", id, "", "");
        return this.ReturnValue(Localization.MsgDelSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Delete, "N/A", Localization.MsgDelFailed, ex.Message, id, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [ActionDescription(ActionCode = "CONFIG_SMS_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    [HttpPost]
    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitChange(ConfigSMS entity)
    {
      if (!this.Request.IsAjaxRequest())
        return (ActionResult) this.RedirectToAction("Index");
      try
      {
        CONFIG_SMS configSms1 = entity.GetConfigSMS();
        if (configSms1.CONFIG_SMS_ID.Equals(0))
        {
          configSms1.CONFIG_SMS_NAME = configSms1.CONFIG_SMS_NAME.Trim();
          int? nullable1 = configSms1.TEMPLATES_ID;
          if ((nullable1.GetValueOrDefault() != -1 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
          {
            CONFIG_SMS configSms2 = configSms1;
            nullable1 = new int?();
            int? nullable2 = nullable1;
            configSms2.TEMPLATES_ID = nullable2;
          }
          configSms1.ISDELETE = new bool?(false);
          configSms1.DATE_CREATE = new DateTime?(DateTime.Now);
          configSms1.USER_CREATE_ID = new int?(this.UserX.ADMIN_USER_ID);
          this.unitOfWork.ConfigSMS.Add(configSms1);
          this.unitOfWork.ConfigSMS.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, "", Localization.MsgAddSuccess, "N/A", configSms1.CONFIG_SMS_ID, "", "");
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        CONFIG_SMS byId = this.unitOfWork.ConfigSMS.GetById(configSms1.CONFIG_SMS_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.CONFIG_SMS_NAME = configSms1.CONFIG_SMS_NAME.Trim();
        byId.ALARM_TIME_HOUR = configSms1.ALARM_TIME_HOUR;
        byId.ALARM_TIME_DAY = configSms1.ALARM_TIME_DAY;
        byId.ALARM_COUNT = configSms1.ALARM_COUNT;
        byId.REPEAT_ALARM_HOUR = configSms1.REPEAT_ALARM_HOUR;
        byId.REPEAT_ALARM_MINUTES = configSms1.REPEAT_ALARM_MINUTES;
        byId.LM_DEPARTMENT_ID = configSms1.LM_DEPARTMENT_ID;
        int? nullable3 = configSms1.TEMPLATES_ID;
        if ((nullable3.GetValueOrDefault() < 0 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
        {
          byId.TEMPLATES_ID = configSms1.TEMPLATES_ID;
        }
        else
        {
          CONFIG_SMS configSms2 = byId;
          nullable3 = new int?();
          int? nullable1 = nullable3;
          configSms2.TEMPLATES_ID = nullable1;
        }
        byId.ISACTIVED = configSms1.ISACTIVED;
        byId.DATE_START = configSms1.DATE_START;
        byId.DATE_END = configSms1.DATE_END;
        this.unitOfWork.ConfigSMS.Update(byId);
        this.unitOfWork.ConfigSMS.Save();
        this.WriteLog(enLogType.NomalLog, enActionType.Update, byId.CONFIG_SMS_NAME, Localization.MsgEditSuccess, "N/A", configSms1.CONFIG_SMS_ID, "", "");
        return this.ReturnValue(Localization.MsgEditSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "Index");
      }
    }

    [CustomAuthorize]
    [HttpPost]
    [ActionDescription(ActionCode = "CONFIG_SMS_ACTIVE", ActionName = "Active/deactive cấu hình", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS", IsMenu = false)]
    [ValidateJsonAntiForgeryToken]
    public ActionResult ActiveChage(int configSMSId, bool active)
    {
      if (configSMSId <= 0)
        return (ActionResult) this.Json(JsonResponse.Json400((object) "Cấu hình không tồn tại "));
      CONFIG_SMS byId = this.unitOfWork.ConfigSMS.GetById(configSMSId);
      if (byId == null)
        return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật thành công !"));
      byId.ISACTIVED = active;
      this.unitOfWork.ConfigSMS.Update(byId);
      this.WriteLog(enLogType.NomalLog, enActionType.Update, "Active/Deactive cấu hình gửi SMS [" + byId.CONFIG_SMS_NAME + "]", "N/A", "N/A", configSMSId, "", "");
      return (ActionResult) this.Json(JsonResponse.Json200((object) "Cập nhật thành công !"));
    }

    [HttpGet]
    [CustomAuthorize]
    [ActionDescription(ActionCode = "CONFIG_SMS_UPDATE", ActionName = "Cập nhật thông tin", GroupCode = "CONFIG_SMS", GroupName = "Cấu hình gửi SMS")]
    public ActionResult GetTemplate(int departmentId)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      List<TEMPLATE> listByDate = this.unitOfWork.Templates.GetListByDate(departmentId, DateTime.Now, Convert.ToInt32((object) TemplateStatus.Aproved));
      selectListItemList.Add(new SelectListItem()
      {
        Text = Localization.CalendarDefault,
        Value = "0",
        Selected = false
      });
      if (listByDate != null && listByDate.Count > 0)
      {
        for (int index = 0; index < listByDate.Count; ++index)
          selectListItemList.Add(new SelectListItem()
          {
            Text = listByDate[index].TEMPLATE_NAME.ToString(),
            Value = listByDate[index].TEMPLATES_ID.ToString(),
            Selected = false
          });
      }
      SelectListItem selectListItem = new SelectListItem()
      {
        Text = Localization.LabelSelect,
        Value = ""
      };
      selectListItemList.Insert(0, selectListItem);
      return (ActionResult) this.Json((object) selectListItemList, JsonRequestBehavior.AllowGet);
    }
  }
}
