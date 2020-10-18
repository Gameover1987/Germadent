using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    /// <summary>
    /// Операции ЦУП
    /// </summary>
    public interface IUmcServiceClient
    {
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        UserDto[] GetUsers();

        /// <summary>
        /// Возвращает пользователя по его Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserDto GetUserById(int  id);

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
    }
}
