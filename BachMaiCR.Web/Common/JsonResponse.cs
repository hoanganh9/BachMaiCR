/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

namespace BachMaiCR.Web.Common
{
    public static class JsonResponse
    {
        /// <summary>
        /// Trả về status 404
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json404(object data)
        {
            return new { status = 404, result = data };
        }

        /// <summary>
        /// trả về 200 với dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json200(object data)
        {
            return new { status = 200, result = data };
        }

     
        /// <summary>
        /// trả về 500 với dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json500(object data)
        {
            return new { status = 500, result = data };
        }

        /// <summary>
        /// Trả về 500 với message
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json500WithMessage(object data)
        {
            return new { status = 500, message = data };
        }

        /// <summary>
        /// Trả về lỗi 400
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json400(object data)
        {
            return new { status = 400, result = data };
        }

        /// <summary>
        /// Trả về lỗi 400
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Json400WithMessage(object data)
        {
            return new { status = 400, message = data };
        }
    }
}