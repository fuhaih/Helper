using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
