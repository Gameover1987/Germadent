using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом данных РМА
    /// </summary>
    public interface IRmaServiceClient
    {
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        void Authorize(string user, string password);

        /// <summary>
        /// Получить список заказнарядов
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OrderLiteDto[] GetOrders(OrdersFilter filter);

        /// <summary>
        /// Получить детали по выбранному заказнаряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto GetOrderById(int id);

        /// <summary>
        /// Возвращает файл по заказ наряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        byte[] GetDataFileByWorkOrderId(int id);

        /// <summary>
        /// Добавить заказнаряд
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        OrderDto AddOrder(OrderDto order);

        /// <summary>
        /// Обновить заказ-наряд
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        OrderDto UpdateOrder(OrderDto order);

        /// <summary>
        /// Закрыть заказ-наряд
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto CloseOrder(int id);

        /// <summary>
        /// Скопировать данные заказ-наряда в буфер обмена
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportListDto[] GetWorkReport(int id);

        /// <summary>
        /// Возвращает список заказчиков
        /// </summary>
        /// <returns></returns>
        CustomerDto[] GetCustomers(string mask);

        /// <summary>
        /// Добавление заказчика
        /// </summary>
        CustomerDto AddCustomer(CustomerDto сustomerDto);

        /// <summary>
        /// Обновляет данные заказчика
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        CustomerDto UpdateCustomer(CustomerDto customerDto);

        /// <summary>
        /// Удаляет заказчика по его Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        CustomerDeleteResult DeleteCustomer(int customerId);

        /// <summary>
        /// Возвращает список ответственных лиц по Id заказчика
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ResponsiblePersonDto[] GetResponsiblePersons();

        /// <summary>
        /// Добавление ответственного лица
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
        /// Удаляет ответственное лицо по Id
        /// </summary>
        /// <param name="responsiblePersonId"></param>
        /// <returns></returns>
        ResponsiblePersonDeleteResult DeleteResponsiblePerson(int responsiblePersonId);

        /// <summary>
        /// Возвращает словарь по его названию
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

        /// <summary>
        /// Возвращает прайс по выбранному типу филиала
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        PriceGroupDto[] GetPrice(BranchType branchType);

        /// <summary>
        /// Собтие изменения репозитория заказчиков
        /// </summary>
        event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;

        /// <summary>
        /// Событие изменения репозитория ответственных лиц
        /// </summary>
        event EventHandler<ResponsiblePersonRepositoryChangedEventArgs> ResponsiblePersonRepositoryChanged;
    }
}