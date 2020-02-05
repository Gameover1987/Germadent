using System.Windows.Input;
using Germadent.Common.FileSystem;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.ViewModels
{
    public class OrderFilesContainerViewModel : IOrderFilesContainerViewModel
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IFileManager _fileManager;

        private OrderDto _orderDto;

        public OrderFilesContainerViewModel(IShowDialogAgent dialogAgent, IFileManager fileManager)
        {
            _dialogAgent = dialogAgent;
            _fileManager = fileManager;

            UploadStlFileCommand = new DelegateCommand(x => UploadStlFileCommandHandler());
            DownloadStlFileCommand = new DelegateCommand(x => DownloadStlFileCommandHandler(), x => CanDownloadStlFileCommandHandler());
        }

        public ICommand UploadStlFileCommand { get; }
        public ICommand DownloadStlFileCommand { get; }
        public ICommand UploadPhotoCommand { get; }
        public ICommand DownloadPhotoCommand { get; }

        public void Initialize(OrderDto orderDto)
        {
            _orderDto = orderDto;
        }

        private void UploadStlFileCommandHandler()
        {
            var filter = "STL файлы (*.stl)|*.stl|Все файлы (*.*)|*.*";

            if (_dialogAgent.ShowOpenFileDialog(filter, out var fileName) == false)
                return;

            _orderDto.StlFile = _fileManager.ReadAllBytes(fileName);
        }

        private bool CanDownloadStlFileCommandHandler()
        {
            return _orderDto.StlFile != null;
        }

        private void DownloadStlFileCommandHandler()
        {
            var filter = "STL файлы (*.stl)|*.stl|Все файлы (*.*)|*.*";
            if (_dialogAgent.ShowSaveFileDialog(filter, null, out var fileName) == false)
                return;

            _fileManager.Save(_orderDto.StlFile, fileName);
        }
    }
}