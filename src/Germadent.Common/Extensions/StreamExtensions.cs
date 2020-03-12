using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Common.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToBytes(this Stream stream, bool disposeSourceStream = true)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                if (disposeSourceStream)
                    stream.Dispose();

                return memoryStream.ToArray();
            }
        }
    }
}
