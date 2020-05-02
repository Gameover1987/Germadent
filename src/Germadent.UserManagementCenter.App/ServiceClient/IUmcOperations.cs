using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    /// <summary>
    /// Операции ЦУП
    /// </summary>
    public interface IUmcOperations
    {
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        UserDto[] GetUsers();

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
