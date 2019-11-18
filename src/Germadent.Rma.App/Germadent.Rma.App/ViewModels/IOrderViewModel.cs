using System.Collections.ObjectModel;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrderViewModel
    {
        void Initialize(bool isReadOnly);

        ObservableCollection<TeethViewModel> Mouth { get; }

        bool IsReadOnly { get; }
    }
}