using System.Collections.ObjectModel;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrderViewModel
    {
        void Initialize(bool isReadOnly);

        ObservableCollection<ToothViewModel> Mouth { get; }

        bool IsReadOnly { get; }
    }
}