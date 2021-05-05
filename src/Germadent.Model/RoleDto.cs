using Germadent.Model.Rights;

namespace Germadent.Model
{
    public class RoleDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public RightDto[] Rights { get; set; }
    }
}