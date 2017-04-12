using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class WebPageActionModel
    {
        public int WEBPAGES_ACTION_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string MENUNAME { get; set; }
        public string CODE { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }

        public WebPageActionModel()
        {

        }

        public WebPageActionModel(WEBPAGES_ACTIONS action)
        {
            WEBPAGES_ACTION_ID = action.WEBPAGES_ACTION_ID;
            DESCRIPTION = action.DESCRIPTION;
            MENUNAME = action.MENU_NAME;
            CODE = action.CODE;
            CREATE_DATE = action.CREATE_DATE;
            IS_ACTIVE = action.IS_ACTIVE;
        }

    }

    public class WebPageActionEditModel
    {
        public int WEBPAGES_ACTION_ID { get; set; }

        [LocalizationDisplay("Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phải nhập mô tả")]
        public string DESCRIPTION { get; set; }

       
        public string MENUNAME { get; set; }

        public bool IS_ACTIVE { get; set; }

        public Nullable<bool> MANUAL_EDITED { get; set; }

        [LocalizationDisplay("Group order")]
        [Digit]
        public Nullable<int> GROUP_ORDER { get; set; }

        [LocalizationDisplay("Menu order")]
        [Digit]
        public Nullable<int> MENU_ORDER { get; set; }

        [LocalizationDisplay("Action class name")]
        public string ACTION_CLASSCSS { get; set; }

        [LocalizationDisplay("Group class name")]
        public string GROUP_CLASSCSS { get; set; }

        public WebPageActionEditModel()
        {

        }

        public WebPageActionEditModel(WEBPAGES_ACTIONS action)
        {
            WEBPAGES_ACTION_ID = action.WEBPAGES_ACTION_ID;
            DESCRIPTION = action.DESCRIPTION;
            MENUNAME = action.MENU_NAME;
            IS_ACTIVE = action.IS_ACTIVE ?? false;
            MANUAL_EDITED = action.MANUAL_EDITED;
            GROUP_ORDER = action.GROUP_ORDER;
            MENU_ORDER = action.MENU_ORDER;
            ACTION_CLASSCSS = action.ACTION_CLASSCSS;
            GROUP_CLASSCSS = action.GROUP_CLASSCSS;
        }

    }
}