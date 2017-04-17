// Decompiled with JetBrains decompiler
// Type: BachMaiCR.Web.Controllers.PartialController
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
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;

namespace BachMaiCR.Web.Controllers
{
  public class PartialController : BaseController
  {
    public PartialController(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
      : base(unitOfWork, cacheProvider)
    {
      this.unitOfWork = unitOfWork;
      this.cacheProvider = cacheProvider;
    }

    [CustomAuthorize]
    public ActionResult WebpageActionTreeViewWithRole(int? roleId)
    {
      if (!string.IsNullOrEmpty(this.User.Identity.Name))
      {
        ADMIN_USER byUserName = this.unitOfWork.Users.GetByUserName(this.User.Identity.Name);
        IRoleRepository roles = this.unitOfWork.Roles;
        int? nullable = roleId;
        int id = nullable ?? 0;
        WEBPAGES_ROLES byId = roles.GetById(id);
        List<int> selectedActionIds = byId == null || byId.WEBPAGES_ACTIONS == null ? new List<int>() : byId.WEBPAGES_ACTIONS.Select<WEBPAGES_ACTIONS, int>((Func<WEBPAGES_ACTIONS, int>) (o => o.WEBPAGES_ACTION_ID)).ToList<int>();
        IEnumerable<WEBPAGES_ACTIONS> source = this.unitOfWork.Actions.GetActiveActionsByUser(byUserName);
        bool? isAdmin = byUserName.IS_ADMIN;
        int num1;
        if (isAdmin.HasValue)
        {
          isAdmin = byUserName.IS_ADMIN;
          num1 = isAdmin.Value ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
          source = this.unitOfWork.Actions.GetAllActiveActions();
        List<UserMenuModel> list = source.Select<WEBPAGES_ACTIONS, UserMenuModel>((Func<WEBPAGES_ACTIONS, UserMenuModel>) (o =>
        {
          UserMenuModel userMenuModel1 = new UserMenuModel();
          userMenuModel1.Name = o.MENU_NAME;
          userMenuModel1.GroupCode = o.GROUP_CODE;
          userMenuModel1.GroupName = o.GROUP_NAME;
          userMenuModel1.Description = o.DESCRIPTION;
          userMenuModel1.Id = o.WEBPAGES_ACTION_ID;
          UserMenuModel userMenuModel2 = userMenuModel1;
          bool? isMenu = o.IS_MENU;
          int num = isMenu.HasValue ? (isMenu.GetValueOrDefault() ? 1 : 0) : 0;
          userMenuModel2.IsMenu = num != 0;
          userMenuModel1.Selected = selectedActionIds.Contains(o.WEBPAGES_ACTION_ID);
          return userMenuModel1;
        })).ToList<UserMenuModel>();
ViewBag.UserMenuModel = list;
                ViewBag.RoleId = roleId;
            ViewBag.RoleName = byId.ROLE_NAME;
      }
      return this.PartialView("_WebpageActionTreeViewPartial");
    }

    [CustomAuthorize]
    public ActionResult UserMenu()
    {
      List<WEBPAGES_ACTIONS> list = this.unitOfWork.Actions.GetAll().Distinct<WEBPAGES_ACTIONS>().ToList<WEBPAGES_ACTIONS>();
      List<UserMenuModel> userMenuModelList = new List<UserMenuModel>();
      if (list.Any<WEBPAGES_ACTIONS>())
      {
        foreach (WEBPAGES_ACTIONS webpagesActions in list)
        {
          WEBPAGES_FUNCTIONS webpagesFunctions = webpagesActions.WEBPAGES_FUNCTIONS.FirstOrDefault<WEBPAGES_FUNCTIONS>((Func<WEBPAGES_FUNCTIONS, bool>) (o => o.ACTION_TYPE.ToUpper() == "GET"));
          if (webpagesFunctions != null && !string.IsNullOrWhiteSpace(webpagesActions.MENU_NAME))
          {
            bool? isMenu = webpagesActions.IS_MENU;
            if ((isMenu.HasValue ? (isMenu.GetValueOrDefault() ? 1 : 0) : 0) != 0)
              userMenuModelList.Add(new UserMenuModel()
              {
                Name = webpagesActions.MENU_NAME,
                ActionName = webpagesFunctions.ACTION_NAME,
                Controller = webpagesFunctions.CONTROLLER,
                GroupCode = webpagesActions.GROUP_CODE,
                GroupName = webpagesActions.GROUP_NAME,
                Description = webpagesActions.DESCRIPTION,
                Id = webpagesActions.WEBPAGES_ACTION_ID,
                GroupCss = webpagesActions.GROUP_CLASSCSS,
                ActionCss = webpagesActions.ACTION_CLASSCSS,
                IsMenu = true
              });
          }
        }
      }
ViewBag.CurrentUserMenu = userMenuModelList;
      return this.PartialView("_MenuViewPartial");
    }
  }
}
