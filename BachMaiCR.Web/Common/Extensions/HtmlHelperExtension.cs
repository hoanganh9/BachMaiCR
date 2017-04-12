/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace BachMaiCR.Web.Common
{
    /// <summary>
    /// Đọc file template và ghi html xuống trang view gọi đến hàm này
    /// </summary>
    public static class HtmlHelperExtension
    {
        public static IHtmlString RenderTemplates(this HtmlHelper htmlHelper, string src)
        {
            var diskPath = HttpContext.Current.Server.MapPath(src);
            //var fi = new FileInfo(diskPath);
            //if (!fi.Exists)
            //{
            //    return htmlHelper.Raw(string.Empty);
            //}
            try
            {
                using (var fileStream = new FileStream(diskPath, FileMode.Open, FileAccess.Read))
                {
                    var data = new StringBuilder();
                    using (var sr = new StreamReader(fileStream))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            data.AppendLine(line);
                        }
                    }
                    return htmlHelper.Raw(data);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                return htmlHelper.Raw(string.Empty);
            }
        }
    }
}