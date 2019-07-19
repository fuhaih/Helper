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
namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {
            decimal dl = 4.5M;
            double result = Convert.ToDouble(dl);

            Type type = typeof(Parent);
            PropertyInfo info = type.GetProperty("Name");
            MethodInfo methord= info.GetGetMethod();
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Rows.Add("test",1);

            Converter<Parent> parentconvert = new Converter<Parent>();
            List<Parent> items = parentconvert.Convert(table);

            Console.ReadKey();
        }

        static int SingleNumber(int[] nums)
        {
            int result = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                result = result | nums[i];
            }
            for (int i = 0; i < nums.Length; i++)
            {
                result = result ^ nums[i];
            }
            for (int i = 0; i < nums.Length; i++)
            {
                result = result ^ nums[i];
            }
            return result;
        }

        static void TestBaseLine()
        {
            string planID = "PL0000000032";
            string alID = "AL0001";
            long timespan = DateTime.Now.Ticks;
            string token= (timespan.ToString() + "key").ToMD5().HexEecode();
            string url = string.Format("http://localhost:3756/api/vpp/{0}/{1}/{2}/{3}/{4}", planID,alID, "Default", timespan, token);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse respon =(HttpWebResponse)request.GetResponse();
        }

        static float sqrt(float x)
        {
            if (x == 0) return 0;
            float result = x;
            float xhalf = 0.5f * result;
            int i = (int)result;
            i = 0x5f375a86 - (i >> 1); // what the fuck? 
            result = (float)i;
            result = result * (1.5f - xhalf * result * result); // Newton step, repeating increases accuracy 
            result = result * (1.5f - xhalf * result * result);
            return 1.0f / result;


        }

        static bool IsPalindrome(int x)
        {
            if (x < 0) return false;
            int compare = x;
            int result = 0;
            do
            {
                result = result * 10 + (x % 10);
                x = x / 10;
            } while (x > 0);

            return result == compare;
        }

        static string CountAndSay(int n)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i <= n; i++)
            {
                if (i == 1)
                {
                    builder.Append("1");
                }
                else {
                    int length = builder.Length;
                    int start = 0;

                    for (int j = 0; j < length; j++)
                    {
                        if (builder[j] != builder[start])
                        {
                            builder.Append((j - start).ToString() + builder[start].ToString());
                            start = j;
                        }
                    }
                    builder.Append((length - start).ToString() + builder[start].ToString());
                    builder.Remove(0, length);
                }
            }
            return builder.ToString();
        }

        static void CreateWord()
        {
            string connectstr = @"Data Source=192.168.68.11;Initial Catalog=TTVVP_System;User ID=sa;Password=TT_database@2106";
            DataTable alltable = new DataTable();
            using (SqlConnection con = new SqlConnection(connectstr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM sys.tables order by name", con);
                adapter.Fill(alltable);
            }
            object missing = Missing.Value;
            MSWord.Application wordApp = null;
            MSWord.Document wordDoc = null;

            wordApp = new MSWord.ApplicationClass();
            wordApp.Visible = true;
            wordDoc = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            for (int i = 0; i < alltable.Rows.Count; i++)
            {
                string tablename = Convert.ToString(alltable.Rows[i]["name"]);
                AddTable(wordDoc, connectstr, tablename);
            }

            object path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.docx");
            wordDoc.SaveAs2(ref path, ref missing, ref missing, ref missing, ref missing);
        }

        static void AddTable(MSWord.Document wordDoc, string connectstr, string tablename)
        {

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectstr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("sp_help " + tablename, con);
                adapter.Fill(ds);
            }
            DataTable dtable = ds.Tables[1];
            DataTable pk = ds.Tables[5];
            var sections = wordDoc.Sections;

            DataRow indexrow = pk.Select().FirstOrDefault(m => Convert.ToString(m["index_name"]).IndexOf("PK") == 0);
            string key = indexrow == null ? "" : Convert.ToString(indexrow["index_keys"]);
            string[] keys = key.Split(',').Select(m => m.Trim()).ToArray();
            MSWord.Range range = wordDoc.Range(wordDoc.Paragraphs.Last.Range.Start, wordDoc.Paragraphs.Last.Range.End);
            range.Text = tablename;
            range.Font.Size = 14;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();
            MSWord.Range range1 = wordDoc.Range(wordDoc.Paragraphs.Last.Range.Start, wordDoc.Paragraphs.Last.Range.End);
            MSWord.Table table = wordDoc.Tables.Add(range1, dtable.Rows.Count, 6, null, null);
            table.Borders.Enable = 1;//Ĭ�ϱ��û�б߿�
                                     //��������������

            //���ñ�ͷ
            table.Cell(1, 1).Range.Text = "���";
            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 1).Range.Font.Name = "����";
            table.Cell(1, 1).Range.Font.Size = 12;
            table.Cell(1, 2).Range.Text = "�ֶ�";

            table.Cell(1, 2).Range.Font.Name = "����";
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 2).Range.Font.Size = 12;
            table.Cell(1, 3).Range.Text = "����";
            table.Cell(1, 3).Range.Font.Name = "����";
            table.Cell(1, 3).Range.Bold = 1;
            table.Cell(1, 3).Range.Font.Size = 12;
            table.Cell(1, 4).Range.Text = "��";
            table.Cell(1, 4).Range.Bold = 1;
            table.Cell(1, 4).Range.Font.Name = "����";
            table.Cell(1, 4).Range.Font.Size = 12;
            table.Cell(1, 5).Range.Text = "����";
            table.Cell(1, 5).Range.Bold = 1;
            table.Cell(1, 5).Range.Font.Size = 12;
            table.Cell(1, 5).Range.Font.Name = "����";
            table.Cell(1, 6).Range.Text = "˵��";
            table.Cell(1, 6).Range.Bold = 1;
            table.Cell(1, 6).Range.Font.Name = "����";
            table.Cell(1, 6).Range.Font.Size = 12;

            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                DataRow row = dtable.Rows[i];
                string type = Convert.ToString(row["Type"]) + "(" + Convert.ToString(row["Length"]) + ")";
                string name = Convert.ToString(row["Column_name"]);
                bool iskey = keys.Contains(name);
                table.Cell(2 + i, 1).Range.Text = (i + 1).ToString();
                table.Cell(2 + i, 1).Range.Font.Name = "������";
                table.Cell(2 + i, 1).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 2).Range.Font.Name = "������";
                table.Cell(2 + i, 2).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 2).Range.Text = name;
                table.Cell(2 + i, 3).Range.Font.Name = "������";
                table.Cell(2 + i, 3).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 3).Range.Text = type;
                table.Cell(2 + i, 4).Range.Font.Name = "������";
                table.Cell(2 + i, 4).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 4).Range.Text = iskey ? "PK" : "";
                table.Cell(2 + i, 5).Range.Text = "";
                table.Cell(2 + i, 5).Range.Font.Name = "������";
                table.Cell(2 + i, 5).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 6).Range.Text = "";
                table.Cell(2 + i, 6).Range.Font.Name = "������";
                table.Cell(2 + i, 6).Range.Font.Size = 9.5f;
            }
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
            //MyTask task = new MyTask();
            //await task;

            //FuncTest test = new FuncTest();
            //await test;
            //IObservable<int> observable = Observable.Range(0, 3).Do(Console.WriteLine);
            //await observable;
            //int result = await new Func<int>(() => 0);
        }

        public async Task Test1()
        {
            await test();
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
                builder.Append(Convert.ToString(buffer[i], 2).PadLeft(8, '0'));
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < builder.Length; i = i + 6)
            {
                string charstr = builder.ToString(i, 6);
                result.Append(base64[Convert.ToInt32(charstr, 2)]);
            }
            return result.ToString();
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
    /// ������ת����
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
