using Corcon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Corcon.ViewModel
{
    public class CorconViewModel : ViewModelBase
    {
        private string _fullFileName;
        private string _processReport;
        private Dictionary<string, string> _implantDict;

        public string FullFileName
        {
            get { return _fullFileName; }
            set { SetProperty(ref _fullFileName, value); }
        }

        

        public string ProcessReport
        {
            get { return _processReport; }
            set { SetProperty(ref _processReport, value); }
        }

        public ICommand OpenProcesingXmlDocumentCommand { get; private set; }
        
        public Dictionary<string, string> ImplantDict 
        { 
            get 
            {
                new Dictionary<string, string>();
                string jsonString = File.ReadAllText("Model\\ImplantDictionary.json");
                return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
            }
            set { SetProperty(ref _implantDict, value); }
        }

        public CorconViewModel()
        {
            OpenProcesingXmlDocumentCommand = new DelegateCommand(OpenProcessingXmlDocumentCommandHandler);
        }

        public void OpenProcessingXmlDocumentCommandHandler(object obj)
        {
            XmlDocumentProcessing xmlDocProc = new XmlDocumentProcessing();
            FullFileName = xmlDocProc.GetFileName();
            xmlDocProc.CopyFile(FullFileName);
            ProcessReport = xmlDocProc.ReadingDocument(FullFileName, ImplantDict);

        }
    }
}
