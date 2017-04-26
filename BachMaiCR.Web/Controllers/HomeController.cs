using BachMaiCR.DataAccess;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Web.Models;
using System.Web.Mvc;

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
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Message = string.Empty;
            return View();
        }

        public string GetUrlActve()
        {
            return unitOfWork.Users.GetUrlActive(User.Identity.Name);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}