using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using FHLog;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Data;
using Helpers;
using Helpers.Etc;
using System.Net;
using Polly;
using Polly.Retry;
using System.Xml.Serialization;
namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {
            string configpath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            IniParser parser = IniParser.Load(configpath);
            var success=parser.Delete("Setting", "Test");
            parser.Write("setting", "port", "465");
            parser.Delete("setting", "port");
            string[] sections= parser.GetSections();
            //写入配置
            //ProfileWriteValue("Settings", "DefaultSerialPort", "8327", configpath);
            string port = parser.ReadString("setting", "port");
            //Console.WriteLine(value);
            Console.ReadKey();
        }

        private static string Base64(byte[] buffer)
        {
            string[] base64 = new string[] {
                "A","B","C","D","E","F","G","H","I","J","K","L","M",
                "N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                "a","b","c","d","e","f","g","h","i","j","k","l","m",
                "n","o","p","q","r","s","t","u","v","w","x","y","z",
                "0","1","2","3","4","5","6","7","8","9","+","/"
            };
            //byte[] buffer = Encoding.Unicode.GetBytes(str);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(Convert.ToString(buffer[i], 2).PadLeft(8,'0'));
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < builder.Length; i = i + 6)
            {
                string charstr = builder.ToString(i, 6);
                result.Append(base64[Convert.ToInt32(charstr,2)]);
            }
            return result.ToString();
        }

        private static void TestInfo(object sender)
        {
            FHLoger.Info("TESTINFO");
        }

        private static void TestError(object sender)
        {
            FHLoger.Error("TESTERROR");        
        }

        private static void TestFatal(object sender)
        {
            FHLoger.Fatal("TESTFATAL");        
        }

        private static void TestWarn(object sender)
        {
            FHLoger.Warn("TESTWARN");            
        }

        private static void TestPolicy()
        {
            var plicy = Policy.Handle<Exception>().Retry(3, (ex, count, context) => {
                Console.WriteLine(string.Format("发生异常{0},尝试第{1}次", ex.Message, count));
            });
            plicy.Execute(() => {
                Thread.Sleep(1000);
                throw new Exception("retry");
            });
        }

        private static void ExcelCopy()
        {

        }

    }

    public class Test
    {
        public DateTime time { get; set; }
    }
}
