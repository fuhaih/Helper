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
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Data;
using Helpers;
using System.Net;
using System.Xml.Serialization;
namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        [MethodImpl(MethodImplOptions.InternalCall), SecurityCritical]
        [DllImport("kernel32.dll")]
        internal static extern string FastAllocateString(int length);

        static void Main(string[] args)
        {
            Person per = new Person()
            {
                birth = DateTime.Now,
                id = 1,
                name = "fuhai",
                son=new Son {
                    name="son",
                    birth=DateTime.Now
                }
            };
            per.XmlSerialize("test.xml");
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

        private static DataTable getTableFormat()
        {
            DateTime startTime = DateTime.Now.Date.AddHours(-2);
            DateTime endTime = DateTime.Now.Date.AddHours(6);
            DataTable result = new DataTable();
            result.Columns.Add("Time").Caption="时间";
            while (startTime < endTime)
            {
                result.Rows.Add(startTime.ToString("HH:mm"));
                startTime = startTime.AddMinutes(15);
            }
            return result;
        }
        
        static string GetDescValue(string desc,string key)
        {
            string result = "";
            Regex reg = new Regex("{"+key+":(.*?)}");
            Match match = reg.Match(desc);
            if (match != null)
            {
                result = match.Groups[1].Value;
            }
            return result;
            
        }

        static void TestMvc()
        {
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://192.168.68.38:8081/shevcs/v1/notification_stationInfo");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://192.168.68.38:8081/shevcs/v1/notification_stationInfo");
            request.Method = "POST";
            Stream stream = request.GetRequestStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("test");
            HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
            Stream responStream= respon.GetResponseStream();
        }
    }

    public class Person
    {
        
        public int id { get; set; }
        public string name { get; set; }
        //[DataMember]
        public DateTime birth { get; set; }
        [XmlElement(ElementName = "myson")]
        public Son son { get; set; }

    }
    [DataContract]
    public class Son
    {
        [XmlElement(ElementName ="sonname")]
        public string name { get; set; }
        public DateTime? birth { get; set; }
        public Glasson glasson { get; set; }
    }

    public class Glasson
    {
        public string name { get; set; }
        public DateTime? birth { get; set; }
    }
}
