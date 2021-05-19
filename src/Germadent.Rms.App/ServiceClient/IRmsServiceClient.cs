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