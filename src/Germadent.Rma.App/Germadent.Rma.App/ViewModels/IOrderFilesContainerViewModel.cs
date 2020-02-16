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
        ICommand UploadFileCommand { get; }

        ICommand DownloadFileCommand { get; }

        void Initialize(OrderDto orderDto);

        void AssemblyOrder(OrderDto orderDto);
    }
}
