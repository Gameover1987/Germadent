using Germadent.Model;
using Germadent.Model.Pricing;

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
        /// Id удаляемого заказчика
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        DeleteResult DeleteCustomer(int customerId);

        /// <summary>
        /// Добавить ответственное лицо
        /// </summary>
        /// <param name="responsiblePersonDto"></param>
        /// <returns></returns>
        ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto);

        /// <summary>
        /// Обновляет данные ответственного лица
        /// </summary>
        /// <param name="responsiblePersonDto"></param>
        /// <returns></returns>
        ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto);

        /// <summary>
        /// Удаляет ответственное лицо
        /// </summary>
        /// <param name="responsiblePersonId"></param>
        /// <returns></returns>
        DeleteResult DeleteResponsiblePerson(int responsiblePersonId);
    }
}