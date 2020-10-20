using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.DataAccess.UserManagement
{
    public interface IUmcDbOperations
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
        UserDto GetUserById(int id);

        /// <summary>
        /// Добавляет пользователя
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        UserDto AddUser(UserDto userDto);

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="userDto"></param>
        UserDto UpdateUser(UserDto userDto);

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
        /// Обновляет роль
        /// </summary>
        /// <param name="roleDto"></param>
        RoleDto UpdateRole(RoleDto roleDto);

        /// <summary>
        /// Удаляет роль
        /// </summary>
        /// <param name="roleId"></param>
        void DeleteRole(int roleId);

        /// <summary>
        /// Возвращает список прав
        /// </summary>
        /// <returns></returns>
        RightDto[] GetRights();
    }
}
