using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Utilities.Ftp;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Models;
using Resources;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace BachMaiCR.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private IFtpClient ftpClient;

        public AccountController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider, IFtpClient ftpClient)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.ftpClient = ftpClient;
            this.cacheProvider = cacheProvider;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(model.UserName.Trim());
                int result = 0;
                if (Session["loginfail"] == null)
                    Session["loginfail"] = 1;
                else if (!string.IsNullOrEmpty(Session["loginfail"].ToString()))
                    int.TryParse(Session["loginfail"].ToString(), out result);
                string str = string.Empty;
                if (Session["Captcha"] != null)
                    str = Session["Captcha"].ToString();
                if (result < 4)
                {
                    int num;
                    if (byUserName != null)
                    {
                        bool? isactived = byUserName.ISACTIVED;
                        num = (!isactived.GetValueOrDefault() ? 0 : (isactived.HasValue ? 1 : 0)) == 0 ? 1 : 0;
                    }
                    else
                        num = 1;
                    if (num == 0 && Encrypt.Sha1HashWithHex(model.Password + byUserName.SALT).Equals(byUserName.PASSWORD))
                    {
                        HttpCookie cookie1 = Request.Cookies[FormsAuthentication.FormsCookieName];
                        if (cookie1 != null)
                        {
                            cookie1.Expires = DateTime.Now.AddDays(-1.0);
                            Response.Cookies.Add(cookie1);
                        }
                        string userData = new JavaScriptSerializer().Serialize(new AppUserPrincipalSerializeModel()
                        {
                            UserId = byUserName.ADMIN_USER_ID,
                            UserName = byUserName.FULLNAME,
                            UserLogin = byUserName.USERNAME
                        });
                        string input = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, model.UserName.Trim(), DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), model.RememberMe, userData, FormsAuthentication.FormsCookiePath));
                        HttpCookie cookie2 = new HttpCookie(FormsAuthentication.FormsCookieName, input);
                        byUserName.SESSION_TOKEN = Encrypt.Sha1HashWithHex(input);
                        unitOfWork.Users.Update(byUserName);
                        Response.Cookies.Add(cookie2);
                        return RedirectToLocal(returnUrl);
                    }
                    Notice.Show(Localization.LoginFail, NoticeType.Error);
                }
                else if (model.ConfirmCaptcha != null && model.ConfirmCaptcha.Equals(str))
                {
                    if (byUserName != null && Encrypt.Sha1HashWithHex(model.Password + byUserName.SALT).Equals(byUserName.PASSWORD))
                    {
                        HttpCookie cookie1 = Request.Cookies[FormsAuthentication.FormsCookieName];
                        if (cookie1 != null)
                        {
                            cookie1.Expires = DateTime.Now.AddDays(-1.0);
                            Response.Cookies.Add(cookie1);
                        }
                        string userData = new JavaScriptSerializer().Serialize(new AppUserPrincipalSerializeModel()
                        {
                            UserId = byUserName.ADMIN_USER_ID,
                            UserName = byUserName.FULLNAME,
                            UserLogin = byUserName.USERNAME
                        });
                        string input = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, model.UserName.Trim(), DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), model.RememberMe, userData, FormsAuthentication.FormsCookiePath));
                        HttpCookie cookie2 = new HttpCookie(FormsAuthentication.FormsCookieName, input);
                        byUserName.SESSION_TOKEN = Encrypt.Sha1HashWithHex(input);
                        unitOfWork.Users.Update(byUserName);
                        Response.Cookies.Add(cookie2);
                        return RedirectToLocal(returnUrl);
                    }
                    Notice.Show(Localization.LoginFail, NoticeType.Error);
                }
                else
                    Notice.Show(Localization.LoginConfirmCaptchaFail, NoticeType.Error);
                ++result;
                Session["loginfail"] = result;
                if (result >= 4)
                {
                    ViewBag.EnableCaptcha = 1;
                }
                else
                {
                    ViewBag.EnableCaptcha = 0;
                }
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogOff()
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            if (byUserName != null)
            {
                byUserName.SESSION_TOKEN = null;
                unitOfWork.Users.Update(byUserName);
            }
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ChangePassword(string returnUrl)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel model, string returnUrl)
        {
            ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
            bool flag1 = byUserName != null;
            ViewBag.HasLocalPassword = flag1;
            ViewBag.ReturnUrl = returnUrl;
            if (flag1)
            {
                if (ModelState.IsValid)
                {
                    bool flag2;
                    try
                    {
                        if (Encrypt.Sha1HashWithHex(model.OldPassword + byUserName.SALT).Equals(byUserName.PASSWORD))
                        {
                            string str = Encrypt.Sha1HashWithHex(model.NewPassword + byUserName.SALT);
                            byUserName.PASSWORD = str;
                            WriteLog(enLogType.NomalLog, enActionType.Update, "Thay đổi mật khẩu. Người dùng : [" + byUserName.FULLNAME + "]_[" + byUserName.USERNAME + "]", "N/A", "N/A", 0, string.Empty, string.Empty);
                            unitOfWork.Users.Update(byUserName);
                            flag2 = true;
                        }
                        else
                            flag2 = false;
                    }
                    catch (Exception ex)
                    {
                        flag2 = false;
                    }
                    if (flag2)
                        Notice.Show("Thay đổi mật khẩu thành công !", NoticeType.Success);
                    else
                        ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CaptchaImage()
        {
            string randomCode = new CaptchaImage().GenerateRandomCode();
            Session["Captcha"] = randomCode;
            FileContentResult fileContentResult;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new CaptchaImage(randomCode, 300, 45).Image.Save((Stream)memoryStream, ImageFormat.Jpeg);
                fileContentResult = File(memoryStream.GetBuffer(), "image/jpeg");
            }
            return fileContentResult;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TestCaptcha()
        {
            return Json(JsonResponse.Json200(Localization.MsgDelSuccess), JsonRequestBehavior.AllowGet);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
    }
}