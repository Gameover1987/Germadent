using System.Collections.Generic;
using Germadent.Model;

namespace Germadent.Rms.App.ServiceClient
{
    public interface IRmsServiceClient
    {
        void Authorize(string login, string password);
        OrderLiteDto[] GetOrders(OrdersFilter filter);
        OrderDto GetOrderById(int workOrderId);
        AuthorizationInfoDto AuthorizationInfo { get; set; }

        /// <summary>
        /// Возвращает словарь по его названию
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);
    }
}