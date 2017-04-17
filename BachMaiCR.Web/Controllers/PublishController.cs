// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.PublishController
// Assembly: BachMaiCR.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BF1F4BA2-20C9-48EC-A3D9-DB01621819C3
// Assembly location: D:\Work\Freelancer\BachMai\BachMaiCR\DLL\BachMaiCR.Web.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Cache;

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
      return this.View();
    }

    [HttpGet]
    public PartialViewResult DepartmentTree(string elmID, int? deptRootID = 0, bool multiselect = false, string selectedlist = null)
    {
      ADMIN_USER userX = this.UserX;
      List<LM_DEPARTMENT> lmDepartmentList = new List<LM_DEPARTMENT>();
      if (userX != null)
      {
        if (deptRootID.HasValue && deptRootID.Value > 0)
        {
          LM_DEPARTMENT byId = this.unitOfWork.Departments.GetById(deptRootID.Value);
          lmDepartmentList.Add(byId);
        }
        else if (userX.USERNAME == "admin")
        {
          lmDepartmentList = this.unitOfWork.Departments.GetChildDepartment(0).ToList<LM_DEPARTMENT>();
        }
        else
        {
          int? lmDepartmentId;
          int num;
          if (userX.LM_DEPARTMENT_ID.HasValue)
          {
            lmDepartmentId = userX.LM_DEPARTMENT_ID;
            num = (lmDepartmentId.GetValueOrDefault() <= 0 ? 0 : (lmDepartmentId.HasValue ? 1 : 0)) == 0 ? 1 : 0;
          }
          else
            num = 1;
          if (num == 0)
          {
            IDepartmentRepository departments = this.unitOfWork.Departments;
            lmDepartmentId = userX.LM_DEPARTMENT_ID;
            int id = lmDepartmentId.Value;
            LM_DEPARTMENT byId = departments.GetById(id);
            lmDepartmentList.Add(byId);
          }
        }
      }
      elmID = string.IsNullOrEmpty(elmID) ? "DepartmentTree" + DateTime.Now.Millisecond.ToString() : elmID.Trim();
ViewBag.ElementID = elmID;
ViewBag.RootDepartment = lmDepartmentList;
ViewBag.Multiselect = multiselect;
ViewBag.SelectedList = selectedlist;
      return this.PartialView("_DepartmentTree");
    }
  }
}
