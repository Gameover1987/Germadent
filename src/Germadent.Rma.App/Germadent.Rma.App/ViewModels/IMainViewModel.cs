using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels
{
    public interface IMainViewModel
    {
        IDelegateCommand OpenOrderCommand { get; }
    }
}