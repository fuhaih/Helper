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
        /// <summary>
        /// 获取异常详细信息(包括方法名，异常信息等)
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetMessageDetail(this Exception ex)
        {
               return GetMessageDetail(ex, 0);
        }
        /// <summary>
        /// 获取异常详细信息(包括方法名，异常信息等)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="deep">异常堆栈深度，0为最里层异常信息，也就是抛出异常的地方，以此类推</param>
        /// <returns></returns>
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
        /// <summary>
        /// 把错误信息写到程序基目录中
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="deep">深度，1指当前函数信息，2指调用当前函数的函数信息，以此类推</param>
        public static void WriteMessageToLocal(this Exception ex, int deep)
        {
            string basepath = AppDomain.CurrentDomain.BaseDirectory;
        
            /*Works differently on different os versions and/or different .NET versions
            最好不用‘+’连接地址，用Path.Combine，不然在不同版本的系统或环境中容易出错*/
            string path =Path.Combine( basepath,"errorInfo.txt");
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
