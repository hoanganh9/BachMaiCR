// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.ErrorController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
  public class ErrorController : Controller
  {
    public ActionResult ShowErrorPage(int? status)
    {
      return (ActionResult) this.View((object) status);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
    }
  }
}
