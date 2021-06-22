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
        WorkDto[] GetRelevantOperations(int workOrderId);

        /// <summary>
        /// Возвращает набор  выполняющихся работ по заказ-наряду
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        WorkDto[] GetWorksInProgressByWorkOrder(int workOrderId);

        /// <summary>
        /// Запускает работу по заказ-наряду
        /// </summary>
        /// <param name="works"></param>
        void StartWorks(WorkDto[] works);

        /// <summary>
        /// Подтверждает выполнение работ по заказ-наряду
        /// </summary>
        /// <param name="works"></param>
        void FinishWorks(WorkDto[] works);

        /// <summary>
        /// Провести контроль качества
        /// </summary>
        /// <param name="workOrderId"></param>
        void PerformQualityControl(int workOrderId);
    }
}