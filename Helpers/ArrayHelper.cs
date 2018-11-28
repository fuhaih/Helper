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
        public static T[][] Split<T>(this IEnumerable<T> list, int partLength)
        {
            if (partLength <= 0)
            {
                throw new ArgumentOutOfRangeException("partLength必须要大于0");
            }
            if (list == null || list.Count() == 0)
            {
                return new T[0][];
            }
            int count = (list.Count() - 1) / partLength + 1;
            T[][] result = new T[count][];
            for (int i = 0; i < count; i++)
            {
                int startindex = i * partLength;
                int secondleve = Math.Min(startindex + partLength, list.Count());
                int secondcount = secondleve - startindex;
                result[i] = new T[secondcount];
                for (int j = 0; j < secondcount; j++)
                {
                    result[i][j] = list.ElementAt(startindex + j);
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

        /// <summary>
        /// 把字节数组转换为16进制字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToHexValues(this byte[] buffer)
        {
            //c#中一个字节的大小是0-256
            //一个两位的十六进制能表示的大小范围也是0-256（16x16）即00-ff
            //所以一个字节能够转换为一个两位的十六进制
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(buffer[i].ToString("x2"));
            }
            return  sb.ToString();
        }
    }
}
