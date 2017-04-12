/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using Ninject;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DataAccess;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
namespace BachMaiCR.Web.Common.Attributes
{
    /// <summary>
    /// Attribute để lấy quyền và xác thực người dùng
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string CONTROLLERKEY = "controller";
        private const string ACTIONKEY = "action";

        [Inject]
        public IUnitOfWork UnitOfWork { get; set; }

        [Inject]
        public ICacheProvider CacheProvider { get; set; }

        /// <summary>
        /// Xem đối tượng đang bị tác động là đối tượng nào? đang được phân ở quyền gì, đọ với thông tin quyền của người dùng để kiểm tra
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
             ADMIN_USER u = UnitOfWork.Users.GetByUserName(httpContext.User.Identity.Name);
            bool isAdmin = u.IS_ADMIN == null ? false : (bool)u.IS_ADMIN;
            bool userActive = u.ISACTIVED == null ? false : (bool)u.ISACTIVED;
            bool userDelete = u.ISDELETE == null ? false : (bool)u.ISDELETE;
            if (!userActive || userDelete)
            {
                FormsAuthentication.SignOut();
                return false;
            }
            // neu la admin thi khong xet quyen nua
            if (!isAdmin)
            {
                string controllerName = httpContext.Request.RequestContext.RouteData.Values[CONTROLLERKEY].ToString();
                if (!controllerName.ToLower().EndsWith(CONTROLLERKEY))
                {
                    controllerName += CONTROLLERKEY;
                }
                string actionName = httpContext.Request.RequestContext.RouteData.Values[ACTIONKEY].ToString();
                string method = httpContext.Request.HttpMethod;
                string uniqueCode = string.Format(@"{0}{1}{2}", controllerName.ToLower(), actionName.ToLower(),
                                                  method.ToLower());
                var actionCodes = UnitOfWork.Functons.GetActionCodeListByUniqueCode(uniqueCode).ToList();
                IPrincipal user = httpContext.User;

                //Nếu các thông tin check quyền không đầy đủ thì sử dụng check quyền mặc định của trình duyệt
                if (user == null)
                {
                    return base.AuthorizeCore(httpContext);
                }
                if (!actionCodes.Any())
                {
                    return base.AuthorizeCore(httpContext);
                }
                var authen = actionCodes.Any(user.IsInRole);

                return authen;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Lấy danh sách quyền của người dùng hiện tại
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated ||
                filterContext.HttpContext.Request.Cookies == null ||
                filterContext.HttpContext.Request.Cookies[cookieName] == null)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }
            // Thông tin người dùng.
            string userName = filterContext.HttpContext.User.Identity.Name.Trim();
            ADMIN_USER u = UnitOfWork.Users.GetByUserName(userName);
            bool userActive = u.ISACTIVED == null ? false : (bool) u.ISACTIVED;
            bool userDelete = u.ISDELETE == null ? false : (bool) u.ISDELETE;
            if(!userActive || userDelete)
            {
                FormsAuthentication.SignOut();
                HandleUnauthorizedRequest(filterContext);
                return;
            }
#if DEBUG

#else
            string sessionToken;
            if (filterContext.HttpContext.Session[WebConst.CacheSessionToken] == null)
            {
                sessionToken = u.SESSION_TOKEN;
                if (!string.IsNullOrWhiteSpace(sessionToken))
                {
                    filterContext.HttpContext.Session[WebConst.CacheSessionToken] = sessionToken;
                }
            }
            else
            {
                sessionToken = filterContext.HttpContext.Session[WebConst.CacheSessionToken].ToString();
            }


            //HttpCookie cookie = filterContext.HttpContext.Request.Cookies[cookieName];
            //if (Encrypt.Sha1HashWithHex(cookie.Value) != sessionToken)
            //{
            //    HandleUnauthorizedRequest(filterContext);
            //    return;
            //}
#endif
            var userRoles = UnitOfWork.Users.GetActionCodesByUserName(userName).ToArray();//cache

            var userIdentity = new GenericIdentity(userName);
            var userPrincipal = new GenericPrincipal(userIdentity, userRoles);
            filterContext.HttpContext.User = userPrincipal;


            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Nếu không authen được thì trả về 403
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {


            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new Http403Result();
            }
            else
            {
                filterContext.Result = new RedirectResult("/Home/Index");
            }
        }

        /// <summary>
        /// Trả về statuscode 403 nếu request không authen được
        /// </summary>
        private class Http403Result : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = 403;
                context.HttpContext.Response.Write("UnAuthorized");
            }
        }
    }
}