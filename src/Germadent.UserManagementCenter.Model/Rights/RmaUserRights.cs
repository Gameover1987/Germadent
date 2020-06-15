using System.ComponentModel;

namespace Germadent.UserManagementCenter.Model.Rights
{
    /// <summary>
    /// Права для РМА
    /// </summary>
    [Description("Рабочее место администратора")]
    public class RmaUserRights : UserRightsBase
    {
        [Description("Запуск приложения")]
        public const string RunApplication = "Germadent.Rma.RunApplication";

        [Description("Редактирование данных заказнаряда")]
        public const string EditOrders = "Germadent.Rma.EditOrders";
    }
}