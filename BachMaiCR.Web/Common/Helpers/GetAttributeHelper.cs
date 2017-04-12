/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Common
{
    public static class GetAttributeHelper
    {
        /// <summary>
        /// Lấy giá trị attr cho class
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static string Get(Type classType)
        {
            var attrHepler = Attribute.GetCustomAttribute(classType, typeof(ActionDescriptionAttribute), false) as ActionDescriptionAttribute;
            if (null == attrHepler)
            {
                return null;
            }
            return attrHepler.ActionName;
        }

        /// <summary>
        /// Lấy giá trị attr của hàm
        /// </summary>
        /// <param name="method"> </param>
        /// <param name="controllerName"> </param>
        /// <returns></returns>
        public static ActionDescriptionHolder Get(MethodInfo method, string controllerName)
        {
            try
            {
                if (method == null)
                {
                    return null;
                }
                var attrHepler = Attribute.GetCustomAttribute(method, typeof(ActionDescriptionAttribute), false) as ActionDescriptionAttribute;
                if (attrHepler == null)
                {
                    return null;
                }
                // để chắc chắn, attribute có chứa code, giúp phân quyền đúng
                if (string.IsNullOrWhiteSpace(attrHepler.ActionCode))
                {
                    return null;
                }
                string actionType;

                var get = Attribute.GetCustomAttribute(method, typeof(HttpGetAttribute), false) as HttpGetAttribute;
                if (get == null)
                {
                    var post = Attribute.GetCustomAttribute(method, typeof(HttpPostAttribute), false) as HttpPostAttribute;
                    if (post == null)
                    {
                        var put = Attribute.GetCustomAttribute(method, typeof(HttpPutAttribute), false) as HttpPutAttribute;
                        if (put == null)
                        {
                            var delete = Attribute.GetCustomAttribute(method, typeof(HttpDeleteAttribute), false) as HttpDeleteAttribute;
                            if (delete == null)
                            {
                                actionType = "GET";
                            }
                            else
                            {
                                actionType = "DELETE";
                            }
                        }
                        else
                        {
                            actionType = "PUT";
                        }
                    }
                    else
                    {
                        actionType = "POST";
                    }

                }
                else
                {
                    actionType = "GET";
                }
                return new ActionDescriptionHolder(code: attrHepler.ActionCode, description: attrHepler.ActionName, isMenu: attrHepler.IsMenu, controller: controllerName, action: method.Name, actionType: actionType, groupCode:attrHepler.GroupCode,groupName:attrHepler.GroupName,groupOrder:attrHepler.GroupOrder,menuOrder:attrHepler.MenuOrder);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy ra toàn bộ các attr của hàm
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static List<ActionDescriptionHolder> GetAll(Type classType)
        {
            if (null == classType) return null;
            MethodInfo[] methods = classType.GetMethods();
            var items = new List<ActionDescriptionHolder>();
            foreach (MethodInfo method in methods)
            {
                ActionDescriptionHolder holder = Get(method, classType.Name);
                if (holder == null)
                {
                    continue;
                }
                items.Add(holder);
            }
            return items;
        }
    }
}