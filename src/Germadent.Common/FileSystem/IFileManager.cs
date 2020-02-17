using System.Diagnostics;
using System.IO;

namespace Germadent.Common.FileSystem
{
    public interface IFileManager
    {
        byte[] ReadAllBytes(string filePath);

        FileInfo Save(byte[] data, string filePath);

        void OpenFile(string fileName);
    }

    public class FileManager : IFileManager
    {
        public byte[] ReadAllBytes(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return bytes;
        }

        public FileInfo Save(byte[] data, string filePath)
        {
            File.WriteAllBytes(filePath, data);

            return new FileInfo(filePath);
        }

        public void OpenFile(string fileName)
        {
            Process.Start(fileName);
        }
    }
}
