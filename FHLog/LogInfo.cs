﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FHLog
{
    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogInfo
    {
        public LogType Type{get;set;}
        public string Info { get; set; }
        public BaseFormat Format { get; set; }
        public DateTime Time { get; set; }
        public override string ToString()
        {
            return string.Format(Format.FormatString, Type.ToString(), Time, Info);
        }
    }
}
