using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        public void Roll(LogSetting logSet)
        {
            switch (_model)
            {
                case RollingModel.FileSize:
                    {
                        using (StreamWriter writer = new StreamWriter(logSet.FullName, true, Encoding.UTF8))
                        {
                            long length = writer.BaseStream.Length;
                            if (length > Value)
                            {
                                writer.Close();
                                string path = Path.Combine(logSet.LogPath, "LogBack");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                string newFile = Path.Combine(path, logSet.LogName + DateTime.Now.ToString("yyyyMMddHHmmss") + logSet.LogExtension);
                                File.Move(logSet.FullName, newFile);
                            }
                        }
                    }break;
                case RollingModel.TimeSpan: 
                    {
                        DateTime now=DateTime.Now;
                        if ((now-logSet.Time).TotalMilliseconds>Value)
                        {
                            logSet.Time = now;
                            string path = Path.Combine(logSet.LogPath, "LogBack");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string newFile = Path.Combine(path, logSet.LogName + DateTime.Now.ToString("yyyyMMddHHmmss") + logSet.LogExtension);
                            File.Move(logSet.FullName, newFile);
                        }
                }break;
            }
        }
    }

    public enum RollingModel
    {
        TimeSpan,
        FileSize
    }
}
