using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Orm
{
    public class Converter
    {
        public static IEnumerable<T> Convert<T>(DataTable table)
        {
            var map = GetMapFunc<T>(table.Columns);
            foreach (DataRow row in table.Rows)
            {
                yield return map(row);
            }
        }

        public static IEnumerable<T> Excute<T>(string connectStr, string commandStr)
        {
            // List<T> result = new List<T>();
            using (SqlConnection con = new SqlConnection(connectStr))
            {
                con.Open();

                SqlCommand com = new SqlCommand(commandStr, con);
                SqlDataReader reader = com.ExecuteReader();
                Func<IDataReader, T> func = GetMapFunc<T>(reader);

                while (reader.Read())
                {
                    yield return func(reader);
                }
            }          
        }

        public async Task<IEnumerable<T>> Execute<T>(string connectStr, string commandStr, params SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectStr);
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(commandStr, connection);
            command.Parameters.Clear();
            foreach (var item in parameters)
            {
                command.Parameters.Add(item);
            }
            //CommandBehavior.CloseConnection 定义reader close的行为，当reader close时关闭连接connection
            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            var datas = ToList<T>(reader);
            return datas;
        }
        public static IEnumerable<T> ToList<T>(IDataReader dataReader, bool autoClose = true)
        {
            try
            {
                List<T> result = new List<T>();
                Func<IDataReader, T> func = GetMapFunc<T>(dataReader);
                while (dataReader.Read())
                {
                    result.Add(func(dataReader));
                }
                return result;
            }
            finally
            {
                if (autoClose)
                {
                    dataReader.Dispose();
                    dataReader.Close();
                }
            }

        }
        /**GetMapFunc得出的结果是
         * (row)=>new T{
         *     propname = row[propname]==system.dbnull.value?defult(T):convertot(row[propname])
         * }
         * 
         */

        private static Func<DataRow, T> GetMapFunc<T>(DataColumnCollection Columns)
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

        private static Func<IDataReader, T> GetMapFunc<T>(IDataReader dataReader)
        {
            var exps = new List<Expression>();

            var columnNames = Enumerable.Range(0, dataReader.FieldCount)
                                .Select(i => new { i, name = dataReader.GetName(i) });
#if DEBUG
            Type ttype = typeof(T);
            Debug.WriteLine(string.Format("模型{0}各个字段数据的类型", ttype.Name));
            DataTable table = dataReader.GetSchemaTable();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string name = System.Convert.ToString(table.Rows[i]["ColumnName"]);
                string type = System.Convert.ToString(table.Rows[i]["DataType"]);
                Debug.WriteLine(string.Format("{0} -- {1}", name, type));
            }
#endif
            var paramRow = Expression.Parameter(typeof(IDataReader), "row");
            var nullvalue = Expression.Constant(System.DBNull.Value);
            List<MemberBinding> memberBindings = new List<MemberBinding>();

            var indexerInfo = typeof(IDataRecord).GetProperty("Item", new[] { typeof(int) });
            foreach (var column in columnNames)
            {
                var outPropertyInfo = typeof(T).GetProperty(
                    column.name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (outPropertyInfo == null)
                    continue;
                var columnNameExp = Expression.Constant(column.i);
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
            Expression<Func<IDataReader, T>> lambda = Expression.Lambda<Func<IDataReader, T>>(init, paramRow);
            Func<IDataReader, T> func = lambda.Compile();
            return func;
        }
    }
}
