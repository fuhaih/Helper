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
                Type colType = pro.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {

                    colType = colType.GetGenericArguments()[0];

                }
                dt.Columns.Add(columnName, colType);
            }
            IEnumerator<T> col = collection.GetEnumerator();
            while (col.MoveNext())
            {
                T obj = col.Current;
                PropertyInfo[] tpros = obj.GetType().GetProperties();
                List<object> objs = new List<object>();
                foreach (PropertyInfo pro in pros)
                {
                    object value = pro.GetValue(obj, null);
                    objs.Add(value == null ? DBNull.Value : value);
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
        /// <summary>
        /// 获取最大值所在元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T MaxOne<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func) where TResult : IComparable
        {
            return collection.CompareOne(func);
        }
        /// <summary>
        /// 获取最大值所在元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取最大值，最大值有多个元素时返回多个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> MaxMany<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func) where TResult : IComparable
        {
            return collection.CompareMany(func);
        }
        /// <summary>
        /// 获取最小值，最小值有多个时返回多个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> MinMany<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func) where TResult : IComparable
        {
            return collection.CompareMany(func, false);
        }

        private static IEnumerable<T> CompareMany<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> func, bool desc = true) where TResult : IComparable
        {
            Func<bool, bool> compareResult = desc ? new Func<bool, bool>(bl => bl) : new Func<bool, bool>(bl => !bl);

            TResult tResult = default(TResult);
            bool flag = false;
            foreach (T current in collection)
            {
                if (flag)
                {
                    if (compareResult(func(current).CompareTo(tResult) > 0))
                    {
                        tResult = func(current);
                    }
                }
                else
                {
                    tResult = func(current);
                    flag = true;
                }
            }
            if (!flag)
            {
                throw new InvalidOperationException("NoElements");
            }
            foreach (T current in collection)
            {
                if (compareResult(func(current).CompareTo(tResult) == 0))
                {
                    yield return current;
                }
            }
        }

        /// <summary>
        /// 构建树形结构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trees"></param>
        /// <param name="ChildMember">子序列</param>
        /// <param name="ValueMember">标识</param>
        /// <param name="ParentMember">父标识</param>
        /// <returns></returns>
        public static List<T> GetTree<T>(this List<T> trees, Expression<Func<T, List<T>>> ChildMember, Func<T, object> ValueMember, Func<T, object> ParentMember)
        {
            MemberExpression body = (MemberExpression)ChildMember.Body;
            if (body == null)
            {
                throw new ArgumentOutOfRangeException("ChildMember");
            }
            ParameterExpression input = Expression.Parameter(typeof(T), "input");
            ParameterExpression setvalue = Expression.Parameter(typeof(List<T>), "setvalue");
            MemberExpression member = Expression.MakeMemberAccess(input, body.Member);
            Expression assignExpr = Expression.Assign(
                member,
                setvalue
            );
            Action<T, List<T>> SetChild = Expression.Lambda<Action<T, List<T>>>(assignExpr, input, setvalue).Compile();

            if (trees.Count == 0) return trees;

            Dictionary<object, List<T>> Childs = new Dictionary<object, List<T>>();
            Queue<T> parents = new Queue<T>();

            List<T> root = new List<T>();
            foreach (T node in trees)
            {
                object value = ValueMember(node);
                object parent = ParentMember(node);
                if (value.Equals(parent))
                {
                    root.Add(node);
                    parents.Enqueue(node);
                }
                else
                {
                    List<T> childs;
                    Childs.TryGetValue(parent, out childs);
                    if (childs == null)
                    {
                        childs = new List<T>();
                        Childs.Add(parent, childs);
                    }
                    childs.Add(node);
                }
            }
            while (parents.Count > 0)
            {
                T item = parents.Dequeue();
                object value = ValueMember(item);
                List<T> childs;
                Childs.TryGetValue(value, out childs);
                if (childs != null)
                {
                    SetChild(item, childs.Select(m => m).ToList());
                    Childs.Remove(value);
                    childs.ForEach(c => parents.Enqueue(c));
                }
            }
            return root;
        }


        private static IEnumerable<TSource> UnionIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            Set<TSource> set = new Set<TSource>(comparer);
            foreach (TSource current in first)
            {
                if (set.Add(current))
                {
                    yield return current;
                }
            }
            IEnumerator<TSource> enumerator = null;
            foreach (TSource current2 in second)
            {
                if (set.Add(current2))
                {
                    yield return current2;
                }
            }
            enumerator = null;
            yield break;
            yield break;
        }
    }

    internal class Set<TElement>
    {
        internal struct Slot
        {
            internal int hashCode;

            internal TElement value;

            internal int next;
        }

        private int[] buckets;

        private Set<TElement>.Slot[] slots;

        private int count;

        private int freeList;

        private IEqualityComparer<TElement> comparer;

        public Set() : this(null)
        {
        }

        public Set(IEqualityComparer<TElement> comparer)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TElement>.Default;
            }
            this.comparer = comparer;
            this.buckets = new int[7];
            this.slots = new Set<TElement>.Slot[7];
            this.freeList = -1;
        }

        public bool Add(TElement value)
        {
            return !this.Find(value, true);
        }

        public bool Contains(TElement value)
        {
            return this.Find(value, false);
        }

        public bool Remove(TElement value)
        {
            int num = this.InternalGetHashCode(value);
            int num2 = num % this.buckets.Length;
            int num3 = -1;
            for (int i = this.buckets[num2] - 1; i >= 0; i = this.slots[i].next)
            {
                if (this.slots[i].hashCode == num && this.comparer.Equals(this.slots[i].value, value))
                {
                    if (num3 < 0)
                    {
                        this.buckets[num2] = this.slots[i].next + 1;
                    }
                    else
                    {
                        this.slots[num3].next = this.slots[i].next;
                    }
                    this.slots[i].hashCode = -1;
                    this.slots[i].value = default(TElement);
                    this.slots[i].next = this.freeList;
                    this.freeList = i;
                    return true;
                }
                num3 = i;
            }
            return false;
        }

        private bool Find(TElement value, bool add)
        {
            int num = this.InternalGetHashCode(value);
            for (int i = this.buckets[num % this.buckets.Length] - 1; i >= 0; i = this.slots[i].next)
            {
                if (this.slots[i].hashCode == num && this.comparer.Equals(this.slots[i].value, value))
                {
                    return true;
                }
            }
            if (add)
            {
                int num2;
                if (this.freeList >= 0)
                {
                    num2 = this.freeList;
                    this.freeList = this.slots[num2].next;
                }
                else
                {
                    if (this.count == this.slots.Length)
                    {
                        this.Resize();
                    }
                    num2 = this.count;
                    this.count++;
                }
                int num3 = num % this.buckets.Length;
                this.slots[num2].hashCode = num;
                this.slots[num2].value = value;
                this.slots[num2].next = this.buckets[num3] - 1;
                this.buckets[num3] = num2 + 1;
            }
            return false;
        }

        private void Resize()
        {
            int num = checked(this.count * 2 + 1);
            int[] array = new int[num];
            Set<TElement>.Slot[] array2 = new Set<TElement>.Slot[num];
            Array.Copy(this.slots, 0, array2, 0, this.count);
            for (int i = 0; i < this.count; i++)
            {
                int num2 = array2[i].hashCode % num;
                array2[i].next = array[num2] - 1;
                array[num2] = i + 1;
            }
            this.buckets = array;
            this.slots = array2;
        }

        internal int InternalGetHashCode(TElement value)
        {
            if (value != null)
            {
                return this.comparer.GetHashCode(value) & 2147483647;
            }
            return 0;
        }
    }
}
