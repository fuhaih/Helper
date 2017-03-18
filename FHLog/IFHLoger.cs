using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHLog
{
    public interface IFHLoger
    {
        /// <summary>
        /// 消息
        /// </summary>
        void Info();
        /// <summary>
        /// 警告
        /// </summary>
        void Warn();
        /// <summary>
        /// 异常
        /// </summary>
        void Error();
        /// <summary>
        /// 错误
        /// </summary>
        void Fatal();
    }
}
