using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Germadent.WebApi.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public List<RoleInUserEntity> RolesInUser { get; set; }
    }

    public class RoleInUserEntity
    {
        [Key]
        public int RoleInUserId { get; set; }

        public int UserEntityId { get; set; }

        public UserEntity UserEntity { get; set; }

        public int RoleEntityId { get; set; }

        public RoleEntity RoleEntity { get; set; }
    }

    public class RoleEntity
    {
        [Key]
        public int RoleId { get; set; }

        public string Name { get; set; }

        public List<RightInRoleEntity> RightsInRole { get; set; }
    }

    public class RightInRoleEntity
    {
        [Key]
        public int RightInRoleId { get; set; }

        public int RoleEntityId { get; set; }

        public RoleEntity RoleEntity { get; set; }

        public int RightEntityId { get; set; }

        public RightEntity RightEntity { get; set; }
    }

    public class RightEntity
    {
        [Key]
        public int RightId { get; set; }

        /// <summary>
        /// Название права
        /// </summary>
        public string RightName { get; set; }

        /// <summary>
        /// Название подсистемы
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
