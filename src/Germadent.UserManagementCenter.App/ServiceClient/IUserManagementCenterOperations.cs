using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    public interface IUserManagementCenterOperations
    {
        UserDto[] GetUsers();
    }

    public class UserManagementCenterOperations : IUserManagementCenterOperations
    {
        public UserDto[] GetUsers()
        {
            return new UserDto[]
            {
                new UserDto
                {
                    Login = "Admin",
                    Description = "Пользователь наделенный исключительными правами! Как говорится 'Админ прежде всего, царь и бог, а уже потом, читак и пидорас'"
                },
                new UserDto
                {
                    Login = "Vyacheslav",
                    Description = "Программист 1"
                },
                new UserDto
                {
                    Login = "Alexey",
                    Description = "Программист 2"
                },
                new UserDto
                {
                    Login = "Vasya",
                    Description = "Какой то Вася ))"
                },
            };
        }
    }
}
