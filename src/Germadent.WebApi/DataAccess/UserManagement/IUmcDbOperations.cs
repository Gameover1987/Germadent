using System.IO;
using Germadent.Model;
using Germadent.Model.Rights;

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
        /// Удаляет пользователя
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(int userId);

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

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AuthorizationInfoDto Authorize(string login, string password);

        /// <summary>
        /// Возвращает путь к файлу привязанному к заказнаряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetUserImage(int userId);

        /// <summary>
        /// Добавляет картинку для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        void SetUserImage(int userId, string fileName, Stream stream);
    }
}
