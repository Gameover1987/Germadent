using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.DataAccessService.Repository
{
    public interface IUmcDbOperations
    {
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        UserDto[] GetUsers();

        /// <summary>
        /// Вовзвращает список ролей
        /// </summary>
        /// <returns></returns>
        RoleDto[] GetRoles();

        /// <summary>
        /// Добавляет роль
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        RoleDto[] AddRole(RoleDto roleDto);

        /// <summary>
        /// Возвращает список прав
        /// </summary>
        /// <returns></returns>
        RightDto[] GetRights();
    }
}
