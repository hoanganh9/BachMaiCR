// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ConfigParameterController
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
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class ConfigParameterController : BaseController
  {
    public string sViewPath = "~/Views/ConfigParameter/";

    public ConfigParameterController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
    [CustomAuthorize]
    public ActionResult Index()
    {
      int? nullable = this.RootDepartment();
ViewBag.RootDepartment = nullable;
      return (ActionResult) this.View(this.sViewPath + "_Add.cshtml", (object) new ConfigParametter());
    }

    [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
    [HttpGet]
    [CustomAuthorize]
    public ActionResult GetDetail(int deparmentId)
    {
      DateTime dateTime = BachMaiCR.Web.Utils.Utils.GetDateTime();
      List<CONFIG_PARAMETES> all = this.unitOfWork.ConfigParameter.GetAll(new int?(deparmentId), dateTime.Year, 2);
      if (all == null || all.Count <= 0)
        return (ActionResult) this.Json((object) new ConfigParametter(), JsonRequestBehavior.AllowGet);
      CONFIG_PARAMETES byId = this.unitOfWork.ConfigParameter.GetById(all[0].CONFIG_PARAMETES_ID);
      if (byId == null)
        throw new Exception(Localization.MsgItemNotExist);
      return (ActionResult) this.Json((object) new ConfigParametter(byId), JsonRequestBehavior.AllowGet);
    }

    private int? RootDepartment()
    {
      return this.unitOfWork.Users.GetByUserName(this.User.Identity.Name).LM_DEPARTMENT_ID;
    }

    private int getUserId(string userName)
    {
      return this.unitOfWork.Users.GetByUserName(userName).ADMIN_USER_ID;
    }

    [HttpPost]
    [ActionDescription(ActionCode = "FEAST_SAVE", ActionName = "Cập nhật danh mục ngày nghỉ lễ", GroupCode = "FEAST", GroupName = "Danh mục ngày nghỉ lễ")]
    [ValidateInput(false)]
    public ActionResult SubmitChange(ConfigParametter entity)
    {
      try
      {
        CONFIG_PARAMETES configParametter = entity.GetConfigParametter();
        if (entity.CONFIG_PARAMETES_ID.Equals(0))
        {
          DateTime dateTime = BachMaiCR.Web.Utils.Utils.GetDateTime();
          configParametter.CONFIG_YEAR = dateTime.Year;
          configParametter.DATE_CREATE = new DateTime?(dateTime);
          configParametter.USER_CREATE_ID = new int?(this.UserX.ADMIN_USER_ID);
          configParametter.CONFIG_TYPE = new int?(2);
          this.unitOfWork.ConfigParameter.Add(configParametter);
          this.unitOfWork.ConfigParameter.Save();
          this.WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", Localization.MsgAddSuccess, "N/A", configParametter.CONFIG_PARAMETES_ID, "", "");
          Notice.Show(Localization.MsgAddSuccess, NoticeType.Success);
          return this.ReturnValue(Localization.MsgAddSuccess, true, "Index");
        }
        CONFIG_PARAMETES byId = this.unitOfWork.ConfigParameter.GetById(configParametter.CONFIG_PARAMETES_ID);
        if (byId == null)
          throw new Exception(Localization.MsgItemNotExist);
        byId.LM_DEPARTMENT_ID = configParametter.LM_DEPARTMENT_ID;
        byId.TIME_DISTANCE = configParametter.TIME_DISTANCE;
        byId.IS_FIX_WEEKEND = configParametter.IS_FIX_WEEKEND;
        byId.TIME_DISTANCE_OF_HOLIDAY = configParametter.TIME_DISTANCE_OF_HOLIDAY;
        byId.NUMBER_DOCTOR_IN_DAY = configParametter.NUMBER_DOCTOR_IN_DAY;
        byId.NORMS_DIRECT = configParametter.NORMS_DIRECT;
        byId.IS_FEMALE_DIRECT_AM = configParametter.IS_FEMALE_DIRECT_AM;
        byId.DESCRIPTION = configParametter.DESCRIPTION;
        this.unitOfWork.ConfigParameter.Update(byId);
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", configParametter.CONFIG_PARAMETES_ID, "", "");
        Notice.Show(Localization.MsgEditSuccess, NoticeType.Success);
        return this.ReturnValue(Localization.MsgEditSuccess, true, "Index");
      }
      catch (Exception ex)
      {
        this.WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, "", "");
        return this.ReturnValue(ex.Message, false, "~/Views/ConfigParameter/_Add.cshtml");
      }
    }
  }
}
