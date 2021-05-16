using System.Collections.Generic;
using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Rms.App.ServiceClient
{
    public interface IRmsServiceClient
    {
        AuthorizationInfoDto AuthorizationInfo { get; set; }

        void Authorize(string login, string password);

        OrderLiteDto[] GetOrders(OrdersFilter filter);

        OrderScope GetOrderById(int workOrderId);

        void UnLockOrder(int workOrderId);

        /// <summary>
        /// Возвращает словарь по его названию
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

        TechnologyOperationByUserDto[] GetRelevantWorkListByWorkOrder(int workOrderId);

        /// <summary>
        /// Запускает прогресс по работам заказнаряда
        /// </summary>
        /// <param name="works"></param>
        void StartWorks(WorkDto[] works);
    }
}