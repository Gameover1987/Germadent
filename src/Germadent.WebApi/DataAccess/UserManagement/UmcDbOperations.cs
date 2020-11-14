using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
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
        private readonly IFileManager _fileManager;
        private readonly string _storageDirectory;

        public UmcDbOperations(IServiceConfiguration configuration, IUmcEntityConverter converter, IFileManager fileManager)
        {
            _configuration = configuration;
            _converter = converter;
            _fileManager = fileManager;
            _storageDirectory = GetFileTableFullPath();

            ActualizeRights();
        }

        private string GetFileTableFullPath()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "select dbo.GetFileTableFullPath()";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var commandResult = command.ExecuteScalar();
                    return commandResult.ToString();
                }
            }
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
                            roleAndRightEntity.RightDescription = reader[nameof(roleAndRightEntity.RightDescription)].ToString();
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
                                RightName = roleAndRightEntity.RightName,
                                RightDescription = roleAndRightEntity.RightDescription
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

        private void UpdateRight(SqlConnection connection, int rightId, string rightDescription, bool isObsolete = false)
        {
            var cmdText = "umc_UpdateRight";
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@rightId ", SqlDbType.NVarChar)).Value = rightId;
                command.Parameters.Add(new SqlParameter("@rightDescription ", SqlDbType.NVarChar)).Value = rightDescription;
                command.Parameters.Add(new SqlParameter("@isObsolete ", SqlDbType.NVarChar)).Value = isObsolete;

                command.ExecuteNonQuery();
            }
        }

        private void AddRight(SqlConnection connection, RightDto right)
        {
            var cmdText = "umc_AddRight";
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@applicationID ", SqlDbType.Int)).Value = (int)right.ApplicationModule;
                command.Parameters.Add(new SqlParameter("@rightName ", SqlDbType.NVarChar)).Value = right.RightName;
                command.Parameters.Add(new SqlParameter("@rightDescription ", SqlDbType.NVarChar)).Value = right.RightDescription;
                command.Parameters.Add(new SqlParameter("@rightID", SqlDbType.Int) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();
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

                    if (authorizartionEntities.Count == 0)
                    {
                        throw new UserNotAuthorizedException( string.Format("Пользователь с логином '{0}' не авторизован!", login));
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
            var rightsFromDb = GetRights();

            var rightsFromCode = UserRightsProvider.GetAllUserRights();

            var rightComparer = new RightComparer();

            var newRights = rightsFromCode.Except(rightsFromDb, rightComparer).ToArray();
            var oldRights = rightsFromDb.Except(rightsFromCode, rightComparer).ToArray();

            var changedRights = new List<RightDto>();
            foreach (var rightFromCode in rightsFromCode)
            {
                var rightToRename = rightsFromDb.FirstOrDefault(x =>
                    x.RightName == rightFromCode.RightName && x.RightDescription != rightFromCode.RightDescription);
                if (rightToRename != null)
                    changedRights.Add(rightToRename);
            }

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                foreach (var oldRight in oldRights)
                {
                    UpdateRight(connection, oldRight.RightId, oldRight.RightDescription, true);
                }

                foreach (var newRight in newRights)
                {
                    AddRight(connection, newRight);
                }

                foreach (var changedRight in changedRights)
                {
                    var rightDescription = rightsFromCode.First(x => x.RightName == changedRight.RightName).RightDescription;
                    UpdateRight(connection, changedRight.RightId, rightDescription);
                }
            }
        }

        public void SetUserImage(int userId, string fileName, Stream stream)
        {
            var resultFileName = Path.Combine(_storageDirectory, fileName);
            var fileInfo = _fileManager.Save(stream, resultFileName);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                LinkFileToUser(userId, fileName, fileInfo.CreationTime, connection);
            }
        }

        private void FillHasDataFile(UserDto user)
        {
            var cmdText = string.Format("select * from GetFileAttributesByWOId({0})", user.UserId);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var dataFileAttributes = new DataFileAttributes();
                    while (reader.Read())
                    {
                        dataFileAttributes.FileName = reader["name"].ToString();
                        dataFileAttributes.StreamId = reader["stream_id"].ToGuid();
                    }

                    if (dataFileAttributes.FileName == null)
                        return;

                    user.FileName = dataFileAttributes.FileName;
                }
            }
        }

        private void LinkFileToUser(int userId, string fileName, DateTime creationTime, SqlConnection connection)
        {
            var cmdText = "AddLink_User_FileStream";
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@fileName", SqlDbType.NVarChar)).Value = fileName;
                command.Parameters.Add(new SqlParameter("@creationTime", SqlDbType.DateTimeOffset)).Value = creationTime;
                command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = userId;

                command.ExecuteNonQuery();
            }
        }

        public string GetUserImage(int userId)
        {
            var cmdText = string.Format("select * from GetFileAttributesByUserId({0})", userId);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var dataFileAttributes = new DataFileAttributes();
                    while (reader.Read())
                    {
                        dataFileAttributes.FileName = reader["name"].ToString();
                        dataFileAttributes.StreamId = reader["stream_id"].ToGuid();
                    }

                    if (dataFileAttributes.FileName == null)
                        return null;

                    var fullPathToDataFile = Path.Combine(_storageDirectory, dataFileAttributes.FileName);
                    return fullPathToDataFile;
                }
            }
        }

        private class RightComparer : IEqualityComparer<RightDto>
        {
            public bool Equals(RightDto x, RightDto y)
            {
                return x.RightName == y.RightName &&
                    x.ApplicationModule == y.ApplicationModule;
            }

            public int GetHashCode(RightDto entity)
            {
                return entity.RightName.GetHashCode();
            }
        }
    }
}
