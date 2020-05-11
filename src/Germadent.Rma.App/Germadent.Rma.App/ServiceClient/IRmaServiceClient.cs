using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class CustomerRepositoryChangedEventArgs : EventArgs
    {
        public CustomerRepositoryChangedEventArgs(CustomerDto[] addedItems, CustomerDto[] deletedItems)
        {
            AddedItems = addedItems;
            DeletedItems = deletedItems;
        }

        public CustomerDto[] AddedItems { get; }

        public CustomerDto[] DeletedItems { get; }
    }

    public class ResponsiblePersonRepositoryChangedEventArgs : EventArgs
    {
        public ResponsiblePersonRepositoryChangedEventArgs(ResponsiblePersonDto[] addedItems, ResponsiblePersonDto[] deletedItems)
        {
            AddedItems = addedItems;
            DeletedItems = deletedItems;
        }

        public ResponsiblePersonDto[] AddedItems { get; }

        public ResponsiblePersonDto[] DeletedItems { get; }
    }

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
        IFileResponse GetDataFileByWorkOrderId(int id);

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
        /// Возвращает словарь по его названию
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

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