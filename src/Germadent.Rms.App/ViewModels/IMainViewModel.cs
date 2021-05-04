using Germadent.Client.Common.ViewModels;
using Germadent.UI.Commands;

namespace Germadent.Rms.App.ViewModels
{
    public interface IMainViewModel
    {
        IDelegateCommand OpenOrderCommand { get; }

        OrderLiteViewModel SelectedOrder { get; }

        void Initialize();
    }
}