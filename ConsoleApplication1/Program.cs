using System;
using FHLog;
using System.Text;
using System.Threading;
using Polly;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {
            

            int count = 0;
            Stack<int> stacks = new Stack<int>();

            Interlocked.CompareExchange(ref count, 1, 1);
            //List<Task> tasks = new List<Task>();
            //for (int i = 0; i < 10000; i++)
            //{
            //    Task task= Task.Factory.StartNew(()=> {
            //        Interlocked.CompareExchange(ref count,)
            //    });
            //    tasks.Add(task);
            //}
            //Task.WaitAll(tasks.ToArray());
            Console.WriteLine(count);
            Console.ReadKey();
        }
        public static Test Copy(Test test)
        {
            Test result = test;
            return result;
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
            Policy.Handle<Exception>();
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
        public string name { get; set; }
        public Parent people { get; set;}
    }
    [XmlInclude(typeof(Children))]
    public class Parent
    {
        public string Name { get; set; }
    }

    public class Children : Parent
    {
        public string MyName { get; set;}
    }
}
