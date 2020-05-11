using System.Windows.Input;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class DesignMockOrderFilesContainerViewModel : IOrderFilesContainerViewModel
    {
        public ICommand UploadFileCommand { get; }
        public ICommand DownloadFileCommand { get; }
        public bool IsBusy => true;

        public void AssemblyOrder(OrderDto orderDto)
        {
            
        }

        public void Initialize(OrderDto orderDto)
        {
            
        }
    }
}