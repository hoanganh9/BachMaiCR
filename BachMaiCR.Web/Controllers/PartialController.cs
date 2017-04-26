using BachMaiCR.DataAccess;
using BachMaiCR.DataAccess.Repository;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities.Cache;
using BachMaiCR.Web.Common.Attributes;
using BachMaiCR.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                ADMIN_USER byUserName = unitOfWork.Users.GetByUserName(User.Identity.Name);
                IRoleRepository roles = unitOfWork.Roles;
                WEBPAGES_ROLES byId = roles.GetById(roleId.GetValueOrDefault());
                List<int> selectedActionIds = byId == null || byId.WEBPAGES_ACTIONS == null ? new List<int>() : byId.WEBPAGES_ACTIONS.Select((o => o.WEBPAGES_ACTION_ID)).ToList();
                IEnumerable<WEBPAGES_ACTIONS> source = unitOfWork.Actions.GetActiveActionsByUser(byUserName);
                //is Admin
                if (byUserName.IS_ADMIN.GetValueOrDefault())
                    source = unitOfWork.Actions.GetAllActiveActions();
                List<UserMenuModel> list = source.Select((o =>
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
               })).ToList();
                ViewBag.UserMenuModel = list;
                ViewBag.RoleId = roleId;
                ViewBag.RoleName = byId.ROLE_NAME;
            }
            return PartialView("_WebpageActionTreeViewPartial");
        }

        [CustomAuthorize]
        public ActionResult UserMenu()
        {
            List<WEBPAGES_ACTIONS> list = unitOfWork.Actions.GetAll().Distinct<WEBPAGES_ACTIONS>().ToList();
            List<UserMenuModel> userMenuModelList = new List<UserMenuModel>();
            if (list.Any())
            {
                foreach (WEBPAGES_ACTIONS webpagesActions in list)
                {
                    WEBPAGES_FUNCTIONS webpagesFunctions = webpagesActions.WEBPAGES_FUNCTIONS.FirstOrDefault<WEBPAGES_FUNCTIONS>((o => o.ACTION_TYPE.ToUpper() == "GET"));
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
            return PartialView("_MenuViewPartial");
        }
    }
}