using Germadent.Rma.Model;

namespace Germadent.Rma.App.Operations
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
        /// Обновить данные заказчика
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        CustomerDto UpdateCustomer(CustomerDto customer);

        /// <summary>
        /// Добавить ответственное лицо
        /// </summary>
        /// <param name="responsiblePersonDto"></param>
        /// <returns></returns>
        ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto);
    }
}