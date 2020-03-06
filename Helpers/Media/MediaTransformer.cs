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

        /// <summary>
        /// 把moov信息移动到视频信息前端
        /// </summary>
        /// <param name="newfile">新文件路径</param>
        /// <returns>true：移动成功，false：moov信息无需移动</returns>
        public bool MoveMoov2Top(string newfile)
        {
            MediaReader reader = new MediaReader(OriginStream);
            List<Atom> atoms = reader.GetAtoms().ToList();
            //判断moov和mdata的位置
            int moovIndex = 0;
            int mdatIndex = 0;
            for (int i = 0; i < atoms.Count; i++)
            {
                Atom atom = atoms[i];
                if (atom.Position < 0 || atom.Size < 0)
                {
                    throw new Exception("视频信息异常，视频过大");
                }
                if (atom.Name == "moov")
                {
                    moovIndex = i;
                }
                else if (atom.Name == "mdat")
                {
                    mdatIndex = i;
                }
            }

            if (moovIndex==0|| mdatIndex == 0)
            {
                throw new Exception("视频信息异常");
            }

            if (moovIndex < mdatIndex)
            {
                return false;
            }


            //移动
            if (atoms[0].Name != "ftyp")
            {
                throw new Exception("视频信息有异常，ftype位置不对");
            }
            Atom moov = atoms[moovIndex];
            for (int i = moovIndex; i> 1; i--)
            {
                atoms[i] = atoms[i - 1];
            }
            atoms[1] = moov;
            //Atom temple = atoms[moovIndex];
            //atoms[moovIndex] = atoms[mdatIndex];
            //atoms[mdatIndex] = temple;

            byte[] moovData = reader.ReadData(moov);

            List<Atom> moovsub = reader.GetAtoms(moov.Position + moov.HeadLength, moov.Size - moov.HeadLength).ToList();

            List<Atom> tracks = moovsub.Where(m => m.Name == "trak").ToList();

            List<Atom> stcos = new List<Atom>();
            byte[] offsetBytes = new byte[4];
            byte[] offset64Bytes = new byte[8];
            foreach (var track in tracks)
            {
                Atom stco = GetStco(reader, track);

                byte[] data = reader.ReadDataWithoutHead(stco);

                byte[] number_of_entries = new byte[4];
                Array.Copy(data, number_of_entries, number_of_entries.Length);
                Array.Reverse(number_of_entries);
                long length = BitConverter.ToUInt32(number_of_entries, 0);
                if (stco.Name == "stco")
                {
                    for (long i = 0; i < length; i++)
                    {
                        Array.Copy(data, 4 + 4 * i, offsetBytes, 0, offsetBytes.Length);
                        Array.Reverse(offsetBytes);
                        UInt32 offset = BitConverter.ToUInt32(offsetBytes,0);
                        offset += (UInt32)moov.Size;
                        offsetBytes = BitConverter.GetBytes(offset);
                        Array.Reverse(offsetBytes);
                        Array.Copy(offsetBytes, 0, moovData, (stco.Position - moov.Position) + stco.HeadLength + 4 + i* 4, offsetBytes.Length);
                    }
                }
                else if (stco.Name == "co64")
                {
                    for (long i = 0; i < length; i++)
                    {
                        Array.Copy(data, 4 + 8 * i, offset64Bytes, 0, offset64Bytes.Length);
                        Array.Reverse(offset64Bytes);
                        long offset = (long)BitConverter.ToUInt64(offset64Bytes, 0);
                        if (offset < 0) throw new Exception("文件过大");
                        offset += moov.Size;
                        offset64Bytes = BitConverter.GetBytes(offset);
                        Array.Reverse(offset64Bytes);
                        Array.Copy(offset64Bytes, 0, moovData, (stco.Position - moov.Position) + stco.HeadLength + 4 + i * 8, offset64Bytes.Length);
                    }
                }
            }

            using (FileStream newStream = File.Create(newfile))
            {
                foreach (Atom atom in atoms)
                {
                    byte[] buffer = null;
                    if (atom.Name == "moov")
                    {
                        buffer = moovData;
                    }
                    else {
                        buffer = new byte[atom.Size];
                        OriginStream.Seek(atom.Position, SeekOrigin.Begin);
                        OriginStream.Read(buffer, 0, buffer.Length);
                    }
                    newStream.Write(buffer, 0, buffer.Length);
                }
            }

            return true;

        }

        public Atom GetStco(MediaReader reader,Atom track)
        {
            List<Atom> tracksub = reader.GetAtoms(track.Position + track.HeadLength, track.Size - track.HeadLength).ToList();
            Atom mdia = tracksub.FirstOrDefault(m=>m.Name =="mdia");
            List<Atom> mdiasub = reader.GetAtoms(mdia.Position + mdia.HeadLength, mdia.Size - mdia.HeadLength).ToList();
            Atom minf = mdiasub.FirstOrDefault(m=>m.Name=="minf");
            List<Atom> minfsub = reader.GetAtoms(minf.Position + minf.HeadLength, minf.Size - minf.HeadLength).ToList();
            Atom stbl = minfsub.FirstOrDefault(m=>m.Name=="stbl");
            List<Atom> stblsub = reader.GetAtoms(stbl.Position + stbl.HeadLength, stbl.Size - stbl.HeadLength).ToList();
            Atom stco = stblsub.FirstOrDefault(m => m.Name == "stco" || m.Name == "co64");
            return stco;
        }

        //public bool MoovAtTop(IEnumerable<Atom> atoms)
        //{

        //}

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
