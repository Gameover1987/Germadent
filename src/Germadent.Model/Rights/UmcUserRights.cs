namespace Germadent.Model.Rights
{
    /// <summary>
    /// Права для ЦУП
    /// </summary>
    [RightGroup(ApplicationModule.Umc)]
    public class UmcUserRights : UserRightsBase
    {
        [ApplicationRight("Запуск приложения")]
        public const string RunApplication = "Germadent.UserManagementCenter.RunApplication";
        
        [ApplicationRight("Редактирование пользователей")]
        public const string EditUsers = "Germadent.UserManagementCenter.EditUsers";

        [ApplicationRight("Редактирование ролей")]
        public const string EditRoles = "Germadent.UserManagementCenter.EditRoles";
    }
}