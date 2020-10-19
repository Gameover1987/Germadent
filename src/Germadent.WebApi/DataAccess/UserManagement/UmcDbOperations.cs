﻿using System.Collections.Generic;
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

                var cmdText = "select * from umc_GetUsersRolesAndEmployees()";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                        }
                    }
                }
            }
        }

        public UserDto GetUserById(int id)
        {
            return null;
        }

        public UserDto AddUser(UserDto userDto)
        {
            return null;
        }

        public void UpdateUser(UserDto userDto)
        {

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

                            rolesAndRights.Add(roleAndRightEntity);
                        }
                    }

                    var rolesWithRights = new List<RoleDto>();
                    var groupings = rolesAndRights.GroupBy(x => x.RoleId);
                    foreach (var grouping in groupings)
                    {
                        var roleDto = new RoleDto();
                        roleDto.RoleId = grouping.First().RoleId;
                        roleDto.Name = grouping.First().RoleName;
                        var rights = new List<RightDto>();
                        foreach (var roleAndRightEntity in grouping)
                        {
                            rights.Add(new RightDto
                            {
                                RightId = roleAndRightEntity.RightId,
                                ApplicationName = roleAndRightEntity.ApplicationName,
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
                    command.Parameters.Add(new SqlParameter("@roleName ", SqlDbType.NVarChar)).Value = roleDto.Name;
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
                command.Parameters.Add(new SqlParameter("@roleID ", SqlDbType.NVarChar)).Value = roleDto.RoleId;
                command.Parameters.Add(new SqlParameter("@roleName ", SqlDbType.NVarChar)).Value = roleDto.Name;
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
                            rightEntity.ApplicationName = reader[nameof(rightEntity.ApplicationName)].ToString();

                            rights.Add(_converter.ConvertToRightDto(rightEntity));
                        }
                    }

                    return rights.ToArray();
                }
            }
        }

        private void ActualizeRights()
        {
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
