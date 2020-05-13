using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Infrastructure
{
    public interface ICatalogUIOperations
    {
        /// <summary>
        /// Добавить заказчика
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        CustomerDto AddCustomer(CustomerDto customer);

        /// <summary>
        /// Добавить ответственное лицо
        /// </summary>
        /// <param name="responsiblePersonDto"></param>
        /// <returns></returns>
        ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto);
    }
}