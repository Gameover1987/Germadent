using System.Collections.ObjectModel;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrderViewModel
    {
        void Initialize(bool isReadOnly);

        ObservableCollection<ToothViewModel> Mouth { get; }

        bool IsReadOnly { get; }
    }
}