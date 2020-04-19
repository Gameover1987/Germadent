using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    public class UserManagementCenterOperations : IUserManagementCenterOperations
    {
        public UserDto[] GetUsers()
        {
            var users = new UserDto[]
            {
                new UserDto
                {
                    Login = "Admin",
                    FullName = "Админ Админыч",
                    Description = "Пользователь наделенный исключительными правами! Как говорится 'Админ прежде всего, царь и бог, а уже потом, читак и пидорас'",
                    Roles = new []{"Admin"}
                },
                new UserDto
                {
                    Login = "Dmitriy",
                    FullName = "Дмитрий",
                    Description = "Руководитель",
                    Roles = new []{"Admin", "Оператор", "Техник", "Руководитель" }
                },
                new UserDto
                {
                    Login = "Vyacheslav",
                    FullName = "Некрасов Вячеслав",
                    Description = "Программист 1",
                    Roles = new []{"Admin",}
                },
                new UserDto
                {
                    Login = "Alexey",
                    FullName = "Колосенок Алексей",
                    Description = "Программист 2",
                    Roles = new []{"Admin", "Оператор", "Техник", "Руководитель" }
                },
                new UserDto
                {
                    Login = "Vasya",
                    FullName = "Василий Алибабаевич",
                    Description = "Какой то Вася ))",
                    Roles = new []{"Лишенец" }
                },
            };

            for (int i = 0; i < users.Length; i++)
            {
                users[i].UserId = i;
            }

            return users;
        }

        public RoleDto[] GetRoles()
        {
            var roles = new RoleDto[]
            {
                new RoleDto
                {
                    Name = "Admin",
                },
                new RoleDto
                {
                    Name = "Руководитель",
                },
                new RoleDto
                {
                    Name = "Администратор",
                },
                new RoleDto
                {
                    Name = "Оператор",
                },
                new RoleDto
                {
                    Name = "Техник",
                },
            };

            for (int i = 0; i < roles.Length; i++)
            {
                roles[i].RoleId = i;
            }

            return roles;
        }

        public RightDto[] GetRights()
        {
            var rights = UserRightsProvider.GetAllUserRights();
            return rights;
        }

        public RightDto[] GetRightsByRole(int roleId)
        {
            var rights = UserRightsProvider.GetAllUserRights();
            return new RightDto[0];
        }
    }
}