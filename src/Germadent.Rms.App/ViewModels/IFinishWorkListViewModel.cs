using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Rms.App.ViewModels
{
    public interface IFinishWorkListViewModel
    {
        void Initialize(OrderDto orderDto);

        WorkDto[] GetWorks();
    }
}