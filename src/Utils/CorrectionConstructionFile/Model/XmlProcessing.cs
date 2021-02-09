using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using Microsoft.Win32;

namespace Germadent.CorrectionConstructionFile.App.Model
{
    public interface IXmlDocumentProcessor
    {
        void Process(string sourceFileName, string destFileName);

        void CopyFile(string fullFileName);

        string GetFileName();

        string ReadingDocument(string fullFileName, Dictionary<string, string> transitDict);
    }

    public class XmlDocumentProcessor : IXmlDocumentProcessor
    {
        public string FileName { get; private set; }
        public string ProcessInfo { get; private set; }

        public string GetFileName()
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы описания конструкций|*.constructionInfo";
            if (ofd.ShowDialog() == true)
            {
                FileName = ofd.FileName;
                return FileName;
            }
            else
                return null;
           
        }
        
        public void CopyFile(string fullFileName)
        {
            FileInfo fileInfo = new FileInfo(fullFileName);
            var fDir = fileInfo.DirectoryName;
            var fName = Path.GetFileNameWithoutExtension(fullFileName);
            var fExtn = fileInfo.Extension;
            var newFileFullName = string.Concat(fDir, "\\", fName, "_КОПИЯ", fExtn);

            try
            {
                fileInfo.CopyTo(newFileFullName, false);
            }
            catch(IOException)
            {
                MessageBox.Show("Первоначальная копия уже есть", "Сохранение копии");
            }
            
        }
    
        public string ReadingDocument(string fullFileName, Dictionary<string, string> transitDict)
        {
            ProcessInfo = "";
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fullFileName);
            XmlNode root = xmlDoc.DocumentElement;

            foreach (XmlNode nodeLevel1 in root.ChildNodes)
            {
                if (nodeLevel1.Name == "Teeth")
                {
                    foreach (XmlNode nodeLevel2 in nodeLevel1.ChildNodes)
                    {
                        if (nodeLevel2.Name == "Tooth")
                        {
                            foreach (XmlNode nodeLevel3 in nodeLevel2.ChildNodes)
                            {
                                switch (nodeLevel3.Name)
                                {
                                    case "Number":
                                        ProcessInfo += string.Concat("Зуб ", nodeLevel3.InnerText, ":", "\r\n");
                                        break;
                                    case "FilenameImplantGeometry":
                                        if (nodeLevel3.InnerText.Contains("exo-plovdiv"))
                                        {
                                            nodeLevel3.RemoveAll();
                                            ProcessInfo += string.Concat("Упоминание Пловдива убрано", "\r\n");
                                        }
                                        break;
                                    case "ImplantLibraryEntryDescriptor":
                                        ProcessInfo += string.Concat("Было:  ", nodeLevel3.InnerText, "\r\n");
                                        nodeLevel3.InnerText = ImplantInformationHadling(nodeLevel3.InnerText, transitDict);
                                        ProcessInfo += string.Concat("Стало: ", nodeLevel3.InnerText, "\r\n", 
                                            "---------------------------------------------------------", "\r\n");
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            xmlDoc.Save(@"D:\Corcon\test.constructionInfo");
            return ProcessInfo;
        }

        public string ImplantInformationHadling(string innerText, Dictionary<string, string> implantDict)
        {
            string[] dividedText = TextCutter(innerText, ":");
            string handledText = "";

            switch (dividedText.Length)
            {
                case 2:
                    foreach (var item in implantDict)
                    {
                        dividedText[1] = dividedText[1].Replace(item.Key, item.Value);
                    }
                    handledText = string.Concat(dividedText[0], ":", dividedText[1]);
                    break;

                case 3:
                    foreach (var item in implantDict)
                    {
                        dividedText[2] = dividedText[2].Replace(item.Key, item.Value);
                    }
                    handledText = string.Concat(dividedText[0], ":", dividedText[1], ":", dividedText[2]);
                    break;
            }
            
            return handledText;
  
        }

        public string[] TextCutter(string txtOrgin, params string[] charsToSplit)
        {
            string[] txtParts = txtOrgin.Split(charsToSplit, StringSplitOptions.None);
            return txtParts;
        }

        public void Process(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }
    }
}
