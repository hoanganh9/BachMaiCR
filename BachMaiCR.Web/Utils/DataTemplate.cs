using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BachMaiCR.Web.Utils
{
    public class DataTemplate
    {
        public static List<SelectListItem> ListStatusSent
        {
            get
            {
                var lst = new List<SelectListItem>();
                lst.Add(new SelectListItem() { Text = "----- Tất cả -----", Value = "-1", Selected = true });
                lst.Add(new SelectListItem() { Text = "Đã gửi", Value = "1" });
                lst.Add(new SelectListItem() { Text = "Chưa gửi", Value = "0" });
                return lst;
            }
        }


        public static List<SelectListItem> ListGender
        {
            get
            {
                var lst = new List<SelectListItem>();
                lst.Add(new SelectListItem() { Text = "Nam", Value = "true" });
                lst.Add(new SelectListItem() { Text = "Nữ", Value = "false" });
                return lst;
            }
        }
    }
}