using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.CorrectionConstructionFile.App.ViewModel
{
    public interface IMainViewModel
    {
    }

    public class CorconViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IXmlDocumentProcessor _xmlDocumentProcessor;
        private string _fullFileName;
        private string _processReport;
        private Dictionary<string, string> _implantDict;
        
        public CorconViewModel(IXmlDocumentProcessor xmlDocumentProcessor)
        {
            _xmlDocumentProcessor = xmlDocumentProcessor;
            OpenProcesingXmlDocumentCommand = new DelegateCommand(OpenProcessingXmlDocumentCommandHandler);

            var jsonString = File.ReadAllText("Model\\ImplantDictionary.json");
            _implantDict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }

        public string FullFileName
        {
            get { return _fullFileName; }
            set
            {
                if (_fullFileName == value)
                    return;
                _fullFileName = value;
                OnPropertyChanged(() => FullFileName);
            }
        }

        public string ProcessReport
        {
            get { return _processReport; }
            set
            {
                if (_processReport == value)
                    return;
                _processReport = value;
                OnPropertyChanged(() => ProcessReport);
            }
        }

        public ICommand OpenProcesingXmlDocumentCommand { get; private set; }

        public Dictionary<string, string> ImplantDict => _implantDict;

        private void OpenProcessingXmlDocumentCommandHandler(object obj)
        {
            FullFileName = _xmlDocumentProcessor.GetFileName();
            _xmlDocumentProcessor.CopyFile(FullFileName);
            ProcessReport = _xmlDocumentProcessor.ReadingDocument(FullFileName, ImplantDict);
        }
    }
}
