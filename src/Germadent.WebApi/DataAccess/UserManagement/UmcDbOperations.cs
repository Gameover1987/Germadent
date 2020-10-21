using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.UserManagement
{
    public class UmcDbOperations : IUmcDbOperations
    {
        private readonly IServiceConfiguration _configuration;
        private readonly IUmcEntityConverter _converter;

        public UmcDbOperations(IServiceConfiguration configuration, IUmcEntityConverter converter)
        {
            _configuration = configuration;
            _converter = converter;

            ActualizeRights();
        }

        public UserDto[] GetUsers()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "select * from umc_GetUsersAndRoles()";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var userAndRolesEntities = new List<UserAndRoleEntity>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new UserAndRoleEntity();
                            entity.UserId = reader[nameof(entity.UserId)].ToInt();
                            entity.FirstName = reader[nameof(entity.FirstName)].ToString();
                            entity.Surname = reader["FamilyName"].ToString();
                            entity.Patronymic = reader[nameof(entity.Patronymic)].ToString();
                            entity.Phone = reader[nameof(entity.Phone)].ToString();
                            entity.Login = reader[nameof(entity.Login)].ToString();
                            entity.Description = reader[nameof(entity.Description)].ToString();
                            entity.Password = reader[nameof(entity.Password)].ToString();
                            entity.RoleName = reader[nameof(entity.RoleName)].ToString();
                            entity.RoleId = reader[nameof(entity.RoleId)].ToInt();

                            userAndRolesEntities.Add(entity);
                        }
                    }

                    var usersWithRoles = new List<UserDto>();
                    var groupings = userAndRolesEntities.GroupBy(x => x.UserId);
                    foreach (var grouping in groupings)
                    {
                        var userDto = new UserDto();
                        userDto.UserId = grouping.First().UserId;
                        userDto.Description = grouping.First().Description;
                        userDto.FirstName = grouping.First().FirstName;
                        userDto.Surname = grouping.First().Surname;
                        userDto.Patronymic = grouping.First().Patronymic;
                        userDto.Phone = grouping.First().Phone;
                        userDto.Login = grouping.First().Login;
                        userDto.Password = grouping.First().Password;
                        var roles = new List<RoleDto>();
                        foreach (var userAndRoleEntity in grouping)
                        {
                            roles.Add(new RoleDto
                            {
                                RoleId = userAndRoleEntity.RoleId,
                                RoleName = userAndRoleEntity.RoleName
                            });
                        }

                        userDto.Roles = roles.ToArray();
                        usersWithRoles.Add(userDto);
                    }

                    return usersWithRoles.ToArray();
                }
            }
        }

        public UserDto GetUserById(int id)
        {
            return null;
        }

        public UserDto AddUser(UserDto userDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("umc_AddUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@familyName", SqlDbType.NVarChar)).Value = userDto.Surname;
                    command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar)).Value = userDto.FirstName;
                    command.Parameters.Add(new SqlParameter("@patronymic", SqlDbType.NVarChar)).Value = userDto.Patronymic;
                    command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar)).Value = userDto.Phone;
                    command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar)).Value = userDto.Login;
                    command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar)).Value = userDto.Password;
                    command.Parameters.Add(new SqlParameter("@isLocked", SqlDbType.Bit)).Value = userDto.IsLocked;
                    command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar)).Value = userDto.Description;
                    command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    userDto.UserId = command.Parameters["@userId"].Value.ToInt();

                    if (userDto.Roles.Any())
                    {
                        UpdateUserImpl(userDto, connection);
                    }
                }

                return userDto;
            }
        }

        private void UpdateUserImpl(UserDto userDto, SqlConnection connection)
        {
            var rolesJson = userDto.Roles
                .Select(x => new { userDto.UserId, x.RoleId })
                .ToArray()
                .SerializeToJson(formatting: Formatting.Indented);

            using (var command = new SqlCommand("umc_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userId ", SqlDbType.Int)).Value = userDto.UserId;
                command.Parameters.Add(new SqlParameter("@familyName", SqlDbType.NVarChar)).Value = userDto.Surname;
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar)).Value = userDto.FirstName;
                command.Parameters.Add(new SqlParameter("@patronymic", SqlDbType.NVarChar)).Value = userDto.Patronymic;
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar)).Value = userDto.Phone;
                command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar)).Value = userDto.Login;
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar)).Value = userDto.Password;
                command.Parameters.Add(new SqlParameter("@isLocked", SqlDbType.Bit)).Value = userDto.IsLocked;
                command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar)).Value = userDto.Description;
                command.Parameters.Add(new SqlParameter("@jsonString", SqlDbType.NVarChar)).Value = rolesJson;

                command.ExecuteNonQuery();
            }
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                UpdateUserImpl(userDto, connection);

                return userDto;
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("umc_DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@userId ", SqlDbType.NVarChar)).Value = userId;

                    command.ExecuteNonQuery();
                }
            }
        }

        public RoleDto[] GetRoles()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "select * from umc_GetRolesWithRights()";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var rolesAndRights = new List<RoleAndRightEntity>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var roleAndRightEntity = new RoleAndRightEntity();
                            roleAndRightEntity.RoleId = reader[nameof(roleAndRightEntity.RoleId)].ToInt();
                            roleAndRightEntity.RoleName = reader[nameof(roleAndRightEntity.RoleName)].ToString();
                            roleAndRightEntity.RightId = reader[nameof(roleAndRightEntity.RightId)].ToInt();
                            roleAndRightEntity.RightName = reader[nameof(roleAndRightEntity.RightName)].ToString();
                            roleAndRightEntity.ApplicationName = reader[nameof(roleAndRightEntity.ApplicationName)].ToString();
                            roleAndRightEntity.ApplicationModule = (ApplicationModule)reader["ApplicationId"].ToInt();

                            rolesAndRights.Add(roleAndRightEntity);
                        }
                    }

                    var rolesWithRights = new List<RoleDto>();
                    var groupings = rolesAndRights.GroupBy(x => x.RoleId);
                    foreach (var grouping in groupings)
                    {
                        var roleDto = new RoleDto();
                        roleDto.RoleId = grouping.First().RoleId;
                        roleDto.RoleName = grouping.First().RoleName;
                        var rights = new List<RightDto>();
                        foreach (var roleAndRightEntity in grouping)
                        {
                            rights.Add(new RightDto
                            {
                                RightId = roleAndRightEntity.RightId,
                                ApplicationModule = roleAndRightEntity.ApplicationModule,
                                RightName = roleAndRightEntity.RightName
                            });
                        }

                        roleDto.Rights = rights.ToArray();
                        rolesWithRights.Add(roleDto);
                    }

                    return rolesWithRights.ToArray();
                }
            }
        }

        public RoleDto AddRole(RoleDto roleDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("umc_AddRole", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@roleName ", SqlDbType.NVarChar)).Value = roleDto.RoleName;
                    command.Parameters.Add(new SqlParameter("@roleID", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    roleDto.RoleId = command.Parameters["@roleID"].Value.ToInt();

                    if (roleDto.Rights.Any())
                    {
                        UpdateRoleIml(roleDto, connection);
                    }
                }

                return roleDto;
            }
        }

        private void UpdateRoleIml(RoleDto roleDto, SqlConnection connection)
        {
            var rightsJson = roleDto.Rights
                .Select(x => new { roleDto.RoleId, x.RightId })
                .ToArray()
                .SerializeToJson(formatting: Formatting.Indented);

            using (var command = new SqlCommand("umc_UpdateRole", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@roleID ", SqlDbType.Int)).Value = roleDto.RoleId;
                command.Parameters.Add(new SqlParameter("@roleName ", SqlDbType.NVarChar)).Value = roleDto.RoleName;
                command.Parameters.Add(new SqlParameter("@jsonString ", SqlDbType.VarChar)).Value = rightsJson;

                command.ExecuteNonQuery();
            }
        }

        public RoleDto UpdateRole(RoleDto roleDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                UpdateRoleIml(roleDto, connection);

                return roleDto;
            }
        }

        public void DeleteRole(int roleId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("umc_DeleteRole", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@roleId ", SqlDbType.NVarChar)).Value = roleId;

                    command.ExecuteNonQuery();
                }
            }
        }

        public RightDto[] GetRights()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "select * from umc_GetRights()";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var rights = new List<RightDto>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rightEntity = new RightEntity();
                            rightEntity.RightId = reader[nameof(rightEntity.RightId)].ToInt();
                            rightEntity.RightName = reader[nameof(rightEntity.RightName)].ToString();
                            rightEntity.RightDescription = reader[nameof(rightEntity.RightDescription)].ToString();
                            rightEntity.ApplicationName = reader[nameof(rightEntity.ApplicationName)].ToString();
                            rightEntity.ApplicationId = reader[nameof(rightEntity.ApplicationId)].ToInt();
                            rightEntity.IsObsolete = reader[nameof(rightEntity.IsObsolete)].ToBool();

                            rights.Add(_converter.ConvertToRightDto(rightEntity));
                        }
                    }

                    return rights.ToArray();
                }
            }
        }

        public AuthorizationInfoDto Authorize(string login, string password)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = string.Format("select * from umc_Authorization('{0}', '{1}')", login, password);
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var authorizationInfo = new AuthorizationInfoDto();
                    var authorizartionEntities = new List<AuthorizationInfoEntity>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new AuthorizationInfoEntity();
                            entity.Login = reader["Login"].ToString();
                            entity.UserId = reader["UserId"].ToInt();
                            entity.IsLocked = reader["IsLocked"].ToBool();

                            var firstName = reader["FirstName"].ToString();
                            var familyName = reader["FamilyName"].ToString();
                            var patronymic = reader["Patronymic"].ToString();

                            entity.FullName = string.Format("{0} {1} {2}", familyName, firstName, patronymic);
                            entity.RightId = reader["rightId"].ToInt();
                            entity.RightName = reader["RightName"].ToString();
                            entity.ApplicationName = reader["ApplicationName"].ToString();
                            entity.ApplicationModule = (ApplicationModule)reader["ApplicationId"].ToInt();

                            authorizartionEntities.Add(entity);
                        }
                    }

                    var groupings = authorizartionEntities.GroupBy(x => x.UserId).ToArray();

                    authorizationInfo.UserId = groupings.First().First().UserId;
                    authorizationInfo.FullName = groupings.First().First().FullName;
                    authorizationInfo.Login = groupings.First().First().Login;
                    authorizationInfo.IsLocked = groupings.First().First().IsLocked;

                    var rights = new List<RightDto>();
                    foreach (var authorizationInfoEntity in groupings.First())
                    {
                        rights.Add(new RightDto
                        {
                            RightId = authorizationInfoEntity.RightId,
                            ApplicationModule = authorizationInfoEntity.ApplicationModule,
                            RightName = authorizationInfoEntity.RightName
                        });
                    }

                    authorizationInfo.Rights = rights.ToArray();

                    return authorizationInfo;
                }
            }
        }

        private void ActualizeRights()
        {
            var currrentRightsSet = GetRights();

            var actualRightsSet = UserRightsProvider.GetAllUserRights().ToArray();

            //var oldRights = _dbContext.Rights.ToArray();

            //var newRights = UserRightsProvider.GetAllUserRights().Select(x => new RightEntity
            //{
            //    RightId = x.RightId,
            //    ApplicationName = x.ApplicationName,
            //    RightName = x.RightName
            //}).ToArray();

            //var rightComparer = new RightComparer();

            //var rightsToDelete = oldRights.Except(newRights, rightComparer).ToArray();
            //_dbContext.Rights.RemoveRange(rightsToDelete);

            //var rightsToAdd = newRights.Except(oldRights, rightComparer).ToArray();
            //_dbContext.Rights.AddRange(rightsToAdd);

            //_dbContext.SaveChanges();
        }

        private class RightComparer : IEqualityComparer<RightEntity>
        {
            public bool Equals(RightEntity x, RightEntity y)
            {
                return x.RightName == y.RightName &&
                       x.ApplicationName == y.ApplicationName;
            }

            public int GetHashCode(RightEntity entity)
            {
                return entity.RightName.GetHashCode();
            }
        }
    }
}
