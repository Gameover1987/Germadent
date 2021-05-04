using Germadent.Model;

namespace Germadent.Rma.App.Operations
{
    public interface ICatalogSelectionUIOperations
    {
        /// <summary>
        /// Выбрать заказчика
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        CustomerDto SelectCustomer(string mask);

        /// <summary>
        /// Показать карточку заказчика
        /// </summary>
        /// <param name="customer"></param>
        void ShowCustomerCart(CustomerDto customer);

        /// <summary>
        /// Выбрать ответственное лицо
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        ResponsiblePersonDto SelectResponsiblePerson(string mask);

        /// <summary>
        /// Показать карточку ответственного лица
        /// </summary>
        /// <param name="responsiblePerson"></param>
        void ShowResponsiblePersonCard(ResponsiblePersonDto responsiblePerson);
    }
}