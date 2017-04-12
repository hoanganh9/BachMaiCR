using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DataAccess;

namespace BachMaiCR.Web.Common.Helpers
{
    public static class GetMenuHelper
    {
        public static bool CheckParrent(int id)
        {
            UnitOfWork unit = new UnitOfWork();
            return unit.Departments.ExistChild(id);
        }

        public static List<ADMIN_MENU> GetMenuByParrent(int parrentId, List<string > listAction)
        {
            UnitOfWork unit = new UnitOfWork();
            //List<ADMIN_MENU> listAllMenu = unit.AdminMenu.GetAll_List();
            //List<ADMIN_MENU> test = listAllMenu.Where(o => o.MENU_PARENT_ID == parrentId).ToList();//.OrderBy("MENU_ORDER")
            List<ADMIN_MENU> listMenu = unit.AdminMenu.GetChildMenu(parrentId);
            List<ADMIN_MENU> returMenu = new List<ADMIN_MENU>();
            foreach (var adminMenu in listMenu)
            {
                if (adminMenu.MENU_URL != null)
                {
                    if (adminMenu.MENU_URL.Trim().Equals("#"))
                    {
                        returMenu.Add(adminMenu);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(adminMenu.ACTIONCODE))
                        {
                            var menuAction = new List<string>(adminMenu.ACTIONCODE.Split(','));
                            var checkExit = menuAction.Where(ma => listAction.Any(code => code == ma));
                            if (checkExit.Any())
                            {
                                returMenu.Add(adminMenu);
                            }
                        }
                    }
                }
            }
            return returMenu;
        }

        public static List<ADMIN_MENU> GetAllMenu(List<string> listAction)
        {
            UnitOfWork unit = new UnitOfWork();
            List<ADMIN_MENU> listMenu = unit.AdminMenu.GetAll().ToList();
            List<ADMIN_MENU> returMenu = new List<ADMIN_MENU>();
            foreach (var adminMenu in listMenu)
            {
                if (adminMenu.MENU_URL != null)
                {
                    if (adminMenu.MENU_URL.Trim().Equals("#"))
                    {
                        returMenu.Add(adminMenu);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(adminMenu.ACTIONCODE))
                        {
                            var menuAction = new List<string>(adminMenu.ACTIONCODE.Split(','));
                            var checkExit = menuAction.Where(ma => listAction.Any(code => code == ma));
                            if (checkExit.Any())
                            {
                                returMenu.Add(adminMenu);
                            }
                        }
                    }
                }
            }
            return returMenu;
        }

        public static List<string> GetCodeActionByUser(string userName)
        {
            UnitOfWork unit = new UnitOfWork();
            ADMIN_USER currentUser = unit.Users.GetByUserName(userName);
            bool isAdmin = currentUser.IS_ADMIN == null ? false : (bool)currentUser.IS_ADMIN;
            if (isAdmin)
            {
                return new List<string>(unit.Actions.GetAllActiveActions().Select(o=>o.CODE));
            }
            return new List<string>(unit.Users.GetActionCodesByUserName(userName));
        }

        public static string GetDepartmentName(int departmentId)
        {
            UnitOfWork unit = new UnitOfWork();
            return unit.Departments.GetById(departmentId).DEPARTMENT_NAME;
        }
    }
}