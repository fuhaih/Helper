using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class TreeHelper
    {
        /** 用法：
         * List<TestTree> result = test.GetTree(m=>m.Childs, m => m.ID, m => m.ParentID);
         */


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

        /** 用法：
        * List<TestTree> result = test.GetTree((m,cl)=>m.Childs=cl, m => m.ID, m => m.ParentID);
        */

        /// <summary>
        /// 构建树形结构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trees"></param>
        /// <param name="ChildAssign">子序列操作</param>
        /// <param name="ValueMember">标识</param>
        /// <param name="ParentMember">父标识</param>
        /// <returns></returns>
        public static List<T> GetTree<T>(this List<T> trees, Action<T, List<T>> ChildAssign, Func<T, object> ValueMember, Func<T, object> ParentMember)
        {
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
                    ChildAssign(item, childs.Select(m => m).ToList());
                    Childs.Remove(value);
                    childs.ForEach(c => parents.Enqueue(c));
                }
            }
            return root;
        }
    }
}
