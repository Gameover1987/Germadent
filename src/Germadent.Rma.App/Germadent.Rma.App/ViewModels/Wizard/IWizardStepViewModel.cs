using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardStepViewModel
    {
        string DisplayName { get; }

        void Initialize(Order order);

        /// <summary>
        /// Дополняет уже созданный заказ-наряд нужными данными
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        void AssemblyOrder(Order order);
    }
}