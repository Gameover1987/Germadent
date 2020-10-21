using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.Entities
{
    public class AuthorizationInfoEntity
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string FullName { get; set; }

        public bool IsLocked { get; set; }

        public int RightId { get; set; }

        public string RightName { get; set; }

        public string ApplicationName { get; set; }

        public ApplicationModule ApplicationModule { get; set; }
    }

    public class UserAndRoleEntity
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

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

        public string RightDescription { get; set; }

        public string ApplicationName { get; set; }

        public ApplicationModule ApplicationModule { get; set; }
    }

    public class RightEntity
    {
        public int RightId { get; set; }

        /// <summary>
        /// Название права
        /// </summary>
        public string RightName { get; set; }

        /// <summary>
        /// Описание права
        /// </summary>
        public string RightDescription { get; set; }

        /// <summary>
        /// Название подсистемы
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Идентификатор подсистемы
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Признак того что право устарело и использовать его больше нельзя
        /// </summary>
        public bool IsObsolete { get; set; }
    }
}
