using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BachMaiCR.Utilities;
using BachMaiCR.Web.App_Start;
using BachMaiCR.Web.Controllers;
using StackExchange.Profiling;
using System.Web.Script.Serialization;
using BachMaiCR.Web.Models;
using AutoMapper;

namespace BachMaiCR.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BundleTable.EnableOptimizations = false;
//#if DEBUG
//            BundleTable.EnableOptimizations = false;
//#else
//            BundleTable.EnableOptimizations = true;
//#endif
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // (new BachMaiCRConfig()).Initialization();
            //AutoMapperStartup.Initialize();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());

        }
        protected void Application_BeginRequest()
        {
            // Sử dụng để redirect sang https nếu có cầu hình
            if (!Request.IsLocal)
            {
                //Request.IsSecureConnection
                if ((ConfigurationManager.AppSettings["requireSsl"] ?? "false").ToLower() == "true" && !Request.IsSecureConnection)
                {
                    string securePort = ConfigurationManager.AppSettings["sslPort"];
                    var path = "https://" + Request.Url.Host + (securePort == "443" || securePort == "" ? "" : string.Format(":{0}", securePort)) + Request.Url.PathAndQuery;
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", path);
                    //Response.Redirect(path);
                }
            }
            //if (Request.IsLocal)
            //{
            //    MiniProfiler.Start();
            //} //or any number of other checks, up to you 
        }

        protected void Application_AcquireRequestState()
        {
        }

        protected void Application_PreRequestHandlerExecute()
        {
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop(); //stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
        }

        /// <summary>
        /// Chú ý phải sử dụng <customErrors mode="Off"/> trong webconfig
        /// Hàm này sử dụng để redirect sang 1 trang khác khi ứng dụng gặp lỗi
        /// Hiện đang để ở trạng thái DEBUG để tiện cho việc phát triển
        /// Khi ứng dụng được build ở dạng release, đoạn code tự có tác dụng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
#if DEBUG

#else
            var ex = Server.GetLastError().GetBaseException();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "ShowErrorPage");
            if (ex.GetType() == typeof(HttpException))
            {
                var httpException = (HttpException)ex;
                var code = httpException.GetHttpCode();
                routeData.Values.Add("status", code);
            }
            else
            {
                routeData.Values.Add("status", 500);
            }
            routeData.Values.Add("error", ex);

            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
#endif

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
#if DEBUG

#else
            if (Context.Response.StatusCode == 401)
            {
                throw new HttpException(401, "You are not authorised");
            }
#endif

        }


        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                // Hàm lấy thông người dùng đã lưu trong  FormsAuthentication
                // Phục cho việc lấy thông người dùng đăng nhập tại Control Base
                // Hungcd1

                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                var serializeModel = serializer.Deserialize<AppUserPrincipalSerializeModel>(authTicket.UserData);
                if (serializeModel != null)
                {
                    var newUser = new AppUser(authTicket.Name);
                    if (newUser != null)
                    {
                        newUser.UserId = serializeModel.UserId;
                        newUser.UserLogin = serializeModel.UserLogin;
                        newUser.UserName = serializeModel.UserName;
                        HttpContext.Current.User = newUser;
                    }
                }
            }
        }
    }


}