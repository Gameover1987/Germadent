using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.CorrectionConstructionFile.App.View;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Germadent.CorrectionConstructionFile.App.ViewModel
{
    public interface IMainViewModel : IDisposable
    {
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private const string DictionaryFileName = "Model\\ImplantDictionary.json";
        private const string DictionaryFile = "Model\\ImplantSystemsDictionary.json";

        private readonly IFileManager _fileManager;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IXmlDocumentProcessor _xmlDocumentProcessor;
        private readonly IAddDictionaryItemViewModel _addDictionaryItemViewModel;
        private string _fullFileName;
        private string _processReport;
        private ObservableCollection<CorrectionDictionaryItem> _implants;
        private CorrectionDictionaryItem _selectedItem;

        public MainViewModel(IShowDialogAgent dialogAgent, IFileManager fileManager, IXmlDocumentProcessor xmlDocumentProcessor, IAddDictionaryItemViewModel addDictionaryItemViewModel)
        {
            _fileManager = fileManager;
            _dialogAgent = dialogAgent;
            _xmlDocumentProcessor = xmlDocumentProcessor;
            _addDictionaryItemViewModel = addDictionaryItemViewModel;

            var dictionary = LoadFromFile(DictionaryFileName);
            _implants = new ObservableCollection<CorrectionDictionaryItem>(dictionary.Select(x => new CorrectionDictionaryItem { Name = x.Key, Value = x.Value }).ToArray());

            AddDictionaryItemCommand = new DelegateCommand(AddDictionaryItemCommandHandler);
            EditDictionaryItemCommand = new DelegateCommand(EditDictionaryItemCommandHandler, CanEditDictionaryItemCommandHandler);
            DeleteDictionaryItemCommand = new DelegateCommand(DeleteDictionaryItemCommandHandler, CanDeleteDictionaryItemCommandHandler);
            OpenProcesingXmlDocumentCommand = new DelegateCommand(OpenProcessingXmlDocumentCommandHandler);

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


        public ObservableCollection<CorrectionDictionaryItem> CorrectionDictionary => _implants;

        public CorrectionDictionaryItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value)
                    return;
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        public IDelegateCommand AddDictionaryItemCommand { get; private set; }

        public IDelegateCommand EditDictionaryItemCommand { get; private set; }

        public IDelegateCommand DeleteDictionaryItemCommand { get; private set; }

        public ICommand OpenProcesingXmlDocumentCommand { get; private set; }

        private void AddDictionaryItemCommandHandler()
        {
            _addDictionaryItemViewModel.Initialize(CorrectionDictionary.ToArray(), null);
            if (_dialogAgent.ShowDialog<AddDictionaryItemWindow>(_addDictionaryItemViewModel) == false)
                return;

            var item = _addDictionaryItemViewModel.GetItem();
            CorrectionDictionary.Add(item);
            SelectedItem = item;
        }

        private bool CanEditDictionaryItemCommandHandler()
        {
            return SelectedItem != null;
        }

        private void EditDictionaryItemCommandHandler()
        {
            _addDictionaryItemViewModel.Initialize(CorrectionDictionary.ToArray(), SelectedItem);
            if (_dialogAgent.ShowDialog<AddDictionaryItemWindow>(_addDictionaryItemViewModel) == false)
                return;

            var item = _addDictionaryItemViewModel.GetItem();
            var position = CorrectionDictionary.IndexOf(SelectedItem);
            CorrectionDictionary.RemoveAt(position);
            CorrectionDictionary.Insert(position, item);
            SelectedItem = item;
        }

        private bool CanDeleteDictionaryItemCommandHandler()
        {
            return SelectedItem != null;
        }

        private void DeleteDictionaryItemCommandHandler()
        {
            CorrectionDictionary.Remove(SelectedItem);
        }

        private void OpenProcessingXmlDocumentCommandHandler(object obj)
        {
            var filter = "Файлы описания конструкций|*.constructionInfo";
            string sourceFileName;
            if (_dialogAgent.ShowOpenFileDialog(filter, out sourceFileName) == false)
                return;

            var destFileName = GetNewFileName(sourceFileName);
            FullFileName = destFileName;

            _xmlDocumentProcessor.Process(sourceFileName, destFileName, CorrectionDictionary.ToArray());

            ProcessReport = _xmlDocumentProcessor.ProcessInfo;
        }

        private Dictionary<string, string> LoadFromFile(string fileName)
        {
            var jsonString = _fileManager.ReadAllText(fileName);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
            return dictionary;
        }

        public string GetNewFileName(string sourceFileName)
        {
            var newFileNameWothoutExtension = Path.GetFileNameWithoutExtension(sourceFileName) + "_processed";
            var extension = Path.GetExtension(sourceFileName);
            var directoryName = Path.GetDirectoryName(sourceFileName);

            return Path.Combine(directoryName, newFileNameWothoutExtension + extension);
        }

        public void Dispose()
        {
            var dictionary = CorrectionDictionary.ToDictionary(x => x.Name, x => x.Value);
            var jsonString = dictionary.SerializeToJson(Formatting.Indented);
            _fileManager.SaveAsText(jsonString, DictionaryFileName);
        }
    }
}
