using System.ComponentModel;

namespace Germadent.UserManagementCenter.Model.Rights
{
    /// <summary>
    /// Права для РМА
    /// </summary>
    [RightGroup(ApplicationModule.Rma)]
    public class RmaUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения")]
        public const string RunApplication = "Germadent.Rma.RunApplication";
        
        [ApplicationRight("Редактирование заказ-наряда")]
        public const string EditOrders = "Germadent.Rma.EditOrders";

        [ApplicationRight("Просмотр всех заказ-нарядов, а не только назначенных")]
        public const string ViewAllOrders = "Germadent.Rma.ViewAllOrders";

        [ApplicationRight("Право ебать гусей")]
        public const string FuckGusi = "Germadent.Rma.FuckGusi";
    }
}