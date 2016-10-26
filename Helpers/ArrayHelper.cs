using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class ArrayHelper
    {
        /// <summary>
        /// 数组拆分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="partLength">每组数组的长度</param>
        /// <returns>差分后的交错数组</returns>
        public static T[][] Split<T>(this IEnumerable<T> t, int partLength)
        {
            List<T> buffer = t.ToList();
            int length = t.Count();
            int count = (length - 1) / partLength + 1;
            int remainder = length % partLength;
            T[][] result = new T[count][];
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    result[i] = new T[remainder == 0 ? partLength : remainder];
                    buffer.CopyTo(i * partLength, result[i], 0, remainder == 0 ? partLength : remainder);
                }
                else
                {
                    result[i] = new T[partLength];
                    buffer.CopyTo(i * partLength, result[i], 0, partLength);
                }
            }
            return result;
        }

        public static string UnicodeToString(this byte[] bytes)
        {
            string result = Encoding.Unicode.GetString(bytes);
            return result;
        }

        public static byte[] ToByteArray(string[] str)
        {
            byte[] result = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                result[i] = byte.Parse(str[i]);
            }
            return result;
        }
    }
}
