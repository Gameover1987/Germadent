using System.ComponentModel;

namespace Germadent.UI.ViewModels.Validation
{
    public interface IValidationSupportableViewModel : INotifyDataErrorInfo
    {
        void Validate();
    }
}