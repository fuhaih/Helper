using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FHLog
{
    public class RollingFile
    {
        private RollingModel _model = RollingModel.FileSize;

        public RollingModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private long _value = 2 * 1024 * 1024;

        public long Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    public enum RollingModel
    {
        TimeSpan,
        FileSize
    }
}
