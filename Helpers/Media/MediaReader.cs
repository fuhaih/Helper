using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Media
{
    public class MediaReader
    {
        byte[] commonBuffer = new byte[8];
        byte[] minSizeBuffer = new byte[4];
        byte[] typeBuffer = new byte[4];
        byte[] maxSizeBuffer = new byte[8];

        private Stream stream;
        public MediaReader(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<Atom> GetAtoms()
        {
            long seek = 0;
            while (seek < stream.Length)
            {
                Atom readAtom = ReadAtom(stream, seek);
                seek += readAtom.Size;
                yield return readAtom;
            }
            if (seek > stream.Length)
            {
                throw new Exception("文件异常");
            }
        }

        public Atom ReadAtom(Stream stream, long seek)
        {
            stream.Seek(seek, SeekOrigin.Begin);
            stream.Read(commonBuffer, 0, 8);
            Array.Copy(commonBuffer, 0, minSizeBuffer, 0, 4);
            Array.Reverse(minSizeBuffer);
            long length = BitConverter.ToUInt32(minSizeBuffer, 0);
            Array.Copy(commonBuffer, 4, typeBuffer, 0, 4);
            string type = Encoding.Default.GetString(typeBuffer);
            if (length == 0)
            {
                stream.Seek(seek + 8, SeekOrigin.Begin);
                stream.Read(maxSizeBuffer, 0, 8);
                Array.Reverse(maxSizeBuffer);
                length =(long)BitConverter.ToUInt64(maxSizeBuffer, 0);
            }
            return new Atom
            {
                Name = type,
                Position = seek,
                Size = length
            };
        }
    }
}
