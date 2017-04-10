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

        /// <summary>
        /// 日志消息队列
        /// </summary>
        private static ConcurrentQueue<LogInfo> logInfo = new ConcurrentQueue<LogInfo>();

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

        public static string Test { get; set; }

        static FHLoger()
        {
            writer = new Task(WriteLog, null);
            writer.Start();
            //TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(TaskError);
            //ThreadPool.QueueUserWorkItem(GCCollect);
        }

        public static void Info(string message)
        {
            logInfo.Enqueue(new LogInfo
            {
                Type = LogType.Info,
                Info = message,
                Format = Format.Info,
                Time=DateTime.Now
            });
        }

        public static void Warn(string message)
        {
            logInfo.Enqueue(new LogInfo
            {
                Type = LogType.Warn,
                Info = message,
                Format = Format.Warn,
                Time = DateTime.Now
            });
        }

        public static void Error(string message)
        {
            logInfo.Enqueue(new LogInfo
            {
                Type = LogType.Error,
                Info = message,
                Format = Format.Error,
                Time = DateTime.Now
            });
        }

        public static void Fatal(string message)
        {
            logInfo.Enqueue(new LogInfo
            {
                Type = LogType.Fatal,
                Info = message,
                Format = Format.Fatal,
                Time = DateTime.Now
            });
        }

        /// <summary>
        /// 记录日志到本地
        /// </summary>
        /// <param name="sender"></param>
        private static void WriteLog(object sender)
        {
            while (true)
            {
                LogInfo log ;
                while (logInfo.TryDequeue(out log))
                {
                    try{
                        Console.ForegroundColor = log.Format.Color;
                        Console.WriteLine(log.ToString());
                        Console.ForegroundColor = ConsoleColor.Gray;
                        new Task(WriteLogToLocal, log).Start();
                    }catch{
                        continue;
                    }
                }
                Thread.Sleep(3000);
            }
        }

        private static void WriteLogToLocal(object sender)
        {
            LogInfo log = sender as LogInfo;
            lock (FileLock)
            {
                using (StreamWriter writer = new StreamWriter(logSet.FullName, true, Encoding.UTF8))
                {
                    long length = writer.BaseStream.Length;
                    if (length > logSet.MaxLength)
                    {
                        writer.Close();
                        string path = Path.Combine(logSet.LogPath, "LogBack");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string newFile = Path.Combine(path,  logSet.LogName+ DateTime.Now.ToString("yyyyMMddHHmmss")+logSet.LogExtension);
                        File.Move(logSet.FullName, newFile);
                    } 
                }
                using (StreamWriter writer = new StreamWriter(logSet.FullName, true, Encoding.UTF8))
                {
                    writer.WriteLine(log.ToString());
                }
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
                        logInfo.Enqueue(new LogInfo { 
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
        private static AppSettingsReader setReader = new AppSettingsReader();

        private long maxLength = 2*1024 * 1024;
        [LogSet("LogMaxLength")]
        public long MaxLength
        {
            get
            {
                return maxLength;
            }
            set
            {
                maxLength = value;
            }
        }

        public LogSetting()
        {
            string fullname = Assembly.GetEntryAssembly().FullName;
            ProjectName=logName = fullname.Split(',')[0]; 
            InitialSetting();
               
        }

        /// <summary>
        /// 获取当前项目名称
        /// </summary>
        /// <returns></returns>
        public string GetProjectName()
        {
            return "";
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


}
