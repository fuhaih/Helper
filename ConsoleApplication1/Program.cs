﻿using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using System.Threading;
using System.Security;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
namespace ConsoleApplication1
{
    public delegate int mydelegate();

    class Program
    {
        [MethodImpl(MethodImplOptions.InternalCall), SecurityCritical]
        [DllImport("kernel32.dll")]
        internal static extern string FastAllocateString(int length);

        static void Main(string[] args)
        {
            DataTable test = new DataTable();
            test.Columns.Add("name");
            test.Columns.Add("value");
            test.Rows.Add("fuhai", "88");
            test.Rows.Add("fuhai", "88");
            test.Rows.Add("fuhai", "88");
            string result = test.ToJsJson();
            Console.ReadKey();
        }
        static int abs( int x ) 
        { 
            int y ;
            y = x >> 31;
            return (x ^ y) - y;  //or: (x+y)^y
        }
    }


    public class myservice
    {
        public DateTime initialTime;
        public int poit=0;
        public myservice()
        {
            initialTime = DateTime.Now;
        }
    }


    public class test
    {
        public static myservice service = new myservice();
        public test()
        {
            //service.poit = 1;
        }

        public void consoTime()
        {
            Console.WriteLine("service初始化时间:" + service.initialTime.ToString("hh:mm:ss.fffffff"));
        }
    }

    /// 注意：要序列化为Binary数组的时候要添加Serializable特性
    /// 但是，用Serializable特性的时候，如果类声明的是自动化属性，
    /// 那么用DataContractJsonSerializer将对象序列化为json格式的时候会带有k__BackingField后缀
    ///如"<name>k__BackingField":"fuhai"
    ///解决方案，再添加DataContract和DataMembe特性

    [Serializable]
    //[DataContract]
    public class Person
    {
        //[DataMember]
        public int id { get; set; }
        //[DataMember]
        public string name { get; set; }
        //[DataMember]
        public DateTime birth { get; set; }

    }

   
}
