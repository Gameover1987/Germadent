using System.IO;

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
