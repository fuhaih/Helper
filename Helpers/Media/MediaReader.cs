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
            return GetAtoms(0,stream.Length);
        }

        public IEnumerable<Atom> GetAtoms(long seek,long length)
        {
            long size = seek + length;
            while (seek < size)
            {
                Atom readAtom = ReadAtom(stream, seek);
                seek += readAtom.Size;
                yield return readAtom;
            }
            if (seek > size)
            {
                throw new Exception("文件异常");
            }
        }

        public byte[] ReadDataWithoutHead(Atom atom)
        {
            byte[] data = new byte[atom.Size - atom.HeadLength];
            stream.Seek(atom.Position + atom.HeadLength,SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            return data;
        }

        public byte[] ReadData(Atom atom)
        {
            byte[] data = new byte[atom.Size];
            stream.Seek(atom.Position, SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            return data;
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
            bool isFullBox = IsFullBox(type);
            int headLength = isFullBox?12:8;
            //int largeOffset = isFullBox ? 12 : 8;
            if (length == 1)
            {
                stream.Seek(seek + headLength, SeekOrigin.Begin);
                stream.Read(maxSizeBuffer, 0, 8);
                Array.Reverse(maxSizeBuffer);
                length =(long)BitConverter.ToUInt64(maxSizeBuffer, 0);
                headLength += 8;
            }
            else if (length == 0)
            {
                length = stream.Length - seek;
            }
            return new Atom
            {
                Name = type,
                Position = seek,
                Size = length,
                HeadLength = headLength
            };
        }

        /// <summary>
        /// 是否是fullbox，目前只用到了stco和co64两个fullbox，所以简单判断。后续再修改
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsFullBox(string type)
        {
            return type == "stco" || type == "co64";
        }
    }
}
