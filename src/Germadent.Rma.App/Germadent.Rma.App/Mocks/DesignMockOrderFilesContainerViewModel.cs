using System.Windows.Input;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class DesignMockOrderFilesContainerViewModel : IOrderFilesContainerViewModel
    {
        public ICommand UploadStlFileCommand { get; }
        public ICommand DownloadStlFileCommand { get; }
        public ICommand UploadPhotoCommand { get; }
        public ICommand DownloadPhotoCommand { get; }

        public void Initialize(OrderDto orderDto)
        {
            
        }
    }
}