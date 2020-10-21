using System.ComponentModel;

namespace Germadent.UserManagementCenter.Model.Rights
{
    /// <summary>
    /// Права для ЦУП
    /// </summary>
    [RightGroup(ApplicationModule.Umc)]
    public class UmcUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения")]
        public const string RunApplication = "Germadent.UserManagementCenter.RunApplication";
        
        [ApplicationRight("Редактирование ролей и пользователей")]
        public const string EditRolesAndUsers = "Germadent.UserManagementCenter.EditRolesAndUsers";
    }
}