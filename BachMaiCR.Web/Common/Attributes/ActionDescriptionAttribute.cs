/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace BachMaiCR.Web.Common
{
    public class ActionDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Mã chức năng
        /// </summary>
        public string ActionCode { get; set; }

        /// <summary>
        /// Tên chức năng
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        //public string MenuName { get; set; }

        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int GroupOrder { get; set; }
        public int MenuOrder { get; set; }
        public bool IsMenu { get; set; }
        public string RequireCode { get; set; }
        public ActionDescriptionAttribute() { }
    }

    public class ActionDescriptionHolder
    {
        private readonly List<string> _code;
        public List<string> Code { get { return _code; } }
        private readonly string _description;
        public string Description { get { return _description; } }
        private readonly string _actionName;
        public string ActionName { get { return _actionName ?? string.Empty; } }
        private readonly bool _isMenu;
        public bool IsMenu { get { return _isMenu; } }
        private readonly string _actionType;
        public string ActionType { get { return _actionType ?? string.Empty; } }
        private readonly string _controller;
        public string Controller { get { return _controller ?? string.Empty; } }

        private readonly string _groupCode;
        public string GroupCode
        {
            get { return _groupCode ?? string.Empty; }
        }

        private readonly string _groupName;
        public string GroupName
        {
            get { return _groupName ?? string.Empty; }
        }

        private readonly int? _groupOrder;
        private readonly int? _menuOrder;

        public int? GroupOrder
        {
            get { return _groupOrder ?? 0; }
        }

        public int? MenuOrder
        {
            get { return _menuOrder ?? 0; }
        }


        /// <summary>
        /// ctor
        /// Khởi tạo ActionDescription
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <param name="isMenu"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="actionType"></param>
        /// <param name="groupCode"></param>
        /// <param name="groupName"></param>
        /// <param name="groupOrder"></param>
        /// <param name="menuOrder"></param>
        public ActionDescriptionHolder(string code, string description, bool isMenu, string controller, string action, string actionType, string groupCode, string groupName, int? groupOrder, int? menuOrder)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                this._code = code.Split(',').ToList();
            }
            else
            {
                this._code = new List<string>();
            }
            this._description = description;
            this._actionName = action;
            this._actionType = actionType;
            this._groupCode = groupCode;
            this._groupName = groupName;
            this._isMenu = isMenu;
            this._controller = controller;
            this._groupOrder = groupOrder;
            this._menuOrder = menuOrder;
        }

        /// <summary>
        /// Khởi tạo có đối số, bỏ qua khởi tạo ko đối số
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <param name="isMenu"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="actionType">post, get, or put or delete</param>
        /// <param name="groupCode"></param>
        /// <param name="groupName"></param>
        public ActionDescriptionHolder(string code, string description, bool isMenu, string controller, string action, string actionType, string groupCode, string groupName)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                this._code = code.Split(',').ToList();
            }
            else
            {
                this._code = new List<string>();
            }
            this._description = description;
            this._actionName = action;
            this._actionType = actionType;
            this._groupCode = groupCode;
            this._groupName = groupName;
            this._isMenu = isMenu;
            this._controller = controller;
            this._groupOrder = 0;
            this._menuOrder = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <param name="isMenu"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="actionType"></param>
        public ActionDescriptionHolder(string code, string description, bool isMenu, string controller, string action, string actionType)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                this._code = code.Split(',').ToList();
            }
            else
            {
                this._code = new List<string>();
            }
            this._description = description;
            this._actionName = action;
            this._actionType = actionType;
            this._groupCode = @"";
            this._groupName = @"";
            this._isMenu = isMenu;
            this._controller = controller;
            this._groupOrder = 0;
            this._menuOrder = 0;
        }

        /// <summary>
        /// Check xem  hàm này có code không, nếu có code thì mới phân quyền
        /// </summary>
        /// <returns></returns>
        public bool HasCode()
        {
            return _code != null && _code.Count > 0;
        }

        /// <summary>
        /// Tạo ra một mã không trùng để phân biệt khi có các controller hay action trùng tên nhau
        /// ở đây chủ yếu lấy ActionType để phân biệt
        /// </summary>
        /// <returns>sha1withhex(controller.tolower + action.tolowser + actiontype.tolower)</returns>
        public string GenerateUniqueCode()
        {
            if (string.IsNullOrWhiteSpace(_controller) || string.IsNullOrWhiteSpace(ActionName) || string.IsNullOrWhiteSpace(ActionType))
            {
                return null;
            }
            return string.Format("{0}{1}{2}", _controller.ToLower(), _actionName.ToLower(), _actionType.ToLower());
        }
    }
}