using System.Diagnostics;
using System.IO;

namespace Germadent.Common.FileSystem
{
    public interface IFileManager
    {
        byte[] ReadAllBytes(string filePath);

        Stream OpenFileAsStream(string path);

        string ReadAllText(string path);

        FileInfo Save(byte[] data, string filePath);

        FileInfo Save(Stream stream, string filePath);

        void SaveAsText(string data, string path);

        string GetShortFileName(string fullFileName);

        void OpenFileByOS(string fileName);

        bool Exists(string path);

        void Copy(string sourceFileName, string destFileName);
    }

    public class FileManager : IFileManager
    {
        public byte[] ReadAllBytes(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return bytes;
        }

        public Stream OpenFileAsStream(string path)
        {
            return File.OpenRead(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
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

        public void SaveAsText(string data, string path)
        {
            File.WriteAllText(path, data);
        }

        public string GetShortFileName(string fullFileName)
        {
            return Path.GetFileName(fullFileName);
        }

        public void OpenFileByOS(string fileName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                }
            };
            process.Start();
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }
    }
}
