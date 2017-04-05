using System;
using System.Text.RegularExpressions;
using System.Security;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using FHLog;
using System.Linq;
using System.IO;
using System.Threading;
using System.Data;
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
            for (int i = 0; i < 5000; i++)
            {
                ThreadPool.QueueUserWorkItem(TestInfo);
                ThreadPool.QueueUserWorkItem(TestError);
                ThreadPool.QueueUserWorkItem(TestFatal);
                ThreadPool.QueueUserWorkItem(TestWarn);
            }
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

        //Regex reg = new System.Text.RegularExpressions.Regex("[\b]");
        //str=reg.Replace(str, "");
        //Console.Write(str);
        //#region 测试接口用例
        //static void getToken()
        //{

        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://192.168.68.38:8081//shevcs/v1/query_token");
        //    request.Method = "Post";
        //    request.ContentType = "application/json;charset=utf-8";

        //    TokenParams tokenparams = new TokenParams
        //    {
        //        OperatorID = "123456789",
        //        OperatorSecret = "1234567890abcdef"
        //    };
        //    ChargingStationMessage message=new ChargingStationMessage{
        //        OperatorID="123456789",
        //        TimeStamp=DateTime.Now.ToString("yyyyMMddHHmmss"),
        //        Data=SecurityHelper.EncryptAes( tokenparams.ToJson(),"1234567890abcdef","1234567890abcdef"),
        //        Seq="0001"
        //    };
        //    string sigContext=message.OperatorID + message.Data + message.TimeStamp + message.Seq;
        //    message.Sig = SecurityHelper.ToHMACMD5(sigContext,"1234567890abcdef").ToUpper();
        //    string str = message.ToJson();
        //    byte[] buffer = Encoding.UTF8.GetBytes(str);
        //    Stream stream = request.GetRequestStream();
        //    stream.Write(buffer, 0, buffer.Length);
        //    HttpWebResponse respon=(HttpWebResponse)request.GetResponse();
        //    Stream responStream=respon.GetResponseStream();
        //    StreamReader reder=new StreamReader(responStream,Encoding.UTF8);
        //    string messageStr=reder.ReadToEnd();
        //    PushRespon pushrespong=JsonHelper.Convert<PushRespon>(messageStr);
        //    string tokenStr = SecurityHelper.DecryptAes(pushrespong.Data, "1234567890abcdef", "1234567890abcdef");
        //    Token token = JsonHelper.Convert<Token>(tokenStr);
        //}
        // <summary>
        // 测试：
        // 正常
        // 运营商异常
        // token异常(缺少，错误，过期)
        // 签名异常
        // 密钥异常
        // </summary>
        //static void push_station_info()
        //{
        //    string tokenStr = "e9eae8dbe51b25a5d9ada51bddb51fb7";
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:6393/shevcs/v1/notification_stationInfo");
        //    request.Method = "Post";
        //    request.ContentType = "application/json;charset=utf-8";
        //    request.Headers.Add("Authorization", string.Format("Bearer {0}", tokenStr));

        //    string stationInfo = "{\"StationInfo\":{\"StationID\":\"000000000000001\",\"OperatorID\":\"123456789\",\"EquipmentOwnerID\":\"123456789\",\"StationName\":\"\u5145\u7535\u7ad9\u540d\u79f0\",\"CountryCode\":\"CN\",\"AreaCode\":\"441781\",\"Address\":\"\u5730\u5740\",\"StationTel\":\"123456789\",\"ServiceTel\":\"123456789\",\"StationType\":1,\"StationStatus\":50,\"ParkNums\":3,\"StationLng\":119.97049,\"StationLat\":31.717877,\"SiteGuide\":\"111111\",\"OpenAllDay\":1,\"MinElectricityPrice\":5.5,\"Construction\":0,\"ParkFree\":1,\"Pictures\":[\"http://www.xxx.com/uploads/plugs/e5/eb/cd/f0469308d9bbd99496618d6d87\",\"http://www.xxx.com/uploads/plugs/7c/0c/81/a8ed867ffdfb597abaf9982b2c\"],\"Payment\":\"1\",\"SupportOrder\":1,\"EquipmentInfos\":[{\"EquipmentID\":\"10000000000000000000003\",\"EquipmentName\":\"电桩001\",\"ManufacturerID\":\"123456789\",\"EquipmentModel\":\"p3\",\"ProductionDate\":\"2016-04-26\",\"EquipmentType\":3,\"EquipmentStatus\":50,\"EquipmentPower\":3.3,\"NewNationalStandard\":1,\"ConnectorInfos\":[{\"ConnectorID\":\"1\",\"ConnectorName\":\"枪1\",\"ConnectorType\":1,\"VoltageUpperLimits\":220,\"VoltageLowerLimits\":220,\"Current\":15,\"Power\":3.3}]}]}}";

        //    ChargingStationMessage message = new ChargingStationMessage
        //    {
        //        OperatorID = "123456789",
        //        TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
        //        Data = SecurityHelper.EncryptAes(stationInfo, "1234567890abcdef", "1234567890abcdef"),
        //        Seq = "0001"
        //    };
        //    string sigContext = message.OperatorID + message.Data + message.TimeStamp + message.Seq;
        //    message.Sig = SecurityHelper.ToHMACMD5(sigContext, "1234567890abcdef");
        //    string str = message.ToJson();
        //    byte[] buffer = Encoding.UTF8.GetBytes(str);
        //    Stream stream = request.GetRequestStream();
        //    stream.Write(buffer, 0, buffer.Length);
        //    HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
        //    Stream responStream = respon.GetResponseStream();
        //    StreamReader reder = new StreamReader(responStream, Encoding.UTF8);
        //    string messageStr = reder.ReadToEnd();
        //    PushRespon pushrespong = JsonHelper.Convert<PushRespon>(messageStr);
        //    try
        //    {
        //        string statusStr = SecurityHelper.DecryptAes(pushrespong.Data, "1234567890abcdef", "1234567890abcdef");
        //        PushStatus status = JsonHelper.Convert<PushStatus>(statusStr);
        //    }catch(Exception ex){
            
        //    }
        //}

        // <summary>
        // 测试
        // 正常运营商异常
        // token异常(缺少，错误，过期)
        // 签名异常
        // 密钥异常
        // </summary>
        //static void push_connectorStatus_info()
        //{
        //    string tokenStr = "e9eae8dbe51b25a5d9ada51bddb51fb7";
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:6393/shevcs/v1/notification_stationStatus");
        //    request.Method = "Post";
        //    request.ContentType = "application/json;charset=utf-8";
        //    request.Headers.Add("Authorization", string.Format("Bearer {0}", tokenStr));

        //    string connectorStatusInfo = "{\"ConnectorStatusInfo\":{\"ConnectorID\":\"1\",\"Status\":4,\"CurrentA\":0,\"CurrentB\":0,\"CurrentC\":0,\"VoltageA\":0,\"VoltageB\":0,\"VoltageC\":0,\"ParkStatus\":10,\"LockStatus\":10,\"SOC\":10}}";

        //    ChargingStationMessage message = new ChargingStationMessage
        //    {
        //        OperatorID = "123456789",
        //        TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
        //        Data = SecurityHelper.EncryptAes(connectorStatusInfo, "1234567890abcdef", "1234567890abcdef"),
        //        Seq = "0001"
        //    };
        //    string sigContext = message.OperatorID + message.Data + message.TimeStamp + message.Seq;
        //    message.Sig = SecurityHelper.ToHMACMD5(sigContext, "1234567890abcdef");
        //    string str = message.ToJson();
        //    byte[] buffer = Encoding.UTF8.GetBytes(str);
        //    Stream stream = request.GetRequestStream();
        //    stream.Write(buffer, 0, buffer.Length);
        //    HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
        //    Stream responStream = respon.GetResponseStream();
        //    StreamReader reder = new StreamReader(responStream, Encoding.UTF8);
        //    string messageStr = reder.ReadToEnd();
        //    PushRespon pushrespong = JsonHelper.Convert<PushRespon>(messageStr);
        //    try
        //    {
        //        string statusStr = SecurityHelper.DecryptAes(pushrespong.Data, "1234567890abcdef", "1234567890abcdef");
        //        PushStatus status = JsonHelper.Convert<PushStatus>(statusStr);
        //    }
        //    catch (Exception ex) { 
                
        //    }
        //}

        // <summary>
        // 测试
        // 正常
        // token异常(缺少，错误，过期)
        // 密钥异常
        // 签名异常
        // </summary>
        //static void push_order_info()
        //{
        //    string tokenStr = "e9eae8dbe51b25a5d9ada51bddb51fb7";
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost:6393/shevcs/v1/notification_orderInfo");
        //    request.Method = "Post";
        //    request.ContentType = "application/json;charset=utf-8";
        //    request.Headers.Add("Authorization", string.Format("Bearer {0}", tokenStr));

        //    string orderInfo = "{\"OrderInfo\":{\"OperatorID\":\"123456789\",\"ConnectorID\":\"1\",\"StartChargeSeq\":\"111111111201608091000000002\",\"UserChargeType\":1,\"MobileNumber\":13800138000,\"Money\":20.80,\"ElectMoney\":10.80,\"ServiceMoney\":10.00,\"Elect\":5.8,\"CuspElect\":0,\"CuspElectPrice\":0,\"CuspServicePrice\":0,\"CuspMoney\":0,\"CuspElectMoney\":0,\"CuspServiceMoney\":0,\"PeakElect\":0,\"PeakElectPrice\":0,\"PeakServicePrice\":0,\"PeakMoney\":0,\"PeakElectMoney\":0,\"PeakServiceMoney\":0,\"FlatElect\":0,\"FlatElectPrice\":0,\"FlatServicePrice\":0,\"FlatMoney\":0,\"FlatElectMoney\":0,\"FlatServiceMoney\":0,\"ValleyElect\":0,\"ValleyElectPrice\":0,\"ValleyServicePrice\":0,\"ValleyMoney\":0,\"ValleyElectMoney\":0,\"ValleyServiceMoney\":0,\"StartTime\":\"2016-08-09 10:00:00\",\"EndTime\":\"2016-08-09 11:00:00\",\"PaymentAmount\":\"20.80\",\"PayTime\":\"2016-08-09 11:05:58\",\"PayChannel\":1,\"DiscountInfo\":\"无\"}}";

        //    ChargingStationMessage message = new ChargingStationMessage
        //    {
        //        OperatorID = "123456789",
        //        TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
        //        Data = SecurityHelper.EncryptAes(orderInfo, "1234567890abcdef", "1234567890abcdef"),
        //        Seq = "0001"
        //    };
        //    string sigContext = message.OperatorID + message.Data + message.TimeStamp + message.Seq;
        //    message.Sig = SecurityHelper.ToHMACMD5(sigContext, "1234567890abcdef");
        //    string str = message.ToJson();
        //    byte[] buffer = Encoding.UTF8.GetBytes(str);
        //    Stream stream = request.GetRequestStream();
        //    stream.Write(buffer, 0, buffer.Length);
        //    HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
        //    Stream responStream = respon.GetResponseStream();
        //    StreamReader reder = new StreamReader(responStream, Encoding.UTF8);
        //    string messageStr = reder.ReadToEnd();
        //    PushRespon pushrespong = JsonHelper.Convert<PushRespon>(messageStr);
        //    try
        //    {
        //        string statusStr = SecurityHelper.DecryptAes(pushrespong.Data, "1234567890abcdef", "1234567890abcdef");
        //        PushStatus status = JsonHelper.Convert<PushStatus>(statusStr);
        //    }
        //    catch (Exception ex) { 
                
        //    }
        //}
        //#endregion

        //#region 测试查询方法
        //public static Token query_token()
        //{
        //    string url = "http://cn.ttbems.cn:8081/shevcs/v1/query_token";
        //    string key = "1234567890abcdef";
        //    string iv = "1234567890abcdef";
        //    string operatorID = "332670086";
        //    string operatorSecret = "1234567890abcdef";
        //    Token result= query.query_token(url,key,iv,operatorID,operatorSecret);
        //    return result;
        //}

        //public static QSIResult query_station_info()
        //{
        //    string url="";
        //    string key="";
        //    string iv="";
        //    string operatorID="";
        //    string operatorSecret="";
        //    string sigSecret = "";
        //    QueryStationInfo querystation=new QueryStationInfo(){
                
        //    };
        //    QSIResult result = query.queryInfo<QueryStationInfo, QSIResult>(url, querystation, key, iv, operatorID, operatorSecret, sigSecret);
        //    return result;
        //}

        //public static QSStatusResult query_station_status()
        //{
        //    string url = "";
        //    string key = "";
        //    string iv = "";
        //    string operatorID = "";
        //    string sigSecret = "";
        //    string operatorSecret = "";
        //    QueryStationStatus stationstatus = new QueryStationStatus()
        //    {

        //    };
        //    QSStatusResult result = query.queryInfo<QueryStationStatus, QSStatusResult>(url, stationstatus, key, iv, operatorID, operatorSecret, sigSecret);
        //    return result;
        //}

        //public static QSStatsResult query_station_stats()
        //{
        //    string url = "";
        //    string key = "";
        //    string iv = "";
        //    string sigSecret = "";
        //    string operatorID = "";
        //    string operatorSecret = "";
        //    QueryStationStats stationstats = new QueryStationStats()
        //    {

        //    };
        //    QSStatsResult result = query.queryInfo<QueryStationStats, QSStatsResult>(url, stationstats, key, iv, operatorID, operatorSecret, sigSecret);
        //    return result;
        //}

        //#endregion
        
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
