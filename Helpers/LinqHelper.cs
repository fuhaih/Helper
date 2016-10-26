using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
namespace Helpers
{
    public static class LinqHelper
    {
        /// <summary>
        /// 匿名类集合转换为table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable AnonymousCopyToTable<T>(this IEnumerable<T> collection)
        {
            if (collection.Count() == 0)
            {
                throw new NullReferenceException("序列为空");
            }
            DataTable dt = new DataTable();
            PropertyInfo[] pros = collection.FirstOrDefault().GetType().GetProperties();
            foreach (var pro in pros)
            {
                string columnName = pro.Name;
                dt.Columns.Add(columnName,pro.PropertyType);
            }
            IEnumerator<T> col = collection.GetEnumerator();
            while (col.MoveNext())
            {
                T obj = col.Current;
                PropertyInfo[] tpros = obj.GetType().GetProperties();
                List<object> objs = new List<object>();
                foreach (PropertyInfo pro in pros)
                {
                    objs.Add(pro.GetValue(obj,null));
                }
                dt.Rows.Add(objs.ToArray());
            }
            return dt;
        }

        public static DataTable AnonymousCopyToTable<T>(this IEnumerable<T> collection, string[] Captions)
        {
            DataTable result = collection.AnonymousCopyToTable();
            for (int i = 0; i < result.Columns.Count; i++)
            {
                result.Columns[i].Caption = Captions[i];
            }
            return result;
        }

        public static T RandomEnumerableValue<T>(this IEnumerable<T> source, Random random)
        {
            if (source is ICollection)
            {
                ICollection collection = source as ICollection;
                int count = collection.Count;
                if (count == 0)
                {
                    throw new Exception("IEnumerable没有数据");
                }
                int index = random.Next(count);
                return source.ElementAt(index);
            }
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw new Exception("IEnumerable没有数据");
                }
                int count = 1;
                T current = iterator.Current;
                while (iterator.MoveNext())
                {
                    count++;
                    if (random.Next(count) == 0)
                        current = iterator.Current;
                }
                return current;
            }
        }
    }
}
