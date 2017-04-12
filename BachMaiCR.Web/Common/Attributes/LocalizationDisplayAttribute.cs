using System.ComponentModel;
using Resources;

namespace BachMaiCR.Web.Common.Attributes
{
    public class LocalizationDisplayAttribute : DisplayNameAttribute
    {
        private readonly string _resourceName;
        public LocalizationDisplayAttribute(string resourceName)
            : base()
        {
            this._resourceName = resourceName;
        }

        public override string DisplayName
        {
            get
            {
                var rm = Localization.ResourceManager;
                string name = rm.GetString(_resourceName);
                return name ?? this._resourceName;
            }
        }
    }
}