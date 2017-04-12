using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BachMaiCR.DBMapping.ModelsExt;
using System.Net.Mail;
using System.Net;
using Resources;
using System.Configuration;
using System.Data;
using System.IO;
using BachMaiCR.DBMapping.ModelsExt;
namespace BachMaiCR.Web.Utils
{
    public static class Utils
    {
        //Biến lưu giá trị URL đầu tiên
        public static string getUrlDefault = "";
        /// <summary>
        /// Chuyển đổi ngày tháng từ string mm/dd/yyyy sang date
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime? ConvetDateVNToDate(string strDate)
        {
            if (strDate == null || strDate == "")
            {
                return null;
            }
            else
            {
                try
                {
                    strDate = strDate.Trim();
                    if (String.IsNullOrEmpty(strDate)) return DateTime.Today;
                    string[] strs = strDate.Split("/".ToCharArray());

                    int day = Convert.ToInt32(strs[0]);
                    int month = Convert.ToInt32(strs[1]);
                    int year = Convert.ToInt32(strs[2]);
                    return new DateTime(year, month, day);
                }
                catch
                {
                    return null;
                }

            }
        }
        public static string ToString(object obj)
        {
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return "";
            }
        }
        public static DateTime ToDateTime(object obj, string strFormat)
        {
            try
            {
                string dtString = ToString(obj);
                string[] arr = new string[2];
                DateTime dt = DateTime.MinValue;

                if (dtString.IndexOf("/") > -1)
                {
                    arr = dtString.Split('/');
                }
                else if (dtString.IndexOf("-") > -1)
                {
                    arr = dtString.Split('-');
                }
                else
                {
                    return dt;
                }

                switch (strFormat)
                {
                    case "dd/mm/yyyy":
                    case "dd-mm-yyyy":
                        dt = ToDateTime(arr[1] + "/" + arr[0] + "/" + arr[2]);
                        break;
                    case "yyyy/mm/dd":
                    case "yyyy-mm-dd":
                        dt = ToDateTime(arr[1] + "/" + arr[2] + "/" + arr[0]);
                        break;
                    case "yyyy/dd/mm":
                    case "yyyy-dd-mm":
                        dt = ToDateTime(arr[2] + "/" + arr[1] + "/" + arr[0]);
                        break;
                    default:
                        dt = ToDateTime(obj);
                        break;
                }
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return DateTime.Today;
            }
        }
        /// <summary>
        /// Chuyển đổi ngày tháng từ string dd/mm/yyyy sang date
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime? ConvetDateVNToDates(string strDate)
        {
            if (strDate == null || strDate == "")
            {
                return null;
            }
            else
            {
                try
                {
                    strDate = strDate.Trim();
                    if (String.IsNullOrEmpty(strDate)) return DateTime.Today;
                    string[] strs = strDate.Split("/".ToCharArray());

                    int day = Convert.ToInt32(strs[0]);
                    int month = Convert.ToInt32(strs[1]);
                    int year = Convert.ToInt32(strs[2]);
                    return new DateTime(year, month, day);
                }
                catch
                {
                    return null;
                }

            }
        }
        public static DateTime? sysDate()
        {
            return DateTime.Now;
        }
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// Thực hiện cắt bỏ các dấu cách thừa ở 2 đầu đối với các trường kiểu string
        /// </summary>
        /// <param name="obj"></param>
        public static void TrimObject(object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] ps = t.GetProperties();

            foreach (PropertyInfo p in ps)
            {
                if (p.PropertyType == typeof(string) || p.PropertyType == typeof(String))
                {
                    string str = (string)p.GetValue(obj, null);
                    str = str == null ? null : str.Trim();
                    p.SetValue(obj, str, null);
                }
            }
        }
        public static string htmlEndCode(string val)
        {
            return "";
        }
        public static string htmlEndCode(object obj)
        {
            string trueEncode = "";
            if (obj == null)
                return trueEncode;
            string encode = HttpUtility.HtmlEncode(obj.ToString());
            if (encode.Contains("&#192;"))
                encode = encode.Replace("&#192;", "À");
            if (encode.Contains("&#193;"))
                encode = encode.Replace("&#193;", "Á");
            if (encode.Contains("&#194;"))
                encode = encode.Replace("&#194;", "Â");
            if (encode.Contains("&#195;"))
                encode = encode.Replace("&#195;", "Ã");
            if (encode.Contains("&#200;"))
                encode = encode.Replace("&#200;", "È");
            if (encode.Contains("&#201;"))
                encode = encode.Replace("&#201;", "É");
            if (encode.Contains("&#202;"))
                encode = encode.Replace("&#202;", "Ê");
            if (encode.Contains("&#204;"))
                encode = encode.Replace("&#204;", "Ì");
            if (encode.Contains("&#205;"))
                encode = encode.Replace("&#205;", "Í");
            if (encode.Contains("&#208;"))
                encode = encode.Replace("&#208;", "Đ");
            if (encode.Contains("&#210;"))
                encode = encode.Replace("&#210;", "Ò");
            if (encode.Contains("&#211;"))
                encode = encode.Replace("&#211;", "Ó");
            if (encode.Contains("&#212;"))
                encode = encode.Replace("&#212;", "Ô");
            if (encode.Contains("&#213;"))
                encode = encode.Replace("&#213;", "Õ");
            if (encode.Contains("&#217;"))
                encode = encode.Replace("&#217;", "Ù");
            if (encode.Contains("&#218;"))
                encode = encode.Replace("&#218;", "Ú");
            if (encode.Contains("&#221;"))
                encode = encode.Replace("&#221;", "Ý");
            if (encode.Contains("&#224;"))
                encode = encode.Replace("&#224;", "à");
            if (encode.Contains("&#225;"))
                encode = encode.Replace("&#225;", "á");
            if (encode.Contains("&#226;"))
                encode = encode.Replace("&#226;", "â");
            if (encode.Contains("&#227;"))
                encode = encode.Replace("&#227;", "ã");
            if (encode.Contains("&amp;#7841;"))
                encode = encode.Replace("&amp;#7841;", "ạ");
            if (encode.Contains("&#232;"))
                encode = encode.Replace("&#232;", "è");
            if (encode.Contains("&#233;"))
                encode = encode.Replace("&#233;", "é");
            if (encode.Contains("&#234;"))
                encode = encode.Replace("&#234;", "ê");
            if (encode.Contains("&#236;"))
                encode = encode.Replace("&#236;", "ì");
            if (encode.Contains("&#237;"))
                encode = encode.Replace("&#237;", "í");
            if (encode.Contains("&#242;"))
                encode = encode.Replace("&#242;", "ò");
            if (encode.Contains("&#243;"))
                encode = encode.Replace("&#243;", "ó");
            if (encode.Contains("&#244;"))
                encode = encode.Replace("&#244;", "ô");
            if (encode.Contains("&#245;"))
                encode = encode.Replace("&#245;", "õ");
            if (encode.Contains("&#249;"))
                encode = encode.Replace("&#249;", "ù");
            if (encode.Contains("&#250;"))
                encode = encode.Replace("&#250;", "ú");
            if (encode.Contains("&#253;"))
                encode = encode.Replace("&#253;", "ý");

            trueEncode = encode;
            return trueEncode;

        }
        /// <summary>
        ///Author: PhuongLT
        /// Kiem tra ngày tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool checkDatetime(string date)
        {
            Regex reg = new Regex(@"^(((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}|\d))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}|\d))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}|\d))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00|[048])))$");
            if (reg.IsMatch(date) == false)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        /// <summary>
        /// Chuyển đổi tiếng Việt có dấu thành không dấu
        /// PhuongLt15: 20/05/2014
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertVN(string value)
        {
            //---------------------------------a^
            value = value.Replace("ấ", "a");
            value = value.Replace("ầ", "a");
            value = value.Replace("ẩ", "a");
            value = value.Replace("ẫ", "a");
            value = value.Replace("ậ", "a");
            //---------------------------------A^ 
            value = value.Replace("Ấ", "A");
            value = value.Replace("Ầ", "A");
            value = value.Replace("Ẩ", "A");
            value = value.Replace("Ẫ", "A");
            value = value.Replace("Ậ", "A");
            //---------------------------------a( 
            value = value.Replace("ắ", "a");
            value = value.Replace("ằ", "a");
            value = value.Replace("ẳ", "a");
            value = value.Replace("ẵ", "a");
            value = value.Replace("ặ", "a");
            //---------------------------------A(
            value = value.Replace("Ắ", "A");
            value = value.Replace("Ằ", "A");
            value = value.Replace("Ẳ", "A");
            value = value.Replace("Ẵ", "A");
            value = value.Replace("Ặ", "A");

            //---------------------------------a
            value = value.Replace("á", "a");
            value = value.Replace("à", "a");
            value = value.Replace("ả", "a");
            value = value.Replace("ã", "a");
            value = value.Replace("ạ", "a");
            value = value.Replace("â", "a");
            value = value.Replace("ă", "a");

            //---------------------------------A
            value = value.Replace("Á", "A");
            value = value.Replace("À", "A");
            value = value.Replace("Ả", "A");
            value = value.Replace("Ã", "A");
            value = value.Replace("Ạ", "A");
            value = value.Replace("Â", "A");
            value = value.Replace("Ă", "A");
            //---------------------------------A
            value = value.Replace("ế", "e");
            value = value.Replace("ề", "e");
            value = value.Replace("ể", "e");
            value = value.Replace("ễ", "e");
            value = value.Replace("ệ", "e");
            //---------------------------------E^
            value = value.Replace("Ế", "E");
            value = value.Replace("Ề", "E");
            value = value.Replace("Ể", "E");
            value = value.Replace("Ễ", "E");
            value = value.Replace("Ệ", "E");
            //---------------------------------e
            value = value.Replace("é", "e");
            value = value.Replace("è", "e");
            value = value.Replace("ẻ", "e");
            value = value.Replace("ẽ", "e");
            value = value.Replace("ẹ", "e");
            value = value.Replace("ê", "e");

            //---------------------------------E
            value = value.Replace("É", "E");
            value = value.Replace("È", "E");
            value = value.Replace("Ẻ", "E");
            value = value.Replace("Ẽ", "E");
            value = value.Replace("Ẹ", "E");
            value = value.Replace("Ê", "E");
            //---------------------------------i
            value = value.Replace("í", "i");
            value = value.Replace("ì", "i");
            value = value.Replace("ỉ", "i");
            value = value.Replace("ĩ", "i");
            value = value.Replace("ị", "i");

            //---------------------------------I
            value = value.Replace("Í", "I");
            value = value.Replace("Ì", "I");
            value = value.Replace("Ỉ", "I");
            value = value.Replace("Ĩ", "I");
            value = value.Replace("Ị", "I");
            //---------------------------------o^ 
            value = value.Replace("ố", "o");
            value = value.Replace("ồ", "o");
            value = value.Replace("ổ", "o");
            value = value.Replace("ỗ", "o");
            value = value.Replace("ộ", "o");
            //---------------------------------O^ 
            value = value.Replace("Ố", "O");
            value = value.Replace("Ồ", "O");
            value = value.Replace("Ổ", "O");
            value = value.Replace("Ô", "O");
            value = value.Replace("Ộ", "O");
            //---------------------------------o*
            value = value.Replace("ớ", "o");
            value = value.Replace("ờ", "o");
            value = value.Replace("ở", "o");
            value = value.Replace("ỡ", "o");
            value = value.Replace("ợ", "o");
            //---------------------------------O*
            value = value.Replace("Ớ", "O");
            value = value.Replace("Ờ", "O");
            value = value.Replace("Ở", "O");
            value = value.Replace("Ỡ", "O");
            value = value.Replace("Ợ", "O");
            //---------------------------------u*
            value = value.Replace("ứ", "u");
            value = value.Replace("ử", "u");
            value = value.Replace("ừ", "u");
            value = value.Replace("ữ", "u");
            value = value.Replace("ự", "u");
            //---------------------------------U*
            value = value.Replace("Ứ", "U");
            value = value.Replace("Ừ", "U");
            value = value.Replace("Ử", "U");
            value = value.Replace("Ữ", "U");
            value = value.Replace("Ự", "U");

            //---------------------------------y
            value = value.Replace("ý", "y");
            value = value.Replace("ỳ", "y");
            value = value.Replace("ỷ", "y");
            value = value.Replace("ỹ", "y");
            value = value.Replace("ỵ", "y");
            //---------------------------------y
            value = value.Replace("Ý", "Y");
            value = value.Replace("Ỳ", "Y");
            value = value.Replace("Ỷ", "Y");
            value = value.Replace("Ỹ", "Y");
            value = value.Replace("Ỵ", "Y");

            //---------------------------------DD 
            value = value.Replace("Đ", "D");
            value = value.Replace("Đ", "D");
            value = value.Replace("đ", "d");

            //---------------------------------0
            value = value.Replace("ó", "o");
            value = value.Replace("ò", "o");
            value = value.Replace("ỏ", "o");
            value = value.Replace("õ", "o");
            value = value.Replace("ọ", "o");

            value = value.Replace("ô", "o");
            value = value.Replace("ơ", "o");
            //---------------------------------0
            value = value.Replace("Ó", "O");
            value = value.Replace("Ò", "O");
            value = value.Replace("Ỏ", "O");
            value = value.Replace("Õ", "O");
            value = value.Replace("Ọ", "O");
            value = value.Replace("Ô", "O");
            value = value.Replace("Ơ", "O");

            //---------------------------------u
            value = value.Replace("ú", "u");
            value = value.Replace("ù", "u");
            value = value.Replace("ủ", "u");
            value = value.Replace("ũ", "u");
            value = value.Replace("ụ", "u");
            value = value.Replace("ư", "u");

            //---------------------------------U
            value = value.Replace("Ú", "U");
            value = value.Replace("Ù", "U");
            value = value.Replace("Ủ", "U");
            value = value.Replace("Ũ", "U");
            value = value.Replace("Ụ", "U");
            value = value.Replace("Ư", "U");
            return value;
        }
        /// <summary>
        /// Cắt bỏ các ký tự HTML
        /// Phuonglt15: 20/05/2014
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHTML(string source)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&bull;", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lsaquo;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&rsaquo;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&trade;", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&frasl;", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lt;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&gt;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&copy;", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&reg;", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result;
            }
            catch
            {

                return source;
            }
        }
         /// <summary>
        ///Author: PhuongLT
        /// Hàm gửi mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool SendMsg(string subject = "N/A", string msg = "N/A", string urlfile = "", int doctorId = 0)
        {
            string _mailServer = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ftpServer"]);
            int _mailPort = Int32.Parse(ConfigurationManager.AppSettings["sslPort"].ToString());
            string _user = Convert.ToString(ConfigurationManager.AppSettings["ftpUser"]);
            string _pass = Convert.ToString(ConfigurationManager.AppSettings["ftpPassword"]);
            MailMessage mailMessage = new MailMessage();
            SmtpClient mailClient = new SmtpClient(_mailServer, _mailPort);
            mailClient.Timeout = 15000;
            mailClient.Credentials = new NetworkCredential(_user, _pass);
            mailClient.EnableSsl = true;        //mailClient.UseDefaultCredentials = true; // no work   
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(_user);
            mailMessage.Subject = subject;
            mailMessage.Body = msg;
            mailMessage.Priority = MailPriority.High;
            string fileName = urlfile;
            if (!String.IsNullOrEmpty(fileName) && fileName.Length > 0)
            {
                string[] vArray = fileName.Split(';');
                if (vArray.Length > 0)
                {
                    for (int i = 0; i < vArray.Length; i++)
                    {
                        string file = vArray[i].ToString();
                        string pathfile = System.Web.HttpContext.Current.Server.MapPath(file);
                        mailMessage.Attachments.Add(new Attachment(pathfile));
                    }
                }

            }
            //-----------------------//
            string mailto = "";// từ DOCTORS_ID lấy ra mail của bác sĩ
            //----------------------//
            mailMessage.To.Add(mailto);
            try
            {
                mailClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                mailMessage.Dispose();
                mailClient.Dispose();
            }
        }
        /// <summary>
        ///Author: PhuongLT
        /// Đếm số ký tự
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static int Count(string value, char chr)
        {

            int result = 0;
            // char c = ';';
            foreach (char c in value)
            {
                if (c == chr)
                {
                    result++;
                }

            }

            return result;
        }
        /// <summary>
        ///Author: PhuongLT
        /// Bôi màu đoạn văn tìm kiếm trên lưới
        /// </summary>
        /// <param name="strChuoi"></param>
        /// <param name="strTuKhoa"></param>
        /// <returns></returns>
        public static string DrawSearch(string strChuoi, string strTuKhoa)
        {
            string str = strTuKhoa.Trim();
            string result = "";
            if (Count(str, ' ') != 0)
            {
                string[] spt = new string[Count(str, ' ')];
                spt = str.Split(' ');
                for (int i = 0; i <= Count(str, ' '); i++)
                {
                    string sp = spt[i].Trim();
                    if (sp != "" && sp != null)
                    {
                        string gd = spt[i].Trim();
                        string gt = gd[0].ToString();
                        result += " " + gd.Remove(0, 1).Insert(0, gt.ToUpper());
                    }
                }

            }
            else
            {
                string gt = str[0].ToString();
                result = str.Remove(0, 1).Insert(0, gt.ToUpper());
            }
            return strChuoi.Replace(result.Trim(), "<span class='searchLight'>" + result.Trim() + "</span>");


        }
        /// <summary>
        /// Chuyển đổi ngày tháng sang thứ
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDayName(DateTime date)
        {
            var day = date.DayOfWeek;
            var result = string.Empty;
            switch (day)
            {
                case DayOfWeek.Friday:
                    result = "Thứ 6";
                    break;
                case DayOfWeek.Monday:
                    result = "Thứ 2";
                    break;
                case DayOfWeek.Saturday:
                    result = "Thứ 7";
                    break;
                case DayOfWeek.Sunday:
                    result = "Chủ nhật";
                    break;
                case DayOfWeek.Thursday:
                    result = "Thứ 5";
                    break;
                case DayOfWeek.Tuesday:
                    result = "Thứ 3";
                    break;
                case DayOfWeek.Wednesday:
                    result = "Thứ 4";
                    break;
            }
            return result;
        }
        public static string GetDayNameVT(DateTime date)
        {
            var day = date.DayOfWeek;
            var result = string.Empty;
            switch (day)
            {
                case DayOfWeek.Friday:
                    result = "T6";
                    break;
                case DayOfWeek.Monday:
                    result = "T2";
                    break;
                case DayOfWeek.Saturday:
                    result = "T7";
                    break;
                case DayOfWeek.Sunday:
                    result = "CN";
                    break;
                case DayOfWeek.Thursday:
                    result = "T5";
                    break;
                case DayOfWeek.Tuesday:
                    result = "T3";
                    break;
                case DayOfWeek.Wednesday:
                    result = "T4";
                    break;
            }
            return result;
        }

        public static string GetRootForder
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["RootContentFolder"]; }
        }

        public static string GetForderAvatar
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["RootContentFolder"] + @"Images\UserAvartars\"; }
        }

        /// <summary>
        /// Hàm đổi đường dẫn ảnh từ sang StringBase64.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ImageToBase64(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path.Trim()))
            {
                //path = GetRootForder + @"\Images\UserAvartars\doctor_avatar_default.png";
                path = GetRootForder + @"/Images/UserAvartars/doctor_avatar_default.png";
                if (!File.Exists(path))
                    return string.Empty;
            }

            var file = new FileStream(path, FileMode.Open, FileAccess.Read);
            var dtByte = new byte[file.Length];
            file.Read(dtByte, 0, System.Convert.ToInt32(file.Length));
            //Close the File Stream
            file.Close();
            //return dtByte; //return the byte data
            //Byte[] _byte = file.ex
            var extension = Path.GetExtension(path).Trim().Replace(".", "");
            return string.Format("data:image/{0};base64,{1}", extension, Convert.ToBase64String(dtByte));
        }


        /// <summary>
        /// Lấy avatar dưới dạng stringbase4
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPathUserImage(string url)
        {
            string path = GetRootForder + "/Images/UserAvartars/";
            var fullPath = path + url;
            if (File.Exists(fullPath))
                return ImageToBase64(fullPath);
            else
                return ImageToBase64(path + "doctor_avatar_default.png");
        }

        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
        /// <summary>
        /// Địa chỉ IP trên máy tính cá nhân
        /// </summary>
        /// <returns></returns>
        public static string GetComputerLanIP()
        {
            string strHostName = Dns.GetHostName();

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            foreach (IPAddress ipAddress in ipEntry.AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    return ipAddress.ToString();
                }
            }

            return "-";
        }
        /// <summary>
        /// Địa chỉ IP trên Server
        /// </summary>
        /// <returns></returns>
        private static string GetComputer_InternetIP()
        {
            // check IP using DynDNS's service
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org");
            WebResponse response = request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());

            // IMPORTANT: set Proxy to null, to drastically INCREASE the speed of request
            request.Proxy = null;

            // read complete response
            string ipAddress = stream.ReadToEnd();

            // replace everything and keep only IP
            return ipAddress.
                Replace("<html><head><title>Current IP Check</title></head><body>Current IP Address: ", string.Empty).
                Replace("</body></html>", string.Empty);
        }

        #region "Design Calendar"
        public static DateTime ConvertToDateTime(string day, string month, string year)
        {
            DateTime dtime = Convert.ToDateTime(day + "/" + Convert.ToString(month) + "/" + Convert.ToString(year));
            return dtime;
        }
        /// <summary>
        /// Lịch tháng của ban giám đốc
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable CalendarWeekly(DateTime date)
        {
            int iMonth = date.Month;
            int iYear = date.Year;
            DataTable dt = new DataTable();
            dt.Columns.Add("Weekly", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Start", typeof(int));
            dt.Columns.Add("End", typeof(int));
            int countDay = GetDaysInMonth(iMonth, iYear);
            DataRow dr = dt.NewRow();
            dr["Weekly"] = "Tuần 1 (01-07/" + iMonth + "/" + iYear + ")";
            dr["Name"] = "Tuần 1";
            dr["Time"] = "01-07/" + iMonth + "/" + iYear + "";
            dr["Start"] = 1;
            dr["End"] = 7;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Weekly"] = "Tuần 2 (08-14/" + iMonth + "/" + iYear + ")";
            dr["Name"] = "Tuần 2";
            dr["Time"] = "08-14/" + iMonth + "/" + iYear + "";
            dr["Start"] = 8;
            dr["End"] = 14;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Weekly"] = "Tuần 3 (15-21/" + iMonth + "/" + iYear + ")";
            dr["Name"] = "Tuần 3";
            dr["Time"] = "15-21/" + iMonth + "/" + iYear + "";
            dr["Start"] = 15;
            dr["End"] = 21;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Weekly"] = "Tuần 4 (22-" + countDay + "/" + iMonth + "/" + iYear + ")";
            dr["Name"] = "Tuần 4";
            dr["Time"] = "22-" + countDay + "/" + iMonth + "/" + iYear + "";
            dr["Start"] = 22;
            dr["End"] = countDay;
            dt.Rows.Add(dr);
            dr = null;
            return dt;

        }
        /// <summary>
        ///Author:  PhuongLT15
        /// Xác định ngày đầu tiên trong háng thuộc thứ mấy
        /// </summary>
        /// <param name="date1"></param>
        /// <returns></returns>
        public static int FirstDate(DateTime date1)
        {
            //tham so thang,nam hien tai
            int month = date1.Month;
            int year = date1.Year;
            //////////
            // DateTime dtime = Convert.ToDateTime(Convert.ToString(month) + "/" + "1" + "/" + Convert.ToString(year));           
            DateTime dtime = ConvertToDateTime("1", Convert.ToString(month), Convert.ToString(year));
            int date = 0;
            string thu = Getday(dtime);
            switch (thu)
            {

                case "ThuHai":
                    date = 0;
                    break;
                case "ThuBa":
                    date = 1;
                    break;
                case "ThuTu":
                    date = 2;
                    break;
                case "ThuNam":
                    date = 3;
                    break;
                case "ThuSau":
                    date = 4;
                    break;
                case "ThuBay":
                    date = 5;
                    break;
                case "ChuNhat":
                    date = 6;
                    break;

            }
            return date;

        }
        /// <summary>
        /// Tạo bảng động lưu dữ liệu thứ
        /// </summary>
        /// <returns></returns>
        public static DataTable CreatTb()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ThuHai", typeof(string));
            dt.Columns.Add("ThuBa", typeof(string));
            dt.Columns.Add("ThuTu", typeof(string));
            dt.Columns.Add("ThuNam", typeof(string));
            dt.Columns.Add("ThuSau", typeof(string));
            dt.Columns.Add("ThuBay", typeof(string));
            dt.Columns.Add("ChuNhat", typeof(string));
            return dt;

        }
        /// <summary>
        /// Đổ toàn bộ dữ liệu ngày của tháng lên Datatable
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable Fill(DateTime date)
        {
            string[,] arr = new string[8, 7];
            arr[0, 0] = "ThuHai";
            arr[0, 1] = "ThuBa";
            arr[0, 2] = "ThuTu";
            arr[0, 3] = "ThuNam";
            arr[0, 4] = "ThuSau";
            arr[0, 5] = "ThuBay";
            arr[0, 6] = "ChuNhat";
            int dem = 0;
            for (int j = 1; j <= CountRow(date); j++)
            {
                if (j == 1)
                {
                    for (int i = FirstDate(date); i < j * 7; i++)
                    {
                        dem = dem + 1;
                        //tham so thang,nam hien tai
                        int month = date.Month;
                        int year = date.Year;
                        //////////                   
                        DateTime dtime = ConvertToDateTime(Convert.ToString(dem), Convert.ToString(month), Convert.ToString(year));
                        int day = dtime.Day;
                        string thu = Getday(dtime);
                        // lblDay.Text = lblDay.Text + thu;
                        switch (thu)
                        {


                            case "ThuHai":
                                arr[j, 0] = day.ToString();
                                break;
                            case "ThuBa":
                                arr[j, 1] = day.ToString();
                                break;
                            case "ThuTu":
                                arr[j, 2] = day.ToString();
                                break;
                            case "ThuNam":
                                arr[j, 3] = day.ToString();
                                break;
                            case "ThuSau":
                                arr[j, 4] = day.ToString();
                                break;
                            case "ThuBay":
                                arr[j, 5] = day.ToString();
                                break;
                            case "ChuNhat":
                                arr[j, 6] = day.ToString();
                                break;

                        }
                    }
                }
                else
                {
                    for (int i = (j - 1) * 7 + 1; i <= j * 7; i++)
                    {
                        dem = dem + 1;
                        if (dem <= GetDaysInMonth(date.Month, date.Year))
                        {
                            //tham so thang,nam hien tai
                            int month = date.Month;
                            int year = date.Year;
                            //////////                          
                            DateTime dtime = ConvertToDateTime(Convert.ToString(dem), Convert.ToString(month), Convert.ToString(year));
                            int day = dtime.Day;
                            string thu = Getday(dtime);
                            // lblDay.Text = lblDay.Text + thu;
                            switch (thu)
                            {
                                case "ThuHai":
                                    arr[j, 0] = day.ToString();
                                    break;
                                case "ThuBa":
                                    arr[j, 1] = day.ToString();
                                    break;
                                case "ThuTu":
                                    arr[j, 2] = day.ToString();
                                    break;
                                case "ThuNam":
                                    arr[j, 3] = day.ToString();
                                    break;
                                case "ThuSau":
                                    arr[j, 4] = day.ToString();
                                    break;
                                case "ThuBay":
                                    arr[j, 5] = day.ToString();
                                    break;
                                case "ChuNhat":
                                    arr[j, 6] = day.ToString();
                                    break;

                            }
                        }

                    }
                }

            }


            DataTable dt = CreatTb();
            //dong chay
            for (int i = 1; i <= CountRow(date); i++)
            {
                DataRow dr = dt.NewRow();
                //cot chay
                for (int j = 0; j <= 6; j++)
                {
                    if (arr[0, j] == "ChuNhat")
                    {
                        dr["ChuNhat"] = arr[i, j];
                    }
                    else if (arr[0, j] == "ThuHai")
                    {
                        dr["ThuHai"] = arr[i, j];
                    }
                    else if (arr[0, j] == "ThuBa")
                    {
                        dr["ThuBa"] = arr[i, j];
                    }
                    else if (arr[0, j] == "ThuTu")
                    {
                        dr["ThuTu"] = arr[i, j];
                    }
                    else if (arr[0, j] == "ThuNam")
                    {
                        dr["ThuNam"] = arr[i, j];
                    }
                    else if (arr[0, j] == "ThuSau")
                    {
                        dr["ThuSau"] = arr[i, j];
                    }
                    else
                    {
                        dr["ThuBay"] = arr[i, j];
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }

        /// <summary>
        /// Xác định 1 ngày nào đó là thuộc thứ mấy
        /// </summary>
        /// <param name="ngay"></param>
        /// <returns></returns>
        public static string Getday(DateTime ngay,int? dayType =null)
        {
            var lstDay = new List<string>();
            string[] arrDaysDefault = { "Chủ nhật","Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"  };
            string[] arrDays = { "Chủ nhật","Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
            string[] thu = { "ChuNhat", "ThuHai", "ThuBa", "ThuTu", "ThuNam", "ThuSau", "ThuBay" };
            if (dayType.HasValue)
            {
                //DayType == 1 or = 2
                //Có thể bổ sung enum cho các trường hợp dùng thứ dạng text, hay dạng số
                lstDay = dayType.Value == 1 ? arrDays.ToList() : arrDaysDefault.ToList();
            }
            else
            {
                lstDay = thu.ToList();
            }
            string re = lstDay[System.Convert.ToInt32(ngay.DayOfWeek)].ToString();
            return re;
        }
        /// <summary>
        /// Xác định số ngày trong 1 tháng là bao ngày
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetDaysInMonth(int month, int year)
        {
            if (month < 1 || month > 12)
            {

            }
            if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month ||
            10 == month || 12 == month)
            {
                return 31;
            }
            else if (2 == month)
            {

                if (0 == (year % 4))
                {

                    if (0 == (year % 400))
                    {
                        return 29;
                    }
                    else if (0 == (year % 100))
                    {
                        return 28;
                    }


                    return 29;
                }

                return 28;
            }
            return 30;
        }
        /// <summary>
        /// Đếm số dòng sẽ hiển thị ra khi dựa vào ngày 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int CountRow(DateTime date)
        {
            //tham so thang,nam hien tai
            int month = date.Month;
            int year = date.Year;
            //////////       
            DateTime dtime = ConvertToDateTime("1", Convert.ToString(month), Convert.ToString(year));
            int countrow = GetDaysInMonth(date.Month, date.Year);
            string thu = Getday(dtime);

            if (countrow < 29 && thu == "ThuHai")
            {
                return 4;
            }
            else if ((countrow >= 30 && thu == "ChuNhat") || (countrow == 31 && thu == "ThuBay"))
            {
                return 6;
            }

            else
            {
                return 5;
            }
        }
        /// <summary>
        /// Trở về tháng trước đó
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NextMonth(DateTime date)
        {

            DateTime pass = date.AddMonths(-1);
            return pass;
        }
        /// <summary>
        /// Trở về tháng phía sau đó
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime PreMonth(DateTime date)
        {
            DateTime pass = date.AddMonths(1);
            return pass;
        }
        /// <summary>
        /// Lấy trạng thái lịch trực
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetStatusCalendar(object obj)
        {
            string status = Convert.ToString(obj);
            string vR = "";

            if (status == "1")
            {
                vR = Resources.Localization.StatusCreateNew;
            }
            else if (status == "2")
            {
                vR = Resources.Localization.StatusPendingApproval;
            }
            else if (status == "3")
            {
                vR = Resources.Localization.StatusIsApproval;
            }
            else if (status == "4")
            {
                vR = Resources.Localization.StatusCancelApproval;
            }
            return vR;
        }
        #endregion


        public static bool IsImage(this HttpPostedFileBase postedFile)
        {
            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                    if (bitmap.Size.IsEmpty)
                    {
                        return false;
                    }
                    else return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


    }


}