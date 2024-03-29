﻿using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Model.Production;
using Germadent.Model.Rights;

namespace Germadent.Model
{
    public class AuthorizationInfoDto
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public bool IsLocked { get; set; }

        public RightDto[] Rights { get; set; }

        public string Token { get; set; }
    }

    public static class AuthorizationInfoDtoExtensions
    {
        /// <summary>
        /// ФИО (полностью)
        /// </summary>
        /// <param name="authorizationInfo"></param>
        /// <returns></returns>
        public static string GetFullName(this AuthorizationInfoDto authorizationInfo)
        {
            return string.Format("{0} {1} {2}", authorizationInfo.LastName, authorizationInfo.FirstName, authorizationInfo.Patronymic);
        }

        /// <summary>
        /// ФИО (полностью)
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public static string GetFullName(this UserDto userDto)
        {
            return string.Format("{0} {1} {2}", userDto.Surname, userDto.FirstName, userDto.Patronymic);
        }

        /// <summary>
        /// ФИО (с инициалами)
        /// </summary>
        /// <returns></returns>
        public static string GetShortFullName(this AuthorizationInfoDto authorizationInfo)
        {
            if (authorizationInfo.FirstName.IsNullOrEmpty() ||
                authorizationInfo.LastName.IsNullOrEmpty())
            {
                return authorizationInfo.LastName;
            }

            return string.Format("{0} {1}.{2}.", authorizationInfo.LastName, authorizationInfo.FirstName.First(), authorizationInfo.Patronymic.First());
        }
    }

    public enum EmployeePosition
    {
        Admin = 1,
        Modeller = 2,
        Technic = 3,
        Operator = 4
    }

    public class UserDto
    {
        public UserDto()
        {
            Positions = new EmployeePositionDto[0];
        }

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

        public EmployeePositionDto[] Positions { get; set; }

        public string FullName => this.GetFullName();

        public bool IsEmployee => Positions.Any();
    }
}
