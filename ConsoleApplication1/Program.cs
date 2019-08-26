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

namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        static void Main(string[] args)
        {
            byte byte0 = 255 ;
            byte byte1 = 255;
            byte[] bytetest = new byte[] { byte0, byte1 };
            string value = Encoding.UTF8.GetString(bytetest);
            //value=value.Replace("�", string.Empty);
            value=value.Replace("?", string.Empty);
            Console.WriteLine(value);
            //string url = "https://www.taobao.com/";

            //string html = GetHtml(url, Encoding.UTF8);
            Console.ReadKey();
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
                table.Cell(2 + i, 4).Range.Text = iskey ? "PK" : "";
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

        private static void TestOrm()
        {
            string connectStr = "server=192.168.68.12;uid=sa;pwd=TT_database@2106;database=TTBEMS_System";
            string commandStr = "SELECT * FROM [TTBEMS_System].[dbo].[T_BD_BuildBaseInfo]";
            Converter converter = new Converter();
            List<BuildBaseInfo> result = converter.Excute<BuildBaseInfo>(connectStr, commandStr);
        }

        private static Func<IDataReader, T> GetMapFunc<T>(IDataReader dataReader)
        {
            var exps = new List<Expression>();

            var columnNames = Enumerable.Range(0, dataReader.FieldCount)
                                .Select(i => new { i, name = dataReader.GetName(i) });
            var paramRow = Expression.Parameter(typeof(IDataReader), "row");
            var nullvalue = Expression.Constant(System.DBNull.Value);
            List<MemberBinding> memberBindings = new List<MemberBinding>();

            var indexerInfo = typeof(IDataRecord).GetProperty("Item", new[] { typeof(int) });
            foreach (var column in columnNames)
            {
                var outPropertyInfo = typeof(T).GetProperty(
                    column.name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (outPropertyInfo == null)
                    continue;
                var columnNameExp = Expression.Constant(column.i);
                var propertyExp = Expression.MakeIndex(
                    paramRow,
                    indexerInfo, new[] { columnNameExp });
                var condition = Expression.Equal(propertyExp, nullvalue);
                var convertExp = Expression.Convert(propertyExp, outPropertyInfo.PropertyType);
                //这个三木表达式可能有点影响速度 reader[field]==System.DBNull.Value?defult():conver(reader[field])
                //这里reader[field]索引使用了两次，需要优化一下
                var setExp = Expression.Condition(condition, Expression.Default(outPropertyInfo.PropertyType), convertExp);
                MemberBinding memberBinding = Expression.Bind(outPropertyInfo, setExp);
                memberBindings.Add(memberBinding);
            }
            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), memberBindings.ToArray());
            Expression<Func<IDataReader, T>> lambda = Expression.Lambda<Func<IDataReader, T>>(init, paramRow);
            Func<IDataReader, T> func = lambda.Compile();
            return func;
        }

    }

    public class BuildBaseInfo
    {
        /// <summary>
        /// 市平台ID
        /// </summary>
        [DisplayName("市平台ID")]
        public string F_BuildCityID { get; set; }
        /// <summary>
        /// 建筑名称
        /// </summary>
        [DisplayName("建筑名称")]
        public string F_BuildName { get; set; }
        /// <summary>
        /// 建筑地址
        /// </summary>
        [DisplayName("建筑地址")]
        public string F_BuildAddr { get; set; }

        /// <summary> 
        /// 业主单位
        /// </summary>
        [DisplayName("业主单位")]
        public string F_OwnerUnit { get; set; }
        /// <summary>
        /// 业主联系人、电话
        /// </summary>
        [DisplayName("业主联系人")]
        public string F_OwnerNum { get; set; }
        /// <summary>
        /// 用能系统单位
        /// </summary>
        [DisplayName("用能系统单位")]
        public string F_EnergyUnit { get; set; }
        /// <summary>
        /// 用能系统单位联系人、电话
        /// </summary>
        [DisplayName("用能系统单位联系人电话")]
        public string F_EnergyUnitNum { get; set; }

        /// <summary>
        /// 建筑年代 80，90，00，10
        /// </summary>
        [DisplayName("建筑年代")]
        public int F_BuildAge { get; set; }

        /// <summary>
        /// 申报类型，1 既有建筑，2 新增建筑
        /// </summary>
        [DisplayName("建筑属性")]
        public int F_DeclareType { get; set; }

        /// <summary>
        /// 建筑类型
        /// </summary>
        [DisplayName("建筑类型")]
        public string F_BuildType { get; set; }
        /// <summary>
        /// 建筑功能
        /// </summary>
        [DisplayName("建筑功能")]
        public string F_BuildFunc { get; set; }
        /// <summary> 
        /// 所属街道
        /// </summary>
        [DisplayName("所属街道")]
        public string F_Street { get; set; }
        /// <summary>
        /// 所属去
        /// </summary>
        [DisplayName("所属区")]
        public string F_DistrictCode { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [DisplayName("经度")]
        public string F_BuildLong { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [DisplayName("纬度")]
        public string F_BuildLat { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        [DisplayName("建筑面积")]
        public string F_TotalArea { get; set; }
        /// <summary>
        /// 凭证号
        /// </summary>
        [DisplayName("凭证号")]
        public string F_KeyCode { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DisplayName("创建人")]
        public string F_Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public string F_CreatTime { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [DisplayName("标签")]
        public string F_Tag { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string F_Desc { get; set; }

        /// <summary> 
        /// 竣工日期
        /// </summary>
        [DisplayName("竣工日期")]
        
        public int? F_BuildYear { get; set; }
        /// <summary> 
        /// 建筑层数
        /// </summary>
        [DisplayName("建筑层数")]
        
        public int? F_Floor { get; set; }
        /// <summary> 
        /// 地上层数
        /// </summary>
        [DisplayName("地上层数")]
        
        public int? F_UpFloor { get; set; }
        /// <summary> 
        /// 空调形式
        /// </summary>
        [DisplayName("空调形式")]
        
        public string F_AirConditionerForm { get; set; }
        /// <summary> 
        /// 可再生能源系统形式
        /// </summary>
        [DisplayName("可再生能源")]
        
        public string F_RenewableEnergy { get; set; }

        /// <summary>
        /// 建筑体系系数
        /// </summary>
        [DisplayName("建筑体系系数")]
        public string F_BuildSysRate { get; set; }

        /// <summary>
        /// 制冷设备
        /// </summary>
        [DisplayName("集中式空调系统冷源设备")]
        public string F_RefEquipment { get; set; }
        /// <summary>
        /// 制热设备
        /// </summary>
        [DisplayName("集中式空调系统热源生活热水设备")]
        public string F_HeatEquipment { get; set; }

        /// <summary> 
        /// 报建编号
        /// </summary>
        [DisplayName("报建编号")]
        public string F_ConstructionNum { get; set; }
        /// <summary> 
        /// 设计单位
        /// </summary>
        [DisplayName("设计单位")]
        public string F_DesignDept { get; set; }
        /// <summary> 
        /// 实施单位
        /// </summary>
        [DisplayName("实施单位")]
        public string F_WorkDept { get; set; }
        /// <summary> 
        /// 监理单位
        /// </summary>
        [DisplayName("监理单位")]
        public string F_ConstructionUnit { get; set; }
        /// <summary> 
        /// 能效设计标准
        /// </summary>
        [DisplayName("执行节能设计标准")]
        public string F_EnergyStandard { get; set; }
        /// <summary> 
        /// 项目联系人
        /// </summary>
        [DisplayName("项目联系人")]
        public string F_ProjectContact { get; set; }
        /// <summary> 
        /// 项目联系人电话
        /// </summary>
        [DisplayName("项目联系人电话")]
        public string F_ProjectContactNum { get; set; }
        /// <summary> 
        /// 上级主管单位
        /// </summary>
        [DisplayName("上级主管单位")]
        public string F_SuperiorAuthority { get; set; }
        /// <summary> 
        /// 物业管理单位
        /// </summary>
        [DisplayName("物业管理单位")]
        public string F_PropManageDept { get; set; }
        /// <summary> 
        /// 物业联系人电话
        /// </summary>
        [DisplayName("物业联系人电话")]
        public string F_PropContactNum { get; set; }

        /// <summary>
        /// 物业服务人数
        /// </summary>
        [DisplayName("物业服务人数")]
        public string F_PropServiceNum { get; set; }
        /// <summary>
        /// 服务形式 集中式/独立式
        /// </summary>
        [DisplayName("服务形式")]
        public string F_FormOfService { get; set; }
        /// <summary>
        /// 用能单位数量
        /// </summary>
        [DisplayName("用能单位数量")]
        public string F_UnitNum { get; set; }
        /// <summary>
        /// 办公人数
        /// </summary>
        [DisplayName("办公人数")]
        public string F_WorkNum { get; set; }

        /// <summary> 
        /// 能耗监测系统实施单位
        /// </summary>
        [DisplayName("能耗监测系统实施单位")]
        public string F_SystemWorkDept { get; set; }
        /// <summary> 
        /// 能耗监测系统联系人电话
        /// </summary>
        [DisplayName("能耗监测系统联系人电话")]
        public string F_SystemContactNum { get; set; }
        /// <summary> 
        /// 地上面积
        /// </summary>
        [DisplayName("地上面积")]
        public double? F_UpArea { get; set; }
        /// <summary>
        /// 地下面积
        /// </summary>
        [DisplayName("地下面积")]
        public double? F_DownArea { get; set; }
        /// <summary> 
        /// 用电户号
        /// </summary>
        [DisplayName("用电户号")]
        public string F_ElectriAccountNum { get; set; }
        /// <summary> 
        /// 用水户号
        /// </summary>
        [DisplayName("用水户号")]
        public string F_WaterAccountNum { get; set; }
        /// <summary> 
        /// 用气户号
        /// </summary>
        [DisplayName("用气户号")]

        public string F_GasAccountNum { get; set; }
        /// <summary> 
        /// 地下层数
        /// </summary>
        [DisplayName("地下层数")]

        public int? F_DownFloor { get; set; }
        /// <summary> 
        /// 建筑高度
        /// </summary>
        [DisplayName("建筑高度")]

        public double? F_BuildHeight { get; set; }
        /// <summary>
        /// 办公面积
        /// </summary>
        [DisplayName("办公面积")]

        public double? F_OfficeArea { get; set; }
        /// <summary>
        /// 商场面积
        /// </summary>
        [DisplayName("商场面积")]

        public double? F_MailArea { get; set; }
        /// <summary>
        /// 宾馆饭店
        /// </summary>
        [DisplayName("宾馆饭店")]

        public double? F_HotelArea { get; set; }
        /// <summary>
        /// 室内车库
        /// </summary>
        [DisplayName("室内车库")]

        public double? F_ParkingArea { get; set; }
        /// <summary>
        /// 设备机房面积
        /// </summary>
        [DisplayName("设备机房面积")]

        public double? F_EquipmentRoomArea { get; set; }

        /// <summary>
        /// 信息机房面积
        /// </summary>
        [DisplayName("信息机房面积")]
        public double? F_InfomationRoomArea { get; set; }

        /// <summary>
        /// 其它
        /// </summary>
        [DisplayName("其它")]

        public double? F_OtherArea { get; set; }
        /// <summary>
        /// 机房等级
        /// </summary>
        [DisplayName("机房等级")]

        public int? F_EngineRoomLevel { get; set; }
        /// <summary>
        /// 供电密度
        /// </summary>
        [DisplayName("供电密度")]

        public double? F_PowerDensity { get; set; }
        /// <summary>
        /// 设计负荷
        /// </summary>
        [DisplayName("设计负荷")]

        public double? F_DesignLoad { get; set; }
        /// <summary>
        /// 服务形式
        /// </summary>
        [DisplayName("服务形式")]

        public int? F_ServiceType { get; set; }
        /// <summary>
        /// 变压器数量
        /// </summary>
        [DisplayName("变压器数量")]

        public int? F_TransformerNum { get; set; }

        [DisplayName("低配电间数量")]
        public string F_LowDistributeRoomNum { get; set; }

        [DisplayName("低配电间回路数")]
        public string F_LowDistributeCircuitNum { get; set; }

        [DisplayName("低配电间安装多功能呢电表回路数")]
        public string F_LowDistributeMeterInstallNum { get; set; }
        [DisplayName("低配电间未安装多功能电表回路数")]
        public string F_LowDistributeMeterUninstallNum { get; set; }
        [DisplayName("变压器容量")]
        public string F_TransformerCapacity { get; set; }

        /// <summary>
        /// 变压器装机容量
        /// </summary>
        [DisplayName("变压器装机容量")]

        public double? F_InstallCapacity { get; set; }
        /// <summary>
        /// 使用能源种类
        /// </summary>
        [DisplayName("使用能源种类")]

        public string F_EnergyType { get; set; }
        /// <summary>
        /// 能源形式
        /// </summary>
        [DisplayName("能源形式")]

        public string F_EnergyForm { get; set; }

        /// <summary>
        /// 主要功能区1
        /// </summary>
        [DisplayName("主要功能区1")]
        public string F_BuildMainFunc1 { get; set; }
        /// <summary>
        /// 主要功能区1面积
        /// </summary>
        [DisplayName("主要功能区1面积")]
        public string F_BuildMainFunc1Area { get; set; }

        /// <summary>
        /// 主要功能区2
        /// </summary>
        [DisplayName("主要功能区2")]
        public string F_BuildMainFunc2 { get; set; }
        /// <summary>
        /// 主要功能区2面积
        /// </summary>
        [DisplayName("主要功能区2面积")]
        public string F_BuildMainFunc2Area { get; set; }
        /// <summary>
        /// 主要功能区2
        /// </summary>
        [DisplayName("主要功能区2")]
        public string F_BuildMainFunc3 { get; set; }
        /// <summary>
        /// 主要功能区2面积
        /// </summary>
        [DisplayName("主要功能区2面积")]
        public string F_BuildMainFunc3Area { get; set; }
        /// <summary>
        /// 主要功能区2
        /// </summary>
        [DisplayName("主要功能区2")]
        public string F_BuildMainFunc4 { get; set; }
        /// <summary>
        /// 主要功能区2面积
        /// </summary>
        [DisplayName("主要功能区2面积")]
        public string F_BuildMainFunc4Area { get; set; }
        /// <summary> 
        /// 建筑结构形式
        /// </summary>
        [DisplayName("建筑结构形式")]
        public string F_StructureFrom { get; set; }
        /// <summary> 
        /// 建筑外墙形式
        /// </summary>
        [DisplayName("建筑外墙形式")]
        public string F_WallFrom { get; set; }


        /// <summary> 
        /// 建筑外墙保温
        /// </summary>
        [DisplayName("建筑外墙保温")]
        public string F_WallWarn { get; set; }
        /// <summary>
        /// 建筑外墙材料
        /// </summary>
        [DisplayName("建筑外墙材料")]
        public string F_WallMaterial { get; set; }
        /// <summary>
        /// 建筑遮阳类型
        /// </summary>
        [DisplayName("建筑遮阳类型")]
        public string F_BuildShadeType { get; set; }

        /// <summary> 
        /// 建筑外窗类型
        /// </summary>
        [DisplayName("建筑外窗类型")]
        public string F_WindowsType { get; set; }

    }

    public class BuildInfoV1:BuildBaseInfo
    {

        /// <summary> 
        /// 物业公司
        /// </summary>
        [DisplayName("物业公司")]
        public string F_PropCompany { get; set; }
        /// <summary> 
        /// 常驻人数
        /// </summary>
        [DisplayName("常驻人数")]
        public int? F_ResidentNum { get; set; }
        /// <summary> 
        /// 监测系统验收信息
        /// </summary>
        [DisplayName("监测系统验收信息")]
        public DateTime? F_SystemAcceptDate { get; set; }
        /// <summary> 
        /// 采暖系统形式
        /// </summary>
        [DisplayName("采暖系统形式")]
        public string F_HeatingEquiForm { get; set; }

        /// <summary> 
        /// 窗框材料类型
        /// </summary>
        [DisplayName("窗框材料类型")]
        public string F_WindowsMaterial { get; set; }
        /// <summary> 
        /// 建筑玻璃类型
        /// </summary>
        [DisplayName("建筑玻璃类型")]
        public string F_GlassType { get; set; }
        /// <summary> 
        /// 办公人数
        /// </summary>
        [DisplayName("办公人数")]
        public string F_OfficeNum { get; set; }
    }

    public class BuildInfoV2 : BuildBaseInfo
    {

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
