using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Data;
using Helpers;
using System.Xml.Serialization;
using System.Data.SqlClient;
using MSWord = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Xml;
using System.Linq.Expressions;
using Helpers.Orm;
using Polly;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls.Expressions;
using System.IO.Compression;
using System.ComponentModel;
using Helpers.Media;

namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {

            TestMoveMoov();
            Console.ReadKey();
        }

        public static void TestMoveMoov()
        {
            string filename = @"D:\fuhai\视频语料1.mp4";
            string newfilename = @"D:\fuhai\视频语料1_temple.mp4";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (MediaTransformer transform = new MediaTransformer(filename))
            {
                transform.MoveMoov2Top(newfilename);
            }
            watch.Stop();
            Console.WriteLine("耗时：{0}",watch.Elapsed);
        }

        public static void ReadFile()
        {
            string path = @"D:\360安全浏览器下载\ZendStudio-13.6.1-win32.win32.x86_64.exe";
            using (FileStream file = File.OpenRead(path))
            {
                byte[] buffer = new byte[file.Length];
                int result = file.Read(buffer, 0, buffer.Length);
                Console.WriteLine("文件读取完成,结果为{0}", result);
            }
            //byte[] buffer = File.ReadAllBytes(path);
            //Console.WriteLine("文件读取完成,结果为{0}", buffer.Length);
        }

        public static string RandomString(Random random)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                int rvalue = random.Next(255);
                byte rbyte = Convert.ToByte(rvalue);
                string rstr = rbyte.ToString("x2");
                builder.Append(rstr);
            }
            return builder.ToString();
        }
        public static string GetHtml(string url, Encoding encoding)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding);
                    else
                        reader = new StreamReader(response.GetResponseStream(), encoding);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch(Exception ex)
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();
                if (request != null)
                    request = null;
            }
            return string.Empty;
        }

        public static void TestPolly()
        {
            var handle = Policy.Handle<Exception>()
                .Retry(3, (exception, retryCount) =>
                {
                    Console.WriteLine("获取recorddata异常，正在进行第{0}次尝试", retryCount);
                    Thread.Sleep(1000 * (int) Math.Pow(3, retryCount));
                    // do something 
                });
            int count = 0;
            handle.Execute(() =>
            {
                if (count < 3)
                {
                    count++;
                    throw new Exception("text");
                }
                
            });
        }

        static async Task DemoAsync()
        {
            var d = new Dictionary<int, int>();

            for (int i = 0; i < 10000; i++)
            {

                int id = Thread.CurrentThread.ManagedThreadId;

                int count;

                d[id] = d.TryGetValue(id, out count) ? count + 1 : 1;

                await Task.Run(() => { });

            }

            foreach (var pair in d) Console.WriteLine(pair);

        }

        public async Task test()
        {
            Task task = new Task(() => { });
            await task;
        }

        public async Task Test1()
        {
            await test();
        }


        

    }


    public class TestTree
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public List<TestTree> Childs { get; set; }
    }

    public interface IService
    {

    }

    public class MyService:IService
    {
        
    }

    public class Parent
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Desc;
    }

    public class Sun : Parent
    {
        public string Classes { get; set; }

        public string GetValue()
        {
            return "test";
        }

        public string GetValue(string value)
        {
            return value;
        }

        public string GetValue(string value1, string value2)
        {
            return value1 + value2;
        }

        public Parent GetParent(Expression<Func<Parent, Parent>> ex)
        {
            Func<Parent, Parent> getmethod = ex.Compile();
            return getmethod(this);
        }
    }


    public enum TitileCategory
    {
        Book,
        Movie
    }

    public class Title
    {

        public Title(string name, TitileCategory category)
        {

        }
    }

    public interface IOrder
    {
        IOrder GetOrderInfo();
    }

    public class OrderA : IOrder
    {
        public string A { get; set; }

        public IOrder GetOrderInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class OrderB : IOrder
    {
        public string B { get; set; }

        IOrder IOrder.GetOrderInfo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 表格对象转换器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Converter<T>
    {
        public List<T> Convert(DataTable table)
        {
            List<T> result = new List<T>();
            var map = GetMapFunc(table.Columns);
            foreach (DataRow row in table.Rows)
            {
                result.Add(map(row));
            }
            return result;
        }

        private Func<DataRow, T> GetMapFunc(DataColumnCollection Columns)
        {
            var exps = new List<Expression>();

            var paramRow = Expression.Parameter(typeof(DataRow), "row");

            List<MemberBinding> memberBindings = new List<MemberBinding>();
            var itemarray = typeof(DataRow).GetProperty("ItemArray");
            var indexerInfo = typeof(DataRow).GetProperty("Item", new[] { typeof(string) });
            foreach (DataColumn column in Columns)
            {
                var outPropertyInfo = typeof(T).GetProperty(
                    column.ColumnName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (outPropertyInfo == null)
                    continue;
                DataRow row = null;
                var columnNameExp = Expression.Constant(column.ColumnName);
                var propertyExp = Expression.MakeIndex(
                    paramRow,
                    indexerInfo, new[] { columnNameExp });
                var convertExp = Expression.Convert(propertyExp, outPropertyInfo.PropertyType);
                MemberBinding memberBinding = Expression.Bind(outPropertyInfo, convertExp);
                memberBindings.Add(memberBinding);
            }

            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), memberBindings.ToArray());
            Expression<Func<DataRow, T>> lambda = Expression.Lambda<Func<DataRow, T>>(init, paramRow);
            Func<DataRow, T> func = lambda.Compile();
            return func;
        }
    }
}
