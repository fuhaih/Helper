using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Media
{
    public class MediaTransformer :IDisposable
    {
        public Stream OriginStream;
        public MediaTransformer(string filename)
        {
            OriginStream = File.OpenRead(filename);
        }
        public MediaTransformer(Stream stream)
        {
            OriginStream = stream;
        }

        public bool MoveMoov(string newfile, bool toEnd = false)
        {
            MediaReader reader = new MediaReader(OriginStream);
            IEnumerable<Atom> atoms = reader.GetAtoms();
            return true;

        }

        #region 垃圾回收

        /// <summary>
        /// 释放标记
        /// </summary>
        private bool disposed;
        void IDisposable.Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
                //这里暂时没有托管对象。
            }
            //清理非托管资源 很少用到非托管资源
            OriginStream.Dispose();
            //告诉自己已经被释放
            disposed = true;
        }

        ~MediaTransformer()
        {
            //垃圾回收器只回收托管对象内存，当对象有析构函数时会放入另一个队列，
            //然后在下一次垃圾回收时调用析构函数
            //所以可以在析构函数中处理非托管对象的回收工作。
            //必须为false
            Dispose(false);
        }

        #endregion
    }
}
