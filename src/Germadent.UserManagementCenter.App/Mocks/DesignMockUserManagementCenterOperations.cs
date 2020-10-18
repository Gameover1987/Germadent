using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.Mocks
{
    public class DesignMockUserManagementCenterOperations : IUmcServiceClient
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
                    Roles = new []{new RoleDto{Name =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Dmitriy",
                    FullName = "Дмитрий",
                    Description = "Руководитель",
                    Roles = new []{new RoleDto{Name =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vyacheslav",
                    FullName = "Некрасов Вячеслав",
                    Description = "Программист 1",
                    Roles = new []{new RoleDto{Name =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Alexey",
                    FullName = "Колосенок Алексей",
                    Description = "Программист 2",
                    Roles = new []{new RoleDto{Name =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vasya",
                    FullName = "Василий Алибабаевич",
                    Description = "Какой то Вася ))",
                    Roles = new []{new RoleDto{Name =   "Admin"}}
                },
            };

            for (int i = 0; i < users.Length; i++)
            {
                users[i].UserId = i;
            }

            return users;
        }

        public UserDto GetUserById(int id)
        {
            throw new System.NotImplementedException();
        }

        public UserDto AddUser(UserDto userDto)
        {
            throw new System.NotImplementedException();
        }

        public UserDto EditUser(UserDto userDto)
        {
            throw new System.NotImplementedException();
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

        public RoleDto AddRole(RoleDto role)
        {
            throw new System.NotImplementedException();
        }

        public RoleDto EditRole(RoleDto role)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteRole(int roleId)
        {
            throw new System.NotImplementedException();
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