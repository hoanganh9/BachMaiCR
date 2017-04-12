using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DataAccess;
using BachMaiCR.Utilities;
using BachMaiCR.Utilities.Enums;

namespace BachMaiCR.Web.Common
{
    public static class DocVFields
    {
        public static SelectList GetDatatypes(object selectType)
        {
            List<KeyTextItem> listEnum = EnumHelper<BachMaiCR.Utilities.Enums.DataType>.ConvertToKeyValueList();

            List<SelectListItem> listDataType = new List<SelectListItem>();
            int i = 1;
            foreach (KeyTextItem keyTextItem in listEnum)
            {
                SelectListItem item = new SelectListItem() { Selected = false, Text = keyTextItem.Text, Value = i.ToString() };
                listDataType.Add(item);
                i++;
            }
            if (selectType != null)
            {
                foreach (var selectListItem in listDataType)
                {
                    if (selectListItem.Value == selectType.ToString())
                    {
                        selectListItem.Selected = true;
                    }
                    else
                    {
                        selectListItem.Selected = false;
                    }
                }
            }
            return new SelectList(listDataType, "Value", "Text");
        }
    }
}