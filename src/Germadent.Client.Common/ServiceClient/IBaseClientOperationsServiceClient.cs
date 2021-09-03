using System;
using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Client.Common.ServiceClient
{
    /// <summary>
    /// Сервис клиент с поддержкой базовых операций по работе с заказ-нарядами
    /// </summary>
    public interface IBaseClientOperationsServiceClient : IAuthSupportableServiceClient
    {
        /// <summary>
        /// Возвращает список заказ-нарядов по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OrderLiteDto[] GetOrders(OrdersFilter filter);

        /// <summary>
        /// Возвращает заказ-наряд блокируя его при этом
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        OrderScope GetOrderById(int workOrderId);

        /// <summary>
        /// Разблокирует заказ-наряд
        /// </summary>
        /// <param name="workOrderId"></param>
        void UnLockOrder(int workOrderId);

        /// <summary>
        /// Возвращает словарь по его названию
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

        /// <summary>
        /// Возвращает шаблон документа определенного типа
        /// </summary>
        /// <param name="documentTemplateType"></param>
        /// <returns></returns>
        byte[] GetTemplate(DocumentTemplateType documentTemplateType);

        /// <summary>
        /// Возвращает список всех работ по заказ-наряду
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        WorkDto[] GetWorksByWorkOrder(int workOrderId);

        /// <summary>
        /// Возвращает список всех пользователей
        /// </summary>
        /// <returns></returns>
        UserDto[] GetAllUsers();

        /// <summary>
        /// Возвращает список выполненных сотрудником работ за период
        /// </summary>
        /// <returns></returns>
        WorkDto[] GetSalaryReport(int? userId, DateTime dateFrom, DateTime dateTo);
    }
}