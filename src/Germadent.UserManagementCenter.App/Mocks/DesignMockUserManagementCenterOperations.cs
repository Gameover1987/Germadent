using Germadent.Model;
using Germadent.Model.Rights;
using Germadent.UserManagementCenter.App.ServiceClient;

namespace Germadent.UserManagementCenter.App.Mocks
{
    public class DesignMockUserManagementCenterOperations : IUmcServiceClient
    {
        public void Authorize(string login, string password)
        {
            throw new System.NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }

        public UserDto[] GetUsers()
        {
            var users = new UserDto[]
            {
                new UserDto
                {
                    Login = "Admin",
                    FirstName = "Админ Админыч",
                    Phone = "+5-222-333-4449",
                    Description = "Пользователь наделенный исключительными правами! Как говорится 'Админ прежде всего, царь и бог, а уже потом, читак и пидорас'",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Dmitriy",
                    FirstName = "Дмитрий",
                    Phone = "+5-222-333-4449",
                    Description = "Руководитель",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vyacheslav",
                    FirstName = "Некрасов Вячеслав",
                    Phone = "+5-222-333-4449",
                    Description = "Программист 1",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Alexey",
                    FirstName = "Колосенок Алексей",
                    Description = "Программист 2",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vasya",
                    FirstName = "Василий Алибабаевич",
                    Phone = "+5-222-333-4449",
                    Description = "Какой то Вася ))",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Admin",
                    FirstName = "Админ Админыч",
                    Phone = "+5-222-333-4449",
                    Description = "Пользователь наделенный исключительными правами! Как говорится 'Админ прежде всего, царь и бог, а уже потом, читак и пидорас'",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Dmitriy",
                    FirstName = "Дмитрий",
                    Phone = "+5-222-333-4449",
                    Description = "Руководитель",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vyacheslav",
                    FirstName = "Некрасов Вячеслав",
                    Description = "Программист 1",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Alexey",
                    FirstName = "Колосенок Алексей",
                    Description = "Программист 2",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vasya",
                    FirstName = "Василий Алибабаевич",
                    Description = "Какой то Вася ))",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Admin",
                    FirstName = "Админ Админыч",
                    Description = "Пользователь наделенный исключительными правами! Как говорится 'Админ прежде всего, царь и бог, а уже потом, читак и пидорас'",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Dmitriy",
                    FirstName = "Дмитрий",
                    Description = "Руководитель",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vyacheslav",
                    FirstName = "Некрасов Вячеслав",
                    Description = "Программист 1",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Alexey",
                    FirstName = "Колосенок Алексей",
                    Description = "Программист 2",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
                },
                new UserDto
                {
                    Login = "Vasya",
                    FirstName = "Василий Алибабаевич",
                    Description = "Какой то Вася ))",
                    Roles = new []{new RoleDto{RoleName =   "Admin"}}
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

        public void DeleteUser(int userId)
        {
            throw new System.NotImplementedException();
        }


        public RoleDto[] GetRoles()
        {
            var roles = new RoleDto[]
            {
                new RoleDto
                {
                    RoleName = "Admin",
                    Rights = new RightDto[0]
                },
                new RoleDto
                {
                    RoleName = "Руководитель",
                    Rights = new RightDto[0]
                },
                new RoleDto
                {
                    RoleName = "Администратор",
                    Rights = new RightDto[0]
                },
                new RoleDto
                {
                    RoleName = "Оператор",
                    Rights = new RightDto[0]
                },
                new RoleDto
                {
                    RoleName = "Техник",
                    Rights = new RightDto[0]
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

        public byte[] GetUserImage(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}