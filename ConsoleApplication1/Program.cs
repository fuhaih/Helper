using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using FHLog;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Data;
using Helpers;
using System.Net;
using Quartz;
using Quartz.Impl;
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

			Path.GetInvalidPathChars().ToString();
			RegExTest.RegExTest.Replace();
			//FHLoger.Format.Error.Color = ConsoleColor.Gray;
			//FHLoger.Format.Fatal.Color = ConsoleColor.Gray;
			//FHLoger.Format.Warn.Color = ConsoleColor.Gray;
			//FHLoger.Action.Output.OpenConsole();
			//for (int i = 0; i < 500000; i++)
			//{
			//    ThreadPool.QueueUserWorkItem(TestInfo);
			//    ThreadPool.QueueUserWorkItem(TestError);
			//    ThreadPool.QueueUserWorkItem(TestFatal);
			//    ThreadPool.QueueUserWorkItem(TestWarn);
			//}
			//try
			//{
			//    socketTest.Send();
			//}
			//catch (Exception ex) {
			//    Console.WriteLine(ex.Message);
			//    Console.WriteLine(ex.StackTrace);
			//}
			//TaskTest test = new TaskTest();
			//test.TestAttachedToParent();
			Console.ReadKey();
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

    public class myservice
    {
        public DateTime initialTime;
        public int poit=0;
        public myservice()
        {
            initialTime = DateTime.Now;
        }
    }


    public class test
    {
        public static myservice service = new myservice();
        public test()
        {
            //service.poit = 1;
        }

        public void consoTime()
        {
            Console.WriteLine("service初始化时间:" + service.initialTime.ToString("hh:mm:ss.fffffff"));
        }
    }

    /// 注意：要序列化为Binary数组的时候要添加Serializable特性
    /// 但是，用Serializable特性的时候，如果类声明的是自动化属性，
    /// 那么用DataContractJsonSerializer将对象序列化为json格式的时候会带有k__BackingField后缀
    ///如"<name>k__BackingField":"fuhai"
    ///解决方案，再添加DataContract和DataMembe特性

    [Serializable]
    //[DataContract]
    public class Person
    {
        //[DataMember]
        public int id { get; set; }
        //[DataMember]
        public string name { get; set; }
        //[DataMember]
        public DateTime birth { get; set; }

    }
}
