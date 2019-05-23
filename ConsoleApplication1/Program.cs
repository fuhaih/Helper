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
namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {
            int value = (int)Math.Pow(2,1);
            Console.WriteLine(Convert.ToInt16("1000", 2));
            //List<Parent> parents = new List<Parent>();
            //parents.Select(m=> { m.Age = 1;return m; });

            //Parent parent = new Parent
            //{
            //    Age = 40,
            //    Desc = "test",
            //    Name = "huang"
            //};

            //Sun sun = parent.MappingTo<Parent, Sun>();
            //string filter = parent.Query(m =>  m.Name == sun.Name);
            //Console.WriteLine(filter);
            //Console.WriteLine(CountAndSay(1));
            //Console.WriteLine(CountAndSay(2));
            //Console.WriteLine(CountAndSay(3));
            
            Console.WriteLine(CountAndSay(4));
            Console.WriteLine(CountAndSay(5));


            Console.ReadKey();
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
            
            return result== compare;
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


        static void AddTable(MSWord.Document wordDoc,string connectstr,string tablename)
        {

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectstr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("sp_help "+ tablename, con);
                adapter.Fill(ds);
            }
            DataTable dtable = ds.Tables[1];
            DataTable pk = ds.Tables[5];
            var sections = wordDoc.Sections;

            DataRow indexrow = pk.Select().FirstOrDefault(m => Convert.ToString(m["index_name"]).IndexOf("PK") == 0);
            string key = indexrow == null ? "" : Convert.ToString(indexrow["index_keys"]);
            string[] keys = key.Split(',').Select(m=>m.Trim()).ToArray();
            MSWord.Range range = wordDoc.Range(wordDoc.Paragraphs.Last.Range.Start, wordDoc.Paragraphs.Last.Range.End);
            range.Text = tablename;
            range.Font.Size = 14;
            range.InsertParagraphAfter();
            range.InsertParagraphAfter();
            MSWord.Range range1 = wordDoc.Range(wordDoc.Paragraphs.Last.Range.Start, wordDoc.Paragraphs.Last.Range.End);
            MSWord.Table table = wordDoc.Tables.Add(range1, dtable.Rows.Count, 6, null, null);
            table.Borders.Enable = 1;//默认表格没有边框
                                     //给表格中添加内容

            //设置表头
            table.Cell(1, 1).Range.Text = "序号";
            table.Cell(1, 1).Range.Bold = 1;
            table.Cell(1, 1).Range.Font.Name = "仿宋";
            table.Cell(1, 1).Range.Font.Size = 12;
            table.Cell(1, 2).Range.Text = "字段";

            table.Cell(1, 2).Range.Font.Name = "仿宋";
            table.Cell(1, 2).Range.Bold = 1;
            table.Cell(1, 2).Range.Font.Size = 12;
            table.Cell(1, 3).Range.Text = "类型";
            table.Cell(1, 3).Range.Font.Name = "仿宋";
            table.Cell(1, 3).Range.Bold = 1;
            table.Cell(1, 3).Range.Font.Size = 12;
            table.Cell(1, 4).Range.Text = "键";
            table.Cell(1, 4).Range.Bold = 1;
            table.Cell(1, 4).Range.Font.Name = "仿宋";
            table.Cell(1, 4).Range.Font.Size = 12;
            table.Cell(1, 5).Range.Text = "名称";
            table.Cell(1, 5).Range.Bold = 1;
            table.Cell(1, 5).Range.Font.Size = 12;
            table.Cell(1, 5).Range.Font.Name = "仿宋";
            table.Cell(1, 6).Range.Text = "说明";
            table.Cell(1, 6).Range.Bold = 1;
            table.Cell(1, 6).Range.Font.Name = "仿宋";
            table.Cell(1, 6).Range.Font.Size = 12;

            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                DataRow row = dtable.Rows[i];
                string type = Convert.ToString(row["Type"]) + "(" + Convert.ToString(row["Length"]) + ")";
                string name = Convert.ToString(row["Column_name"]);
                bool iskey = keys.Contains(name);
                table.Cell(2 + i, 1).Range.Text = (i + 1).ToString();
                table.Cell(2 + i, 1).Range.Font.Name = "新宋体";
                table.Cell(2 + i, 1).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 2).Range.Font.Name = "新宋体";
                table.Cell(2 + i, 2).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 2).Range.Text = name;
                table.Cell(2 + i, 3).Range.Font.Name = "新宋体";
                table.Cell(2 + i, 3).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 3).Range.Text = type;
                table.Cell(2 + i, 4).Range.Font.Name = "新宋体";
                table.Cell(2 + i, 4).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 4).Range.Text = iskey ? "PK":"";
                table.Cell(2 + i, 5).Range.Text = "";
                table.Cell(2 + i, 5).Range.Font.Name = "新宋体";
                table.Cell(2 + i, 5).Range.Font.Size = 9.5f;
                table.Cell(2 + i, 6).Range.Text = "";
                table.Cell(2 + i, 6).Range.Font.Name = "新宋体";
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

                await Task.Run(()=> { });

            }

            foreach (var pair in d) Console.WriteLine(pair);

        }

        public async Task test()
        {
            Task task = new Task(()=> { });
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

}
