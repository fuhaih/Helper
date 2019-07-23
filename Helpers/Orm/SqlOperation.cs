using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Orm
{
    public static class SqlOperation
    {
        public static string ToSqlOperation(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Add: return "+";
                case ExpressionType.AddAssign: return "+=";
                case ExpressionType.And: return "and";
                case ExpressionType.AndAlso:return "and";
                case ExpressionType.Or: return "or";
                case ExpressionType.OrElse: return "or";
                case ExpressionType.Equal: return "=";
                default: throw new Exception("异常");
            }
        }

        public static object GetMemberValue(this MemberInfo member, object value)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property: {
                        PropertyInfo info = value.GetType().GetProperty(member.Name);
                        value = info.GetValue(value);
                    }
                    break;
                case MemberTypes.Field: {
                        FieldInfo info = value.GetType().GetField(member.Name);
                        value = info.GetValue(value);
                    }
                    break;
                case MemberTypes.Method: {
                        MethodInfo info = value.GetType().GetMethod(member.Name);
                        value = info.Invoke(value, null);
                    }break;
                default:break;
            }
            return value;
        }

        public static object GetValue(this Expression expression,object value)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        return ((MemberExpression)expression).Member.GetMemberValue(value);
                    }
                case ExpressionType.Call:
                    {
                        return ((MethodCallExpression)expression).Method.Invoke(value,new object[] { "test1","test2" });
                    }
                default:return value;
            }
        }


    }
}
