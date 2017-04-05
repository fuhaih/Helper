using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FHLog
{
    public class LogFormat
    {
        public WarnFormat Warn = new WarnFormat();
        public InfoFormat Info = new InfoFormat();
        public ErrorFormat Error = new ErrorFormat();
        public FatalFormat Fatal = new FatalFormat();
    }

    public class BaseFormat
    {
        public ConsoleColor Color = ConsoleColor.Gray;
        public readonly string FormatString = "[{0}]-{1}\r\n{2}";
    }

    public class WarnFormat : BaseFormat
    {
        public WarnFormat()
        {
            this.Color = ConsoleColor.Yellow;
        }
    }

    public class InfoFormat : BaseFormat
    {
    }

    public class ErrorFormat : BaseFormat
    {
        public ErrorFormat()
        {
            this.Color = ConsoleColor.Red;
        }
    }

    public class FatalFormat : BaseFormat
    {
        public FatalFormat()
        {
            this.Color = ConsoleColor.Cyan;
        }
    }
}
