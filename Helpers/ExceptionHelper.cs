using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Helpers
{
    public static class ExceptionHelper
    {
        public static string GetMessageDetail(this Exception ex)
        {
               return GetMessageDetail(ex, 0);
        }

        public static string GetMessageDetail(this Exception ex, int deep)
        {
            string result="";
            string msg = ex.StackTrace;
            Regex reg = new Regex(@"在(.*?)位置.*?行号 (\d*)");
            MatchCollection collection = reg.Matches(msg);
            if (collection.Count < deep)
            {
                result = ex.Message;
                goto end;
            }
            string mehordName = collection[deep].Groups[1].Value;
            string lineNum = collection[deep].Groups[2].Value;
            result = string.Format("在方法{0}中出现错误\r\n错误信息：{1}\r\n行号：{2}", mehordName, ex.Message, lineNum);
            end:
            return result;
        }

        public static void WriteMessageToLocal(this Exception ex, int deep)
        {
            string basepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = basepath + "errorInfo.txt";
            StackTrace st = new StackTrace(true);
            string methodName = st.GetFrame(deep).GetMethod().Name;
            string fileName = st.GetFrame(deep).GetFileName();
            int line = st.GetFrame(deep).GetFileLineNumber();
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine("发生错误：" + DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒"));
                sw.WriteLine("文件：" + fileName);
                sw.WriteLine("方法：" + methodName);
                sw.WriteLine("行号：" + line);
                sw.WriteLine("错误信息：" + ex.Message);
                sw.WriteLine("----------------------------------------------------------------");
            }        
        }

    }
}
