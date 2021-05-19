using Germadent.Model;

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
    }
}