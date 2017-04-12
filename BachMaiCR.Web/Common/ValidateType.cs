using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BachMaiCR.Web.Common
{
    public class ValidateType
    {
        public bool IsInt(string intValue)
        {
            try
            {
                int.Parse(intValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsDouble(string doubleValue)
        {
            try
            {
                Double.Parse(doubleValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // kiểm tra ngày tháng theo định dạng dd/MM/yyyy
        public bool IsDatetime(string dateValue)
        {
            try
            {
                DateTime.Parse(dateValue, new CultureInfo("vi-VN"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsBool(string boolValue)
        {
            try
            {
                bool.Parse(boolValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Validate(int dataType, string value )
        {
            if (dataType == 3)
            {
                return IsDouble(value);
            }
            else if (dataType == 4)
            {
                return IsDatetime(value);
            }
            else if (dataType == 5)
            {
                return IsInt(value);
            }
            else if (dataType == 6)
            {
                return IsBool(value);
            }
            else
            {
                return true;
            }
        }
    }
}