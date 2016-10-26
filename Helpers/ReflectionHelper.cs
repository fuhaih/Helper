using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取调用该方法的方法的类的完全限定名
        /// </summary>
        /// <returns></returns>
        public static string CurrentTypeFullName()
        { 
            StackTrace st = new StackTrace(true);
            return st.GetFrame(1).GetMethod().ReflectedType.FullName;
        }

        /// <summary>
        /// 获取调用该方法的方法的类的命名空间
        /// </summary>
        /// <returns></returns>
        public static string CurrentTypeNamespace()
        {
            StackTrace st = new StackTrace(true);
            return st.GetFrame(1).GetMethod().ReflectedType.Namespace;
        }
    }
}
