using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BachMaiCR.Web.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            string serializedCookieToken = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[AntiForgeryConfig.CookieName];
            if (cookie != null)
            {
                serializedCookieToken = cookie.Value;
            }

            string serializedHeaderToken = HttpContext.Current.Request.Headers["__RequestVerificationToken"];
            AntiForgery.Validate(serializedCookieToken, serializedHeaderToken);
        }
    }
}
