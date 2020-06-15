namespace Germadent.UserManagementCenter.Model
{
    public class UserDto
    {

        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public RoleDto[] Roles { get; set; }
    }
}
