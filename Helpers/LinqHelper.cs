using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Linq.Expressions;
using Helpers.Orm;

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
        /// <summary>
        /// 匿名类集合转换为table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="Captions">table的Captions（一一对应，否则报错）</param>
        /// <returns></returns>
        public static DataTable AnonymousCopyToTable<T>(this IEnumerable<T> collection, string[] Captions)
        {
            DataTable result = collection.AnonymousCopyToTable();
            for (int i = 0; i < result.Columns.Count; i++)
            {
                result.Columns[i].Caption = Captions[i];
            }
            return result;
        }
        /// <summary>
        /// 随机获取集合的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random">多次随机时需要传入相同random</param>
        /// <returns>返回集合的随机成员</returns>
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

        public static Func<TIn, TOut> GetMappingFunc<TIn, TOut>(this TIn objin)
        {
            ParameterExpression input = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindings = new List<MemberBinding>();

            //绑定属性
            PropertyInfo[] outPropertyInfos = typeof(TOut).GetProperties();
            foreach (var prop in outPropertyInfos)
            {
                PropertyInfo inPropertyInfo = typeof(TIn).GetProperty(prop.Name);
                if (inPropertyInfo != null)
                {
                    MemberExpression property = Expression.Property(input, inPropertyInfo);
                    MemberBinding memberBinding = Expression.Bind(prop, property);
                    memberBindings.Add(memberBinding);
                }
            }

            //绑定字段
            FieldInfo[] outFieldInfos = typeof(TOut).GetFields();
            foreach (var field in outFieldInfos)
            {
                FieldInfo inFieldInfo = typeof(TIn).GetField(field.Name);
                if (inFieldInfo != null)
                {
                    MemberExpression fieldInfo = Expression.Field(input, inFieldInfo);
                    MemberBinding memberBinding = Expression.Bind(field, fieldInfo);
                    memberBindings.Add(memberBinding);
                }
            }

            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindings.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(init, input);
            Func<TIn, TOut> func = lambda.Compile();
            return func;
        }

        /// <summary>
        /// 把一个类型的对象映射到另一个类型中
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="objin"></param>
        /// <returns></returns>
        public static TOut MappingTo<TIn, TOut>(this TIn objin)
        {
            //也可以直接通过反射来完成
            //通过lambda来实现的优点:
            //lambda可以编译成Func<>,然后反复使用，无需重复构造。
            Func<TIn, TOut> func = objin.GetMappingFunc<TIn, TOut>();
            return func.Invoke(objin);
        }

        public static string Query<T>(this T obj, Expression<Func<T, bool>> filter)
        {
            CustomVisitor visitor = new CustomVisitor();
            Expression epress = visitor.Visit(filter);
            return visitor.ToSqlCommand();
        }

        public static T MaxOne<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func) where TResult : IComparable
        {
            return collection.CompareOne(func);
        }

        public static T MinOne<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func) where TResult : IComparable
        {
            return collection.CompareOne(func, false);
        }

        private static T CompareOne<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func, bool desc = true) where TResult : IComparable
        {
            /**结果表达式
             * compareResult 结果表达式
             * desc=true : bl => bl;返回原比较结果，也就能取出最大值
             * desc=false : bl => ！bl;返回原比较结果的非，也就能取出最小值
             */
            Func<bool, bool> compareResult = desc ? new Func<bool, bool>(bl => bl) : new Func<bool, bool>(bl => !bl);

            T tSource = default(T);
            TResult tResult = default(TResult);
            bool flag = false;
            foreach (T current in collection)
            {
                if (flag)
                {
                    if (compareResult(func(current).CompareTo(tResult) > 0))
                    {
                        tSource = current;
                        tResult = func(tSource);
                    }
                }
                else
                {
                    tSource = current;
                    tResult = func(tSource);
                    flag = true;
                }
            }
            if (flag)
            {
                return tSource;
            }
            throw new InvalidOperationException("NoElements");
        }

    }
}
