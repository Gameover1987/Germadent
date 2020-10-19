using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Germadent.WebApi.Entities
{
    public class UserAndRoleEntity
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }

    public class RoleAndRightEntity
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int RightId { get; set; }

        public string RightName { get; set; }

        public string ApplicationName { get; set; }
    }

    public class RightEntity
    {
        public int RightId { get; set; }

        /// <summary>
        /// Название права
        /// </summary>
        public string RightName { get; set; }

        /// <summary>
        /// Название подсистемы
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Описание подсистемы
        /// </summary>
        public string ApplicationDescription { get; set; }
    }
}
