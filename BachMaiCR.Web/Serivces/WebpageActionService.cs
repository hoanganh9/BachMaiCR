using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DataAccess;
using BachMaiCR.Web.Common;

namespace BachMaiCR.Web.Serivces
{
    public class WebpageActionService
    {
        private IUnitOfWork unitOfWork;
        public WebpageActionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Đưa toàn bộ chức nang của website vào cho root-admin
        /// </summary>
        /// <param name="admin"></param>
        public void AddOrUpdateWebpageActionToRootAdmin(ADMIN_USER admin)
        {
            if (admin == null)
            {
                return;
            }
            List<WEBPAGES_ACTIONS> actions = unitOfWork.Actions.GetAll().ToList();
            if (actions.Any())
            {
                foreach (var action in actions)
                {
                    var userInAction = new USERS_ACTIONS()
                        {
                            UPDATE_DATE = DateTime.Now,
                            CREATE_DATE = DateTime.Now,
                            ADMIN_USER_ID = admin.ADMIN_USER_ID,
                            WEBPAGES_ACTION_ID = action.WEBPAGES_ACTION_ID,
                            IS_ACTIVE = true,
                            ORDERS = 0
                        };
                    unitOfWork.UsersInActions.AddOrUpdate(userInAction);
                }
            }
        }

        /// <summary>
        /// Refresh lại toàn bộ chức năng của website
        /// </summary>
        public void RefreshAllWebpageAction()
        {
            //Lấy toàn bộ các action trong controller
            var holders = new List<ActionDescriptionHolder>();
            List<Type> types = MvcHelper.GetSubClasses<Controller>();
            foreach (var type in types)
            {
                var list = GetAttributeHelper.GetAll(type);
                if (list != null && list.Any())
                {
                    holders.AddRange(list);
                }
            }
            //So sánh vơi CSDL để thực hiện insert hoặc update
            if (holders.Any())
            {
                
                // unique code đã có trong csdl
                foreach (var holder in holders)
                {
                    var unique = holder.GenerateUniqueCode();
                    foreach (var code in holder.Code)
                    {
                        var webpageAction = unitOfWork.Actions.GetByCode(code);
                        if (webpageAction == null)
                        {
                            webpageAction = new WEBPAGES_ACTIONS()
                                {
                                    DESCRIPTION = holder.Description,
                                    MENU_NAME = holder.Description,
                                    CODE = code,
                                    CREATE_DATE = DateTime.Now,
                                    IS_ACTIVE = true,
                                    GROUP_ORDER = holder.GroupOrder,
                                    MENU_ORDER = holder.MenuOrder,
                                    GROUP_NAME = holder.GroupName,
                                    IS_MENU = holder.IsMenu,
                                    GROUP_CODE = holder.GroupCode
                                };
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(holder.Description))
                            {
                                webpageAction.DESCRIPTION = holder.Description;
                                webpageAction.MENU_NAME = holder.Description;
                            }

                            if (!string.IsNullOrWhiteSpace(holder.GroupCode))
                            {
                                webpageAction.GROUP_CODE = holder.GroupCode;

                                if (!string.IsNullOrWhiteSpace(holder.GroupName))
                                {
                                    webpageAction.GROUP_NAME = holder.GroupName;
                                }
                            }
                            webpageAction.IS_MENU = holder.IsMenu;
                        }
                        unitOfWork.Actions.AddOrUpdate(webpageAction);

                        var webpageFunction = unitOfWork.Functons.GetByUniqueCode(unique);
                        if (webpageFunction == null)
                        {
                            webpageFunction = new WEBPAGES_FUNCTIONS()
                                {
                                    UNIQUE_CODE = unique,
                                    ACTION_NAME = holder.ActionName,
                                    CONTROLLER = holder.Controller,
                                    ACTION_TYPE = holder.ActionType,
                                    CREATE_DATE = DateTime.Now
                                };
                            if (webpageAction.WEBPAGES_FUNCTIONS == null)
                            {
                                webpageAction.WEBPAGES_FUNCTIONS = new Collection<WEBPAGES_FUNCTIONS>();
                            }
                            webpageAction.WEBPAGES_FUNCTIONS.Add(webpageFunction);
                        }
                        else
                        {
                            if (webpageAction.WEBPAGES_FUNCTIONS == null)
                            {
                                webpageAction.WEBPAGES_FUNCTIONS = new Collection<WEBPAGES_FUNCTIONS>();
                            }
                            if (
                                !webpageAction.WEBPAGES_FUNCTIONS.Select(o => o.WEBPAGES_FUNCTIONS_ID)
                                              .Contains(webpageFunction.WEBPAGES_FUNCTIONS_ID))
                            {
                                webpageAction.WEBPAGES_FUNCTIONS.Add(webpageFunction);
                            }
                        }
                       
                    }
                }
            }
            var codeList = new List<string>();
            foreach (var list in holders.Select(o => o.Code).ToList())
            {
                codeList.AddRange(list);
            }
            List<WEBPAGES_ACTIONS> actions;
            if (codeList.Any())
            {
                actions = unitOfWork.Actions.GetNotInACodeList(codeList).ToList();
            }
            else
            {
                actions = unitOfWork.Actions.GetAll().ToList();
            }

            if (actions.Any())
            {
                foreach (var webpagesAction in actions)
                {
                    if (webpagesAction.WEBPAGES_ROLES != null)
                    {
                        var arList = webpagesAction.WEBPAGES_ROLES.ToList();
                        foreach (var ar in arList)
                        {
                            webpagesAction.WEBPAGES_ROLES.Remove(ar);
                        }
                    }
                    if (webpagesAction.WEBPAGES_FUNCTIONS != null)
                    {
                        var afList = webpagesAction.WEBPAGES_FUNCTIONS.ToList();
                        foreach (var af in afList)
                        {
                            webpagesAction.WEBPAGES_FUNCTIONS.Remove(af);
                        }
                    }
                    if (webpagesAction.USERS_ACTIONS != null)
                    {
                        var uaList = webpagesAction.USERS_ACTIONS.ToList();
                        foreach (var ua in uaList)
                        {
                            webpagesAction.USERS_ACTIONS.Remove(ua);
                        }
                    }
                    unitOfWork.Actions.Delete(webpagesAction);
                }
            }
        }
    }
}