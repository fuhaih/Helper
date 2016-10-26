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
        //public static string ToMD5<T>(this T t)
        //{
        //    string result = "";
        //    byte[] buffer;
        //    if(typeof(T)==typeof(String))
        //    {
        //        buffer = Encoding.Default.GetBytes(t as string);
        //    }
        //    else if (typeof(T) == typeof(char[]))
        //    {
        //        buffer = Encoding.Default.GetBytes(t as char[]);
        //    }
        //    else {
        //        buffer = new byte[1024];
        //    }
        //    //else {
        //    //    buffer = t.BinarySerialize();
        //    //}
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    byte[] retVal = md5.ComputeHash(buffer);
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < retVal.Length; i++)
        //    {
        //        sb.Append(retVal[i].ToString("x2"));
        //    }
        //    result = sb.ToString();
        //    return result;
        //}
    }
}
