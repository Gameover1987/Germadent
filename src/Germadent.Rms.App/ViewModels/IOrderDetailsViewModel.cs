using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Rms.App.ViewModels
{
    public interface IOrderDetailsViewModel
    {
        void Initialize(int workOrderId);
    }
}