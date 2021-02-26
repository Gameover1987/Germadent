using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Germadent.CorrectionConstructionFile.App.Model
{

    public interface IXmlDocumentProcessor
    {
        void Process(string sourceFileName, string destFileName, ImplantSystem[] implantSystems);

        string ProcessInfo { get; }
    }

    public class XmlDocumentProcessor : IXmlDocumentProcessor
    {
        public void Process(string sourceFileName, string destFileName, ImplantSystem[] implantSystems)
        {
            File.Copy(sourceFileName, destFileName, true);
            var correctedDoc = CorrectXmlDoc(destFileName, implantSystems);
            correctedDoc.Save(destFileName);
        }

        public string ProcessInfo { get; private set; }
        public bool ToDeleteElement { get; private set; }

        private XmlDocument CorrectXmlDoc(string fileName, ImplantSystem[] implantSystems)
        {
            var xmlDoc = new XmlDocument();

            ProcessInfo = null;

            string soughtCode = "";
                        
            var stringBuilder = new StringBuilder();

            xmlDoc.Load(fileName);
            XmlNode root = xmlDoc.DocumentElement;

            foreach (XmlNode nodeLevel1 in root.ChildNodes)
            {
                if (nodeLevel1.Name != "Teeth")
                    continue;

                foreach (XmlNode nodeLevel2 in nodeLevel1.ChildNodes)
                {
                    ToDeleteElement = false;

                    if (nodeLevel2.Name != "Tooth")
                        continue;

                    foreach (XmlElement nodeLevel3 in nodeLevel2.ChildNodes)
                    {
                        switch (nodeLevel3.Name)
                        {
                            case "Number":
                                stringBuilder.Append(string.Concat("Зуб ", nodeLevel3.InnerText, ":", "\r\n"));
                                break;
                            case "ImplantLibraryEntryDisplayInformation":
                                stringBuilder.Append(string.Concat("Было:  ", nodeLevel3.InnerText, "\r\n"));
                                nodeLevel3.InnerText = CodeChanger(nodeLevel3.InnerText, implantSystems, out soughtCode);
                                stringBuilder.Append(string.Concat("Стало: ", nodeLevel3.InnerText, "\r\n",
                                      "++++++++++++++++++++++++++++++++++++++++++++", "\r\n"));
                                break;
                        }
                    }

                    foreach (XmlElement nodeLevel3 in nodeLevel2.ChildNodes)
                    {
                        if (nodeLevel3.Name == "ImplantLibraryEntryDescriptor")
                        {
                            stringBuilder.Append(string.Concat("Было:  ", nodeLevel3.InnerText, "\r\n"));
                            string[] cutText = TextCutter(nodeLevel3.InnerText, ":");
                            if (cutText.Length == 3)
                            {
                                cutText[2] = soughtCode;
                                nodeLevel3.InnerText = string.Concat(cutText[0], ":", cutText[1], ": ", cutText[2]);
                                stringBuilder.Append(string.Concat("Стало: ", nodeLevel3.InnerText, "\r\n",
                                      "-------------------------------------------------------------------------------", "\r\n"));
                            }
                        }
                    }

                    foreach (XmlElement nodeLevel3 in nodeLevel2.ChildNodes)
                    {
                        if (nodeLevel3.Name == "FilenameImplantGeometry" && nodeLevel3.InnerText.Contains("exo-plovdiv"))
                            ToDeleteElement = true;
                        
                    }

                    foreach (XmlElement nodeLevel3 in nodeLevel2.ChildNodes)
                    {
                        if (nodeLevel3.Name == "MatrixImplantGeometryTargetOutputConstructionFile" && ToDeleteElement == true)
                        {
                            nodeLevel3.ParentNode.RemoveChild(nodeLevel3);
                            stringBuilder.Append(string.Concat("Лишний элемент убран", "\r\n",
                             "================================================", "\r\n"));
                        }
                    }
                }
            }

            ProcessInfo = stringBuilder.ToString();

            return xmlDoc;
        }

        private string CodeChanger(string innerText, ImplantSystem[] implantSystems, out string soughtCode)
        {

            string[] dividedText = TextCutter(innerText, ":");
            string handledText = "";
            soughtCode = "";

            switch (dividedText.Length)
            {
                case 2:
                    handledText = innerText;
                    break;
                case 3:
                    foreach (var implSystemItem in implantSystems)
                    {
                        if (dividedText[0].ToLower().Contains(implSystemItem.Name.ToLower()))
                        {
                            foreach (var implModel in implSystemItem.CorrectionDictionary)
                            {
                                if (dividedText[1].ToLower().Contains(implModel.Name.ToLower()))
                                {
                                    dividedText[2] = implModel.Value;
                                    soughtCode = implModel.Value;
                                }
                            }
                        }
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
