using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using Microsoft.Win32;

namespace Germadent.CorrectionConstructionFile.App.Model
{
    public class CorrectionDictionaryItem
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public interface IXmlDocumentProcessor
    {
        void Process(string sourceFileName, string destFileName, CorrectionDictionaryItem[] correctionDictionary);

        string ProcessInfo { get; }
    }

    public class XmlDocumentProcessor : IXmlDocumentProcessor
    {

        public XmlDocumentProcessor()
        {
        }

        public void Process(string sourceFileName, string destFileName, CorrectionDictionaryItem[] correctionDictionary)
        {
            File.Copy(sourceFileName, destFileName);
            var correctedDoc = CorrectXmlDoc(destFileName, correctionDictionary);
            correctedDoc.Save(destFileName);
        }

        public string ProcessInfo { get; private set; }

        private XmlDocument CorrectXmlDoc(string fileName, CorrectionDictionaryItem[] correctionDictionary)
        {
            var xmlDoc = new XmlDocument();

            ProcessInfo = null;

            var stringBuilder = new StringBuilder();

            xmlDoc.Load(fileName);
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
                                        stringBuilder.Append(string.Concat("Зуб ", nodeLevel3.InnerText, ":", "\r\n"));
                                        break;
                                    case "FilenameImplantGeometry":
                                        if (nodeLevel3.InnerText.Contains("exo-plovdiv"))
                                        {
                                            nodeLevel3.RemoveAll();
                                            stringBuilder.Append(string.Concat("Упоминание Пловдива убрано", "\r\n"));
                                        }
                                        break;
                                    case "ImplantLibraryEntryDescriptor":
                                        stringBuilder.Append(string.Concat("Было:  ", nodeLevel3.InnerText, "\r\n"));
                                        nodeLevel3.InnerText = ImplantInformationHadling(nodeLevel3.InnerText, correctionDictionary);
                                        stringBuilder.Append(string.Concat("Стало: ", nodeLevel3.InnerText, "\r\n",
                                            "---------------------------------------------------------", "\r\n"));
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            ProcessInfo = stringBuilder.ToString();

            return xmlDoc;
        }

        private string ImplantInformationHadling(string innerText, CorrectionDictionaryItem[] correctionDictionary)
        {
            string[] dividedText = TextCutter(innerText, ":");
            string handledText = "";

            switch (dividedText.Length)
            {
                case 2:
                    foreach (var item in correctionDictionary)
                    {
                        dividedText[1] = dividedText[1].Replace(item.Name, item.Value);
                    }
                    handledText = string.Concat(dividedText[0], ":", dividedText[1]);
                    break;

                case 3:
                    foreach (var item in correctionDictionary)
                    {
                        dividedText[2] = dividedText[2].Replace(item.Name, item.Value);
                    }
                    handledText = string.Concat(dividedText[0], ":", dividedText[1], ":", dividedText[2]);
                    break;
            }

            return handledText;

        }

        private string[] TextCutter(string txtOrgin, params string[] charsToSplit)
        {
            string[] txtParts = txtOrgin.Split(charsToSplit, StringSplitOptions.None);
            return txtParts;
        }

    }
}
