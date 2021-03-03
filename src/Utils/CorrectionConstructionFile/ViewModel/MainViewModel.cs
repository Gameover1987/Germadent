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
        IDelegateCommand EditDictionaryItemCommand { get; }
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private const string DictionaryFileName = "ImplantSystemsDictionary.json";

        private readonly IFileManager _fileManager;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IXmlDocumentProcessor _xmlDocumentProcessor;
        private readonly IAddImplantSystemViewModel _addImplantSystemViewModel;
        private string _fullFileName;
        private string _processReport;
        private ObservableCollection<ImplantSystemViewModel> _implantSystems;

        private ImplantSystemViewModel _selectedItem;

        public MainViewModel(IShowDialogAgent dialogAgent, IFileManager fileManager, IXmlDocumentProcessor xmlDocumentProcessor, IAddImplantSystemViewModel addImplantSystemViewModel)
        {
            _fileManager = fileManager;
            _dialogAgent = dialogAgent;
            _xmlDocumentProcessor = xmlDocumentProcessor;
            _addImplantSystemViewModel = addImplantSystemViewModel;

            var doubleDictionary = LoadDictionaryFromFile(DictionaryFileName);
            _implantSystems = new ObservableCollection<ImplantSystemViewModel>(doubleDictionary.Select(x => new ImplantSystemViewModel(x)));

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

        public ObservableCollection<ImplantSystemViewModel> ImplantSystems => _implantSystems;

        public ImplantSystemViewModel SelectedItem
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
            _addImplantSystemViewModel.Initialize(ImplantSystems.ToArray(), null);
            if (_dialogAgent.ShowDialog<AddImplantSystemWindow>(_addImplantSystemViewModel) == false)
                return;

            var item = _addImplantSystemViewModel.GetItem();
            ImplantSystems.Add(item);
            SelectedItem = item;
        }

        private bool CanEditDictionaryItemCommandHandler()
        {
            return SelectedItem != null;
        }

        private void EditDictionaryItemCommandHandler()
        {
            _addImplantSystemViewModel.Initialize(ImplantSystems.ToArray(), SelectedItem);
            if (_dialogAgent.ShowDialog<AddImplantSystemWindow>(_addImplantSystemViewModel) == false)
                return;

            var item = _addImplantSystemViewModel.GetItem();
            var position = ImplantSystems.IndexOf(SelectedItem);
            ImplantSystems.RemoveAt(position);
            ImplantSystems.Insert(position, item);
            SelectedItem = item;
        }

        private bool CanDeleteDictionaryItemCommandHandler()
        {
            return SelectedItem != null;
        }

        private void DeleteDictionaryItemCommandHandler()
        {
            ImplantSystems.Remove(SelectedItem);
        }

        private void OpenProcessingXmlDocumentCommandHandler(object obj)
        {
            var filter = "Файлы описания конструкций|*.constructionInfo";
            string sourceFileName;
            if (_dialogAgent.ShowOpenFileDialog(filter, out sourceFileName) == false)
                return;

            var destFileName = GetNewFileName(sourceFileName);
            FullFileName = destFileName;

            _xmlDocumentProcessor.Process(sourceFileName, destFileName, ImplantSystems.Select(x => x.ToModel()).ToArray());

            ProcessReport = _xmlDocumentProcessor.ProcessInfo;
        }

        private ImplantSystem[] LoadDictionaryFromFile(string fileName)
        {
            var jsonString = _fileManager.ReadAllText(fileName);
            var dictionaryOfDictionariesFuckYeah = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonString);
            return dictionaryOfDictionariesFuckYeah.Select(x => new ImplantSystem
            {
                Name = x.Key,
                CorrectionDictionary = x.Value.Select(y => new CorrectionDictionaryItem
                {
                    Name = y.Key,
                    Value = y.Value
                }).ToArray()
            }).ToArray();
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
            var dictionary = ImplantSystems.Select(x => x.ToModel()).ToDictionary(x => x.Name, x => x.CorrectionDictionary.ToDictionary(y => y.Name, y => y.Value));
            var jsonString = dictionary.SerializeToJson(Formatting.Indented);
            _fileManager.SaveAsText(jsonString, DictionaryFileName);
        }
    }
}
