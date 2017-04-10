using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FHLog
{
    public class LogFormat
    {
        private WarnFormat warn = new WarnFormat();

        public WarnFormat Warn
        {
            get {
                return warn;
            }
            set {
                warn = value;
            }
        }

        private InfoFormat info = new InfoFormat();

        public InfoFormat Info
        {
            get { return info; }
            set { info = value; }
        }

        private ErrorFormat error = new ErrorFormat();

        public ErrorFormat Error
        {
            get { return error; }
            set { error = value; }
        }

        private FatalFormat fatal = new FatalFormat();

        public FatalFormat Fatal
        {
            get { return fatal; }
            set { fatal = value; }
        }

    }

    public class BaseFormat
    {
        private ConsoleColor color = ConsoleColor.Gray;

        public ConsoleColor Color
        {
            get {
                return color;
            }
            set {
                color = value;
            }
        }

        private string formatString = "[{0}]-{1}\r\n{2}";

        public string FormatString
        {
            get {
                return formatString;
            }
        }
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
