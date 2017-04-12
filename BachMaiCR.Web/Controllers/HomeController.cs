// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.HomeController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class HomeController : BaseController
  {
    public HomeController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    public ActionResult Index()
    {
      if (string.IsNullOrEmpty(this.User.Identity.Name))
      {
        LoginModel loginModel = new LoginModel();
        return (ActionResult) this.RedirectToAction("Login", "Account");
      }
ViewBag.Message = "";
      return (ActionResult) this.View();
    }

    public string GetUrlActve()
    {
      return this.unitOfWork.Users.GetUrlActive(this.User.Identity.Name);
    }

    public ActionResult About()
    {
ViewBag.Message = "Your app description page.";
      return (ActionResult) this.View();
    }

    public ActionResult Contact()
    {
ViewBag.Message = "Your contact page.";
      return (ActionResult) this.View();
    }
  }
}
