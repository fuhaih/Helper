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
        public static byte[] ToMD5(this string t)
        {
            byte[] buffer= Encoding.Default.GetBytes(t);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < retVal.Length; i++)
            //{
            //    sb.Append(retVal[i].ToString("x2"));
            //}
            //result = sb.ToString();
            return retVal;
        }
        /// <summary>
        /// 转换为2位16进制字符串格式
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToX2(this byte[] bytes)
        {
            /**
             * 每个字节8位，可以表示为两个四位
             * 1111 1111
             */
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 计算HMACMD5哈希序列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ToHMACMD5(this string t,string key)
        {
            HMACMD5 MDS = new HMACMD5(Encoding.UTF8.GetBytes(key));
            byte[] buffer = MDS.ComputeHash(Encoding.UTF8.GetBytes(t));
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < buffer.Length; i++)
            //{
            //    sb.Append(buffer[i].ToString("x2"));
            //}
            //string result = sb.ToString();
            return buffer;
        }

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>加密后的字符串</returns>
        public static byte[] EncryptAes(string source, string key, string iv)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = Encoding.UTF8.GetBytes(key);
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.IV = Encoding.UTF8.GetBytes(iv);
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = Encoding.UTF8.GetBytes(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    //return Convert.ToBase64String(results, 0, results.Length);
                    return results;
                }
            }
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>解密后的字符串</returns>
        public static byte[] DecryptAes(string source, string key, string iv)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = Encoding.UTF8.GetBytes(key);
                aesProvider.IV = Encoding.UTF8.GetBytes(iv);
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = Convert.FromBase64String(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return results;
                }
            }
        }
    }
}
