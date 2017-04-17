using Resources;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Utilities.Enums;
using BachMaiCR.Utilities.Ftp;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Models;

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
        if (this.Request.IsAuthenticated)
            return this.RedirectToAction("Index", "Home");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [AllowAnonymous]
    public ActionResult ForgotPassword()
    {
      if (this.Request.IsAuthenticated)
        return this.RedirectToAction("Index", "Home");
      return this.View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LoginModel model, string returnUrl)
    {
      if (this.ModelState.IsValid)
      {
        ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(model.UserName.Trim());
        int result = 0;
        if (this.Session["loginfail"] == null)
          this.Session["loginfail"] = 1;
        else if (!string.IsNullOrEmpty(this.Session["loginfail"].ToString()))
          int.TryParse(this.Session["loginfail"].ToString(), out result);
        string str = "";
        if (this.Session["Captcha"] != null)
          str = this.Session["Captcha"].ToString();
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
            HttpCookie cookie1 = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie1 != null)
            {
              cookie1.Expires = DateTime.Now.AddDays(-1.0);
              this.Response.Cookies.Add(cookie1);
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
            this.unitOfWork.Users.Update(byUserName);
            this.Response.Cookies.Add(cookie2);
            return this.RedirectToLocal(returnUrl);
          }
          Notice.Show(Localization.LoginFail, NoticeType.Error);
        }
        else if (model.ConfirmCaptcha != null && model.ConfirmCaptcha.Equals(str))
        {
          if (byUserName != null && Encrypt.Sha1HashWithHex(model.Password + byUserName.SALT).Equals(byUserName.PASSWORD))
          {
            HttpCookie cookie1 = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie1 != null)
            {
              cookie1.Expires = DateTime.Now.AddDays(-1.0);
              this.Response.Cookies.Add(cookie1);
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
            this.unitOfWork.Users.Update(byUserName);
            this.Response.Cookies.Add(cookie2);
            return this.RedirectToLocal(returnUrl);
          }
          Notice.Show(Localization.LoginFail, NoticeType.Error);
        }
        else
          Notice.Show(Localization.LoginConfirmCaptchaFail, NoticeType.Error);
        ++result;
        this.Session["loginfail"] = result;
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
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      if (byUserName != null)
      {
        byUserName.SESSION_TOKEN = null;
        this.unitOfWork.Users.Update(byUserName);
      }
      this.Session.Abandon();
      FormsAuthentication.SignOut();
      return this.RedirectToAction("Login", "Account");
    }

    public ActionResult ChangePassword(string returnUrl)
    {
      if (!this.Request.IsAuthenticated)
        return this.RedirectToAction("Login");
      
        ViewBag.ReturnUrl = returnUrl;
      return this.View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ChangePassword(LocalPasswordModel model, string returnUrl)
    {
      ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
      bool flag1 = byUserName != null;
        ViewBag.HasLocalPassword = flag1;
        ViewBag.ReturnUrl = returnUrl;
            
      if (flag1)
      {
        if (this.ModelState.IsValid)
        {
          bool flag2;
          try
          {
            if (Encrypt.Sha1HashWithHex(model.OldPassword + byUserName.SALT).Equals(byUserName.PASSWORD))
            {
              string str = Encrypt.Sha1HashWithHex(model.NewPassword + byUserName.SALT);
              byUserName.PASSWORD = str;
              this.WriteLog(enLogType.NomalLog, enActionType.Update, "Thay đổi mật khẩu. Người dùng : [" + byUserName.FULLNAME + "]_[" + byUserName.USERNAME + "]", "N/A", "N/A", 0, "", "");
              this.unitOfWork.Users.Update(byUserName);
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
            this.ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        }
      }
      return this.View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult CaptchaImage()
    {
      string randomCode = new CaptchaImage().GenerateRandomCode();
      this.Session["Captcha"] = randomCode;
      FileContentResult fileContentResult;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new CaptchaImage(randomCode, 300, 45).Image.Save((Stream) memoryStream, ImageFormat.Jpeg);
        fileContentResult = this.File(memoryStream.GetBuffer(), "image/jpeg");
      }
      return fileContentResult;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult TestCaptcha()
    {
      return this.Json(JsonResponse.Json200(Localization.MsgDelSuccess), JsonRequestBehavior.AllowGet);
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
      if (this.Url.IsLocalUrl(returnUrl))
        return this.Redirect(returnUrl);
      return this.RedirectToAction("Index", "Home");
    }

    public enum ManageMessageId
    {
      ChangePasswordSuccess,
      SetPasswordSuccess,
      RemoveLoginSuccess,
    }
  }
}
