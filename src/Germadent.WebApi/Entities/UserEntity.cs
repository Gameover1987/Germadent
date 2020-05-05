namespace Germadent.WebApi.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public RoleEntity[] Roles { get; set; }
    }

    public class RoleEntity
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public RightEntity[] Rights { get; set; }
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
    }
}
