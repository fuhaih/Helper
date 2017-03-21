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
        private delegate void writerAsync(LogType type, string message);

        private static ConcurrentQueue<LogInfo> logInfo = new ConcurrentQueue<LogInfo>();

        private static LogSetting logSet=new LogSetting();

        private const string logFormat = "[{0}]-{1}\r\n{2}";
    
        static FHLoger()
        {
            ThreadPool.QueueUserWorkItem(WriteLog);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志信息</param>
        public  static void Write(LogType type, string message)
        {
            writerAsync writer = new writerAsync(WriteLogAsync);
            writer.BeginInvoke(type, message, null, null);
        }

        /// <summary>
        /// 记录日志异步方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private static void WriteLogAsync(LogType type, string message)
        {
            string info = string.Format(logFormat, type.ToString(), DateTime.Now, message);
            logInfo.Enqueue(new LogInfo {
                Type = type,
                Info=info
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
                        Console.ForegroundColor = (ConsoleColor)log.Type;
                        Console.WriteLine(log.Info);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        using (StreamWriter writer = new StreamWriter(logSet.FullName, true, Encoding.UTF8))
                        {
                            writer.WriteLine(log.Info);
                        }
                    }catch{
                        continue;
                    }
                }
                Thread.Sleep(3000);
            }
        }

        private static void sockSend()
        { 
        
        }
    }

    public class LogSetting 
    {
        public string ProjectName{get;set;}
        public string FullName {
            get {
                return Path.Combine(LogPath, FileName);
            }
        }
        public string FileName {
            get {
                return  LogName+logExtension;
            }
        }
        public  string logName="";
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
        public string logPath = "";
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
        public string logExtension = ".log";
        /// <summary>
        /// 日志后缀名
        /// </summary>
        /// <summary>
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

        public LogSetting()
        {
            string fullname = Assembly.GetEntryAssembly().FullName;
            ProjectName=logName = fullname.Split(',')[0]; 
            InitialSetting();
               
        }

        public string GetProjectName()
        {
            return "";
        }

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
