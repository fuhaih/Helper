using System.Reflection;
using System.IO;
using System.Text;
namespace FHLog
{
    public class FHLogManager
    {
        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <returns></returns>
        public static string GetProjectName()
        {
            string name = Assembly.GetEntryAssembly().FullName;
            return name.Split(',')[0];
        }
        /// <summary>
        /// 记录日志到本地（iocp）
        /// </summary>
        public static void WriteLog(string message)
        {
            string path = string.Format("{0}.log",GetProjectName());
            using (StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                writer.WriteLine(message);
            }
        }
    }
}
