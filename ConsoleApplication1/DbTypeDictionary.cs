using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ConsoleApplication1
{
    public class DbTypeDictionary
    {
        public Dictionary<SqlDbType, RuntimeTypeHandle> typedic = new Dictionary<SqlDbType, RuntimeTypeHandle> {
            { SqlDbType.Binary,typeof(byte[]).TypeHandle},
            { SqlDbType.Image,typeof(byte[]).TypeHandle},
            { SqlDbType.Timestamp,typeof(byte[]).TypeHandle},
            { SqlDbType.VarBinary,typeof(byte[]).TypeHandle},
            { SqlDbType.Char,typeof(string).TypeHandle},
            { SqlDbType.NChar,typeof(string).TypeHandle},
            { SqlDbType.NText,typeof(string).TypeHandle},
            { SqlDbType.NVarChar,typeof(string).TypeHandle},
            { SqlDbType.Text,typeof(string).TypeHandle},
            { SqlDbType.VarChar,typeof(string).TypeHandle},
            { SqlDbType.Xml,typeof(string).TypeHandle},
            { SqlDbType.DateTime,typeof(DateTime).TypeHandle},
            { SqlDbType.SmallDateTime,typeof(DateTime).TypeHandle},
            { SqlDbType.Date,typeof(DateTime).TypeHandle},
            { SqlDbType.Time,typeof(DateTime).TypeHandle},
            { SqlDbType.DateTime2,typeof(DateTime).TypeHandle},
            { SqlDbType.BigInt,typeof(long).TypeHandle},
            { SqlDbType.Bit,typeof(bool).TypeHandle},
            { SqlDbType.Decimal,typeof(decimal).TypeHandle},
            { SqlDbType.Money,typeof(decimal).TypeHandle},
            { SqlDbType.SmallMoney,typeof(decimal).TypeHandle},
            { SqlDbType.Float,typeof(double).TypeHandle},
            { SqlDbType.Int,typeof(int).TypeHandle},
            { SqlDbType.Real,typeof(float).TypeHandle},
            { SqlDbType.UniqueIdentifier,typeof(Guid).TypeHandle},
            { SqlDbType.SmallInt,typeof(short).TypeHandle},
            { SqlDbType.TinyInt,typeof(byte).TypeHandle},
            { SqlDbType.Variant,typeof(object).TypeHandle},
            { SqlDbType.Udt,typeof(object).TypeHandle},
            { SqlDbType.Structured,typeof(DataTable).TypeHandle},
            { SqlDbType.DateTimeOffset,typeof(DateTimeOffset).TypeHandle}
        };
    }
}
