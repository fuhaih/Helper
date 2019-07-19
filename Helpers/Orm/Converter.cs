using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Orm
{
    public class Converter
    {
        public List<T> Convert<T>(DataTable table)
        {
            List<T> result = new List<T>();
            var map = GetMapFunc<T>(table.Columns);
            foreach (DataRow row in table.Rows)
            {
                result.Add(map(row));
            }
            return result;
        }
        /**GetMapFunc得出的结果是
         * (row)=>new T{
         *     propname = row[propname]==system.dbnull.value?defult(T):convertot(row[propname])
         * }
         * 
         */

        private Func<DataRow, T> GetMapFunc<T>(DataColumnCollection Columns)
        {
            var exps = new List<Expression>();

            var paramRow = Expression.Parameter(typeof(DataRow), "row");
            var nullvalue = Expression.Constant(System.DBNull.Value);
            List<MemberBinding> memberBindings = new List<MemberBinding>();
            var itemarray = typeof(DataRow).GetProperty("ItemArray");
            var indexerInfo = typeof(DataRow).GetProperty("Item", new[] { typeof(string) });
            foreach (DataColumn column in Columns)
            {
                var outPropertyInfo = typeof(T).GetProperty(
                    column.ColumnName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (outPropertyInfo == null)
                    continue;
                DataRow row = null;
                var columnNameExp = Expression.Constant(column.ColumnName);
                var propertyExp = Expression.MakeIndex(
                    paramRow,
                    indexerInfo, new[] { columnNameExp });
                var condition = Expression.Equal(propertyExp, nullvalue);
                var convertExp = Expression.Convert(propertyExp, outPropertyInfo.PropertyType);
                var setExp = Expression.Condition(condition, Expression.Default(outPropertyInfo.PropertyType), convertExp);
                MemberBinding memberBinding = Expression.Bind(outPropertyInfo, setExp);
                memberBindings.Add(memberBinding);
            }

            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), memberBindings.ToArray());
            Expression<Func<DataRow, T>> lambda = Expression.Lambda<Func<DataRow, T>>(init, paramRow);
            Func<DataRow, T> func = lambda.Compile();
            return func;
        }
    }
}
