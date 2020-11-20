using System;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UserManagementCenter.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class UserAuthorizedEventArgs : EventArgs
    {
        public UserAuthorizedEventArgs(AuthorizationInfoDto info)
        {
            AuthorizationInfo = info;
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }
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
        /// Данные авторизации
        /// </summary>
        AuthorizationInfoDto AuthorizationInfo { get; }

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
        DeleteResult DeleteCustomer(int customerId);

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
        DeleteResult DeleteResponsiblePerson(int responsiblePersonId);

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
        PriceGroupDto[] GetPriceGroups(BranchType branchType);

        /// <summary>
        /// Добавляет ценовую группу
        /// </summary>
        /// <param name="priceGroupDto"></param>
        /// <returns></returns>
        PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto);

        /// <summary>
        /// Обновляет ценовую группу
        /// </summary>
        /// <param name="priceGroupDto"></param>
        /// <returns></returns>
        PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto);

        /// <summary>
        /// Удаляет ценовую группу
        /// </summary>
        /// <param name="priceGroupId"></param>
        /// <returns></returns>
        DeleteResult DeletePriceGroup(int priceGroupId);

        /// <summary>
        /// Возвращает ценовые позиции по выбранному типу филиала
        /// 
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        PricePositionDto[] GetPricePositions(BranchType branchType);

        /// <summary>
        /// Добавляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionDto"></param>
        PricePositionDto AddPricePosition(PricePositionDto pricePositionDto);

        /// <summary>
        /// Обновляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionDto"></param>
        /// <returns></returns>
        PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto);

        /// <summary>
        /// Удаляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionId"></param>
        /// <returns></returns>
        DeleteResult DeletePricePosition(int pricePositionId);

        /// <summary>
        /// Возвращает набор изделий с материалами и ценами для ценовой группы
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        ProductSetForPriceGroupDto[] GetProductSetForPrice(BranchType branchType);
    }
}