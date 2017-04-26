using System;
using System.Collections.Generic;
using System.Reflection;

namespace BachMaiCR.Utilities.Enums
{
    public class StringEnum
    {
        private static Dictionary<string, string> _stringValues = new Dictionary<string, string>();
        private Type _enumType;

        public Type EnumType
        {
            get
            {
                return this._enumType;
            }
        }

        public StringEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", enumType.ToString()));
            this._enumType = enumType;
        }

        public string GetStringValue(string valueName)
        {
            string str = (string)null;
            try
            {
                str = StringEnum.GetStringValue((Enum)Enum.Parse(this._enumType, valueName, false));
            }
            catch (Exception ex)
            {
            }
            return str;
        }

        public Array GetStringValues()
        {
            List<string> stringList = new List<string>();
            foreach (MemberInfo field in this._enumType.GetFields())
            {
                StringValueAttribute[] customAttributes = field.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (customAttributes.Length > 0)
                    stringList.Add(customAttributes[0].Value);
            }
            return stringList.ToArray();
        }

        public Dictionary<string, string> GetListValues()
        {
            Type underlyingType = Enum.GetUnderlyingType(this._enumType);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (FieldInfo field in this._enumType.GetFields())
            {
                StringValueAttribute[] customAttributes = field.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (customAttributes != null && customAttributes.Length > 0)
                    dictionary.Add(Enum.ToObject(underlyingType, field.Name).ToString(), customAttributes[0].Value);
            }
            return dictionary;
        }

        public bool IsStringDefined(string stringValue)
        {
            return StringEnum.Parse(this._enumType, stringValue) != null;
        }

        public bool IsStringDefined(string stringValue, bool ignoreCase)
        {
            return StringEnum.Parse(this._enumType, stringValue, ignoreCase) != null;
        }

        public static string GetStringValue(Enum value)
        {
            string empty = string.Empty;
            StringValueAttribute[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (customAttributes != null && customAttributes.Length > 0)
                empty = customAttributes[0].Value;
            return empty;
        }

        public static object Parse(Type type, string stringValue)
        {
            return StringEnum.Parse(type, stringValue, false);
        }

        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object obj = null;
            string strA = (string)null;
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));
            foreach (FieldInfo field in type.GetFields())
            {
                StringValueAttribute[] customAttributes = field.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (customAttributes.Length > 0)
                    strA = customAttributes[0].Value;
                if (string.Compare(strA, stringValue) == 0)
                {
                    obj = Enum.Parse(type, field.Name, false);
                    break;
                }
            }
            return obj;
        }

        public static bool IsStringDefined(Type enumType, string stringValue)
        {
            return StringEnum.Parse(enumType, stringValue) != null;
        }

        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
        {
            return StringEnum.Parse(enumType, stringValue, ignoreCase) != null;
        }
    }
}