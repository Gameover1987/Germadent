using System.ComponentModel;

namespace Germadent.UserManagementCenter.Model.Rights
{
    /// <summary>
    /// Права для ЦУП
    /// </summary>
    [Description("Центр управления пользователями")]
    public class UmcUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения", ApplicationModule.Umc)]
        public const string RunApplication = "Germadent.UserManagementCenter.RunApplication";
        
        [ApplicationRight("Редактирование ролей и пользователей", ApplicationModule.Umc)]
        public const string EditRolesAndUsers = "Germadent.UserManagementCenter.EditRolesAndUsers";
    }
}