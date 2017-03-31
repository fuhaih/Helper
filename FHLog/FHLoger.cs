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

        private delegate void queueAsync(LogType type, string message);

        /// <summary>
        /// 日志消息队列
        /// </summary>
        private static ConcurrentQueue<LogInfo> logInfo = new ConcurrentQueue<LogInfo>();

        /// <summary>
        /// 日志配置信息
        /// </summary>
        private static LogSetting logSet=new LogSetting();

        /// <summary>
        /// 日志信息格式
        /// </summary>
        private const string logFormat = "[{0}]-{1}\r\n{2}";
    
        static FHLoger()
        {
            writer = new Task(WriteLog, null);
            writer.Start();
            //TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(TaskError);
            //ThreadPool.QueueUserWorkItem(GCCollect);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志信息</param>
        public  static void Write(LogType type, string message)
        {
            queueAsync queue = new queueAsync(EnqueueAsync);
            queue.BeginInvoke(type, message, null, null);
        }

        /// <summary>
        /// 日志信息异步插入到队列中
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private static void EnqueueAsync(LogType type, string message)
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

        /// <summary>
        /// 日志信息发送到服务端
        /// </summary>
        private static void sockSend()
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
