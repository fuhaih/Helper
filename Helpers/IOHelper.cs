using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static string GetRealPath(string path)
        {
            Regex regex = new Regex(@"^[a-zA-Z]:\\");
            if(regex.IsMatch(path)) return path;
            string basepath = AppDomain.CurrentDomain.BaseDirectory;
            string[] segments=path.Split('\\','/');
            string[] realPathSegment = new string[segments.Length + 1];
            realPathSegment[0] = basepath;
            segments.CopyTo(realPathSegment, 1);
            return Path.Combine(realPathSegment);
        }
    }
}
