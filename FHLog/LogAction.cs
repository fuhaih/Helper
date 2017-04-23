using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace FHLog
{
    public class LogAction
    {
        private RollingFile _rolling = new RollingFile();

        public RollingFile Rolling
        {
            get { return _rolling; }
            set { _rolling = value; }
        }

        public LogOutput Output { get => _output; set => _output = value; }

        private LogOutput _output = new LogOutput();

    }

    public class LogOutput
    {
        private bool _isOpenConsole = false;

        /// <summary>
        /// 是否开启控制台输出，只读
        /// </summary>
        public bool IsOpenConsole { get => _isOpenConsole;}

        /// <summary>
        /// 打开控制台输出
        /// 控制台输出很慢，如果每秒千条以上的日志最好不要开启
        /// </summary>
        public void OpenConsole()
        {
            _isOpenConsole = true;
        }

        /// <summary>
        /// 关闭控制台输出
        /// </summary>
        public void CloseConsole()
        {
            _isOpenConsole = false;
        }
    }
}
