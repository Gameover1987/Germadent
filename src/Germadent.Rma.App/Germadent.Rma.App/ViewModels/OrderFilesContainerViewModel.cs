using System.IO;
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
        private readonly IRmaOperations _rmaOperations;

        private OrderDto _order;
        private string _fileName;

        public OrderFilesContainerViewModel(IShowDialogAgent dialogAgent, IFileManager fileManager, IRmaOperations rmaOperations)
        {
            _dialogAgent = dialogAgent;
            _fileManager = fileManager;
            _rmaOperations = rmaOperations;

            UploadFileCommand = new DelegateCommand(x => UploadDataFileCommandHandler());
            DownloadFileCommand = new DelegateCommand(x => DownloadDataFileCommandHandler(), x => CanDownloadDataFileCommandHandler());
        }

        public ICommand UploadFileCommand { get; }

        public ICommand DownloadFileCommand { get; }

        public void Initialize(OrderDto orderDto)
        {
            _order = orderDto;
        }

        private void UploadDataFileCommandHandler()
        {
            var filter = "Все файлы (*.*)|*.*";

            if (_dialogAgent.ShowOpenFileDialog(filter, out var fileName) == false)
                return;

            _fileName = fileName;
        }

        private bool CanDownloadDataFileCommandHandler()
        {
            return _order.DataFileName != null;
        }

        private void DownloadDataFileCommandHandler()
        {
            var fileDto = _rmaOperations.GetDataFileByWorkOrderId(_order.WorkOrderId);

            var filter = "Все файлы (*.*)|*.*";
            if (_dialogAgent.ShowSaveFileDialog(filter, fileDto.FileName, out var fileName) == false)
                return;

            _fileManager.Save(fileDto.Data, fileName);
        }

        public void AssemblyOrder(OrderDto order)
        {
            order.DataFileName = _fileName;
        }
    }
}