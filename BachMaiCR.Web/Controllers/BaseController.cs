// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.BaseController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.ServiceSMS6x00;

namespace BachMaiCR.Web.Controllers
{
  public abstract class BaseController : Controller
  {
    protected IUnitOfWork unitOfWork;
    protected ICacheProvider cacheProvider;
    protected List<string> PermistionList;

    public ADMIN_USER UserX
    {
      get
      {
        return this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      }
    }

    protected BaseController(IUnitOfWork uow, ICacheProvider cache)
    {
      this.cacheProvider = cache;
      this.unitOfWork = uow;
    }

    protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
      Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
      return base.BeginExecuteCore(callback, state);
    }

    protected override void EndExecute(IAsyncResult asyncResult)
    {
      if (this.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower().Contains("user"))
        this.cacheProvider.RemoveByTerm("OFFICELIST-");
      base.EndExecute(asyncResult);
    }

    protected ActionResult IsAjaxRequest()
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction("Index", "Home");
      return null;
    }

    public bool WriteLog(enLogType typeLog, enActionType action, string content = "N/A", string description = "N/A", string errorCode = "N/A", int objId = 0, string menuCode = "", string menuName = "")
    {
      ActionDescriptionAttribute attributeDescription = BaseController.GetAttributeDescription(this.GetType().Name, this.ControllerContext.RouteData.Values["action"].ToString());
      try
      {
        ADMIN_LOG entity = new ADMIN_LOG();
        entity.START_TIME = DateTime.Now;
        entity.END_TIME = new DateTime?(DateTime.Now);
        entity.SESSION_ID = this.Session.SessionID;
        entity.STATUS = (int) typeLog;
        entity.ACTION_TYPE = (int) action;
        string name = Assembly.GetExecutingAssembly().GetName().Name;
        entity.APP_CODE = string.IsNullOrEmpty(name) ? "N/A" : name.Trim();
        entity.CONTENT = string.IsNullOrEmpty(content) ? "N/A" : content.Trim();
        entity.DESCRIPTION = string.IsNullOrEmpty(description) ? "N/A" : description.Trim();
        entity.ERROR_CODE = string.IsNullOrEmpty(errorCode) ? "N/A" : errorCode.Trim();
        entity.ACTION_CODE = string.IsNullOrEmpty(attributeDescription.ActionCode) ? "N/A" : attributeDescription.ActionCode.Trim();
        entity.ACTION_NAME = string.IsNullOrEmpty(attributeDescription.ActionName) ? "N/A" : attributeDescription.ActionName.Trim();
        entity.MENU_CODE = !(menuCode != "") ? (string.IsNullOrEmpty(attributeDescription.GroupCode) ? "N/A" : attributeDescription.GroupCode.Trim()) : menuCode;
        entity.MENU_NAME = !(menuName != "") ? (string.IsNullOrEmpty(attributeDescription.GroupName) ? "N/A" : attributeDescription.GroupName.Trim()) : menuName;
        string ip4Address = BachMaiCR.Web.Utils.Utils.GetIP4Address();
        entity.IP_ADDRESS = string.IsNullOrEmpty(ip4Address) ? "N/A" : ip4Address.Trim();
        entity.LEVEL = new int?(0);
        entity.OBJECT_ID = new int?(objId);
        entity.THREAD_ID = Process.GetCurrentProcess().Id.ToString();
        ADMIN_USER userX = this.UserX;
        if (userX == null)
        {
          entity.USER_NAME = "N/A";
          entity.USER_LOGIN = "N/A";
          entity.ADMIN_USER_ID = new int?(0);
        }
        else
        {
          entity.USER_NAME = userX.FULLNAME;
          entity.USER_LOGIN = userX.USERNAME;
          entity.ADMIN_USER_ID = new int?(userX.ADMIN_USER_ID);
        }
        this.unitOfWork.AdminLogs.Add(entity);
        this.unitOfWork.Save();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void sendSMS(List<SendSms6x00> listSms)
    {
      if (!(ConfigurationManager.AppSettings["sendSMS"] == "1"))
        return;
      InsertMTInfClient insertMtInfClient = new InsertMTInfClient();
      insertMtInfClient.ClientCredentials.UserName.UserName = "admin";
      insertMtInfClient.ClientCredentials.UserName.Password = "admin";
      string username = "cpbvbachmai";
      string password = "123456a@";
      string cpCode = "CPBVBACHMAI";
      string requestID = "4";
      string serviceID = "BM-Lichtruc";
      string commandCode = "CPBVBACHMAI";
      string contentType = "0";
      if (insertMtInfClient.State != CommunicationState.Opening)
        insertMtInfClient.Open();
      if (listSms != null && listSms.Count > 0)
      {
        for (int index = 0; index < listSms.Count; ++index)
        {
          string phone = listSms[index].Phone;
          string receiveID;
          string userID = receiveID = phone;
          string str = BachMaiCR.Web.Utils.Utils.ConvertVN(BachMaiCR.Web.Utils.Utils.StripHTML(listSms[index].Contents));
          try
          {
            if (insertMtInfClient.insertMT(username, password, cpCode, requestID, userID, receiveID, serviceID, commandCode, contentType, str) == "1")
            {
              string content;
              if (listSms[index].Types != "1")
                content = "Gửi thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
              else
                content = "Gửi thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
              this.WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, str, "N/A", listSms[index].DoctorId, "", "");
            }
            else
            {
              string content;
              if (listSms[index].Types != "1")
                content = "Gửi không thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
              else
                content = "Gửi không thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
              this.WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, str, "N/A", listSms[index].DoctorId, "", "");
            }
          }
          catch (Exception ex)
          {
            string content;
            if (listSms[index].Types != "1")
              content = "Gửi không thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
            else
              content = "Gửi không thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
            this.WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, "N/A", ex.Message, 0, "", "");
          }
        }
      }
      insertMtInfClient.Close();
    }

    public static IEnumerable<MethodInfo> GetActions(string controller, string action)
    {
      return ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).Where((t => t.Name == controller && typeof (Controller).IsAssignableFrom(t))).SelectMany<Type, MethodInfo>((Func<Type, IEnumerable<MethodInfo>>) (type => ((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Instance | BindingFlags.Public)).Where((a => a.Name == action))));
    }

    public static ActionDescriptionAttribute GetAttributeDescription(string controller, string actionName)
    {
      foreach (MemberInfo memberInfo in ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).Where((t => t.Name == controller && typeof (Controller).IsAssignableFrom(t))).SelectMany<Type, MethodInfo>((Func<Type, IEnumerable<MethodInfo>>) (type => ((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Instance | BindingFlags.Public)).Where((a => a.Name == actionName)))))
      {
        ActionDescriptionAttribute descriptionAttribute = memberInfo.GetCustomAttributes(typeof (ActionDescriptionAttribute), false).Cast<ActionDescriptionAttribute>().FirstOrDefault();
        if (descriptionAttribute != null)
          return descriptionAttribute;
      }
      return new ActionDescriptionAttribute();
    }

    public ActionResult ReturnValue(string msg, bool success = true, string actionRedirect = "Index")
    {
      if (!this.Request.IsAjaxRequest())
        return this.RedirectToAction(actionRedirect);
      if (success)
        return this.Json(JsonResponse.Json200(msg));
      return this.Json(JsonResponse.Json500(msg));
    }

    public List<string> GetActionCodesByUserName()
    {
      if (this.PermistionList == null)
        this.PermistionList = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
      return this.PermistionList;
    }

    public virtual bool CheckPermistion(string actCode)
    {
      if (this.PermistionList == null)
        this.PermistionList = this.unitOfWork.Users.GetActionCodesByUserName(this.User.Identity.Name).ToList();
      return this.PermistionList.Any<string>() && this.PermistionList.Contains(actCode);
    }

    public List<LM_DEPARTMENT> GetDeptCurrent()
    {
      List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
      ADMIN_USER userX = this.UserX;
      if (userX.USERNAME == "admin")
      {
        lmDepartmentList = this.unitOfWork.Departments.GetChildDepartment(0).ToList();
      }
      else
      {
        int? lmDepartmentId;
        int num;
        if (userX.LM_DEPARTMENT_ID.HasValue)
        {
          lmDepartmentId = userX.LM_DEPARTMENT_ID;
          num = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
        }
        else
          num = 1;
        if (num == 0)
        {
          IDepartmentRepository departments = this.unitOfWork.Departments;
          lmDepartmentId = userX.LM_DEPARTMENT_ID;
          int id = lmDepartmentId.Value;
          LM_DEPARTMENT byId = departments.GetById(id);
          lmDepartmentList.Add(byId);
        }
      }
      return lmDepartmentList;
    }
  }
}
