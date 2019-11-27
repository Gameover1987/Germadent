using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Common.FileSystem
{
    public interface IFileManager
    {
        byte[] ReadAllBytes(string filePath);

        void Save(byte[] data, string filePath);
    }

    public class FileManager : IFileManager
    {
        public byte[] ReadAllBytes(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return bytes;
        }

        public void Save(byte[] data, string filePath)
        {
            File.WriteAllBytes(filePath, data);
        }
    }
}
