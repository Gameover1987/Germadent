using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.Model
{
    public class AuthorizationInfoDto
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string FullName { get; set; }

        public bool IsLocked { get; set; }

        public RightDto[] Rights { get; set; }
    }

    public class UserDto
    {

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public bool IsLocked { get; set; }

        public RoleDto[] Roles { get; set; }

        public string FileName { get; set; }
    }
}
