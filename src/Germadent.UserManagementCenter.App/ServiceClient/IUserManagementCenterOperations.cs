using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    /// <summary>
    /// Операции ЦУП
    /// </summary>
    public interface IUserManagementCenterOperations
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
        /// Возвращает список всех прав
        /// </summary>
        /// <returns></returns>
        RightDto[] GetAllRights();

        /// <summary>
        /// Возвращает список прав для конкртеной роли
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RightDto[] GetRightsByRole(int roleId);
    }
}
