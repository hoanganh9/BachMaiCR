using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ShowErrorPage(int? status)
        {
            return View(status);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}