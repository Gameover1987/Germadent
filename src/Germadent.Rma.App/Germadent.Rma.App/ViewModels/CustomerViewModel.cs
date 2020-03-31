using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface ICustomerViewModel
    {
        string DisplayName { get; }

        string Description { get; }
    }

    public class CustomerViewModel : ViewModelBase, ICustomerViewModel
    {
        public string DisplayName { get; }

        public string Description { get; }
    }
}