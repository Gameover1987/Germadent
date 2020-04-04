using System.ComponentModel;

namespace Germadent.UserManagementCenter.Model.Rights
{
    /// <summary>
    /// Права для ЦУП
    /// </summary>
    [Description("Центр управления пользователями")]
    public class UmcUserRights : UserRightsBase
    {
        [Description("Запуск приложения")]
        public const string RunApplication = "Germadent.UserManagementCenter.RunApplication";

        [Description("Редактирование ролей и пользователей")]
        public const string EditRolesAndUsers = "Germadent.UserManagementCenter.EditRolesAndUsers";
    }
}