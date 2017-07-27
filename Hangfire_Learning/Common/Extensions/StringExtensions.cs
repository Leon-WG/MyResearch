using System;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string s, int defaultVal = 0)
        {
            int tmp = defaultVal;
            return (int.TryParse(s, out tmp)) ? tmp : defaultVal;
        }

        public static double ToDouble(this string s, double defaultVal = 0d)
        {
            double tmp = defaultVal;

            return (double.TryParse(s, out tmp)) ? tmp : defaultVal;

        }

        public static string RemovePrefix(this string s, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                if (s.StartsWith(prefix))
                {
                    var pos = prefix.Length;
                    s = s.Substring(pos);
                }
            }
            return s;
        }

        public static DateTime? ToDatetime(this string s)
        {
            var tmp = DateTime.MinValue;
            if (DateTime.TryParse(s, out tmp))
            {
                return tmp;
            }
            return null;
        }
    }
}
