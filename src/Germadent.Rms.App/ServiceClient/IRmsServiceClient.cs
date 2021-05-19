using Germadent.Client.Common.ServiceClient;
using Germadent.Model.Production;

namespace Germadent.Rms.App.ServiceClient
{
    public interface IRmsServiceClient : IBaseClientOperationsServiceClient
    {
        /// <summary>
        /// Возвращает набор технологических операций по заказ-наряду доступных пользователю
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        TechnologyOperationByUserDto[] GetRelevantWorkListByWorkOrder(int workOrderId);

        /// <summary>
        /// Запускает прогресс по работам заказнаряда
        /// </summary>
        /// <param name="works"></param>
        void StartWorks(WorkDto[] works);
    }
}