using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.UserManagementCenter.Model
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public string[] Roles { get; set; }
    }
}
