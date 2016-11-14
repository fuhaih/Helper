using System;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;

namespace Helpers
{
    public static class  SerializeHelper
    {
        /*注意：
         * Binary序列化：需要标记[Serializable]特性
         * Json序列化：
         * xml序列化 ：不能序列化私有成员，不需要标记[Serializable]特性
         * soap序列化：需要标记[Serializable]特性，不支持泛型序列化，即List<>等集合均无法序列化，而且soap序列化已经过时，建议用Binary序列化
         * 
         */

        #region json序列化

        /// <summary>
        /// Json序列化
        /// 注标记有[Serializable]特性的对象需要标记[DataContract]特性
        /// 其属性需要标记[DataMember]特性
        /// 用起来比较繁琐，请使用ToJsJson()方法
        /// </summary>
        public static string ToJson(this object item)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();

            }
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToJsJson<T>(this T t)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return serialize.Serialize(t);
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="deep">深度</param>
        /// <returns></returns>
        public static string ToJsJson<T>(this T t, int deep)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            serialize.RecursionLimit = deep;
            return serialize.Serialize(t);
        }

        /// <summary>
        /// 将Table转换为json格式
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string ToJsJson(this DataTable tb)
        {
            JsonTable table = new JsonTable();
            table.TableName = tb.TableName;
            table.Columns=new List<JsonColums>();
            foreach (DataColumn col in tb.Columns)
            {
                table.Columns.Add(new JsonColums
                {
                    ColumnName = col.ColumnName,
                    Caption = col.Caption
                });
            }
            table.Rows = new List<Dictionary<string, object>>();
            foreach(DataRow row in tb.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                for (int i = 0; i < tb.Columns.Count; i++)
                {
                    dict.Add(tb.Columns[i].ColumnName, row[i]);
                }
                table.Rows.Add(dict);
            }
            return table.ToJsJson();
        }

        /// <summary>
        /// 把Table的指定列转换为json格式
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="columnsNames">列名</param>
        /// <returns></returns>
        public static string ToJsJson(this DataTable tb, params string[] columnsNames)
        {
            DataView view = tb.DefaultView;
            DataTable data = view.ToTable(false, columnsNames);
            return data.ToJsJson();
        }

        /// <summary>
        /// Json反序列化
        /// 注标记有[Serializable]特性的对象需要标记[DataContract]特性
        /// 其属性需要标记[DataMember]特性
        /// 用起来比较繁琐，请使用JsJsonTo<T>()方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T JsonTo<T>(string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            T jsonObject = (T)ser.ReadObject(ms);
            ms.Close();
            return jsonObject;
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T JsJsonTo<T>(string json)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return serialize.Deserialize<T>(json);
        }

        

        #endregion

        #region binary序列化

        /// <summary>
        /// 将一个对象序列化为字节（byte）数组
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] BinarySerialize(this object t)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter former = new BinaryFormatter();
            former.Serialize(stream, t);
            return stream.GetBuffer();
        }

        /// <summary>
        /// 将字节（byte）数组
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T BinaryDeserializeTo<T>(this byte[] b)
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            return (T)bFormatter.Deserialize(new MemoryStream(b));
        }

        /// <summary>
        /// 将一个对象序列化为字节（byte）数组并保存到指定文件路径
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <param name="t"></param>
        /// <param name="path"></param>
        public static void BinarySerializeToFile(this object t, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, t);
            }
        }

        /// <summary>
        /// 从指定文件路径中读取数据并反序列化为对象
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T BinaryDeserializeFromFileTo<T>(string path)
        {
            T t;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                t = (T)bf.Deserialize(fs);
            }
            return t;
        }

        #endregion

        #region xml序列化

        /// <summary>
        /// 将一个对象序列化并写入xml文件中
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void XmlSerialize<T>(this T t,string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, t);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        /// <summary>
        /// 读取xml文档并转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T XmlDeserializeTo<T>(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        #endregion

        #region soap序列化

        /// <summary>
        /// 将一个对象序列化为字节（byte）数组
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [Obsolete("建议使用BinarySerialize和BinaryDeserializeTo进行序列化和反序列化")]
        public static byte[] SoapSerialize(this object t)
        {
            MemoryStream stream = new MemoryStream();
            SoapFormatter former = new SoapFormatter();
            former.Serialize(stream, t);
            return stream.GetBuffer();
        }

        /// <summary>
        /// 将字节（byte）数组转换为对象
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <returns></returns>
        [Obsolete("建议使用BinarySerialize和BinaryDeserializeTo进行序列化和反序列化")]
        public static T SoapDeserializeTo<T>(this byte[] b)
        {
            SoapFormatter bFormatter = new SoapFormatter();
            return (T)bFormatter.Deserialize(new MemoryStream(b));
        }

        /// <summary>
        /// 将一个对象序列化为保存为soap格式文件
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <param name="t"></param>
        /// <param name="path"></param>
        [Obsolete("建议使用BinarySerializeToFile和BinaryDeserializeFromFileTo进行序列化和反序列化")]
        public static void SoapSerializeToFile(this object t, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                SoapFormatter bf = new SoapFormatter();
                bf.Serialize(fs, t);
            }
        }

        /// <summary>
        /// 从指定文件路径中读取数据并反序列化为对象
        /// 注：需要添加[Serializable]特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete("建议使用BinarySerializeToFile和BinaryDeserializeFromFileTo进行序列化和反序列化")]        
        public static T SoapDeserializeFromFileTo<T>(string path)
        {
            T t;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                SoapFormatter bf = new SoapFormatter();
                t = (T)bf.Deserialize(fs);
            }
            return t;
        }

        #endregion

    }

    internal class JsonTable
    {
        public string TableName { get; set; }
        public List<JsonColums> Columns { get; set; }
        public List<Dictionary<string, object>> Rows { get; set; }
    }

    internal class JsonColums
    {
        public string ColumnName { get; set; }
        public string Caption { get; set; }
    }
}
