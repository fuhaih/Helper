using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
namespace Helpers
{
    public static class SecurityHelper
    {
        /// <summary>
        /// 获取string的MD5
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToMD5(this string t)
        {
            string result = "";
            byte[] buffer= Encoding.Default.GetBytes(t);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);
                           
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            result = sb.ToString();
            return result;
        }

        /// <summary>
        /// 计算HMACMD5哈希序列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ToHMACMD5(this string t,string key)
        {
            HMACMD5 MDS = new HMACMD5(Encoding.UTF8.GetBytes(key));
            byte[] buffer = MDS.ComputeHash(Encoding.UTF8.GetBytes(t));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(buffer[i].ToString("x2"));
            }
            string result = sb.ToString();
            return result;
        }
    }
}
