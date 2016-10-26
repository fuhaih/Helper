using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 深拷贝,需要标记[Serializable]特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T t)
        {
            return t.BinarySerialize().BinaryDeserializeTo<T>(); 
        }
    }
}
