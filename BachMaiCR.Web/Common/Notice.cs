/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System.Collections.Generic;
using System.Web;

namespace BachMaiCR.Web.Common
{
    /// <summary>
    /// Loại thông báo hỗ trợ, tùy loại mà có thể thiển thị css khác nhau
    /// Chú ý nếu thêm trạng thái thì nên thêm số mới có 1 chữ số
    /// </summary>
    public enum NoticeType
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 0,
        /// <summary>
        /// Cảnh báo
        /// </summary>
        Warning = 1,
        /// <summary>
        /// Thông tin, mang tính trung lập
        /// </summary>
        Info = 2,
        /// <summary>
        /// Lỗi
        /// </summary>
        Error = 3,
    }
    /// <summary>
    /// Class cung cấp hàm hiển thị thông báo cho người dùng kết quả một hành động nào đó mà họ yêu cầu
    /// Nội dung thông báo sẽ được hiển thị trong partical _notice
    /// </summary>
    public static class Notice
    {
        public const string NoticeSessionMessage = @"NoticeSessionMessage";
        /// <summary>
        /// Hiển thị thông báo cho người dùng
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        /// <param name="status">Loại thông báo</param>
        public static void Show(string message, NoticeType status)
        {
            Dictionary<int, string> listNotice = null;
            if (HttpContext.Current.Session[NoticeSessionMessage] != null)
            {
                listNotice = HttpContext.Current.Session[NoticeSessionMessage] as Dictionary<int, string>;
            }
            if (listNotice == null)
            {
                listNotice = new Dictionary<int, string>();
            }
            listNotice.Add(listNotice.Count, string.Format("{0}{1}", (int)status, message));
            HttpContext.Current.Session[NoticeSessionMessage] = listNotice;
        }

        /// <summary>
        /// Xóa các session chứa thông báo
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session[NoticeSessionMessage] = null;
        }

        public static int TotalNotice()
        {
            if (HttpContext.Current.Session[NoticeSessionMessage] == null)
            {
                return 0;
            }
            var listNotice = HttpContext.Current.Session[NoticeSessionMessage] as Dictionary<int, string>;
            if (listNotice == null)
            {
                return 0;
            }
            return listNotice.Count;
        }

        public static string GetNoticeMessage(int key = 0)
        {
            var listNotice = HttpContext.Current.Session[NoticeSessionMessage] as Dictionary<int, string>;
            if (listNotice == null)
            {
                return string.Empty;
            }
            if (!listNotice.ContainsKey(key))
            {
                return string.Empty;
            }
            string message = listNotice[key];
            listNotice.Remove(key);
            HttpContext.Current.Session[NoticeSessionMessage] = listNotice;
            return message;
        }
    }
}