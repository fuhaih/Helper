using System.IO;
namespace Helpers
{

    public static class IOHelper
    {
        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            MemoryStream result = new MemoryStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                int sz = stream.Read(buffer, 0, 1024);
                if (sz == 0) break;
                result.Write(buffer, 0, sz);
            }
            result.Position = 0;
            return result;
        }
    }
}
