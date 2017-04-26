using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

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
            int? nullable = RootDepartment();
            ViewBag.RootDepartment = nullable;
            return View(sViewPath + "_Add.cshtml", new ConfigParametter());
        }

        [ActionDescription(ActionCode = "Template_View", ActionName = "Xem biểu mẫu", GroupCode = "Template", GroupName = "Cấu hình biểu mẫu")]
        [HttpGet]
        [CustomAuthorize]
        public ActionResult GetDetail(int deparmentId)
        {
            DateTime dateTime = Utils.Utils.GetDateTime();
            List<CONFIG_PARAMETES> all = unitOfWork.ConfigParameter.GetAll(deparmentId, dateTime.Year, 2);
            if (all == null || all.Count <= 0)
                return Json(new ConfigParametter(), JsonRequestBehavior.AllowGet);
            CONFIG_PARAMETES byId = unitOfWork.ConfigParameter.GetById(all[0].CONFIG_PARAMETES_ID);
            if (byId == null)
                throw new Exception(Localization.MsgItemNotExist);
            return Json(new ConfigParametter(byId), JsonRequestBehavior.AllowGet);
        }

        private int? RootDepartment()
        {
            return unitOfWork.Users.GetByUserName(User.Identity.Name).LM_DEPARTMENT_ID;
        }

        private int getUserId(string userName)
        {
            return unitOfWork.Users.GetByUserName(userName).ADMIN_USER_ID;
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
                    DateTime dateTime = Utils.Utils.GetDateTime();
                    configParametter.CONFIG_YEAR = dateTime.Year;
                    configParametter.DATE_CREATE = dateTime;
                    configParametter.USER_CREATE_ID = UserX.ADMIN_USER_ID;
                    configParametter.CONFIG_TYPE = 2;
                    unitOfWork.ConfigParameter.Add(configParametter);
                    unitOfWork.ConfigParameter.Save();
                    WriteLog(enLogType.NomalLog, enActionType.Insert, "N/A", Localization.MsgAddSuccess, "N/A", configParametter.CONFIG_PARAMETES_ID, string.Empty, string.Empty);
                    Notice.Show(Localization.MsgAddSuccess, NoticeType.Success);
                    return ReturnValue(Localization.MsgAddSuccess, true, "Index");
                }
                CONFIG_PARAMETES byId = unitOfWork.ConfigParameter.GetById(configParametter.CONFIG_PARAMETES_ID);
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
                unitOfWork.ConfigParameter.Update(byId);
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", Localization.MsgEditSuccess, "N/A", configParametter.CONFIG_PARAMETES_ID, string.Empty, string.Empty);
                Notice.Show(Localization.MsgEditSuccess, NoticeType.Success);
                return ReturnValue(Localization.MsgEditSuccess, true, "Index");
            }
            catch (Exception ex)
            {
                WriteLog(enLogType.NomalLog, enActionType.Update, "N/A", "N/A", ex.Message, 0, string.Empty, string.Empty);
                return ReturnValue(ex.Message, false, "~/Views/ConfigParameter/_Add.cshtml");
            }
        }
    }
}