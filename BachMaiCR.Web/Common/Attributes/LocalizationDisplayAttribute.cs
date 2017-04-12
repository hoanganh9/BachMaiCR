/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

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