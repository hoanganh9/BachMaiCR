namespace BachMaiCR.Web.Models
{
    public class UserMenuModel
    {
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string GroupCss { get; set; }
        public string ActionCss { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsMenu { get; set; }
        public bool Selected { get; set; }
        public string Roles { get; set; }
    }
}