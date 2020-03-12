using System.IO;
using System.Windows.Input;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class OrderFilesContainerViewModel : ViewModelBase, IOrderFilesContainerViewModel
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IFileManager _fileManager;
        private readonly IRmaOperations _rmaOperations;

        private OrderDto _order;
        private string _fileName;
        private bool _isBusy;

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

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public void Initialize(OrderDto orderDto)
        {
            _fileName = null;
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

        private async void DownloadDataFileCommandHandler()
        {
            var filter = "Все файлы (*.*)|*.*";
            if (_dialogAgent.ShowSaveFileDialog(filter, GetFileNameByWorkOrder(_order), out var fileName) == false)
                return;

            IsBusy = true;

            try
            {
                await ThreadTaskExtensions.Run(() =>
                {
                    using (var data = _rmaOperations.GetDataFileByWorkOrderId(_order.WorkOrderId))
                    {
                        using (var fileStream = data.GetFileStream())
                        {
                            _fileManager.Save(fileStream, fileName);
                        }
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GetFileNameByWorkOrder(OrderDto order)
        {
            var fileExtension = Path.GetExtension(order.DataFileName);
            return string.Format("{0}-{1}{2}", order.DocNumber, order.Patient, fileExtension);
        }

        public void AssemblyOrder(OrderDto order)
        {
            order.DataFileName = _fileName;
        }
    }
}