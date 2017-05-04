using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.ServiceSMS6x00;
using BachMaiCR.Web.ServiceSMSBrandName;
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
                return unitOfWork.Users.GetByUserName(User.Identity.Name);
            }
        }

        protected BaseController(IUnitOfWork uow, ICacheProvider cache)
        {
            cacheProvider = cache;
            unitOfWork = uow;
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            if (HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower().Contains("user"))
                cacheProvider.RemoveByTerm("OFFICELIST-");
            base.EndExecute(asyncResult);
        }

        protected ActionResult IsAjaxRequest()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", "Home");
            return null;
        }

        public bool WriteLog(enLogType typeLog, enActionType action, string content = "N/A", string description = "N/A", string errorCode = "N/A", int objId = 0, string menuCode = "", string menuName = "")
        {
            ActionDescriptionAttribute attributeDescription = BaseController.GetAttributeDescription(GetType().Name, ControllerContext.RouteData.Values["action"].ToString());
            try
            {
                ADMIN_LOG entity = new ADMIN_LOG();
                entity.START_TIME = DateTime.Now;
                entity.END_TIME = DateTime.Now;
                entity.SESSION_ID = Session.SessionID;
                entity.STATUS = (int)typeLog;
                entity.ACTION_TYPE = (int)action;
                string name = Assembly.GetExecutingAssembly().GetName().Name;
                entity.APP_CODE = string.IsNullOrEmpty(name) ? "N/A" : name.Trim();
                entity.CONTENT = string.IsNullOrEmpty(content) ? "N/A" : content.Trim();
                entity.DESCRIPTION = string.IsNullOrEmpty(description) ? "N/A" : description.Trim();
                entity.ERROR_CODE = string.IsNullOrEmpty(errorCode) ? "N/A" : errorCode.Trim();
                entity.ACTION_CODE = string.IsNullOrEmpty(attributeDescription.ActionCode) ? "N/A" : attributeDescription.ActionCode.Trim();
                entity.ACTION_NAME = string.IsNullOrEmpty(attributeDescription.ActionName) ? "N/A" : attributeDescription.ActionName.Trim();
                entity.MENU_CODE = !(menuCode != string.Empty) ? (string.IsNullOrEmpty(attributeDescription.GroupCode) ? "N/A" : attributeDescription.GroupCode.Trim()) : menuCode;
                entity.MENU_NAME = !(menuName != string.Empty) ? (string.IsNullOrEmpty(attributeDescription.GroupName) ? "N/A" : attributeDescription.GroupName.Trim()) : menuName;
                string ip4Address = Utils.Utils.GetIP4Address();
                entity.IP_ADDRESS = string.IsNullOrEmpty(ip4Address) ? "N/A" : ip4Address.Trim();
                entity.LEVEL = 0;
                entity.OBJECT_ID = objId;
                entity.THREAD_ID = Process.GetCurrentProcess().Id.ToString();
                ADMIN_USER userX = UserX;
                if (userX == null)
                {
                    entity.USER_NAME = "N/A";
                    entity.USER_LOGIN = "N/A";
                    entity.ADMIN_USER_ID = 0;
                }
                else
                {
                    entity.USER_NAME = userX.FULLNAME;
                    entity.USER_LOGIN = userX.USERNAME;
                    entity.ADMIN_USER_ID = userX.ADMIN_USER_ID;
                }
                unitOfWork.AdminLogs.Add(entity);
                unitOfWork.Save();
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
                    string str = Utils.Utils.ConvertVN(Utils.Utils.StripHTML(listSms[index].Contents));
                    try
                    {
                        if (insertMtInfClient.insertMT(username, password, cpCode, requestID, userID, receiveID, serviceID, commandCode, contentType, str) == "1")
                        {
                            string content;
                            if (listSms[index].Types != "1")
                                content = "Gửi thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                            else
                                content = "Gửi thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                            WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, str, "N/A", listSms[index].DoctorId, string.Empty, string.Empty);
                        }
                        else
                        {
                            string content;
                            if (listSms[index].Types != "1")
                                content = "Gửi không thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                            else
                                content = "Gửi không thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                            WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, str, "N/A", listSms[index].DoctorId, string.Empty, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        string content;
                        if (listSms[index].Types != "1")
                            content = "Gửi không thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                        else
                            content = "Gửi không thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                        WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, "N/A", ex.Message, 0, string.Empty, string.Empty);
                    }
                }
            }
            insertMtInfClient.Close();
        }
        
        //Send sms BrandName with service LaHongMedia
        public void sendSMSBrandname(List<SendSms6x00> listSms)
        {
            if (!(ConfigurationManager.AppSettings["sendSMS"] == "1"))
                return;
            ServiceSoapClient serviceBrandname = new ServiceSoapClient();
            string brandName = ConfigurationManager.AppSettings["sendBrandname_Brandname"];
            string userName = ConfigurationManager.AppSettings["sendBrandname_UserName"];
            string passWord = ConfigurationManager.AppSettings["sendBrandname_PassWord"];
            string blockId = serviceBrandname.OpenBlock(brandName, userName, passWord);
            //Loi
            if(blockId == "-1")
            {

            }
            else
            {
                if (listSms != null && listSms.Count > 0)
                {
                    for (int index = 0; index < listSms.Count; ++index)
                    {
                        string phone = listSms[index].Phone;
                        string str = Utils.Utils.ConvertVN(Utils.Utils.StripHTML(listSms[index].Contents));
                        try
                        {
                            var result = serviceBrandname.SendMT(brandName, userName, passWord, phone, str, blockId);
                            if (result.StartsWith("200"))
                            {
                                string content;
                                if (listSms[index].Types != "1")
                                    content = "Gửi thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                                else
                                    content = "Gửi thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                                WriteLog(enLogType.NomalLog, enActionType.SendSMSBrandname, content, str, "N/A", listSms[index].DoctorId, string.Empty, string.Empty);
                            }
                            else
                            {
                                string content;
                                if (listSms[index].Types != "1")
                                    content = "Gửi không thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                                else
                                    content = "Gửi không thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                                WriteLog(enLogType.NomalLog, enActionType.SendSMSBrandname, content, str, MsgErrorSMSBrandname(result), listSms[index].DoctorId, string.Empty, string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            string content;
                            if (listSms[index].Types != "1")
                                content = "Gửi không thành công tin nhắn tới cho cán bộ: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                            else
                                content = "Gửi không thành công tin nhắn nhắc lịch tới đơn vị: '" + listSms[index].DoctorName + "' - Số điện thoại: '" + phone + "'";
                            WriteLog(enLogType.NomalLog, enActionType.SendSMS, content, "N/A", ex.Message, listSms[index].DoctorId, string.Empty, string.Empty);
                        }
                    }
                }
                serviceBrandname.CloseBlock(brandName, userName, passWord, blockId);
            }
        }

        private string MsgErrorSMSBrandname(string smsResult)
        {
            switch (smsResult)
            {
                case "201":
                    return "Gửi tin thất bại";
                case "202":
                    return "Tài khoản không đúng";
                case "203":
                    return "Số điện thoại không hợp lệ hoặc không hỗ trợ";
                case "204":
                    return "Tài khoản không hợp lệ";
                case "205":
                    return "Không đủ MT gửi tin";
                case "206":
                    return "Sai BlockId";
                case "207":
                    return "SMSId đã tồn tại";
                default://200|Số MT của tin nhắn gửi vừa|SMSId của tin nhắn 
                    return smsResult;

            }
        }

        public static IEnumerable<MethodInfo> GetActions(string controller, string action)
        {
            return (Assembly.GetExecutingAssembly().GetTypes()).Where((t => t.Name == controller && typeof(Controller).IsAssignableFrom(t))).SelectMany<Type, MethodInfo>((Func<Type, IEnumerable<MethodInfo>>)(type => (type.GetMethods(BindingFlags.Instance | BindingFlags.Public)).Where((a => a.Name == action))));
        }

        public static ActionDescriptionAttribute GetAttributeDescription(string controller, string actionName)
        {
            foreach (MemberInfo memberInfo in (Assembly.GetExecutingAssembly().GetTypes()).Where((t => t.Name == controller && typeof(Controller).IsAssignableFrom(t))).SelectMany<Type, MethodInfo>((Func<Type, IEnumerable<MethodInfo>>)(type => (type.GetMethods(BindingFlags.Instance | BindingFlags.Public)).Where((a => a.Name == actionName)))))
            {
                ActionDescriptionAttribute descriptionAttribute = memberInfo.GetCustomAttributes(typeof(ActionDescriptionAttribute), false).Cast<ActionDescriptionAttribute>().FirstOrDefault<ActionDescriptionAttribute>();
                if (descriptionAttribute != null)
                    return descriptionAttribute;
            }
            return new ActionDescriptionAttribute();
        }

        public ActionResult ReturnValue(string msg, bool success = true, string actionRedirect = "Index")
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction(actionRedirect);
            if (success)
                return Json(JsonResponse.Json200(msg));
            return Json(JsonResponse.Json500(msg));
        }

        public List<string> GetActionCodesByUserName()
        {
            if (PermistionList == null)
                PermistionList = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            return PermistionList;
        }

        public virtual bool CheckPermistion(string actCode)
        {
            if (PermistionList == null)
                PermistionList = unitOfWork.Users.GetActionCodesByUserName(User.Identity.Name).ToList();
            return PermistionList.Any() && PermistionList.Contains(actCode);
        }

        public List<LM_DEPARTMENT> GetDeptCurrent()
        {
            List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
            ADMIN_USER userX = UserX;
            if (userX.USERNAME == "admin")
            {
                lmDepartmentList = unitOfWork.Departments.GetChildDepartment(0).ToList();
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
                    IDepartmentRepository departments = unitOfWork.Departments;
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