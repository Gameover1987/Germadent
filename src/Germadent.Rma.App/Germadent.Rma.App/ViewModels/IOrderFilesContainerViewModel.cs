using System.Windows.Input;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrderFilesContainerViewModel
    {
        ICommand UploadFileCommand { get; }

        ICommand DownloadFileCommand { get; }

        bool IsBusy { get; }

        void Initialize(OrderDto orderDto);

        void AssemblyOrder(OrderDto orderDto);
    }
}
