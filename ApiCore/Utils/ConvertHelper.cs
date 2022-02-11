using System;
using System.Text.RegularExpressions;

namespace ApiCore.Utils
{
    public class ConvertHelper
    {
        public class UnixTimestamp
        {
            private static readonly DateTime BEGIN_DATE = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public static long FromDateTime(DateTime t, bool ms = false)
            {
                if (ms == true)
                {
                    return (long)t.ToUniversalTime().Subtract(BEGIN_DATE).TotalMilliseconds;
                }
                return (long)t.ToUniversalTime().Subtract(BEGIN_DATE).TotalSeconds;
            }

            public static DateTime ToDateTime(long timestamp, bool ms = false)
            {
                if (ms)
                {
                    return BEGIN_DATE.AddMilliseconds(timestamp).ToLocalTime();
                }
                return BEGIN_DATE.AddSeconds(timestamp).ToLocalTime();
            }
        }

        public static string ToString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            if (obj is string)
            {
                return (string)obj;
            }
            return obj.ToString();
        }

        public static DateTime ToDateTime(object obj)
        {
            if (obj == null)
            {
                return DateTime.MinValue;
            }
            if (obj is DateTime)
            {
                return (DateTime)obj;
            }
            DateTime result;
            if (DateTime.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return DateTime.MinValue;
        }

        public static double ToDouble(object obj)
        {
            if (obj == null)
            {
                return 0.0;
            }
            if (obj is double)
            {
                return (double)obj;
            }
            double result;
            if (double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            try
            {
                return (double)Convert.ChangeType(obj, typeof(double));
            }
            catch
            {
            }
            return 0.0;
        }

        public static long ToInt64(object obj)
        {
            return (long)ToDouble(obj);
        }

        public static int ToInt32(object obj)
        {
            return (int)ToDouble(obj);
        }

        public static string Encode(string strEncode)
        {
            string text = "";
            if (!string.IsNullOrEmpty(strEncode))
            {
                char[] array = strEncode.ToCharArray();
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    text += ((short)array[i]).ToString("X4");
                }
            }
            return text;
        }

        public static string Decode(string strDecode)
        {
            string text = "";
            string text2 = "";
            if (!string.IsNullOrEmpty(strDecode))
            {
                for (int i = 0; i < strDecode.Length / 4; i++)
                {
                    text += (char)short.Parse(strDecode.Substring(i * 4, 4), System.Globalization.NumberStyles.HexNumber);
                }
                char[] array = text.ToCharArray();
                for (int j = array.Length - 1; j >= 0; j--)
                {
                    text2 += array[j].ToString();
                }
            }
            return text2;
        }
    }

    public class StringHandle
    {
        /// <summary>
        /// 去掉字符串的空格/换行符等非法字符
        /// </summary>
        public static string FilterChat(string str)
        {
            str = str.Trim();
            str = Regex.Replace(str, "\\|\t|\r|\n", string.Empty);
            return str;
        }
    }
}
