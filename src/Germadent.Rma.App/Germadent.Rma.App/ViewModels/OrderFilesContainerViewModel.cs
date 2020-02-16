using System.IO;
using System.Windows.Input;
using Germadent.Common.FileSystem;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.ViewModels
{
    public class OrderFilesContainerViewModel : IOrderFilesContainerViewModel
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IFileManager _fileManager;

        private string _fileName;
        private byte[] _dataFile;

        public OrderFilesContainerViewModel(IShowDialogAgent dialogAgent, IFileManager fileManager)
        {
            _dialogAgent = dialogAgent;
            _fileManager = fileManager;

            UploadFileCommand = new DelegateCommand(x => UploadDataFileCommandHandler());
            DownloadFileCommand = new DelegateCommand(x => DownloadDataFileCommandHandler(), x => CanDownloadDataFileCommandHandler());
        }

        public ICommand UploadFileCommand { get; }
        public ICommand DownloadFileCommand { get; }

        public void Initialize(OrderDto orderDto)
        {
            _fileName = orderDto.DataFileName;
            _dataFile = orderDto.DataFile;
        }

        private void UploadDataFileCommandHandler()
        {
            var filter = "Все файлы (*.*)|*.*";

            if (_dialogAgent.ShowOpenFileDialog(filter, out var fileName) == false)
                return;

            _fileName = Path.GetFileName(fileName);
            _dataFile= _fileManager.ReadAllBytes(fileName);
        }

        private bool CanDownloadDataFileCommandHandler()
        {
            return _dataFile != null;
        }

        private void DownloadDataFileCommandHandler()
        {
            var filter = "Все файлы (*.*)|*.*";
            if (_dialogAgent.ShowSaveFileDialog(filter, null, out var fileName) == false)
                return;

            _fileManager.Save(_dataFile, fileName);
        }

        public void AssemblyOrder(OrderDto order)
        {
            //order.DataFileName = order.DataFileName;
            //order.DataFile = _dataFile;
        }
    }
}