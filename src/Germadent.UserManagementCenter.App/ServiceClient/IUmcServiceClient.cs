using Germadent.Model;
using Germadent.Model.Rights;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    /// <summary>
    /// Операции ЦУП
    /// </summary>
    public interface IUmcServiceClient
    {
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        void Authorize(string login, string password);

        AuthorizationInfoDto AuthorizationInfo { get; }

        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        UserDto[] GetUsers();

        /// <summary>
        /// Добавляет пользователя
        /// </summary>
        /// <returns></returns>
        UserDto AddUser(UserDto userDto);

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <returns></returns>
        UserDto EditUser(UserDto userDto);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(int userId);

        /// <summary>
        /// Возвращает список ролей
        /// </summary>
        /// <returns></returns>
        RoleDto[] GetRoles();

        /// <summary>
        /// Добавляет роль
        /// </summary>
        /// <returns></returns>
        RoleDto AddRole(RoleDto role);

        /// <summary>
        /// Редактирование роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        RoleDto EditRole(RoleDto role);

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="roleId"></param>
        void DeleteRole(int roleId);

        /// <summary>
        /// Возвращает список всех прав
        /// </summary>
        /// <returns></returns>
        RightDto[] GetRights();

        /// <summary>
        /// Возвращает список прав для конкртеной роли
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RightDto[] GetRightsByRole(int roleId);

        /// <summary>
        /// Возвращает изображение пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        byte[] GetUserImage(int userId);
    }
}
