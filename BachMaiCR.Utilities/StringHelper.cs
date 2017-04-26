using System.Text;
using System.Text.RegularExpressions;

namespace BachMaiCR.Utilities
{
    public static class StringHelper
    {
        public static string ConvertToShortName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            name = Regex.Replace(name, "\\s+", " ");
            string[] strArray = name.Trim().Split(' ');
            name = strArray[strArray.Length - 1];
            name = name.Substring(0, 1).ToUpper() + name.Remove(0, 1);
            for (int index = 0; index < strArray.Length - 1; ++index)
                name = strArray[index].Substring(0, 1).ToUpper() + name;
            return name;
        }

        public static string ConvertToUnsign(string s)
        {
            return new Regex("\\p{IsCombiningDiacriticalMarks}+").Replace(s.Normalize(NormalizationForm.FormD), string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        }
    }
}