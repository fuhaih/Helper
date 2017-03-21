using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
namespace FHLog
{
    public class FHLoger
    {
        private delegate void writerAsync(LogType type, string message);

        private delegate void writeToLocalAsync(LogInfo log);

        private static ConcurrentQueue<LogInfo> logInfo = new ConcurrentQueue<LogInfo>();

        private static string logPath;

        private const string logFormat = "[{0}]-{1}\r\n{2}";

        private static string ProjectName
        {
            get
            {
                string name = Assembly.GetEntryAssembly().FullName;
                return name.Split(',')[0];
            }
        }

        static FHLoger()
        {
            logPath = string.Format("{0}.log", ProjectName);
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
                    Console.ForegroundColor = (ConsoleColor)log.Type;
                    Console.WriteLine(log.Info);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    using (StreamWriter writer = new StreamWriter(logPath, true, Encoding.UTF8))
                    {
                        writer.WriteLine(log.Info);
                    }
                }
                Thread.Sleep(3000);
            }
        }
    }
}
