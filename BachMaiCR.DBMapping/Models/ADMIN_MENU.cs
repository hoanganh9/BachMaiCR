namespace BachMaiCR.DBMapping.Models
{
    public class ADMIN_MENU
    {
        public int MENU_ID { get; set; }

        public int? MENU_PARENT_ID { get; set; }

        public string MENU_NAME { get; set; }

        public string MENU_IMAGE_CSS { get; set; }

        public string MENU_CODE { get; set; }

        public string DESCRIPTION { get; set; }

        public string ACTIONCODE { get; set; }

        public string MENU_CLASS { get; set; }

        public string MENU_URL { get; set; }

        public int? MENU_ORDER { get; set; }

        public int? MENU_LEVEL { get; set; }

        public bool? ISACTIVE { get; set; }
    }
}