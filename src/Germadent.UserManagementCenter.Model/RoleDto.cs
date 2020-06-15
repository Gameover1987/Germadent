using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.Model
{
    public class RoleDto
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public RightDto[] Rights { get; set; }
    }
}