using System.IO;
using Germadent.Rma.Model;

namespace Germadent.WebApi.Repository
{
    public interface IRmaDbOperations
    {
        /// <summary>
        /// Возвращает заказ наряд по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto GetOrderDetails(int id);

        /// <summary>
        /// Возвращает список заказ нарядов по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OrderLiteDto[] GetOrders(OrdersFilter filter);

        /// <summary>
        /// Добавляет заказ наряд
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stream"></param>
        OrderDto AddOrder(OrderDto order);

        /// <summary>
        /// Присоединяет файл к созданному заказнаряду
        /// </summary>
        void AttachDataFileToOrder(int id, string fileName, Stream stream);

        /// <summary>
        /// Возвращает путь к файлу привязанному к заказнаряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetFileByWorkOrder(int id);

        /// <summary>
        /// Обновляет заказ наряд
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stream"></param>
        void UpdateOrder(OrderDto order, Stream stream);

        /// <summary>
        /// Закрывает заказ-наряд по id
        /// </summary>
        /// <param name="id"></param>
        OrderDto CloseOrder(int id);

        /// <summary>
        /// Возвращает словарь
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

        /// <summary>
        /// Возвращает список свойств для вставки в Excel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportListDto[] GetWorkReport(int id);

        /// <summary>
        /// Возвращает список клиентов по вхождению в наименование
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        CustomerDto[] GetCustomers(string name);

        /// <summary>
        /// Добавляет заказчика
        /// </summary>
        /// <param name="customer"></param>
        CustomerDto AddCustomer(CustomerDto customer);

        /// <summary>
        /// Обновляет данные заказчика
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        CustomerDto UpdateCustomer(CustomerDto customer);

        /// <summary>
        /// Возвращает спиок ответственных лиц по Id заказчика
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ResponsiblePersonDto[] GetResponsiblePersons();

        /// <summary>
        /// Добавляет ответственное лицо
        /// </summary>
        /// <returns></returns>
        ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePerson);

        /// <summary>
        /// Удаляет заказчика по его Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        CustomerDeleteResult DeleteCustomer(int customerId);

        /// <summary>
        /// Обновляет данные по ответственному лицу
        /// </summary>
        /// <param name="responsiblePerson"></param>
        /// <returns></returns>
        ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson);

        /// <summary>
        /// Удаляет ответственное лицо по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponsiblePersonDeleteResult DeleteResponsiblePerson(int id);
    }
}