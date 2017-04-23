using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Configuration;
namespace FHLog
{
    public class FHLoger
    {
        /// <summary>
        /// 写者，负责把队列中的日志信息打印到控制台并写进日志文件中
        /// </summary>
        private static Task writer;

        private static object FileLock = new object();

        private static object ConsoleLock = new object();

        /// <summary>
        /// 日志消息队列
        /// </summary>
        private static ConcurrentQueue<LogInfo> logInfo1 = new ConcurrentQueue<LogInfo>();

        private static ConcurrentQueue<LogInfo> logInfo2 = new ConcurrentQueue<LogInfo>();

        private static ConcurrentQueue<LogInfo> writeInfo = new ConcurrentQueue<LogInfo>();

        private static ConcurrentQueue<LogInfo> readInfo = new ConcurrentQueue<LogInfo>();

        /// <summary>
        /// 日志配置信息
        /// </summary>
        private static LogSetting logSet=new LogSetting();

        private static LogFormat format = new LogFormat();

        /// <summary>
        /// 日志信息格式
        /// </summary>
        public static LogFormat Format
        {
            get { return FHLoger.format; }
            set { FHLoger.format = value; }
        }

        private static LogAction action = new LogAction();

        /// <summary>
        /// 一些操作的配置
        /// </summary>
        public static LogAction Action
        {
            get { return FHLoger.action; }
            set { FHLoger.action = value; }
        }

        static FHLoger()
        {
            writer = new Task(WriteLog, null);
            writer.Start();
            writeInfo = logInfo1;
            readInfo = logInfo2;
            //TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(TaskError);
            //ThreadPool.QueueUserWorkItem(GCCollect);
        }

        public static void Info(string message)
        {
            writeInfo.Enqueue(new LogInfo
            {
                Type = LogType.Info,
                Info = message,
                Format = Format.Info,
                Time=DateTime.Now
            });
        }

        public static void Warn(string message)
        {
            writeInfo.Enqueue(new LogInfo
            {
                Type = LogType.Warn,
                Info = message,
                Format = Format.Warn,
                Time = DateTime.Now
            });
        }

        public static void Error(string message)
        {
            writeInfo.Enqueue(new LogInfo
            {
                Type = LogType.Error,
                Info = message,
                Format = Format.Error,
                Time = DateTime.Now
            });
        }

        public static void Fatal(string message)
        {
            writeInfo.Enqueue(new LogInfo
            {
                Type = LogType.Fatal,
                Info = message,
                Format = Format.Fatal,
                Time = DateTime.Now
            });
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="sender"></param>
        private static void WriteLog(object sender)
        {
            while (true)
            {
                List<LogInfo> infos = new List<LogInfo>();
                LogInfo log ;
                while (readInfo.TryDequeue(out log))
                {
                    LogInfo info = log;
                    infos.Add(info);
                }
                LogInfo[][] writeInfos = infos.Split(200);
                foreach (LogInfo[] item in writeInfos)
                {
                    WriteLogToLocal(item);
                    if (Action.Output.IsOpenConsole)
                    {
                        WriteLogToConsole(item);
                    }
                }
                Thread.Sleep(100);
                if (Interlocked.Equals(writeInfo, logInfo1))
                {
                    Interlocked.Exchange<ConcurrentQueue<LogInfo>>(ref writeInfo, logInfo2);
                    Interlocked.Exchange<ConcurrentQueue<LogInfo>>(ref readInfo, logInfo1);
                }
                else {
                    Interlocked.Exchange<ConcurrentQueue<LogInfo>>(ref writeInfo, logInfo1);
                    Interlocked.Exchange<ConcurrentQueue<LogInfo>>(ref readInfo, logInfo2);
                }
            }
        }

        /// <summary>
        /// 把日志写到本地
        /// </summary>
        /// <param name="sender"></param>
        private static void WriteLogToLocal(LogInfo[] logs)
        {
            action.Rolling.Roll(logSet);
            using (StreamWriter writer = new StreamWriter(logSet.FullName, true, Encoding.UTF8))
            {
                string[] strs = logs.Select(m => m.ToString()).ToArray();
                string writeText = string.Join("\r\n", strs);
                writer.WriteLine(writeText);
            }
        }

        /// <summary>
        /// 把日志输出到控制台
        /// </summary>
        private static void WriteLogToConsole(LogInfo[] logs)
        {
            foreach (LogInfo log in logs)
            {
                Console.ForegroundColor = log.Format.Color;
                Console.WriteLine(log.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        /// <summary>
        /// 日志信息发送到服务端
        /// </summary>
        private static void SockSend()
        { 
        
        }

        private static void GCCollect(object sender)
        { 
            while(true){
                GC.Collect();
                if (writer == null||writer.Status==TaskStatus.Faulted)
                {
                    foreach (var ex in writer.Exception.InnerExceptions)
                    {
                        writeInfo.Enqueue(new LogInfo { 
                            Type=LogType.Error,
                            Info=string.Format("writer线程异常\r\n{0}\r\n已重启",ex.Message)
                        });
                    }
                    writer = new Task(WriteLog, null);
                    writer.Start();
                }
                Thread.Sleep(3000);
            }
        }
    }

    public class LogSetting 
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName{get;set;}

        /// <summary>
        /// 日志全名
        /// </summary>
        public string FullName {
            get {
                return Path.Combine(LogPath, FileName);
            }
        }

        /// <summary>
        /// 日志文件名称
        /// </summary>
        public string FileName {
            get {
                return  LogName+logExtension;
            }
        }

        private string logName = "";

        /// <summary>
        /// 日志名称
        /// </summary>
        [LogSet("LogName")]
        public string LogName
        {
            get {
                return logName;
            }
            set {
                logName = value;
            }
        }

        private string logPath = "";

        /// <summary>
        /// 日志路径
        /// </summary>
        [LogSet("LogPath")]
        public string LogPath
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
            }
        }

        private string logExtension = ".log";

        /// <summary>
        /// 日志后缀.*
        /// </summary>
        [LogSet("LogExtension")]
        public string LogExtension
        {
            get{
                return logExtension;
            }set{
                logExtension=value;
            }
        }

        private AppSettingsReader setReader = new AppSettingsReader();
        
        private DateTime time = DateTime.Now;

        /// <summary>
        /// 日志轮训时间
        /// </summary>
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public LogSetting()
        {
            string fullname = Assembly.GetEntryAssembly().FullName;
            ProjectName=logName = fullname.Split(',')[0]; 
            InitialSetting();
               
        }

        /// <summary>
        /// 初始化设置项
        /// </summary>
        private void InitialSetting()
        {
            Type setType = this.GetType();
            PropertyInfo[] propertys = setType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pro in propertys)
            {
                try
                {
                    object[] attrs = pro.GetCustomAttributes(typeof(LogSet),false);
                    if(attrs.Length==0) continue;
                    string Name = attrs[0].ToString();
                    object Value = setReader.GetValue(Name, typeof(String));
                    pro.SetValue(this, Value, null);
                }
                catch
                {
                    continue;
                }
            }
        }
    }

    public class LogSet : Attribute
    {
        /// <summary>
        /// 对应配置名称
        /// </summary>
        public string Name { get; set; }
        public LogSet(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public static class LogExtend
    {
        /// <summary>
        /// 数组拆分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="partLength">每组数组的长度</param>
        /// <returns>差分后的交错数组</returns>
        public static T[][] Split<T>(this IEnumerable<T> t, int partLength)
        {
            if (t.Count() == 0)
            {
                return new T[0][];
            }
            List<T> buffer = t.ToList();
            int length = t.Count();
            int count = (length - 1) / partLength + 1;
            int remainder = length % partLength;
            T[][] result = new T[count][];
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    result[i] = new T[remainder == 0 ? partLength : remainder];
                    buffer.CopyTo(i * partLength, result[i], 0, remainder == 0 ? partLength : remainder);
                }
                else
                {
                    result[i] = new T[partLength];
                    buffer.CopyTo(i * partLength, result[i], 0, partLength);
                }
            }
            return result;
        }
    }
}
