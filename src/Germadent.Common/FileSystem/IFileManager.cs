using System.Diagnostics;
using System.IO;

namespace Germadent.Common.FileSystem
{
    public interface IFileManager
    {
        byte[] ReadAllBytes(string filePath);

        Stream OpenFile(string path);

        FileInfo Save(byte[] data, string filePath);

        FileInfo Save(Stream stream, string filePath);

        void OpenFileByOS(string fileName);
    }

    public class FileManager : IFileManager
    {
        public byte[] ReadAllBytes(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return bytes;
        }

        public Stream OpenFile(string path)
        {
            return File.OpenRead(path);
        }

        public FileInfo Save(byte[] data, string filePath)
        {
            File.WriteAllBytes(filePath, data);

            return new FileInfo(filePath);
        }

        public FileInfo Save(Stream stream, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }

            return new FileInfo(filePath);
        }

        public void OpenFileByOS(string fileName)
        {
            Process.Start(fileName);
        }
    }
}
