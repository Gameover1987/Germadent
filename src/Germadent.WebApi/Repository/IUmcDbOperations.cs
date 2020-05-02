using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.Repository
{
    public interface IUmcDbOperations
    {
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns></returns>
        UserDto[] GetUsers();

        /// <summary>
        /// Добавляет пользователя
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        UserDto AddUser(UserDto userDto);

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
        RoleDto AddRole(RoleDto roleDto);

        /// <summary>
        /// Возвращает список прав
        /// </summary>
        /// <returns></returns>
        RightDto[] GetRights();
    }
}
