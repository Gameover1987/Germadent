using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardStepViewModel
    {
        string DisplayName { get; }

        bool IsValid { get; }

        void Initialize(OrderDto order);

        /// <summary>
        /// Дополняет уже созданный заказ-наряд нужными данными
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        void AssemblyOrder(OrderDto order);
    }
}