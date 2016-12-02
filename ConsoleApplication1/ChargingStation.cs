using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Helpers;
namespace ConsoleApplication1
{
    class ChargingStation
    {
        public static string ToHmacMD5()
        {
            string result = "";
            string key = "1234567890abcdef";
            HMACMD5 md5 = new HMACMD5();
            return result;
        }

        public static string sign()
        {
            HMACMD5 MDS = new HMACMD5(Encoding.UTF8.GetBytes("1234567890abcdef"));
            string operatorID = "123456789";
            string data = "il7B0BSEjFdzpyKzfOFpvg/Se1CP802RItKYFPfSLRxJ3jf0bVl9hvYOEktPAYW2nd7S8MBcyHYyacHKbISq5iTmDzG+ivnR+SZJv3USNTYVMz9rCQVSxd0cLlqsJauko79NnwQJbzDTyLooYoIwz75qBOH2/xOMirpeEqRJrF/EQjWekJmGk9RtboXePu2rka+Xm51syBPhiXJAq0GfbfaFu9tNqs/e2Vjja/ltE1M0lqvxfXQ6da6HrThsm5id4ClZFIi0acRfrsPLRixS/IQYtksxghvJwbqOsbIsITail9Ayy4tKcogeEZiOO+4Ed264NSKmk7l3wKwJLAFjCFogBx8GE3OBz4pqcAn/ydA=";
            string Timestamp = "20160729142400";
            string seq = "0001";
            byte[] buffer = MDS.ComputeHash(Encoding.UTF8.GetBytes(operatorID + data + Timestamp + seq));
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
