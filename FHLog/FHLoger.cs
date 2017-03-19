using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
namespace FHLog
{
    public class FHLoger
    {
        private delegate void writerAsync(LogType type, string message);

        private static object logFileLock=new object();

        private static object colorChangeLock = new object();

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
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志信息</param>
        public  static void Write(LogType type, string message)
        {
            writerAsync writer = new writerAsync(WriteLog);
            writer.BeginInvoke(type, message, null, null);
        }

        /// <summary>
        /// 记录日志异步方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private static void WriteLog(LogType type, string message)
        {
            string info = string.Format(logFormat, type.ToString(), DateTime.Now, message);
            lock (colorChangeLock)
            {
                Console.ForegroundColor = (ConsoleColor)type;
                Console.WriteLine(info);
                Console.ForegroundColor = ConsoleColor.White;
            }
            WriteLog(info);
        }

        /// <summary>
        /// 记录日志到本地（iocp）
        /// </summary>
        /// <param name="message">日志信息</param>
        private static void WriteLog(string message)
        {
            lock(logFileLock)
            {
                //using (FileStream stream = new FileStream(logPath, FileMode.Open, FileAccess.ReadWrite))
                //{
                //    long length = stream.Length;
                //}
                using (StreamWriter writer = new StreamWriter(logPath, true, Encoding.UTF8))
                {
                    writer.WriteLine(message);
                }
            } 
        }
    }
}
