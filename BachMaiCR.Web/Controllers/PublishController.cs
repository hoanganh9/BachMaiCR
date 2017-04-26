using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BachMaiCR.Web.Controllers
{
    public class PublishController : BaseController
    {
        public PublishController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
          : base(unitOfWork, cacheProvider)
        {
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult DepartmentTree(string elmID, int? deptRootID = 0, bool multiselect = false, string selectedlist = null)
        {
            ADMIN_USER userX = UserX;
            List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
            if (userX != null)
            {
                if (deptRootID.HasValue && deptRootID.Value > 0)
                {
                    LM_DEPARTMENT byId = unitOfWork.Departments.GetById(deptRootID.Value);
                    lmDepartmentList.Add(byId);
                }
                else if (userX.USERNAME == "admin")
                {
                    lmDepartmentList = unitOfWork.Departments.GetChildDepartment(0).ToList();
                }
                else if (userX.LM_DEPARTMENT_ID.HasValue && userX.LM_DEPARTMENT_ID.Value>0)
                {
                    LM_DEPARTMENT byId = unitOfWork.Departments.GetById(userX.LM_DEPARTMENT_ID.Value);
                    lmDepartmentList.Add(byId);
                }
            }
            elmID = string.IsNullOrEmpty(elmID) ? "DepartmentTree" + DateTime.Now.Millisecond : elmID.Trim();
            ViewBag.ElementID = elmID;
            ViewBag.RootDepartment = lmDepartmentList;
            ViewBag.Multiselect = multiselect;
            ViewBag.SelectedList = selectedlist;
            return PartialView("_DepartmentTree");
        }
    }
}