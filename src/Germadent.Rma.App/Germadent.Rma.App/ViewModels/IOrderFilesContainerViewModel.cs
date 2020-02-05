using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrderFilesContainerViewModel
    {
        ICommand UploadStlFileCommand { get; }

        ICommand DownloadStlFileCommand { get; }

        ICommand UploadPhotoCommand { get; }

        ICommand DownloadPhotoCommand { get; }

        void Initialize(OrderDto orderDto);
    }
}
