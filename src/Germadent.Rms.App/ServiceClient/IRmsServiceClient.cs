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

        /// <summary>
        /// Запускает работу по заказ-наряду
        /// </summary>
        /// <param name="work"></param>
        /// <param name="lastEditorId"></param>
        void StartWork(WorkDto work, int lastEditorId);

        /// <summary>
        /// Редактирует / закрывает работу по заказ-наряду
        /// </summary>
        /// <param name="work"></param>
        /// <param name="lastEditorId"></param>
        void UpdateWork(WorkDto work, int lastEditorId);

        /// <summary>
        /// Удаляет работу из заказ-наряда
        /// </summary>
        /// <param name="work"></param>
        void DeleteWork(WorkDto work);
    }
}