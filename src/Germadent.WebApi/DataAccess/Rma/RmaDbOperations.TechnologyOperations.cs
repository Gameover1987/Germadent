using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Germadent.Common.Extensions;
using Germadent.Rma.Model.Pricing;
using Germadent.Rma.Model.Production;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        public EmployeePositionDto[] GetEmployeePositions()
        {
            var cmdText = "select distinct EmployeePositionID, EmployeePositionName from dbo.GetTechnologyOperations(default)";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var employeePositions = new List<EmployeePositionDto>();
                    while (reader.Read())
                    {
                        var employeePosition = new EmployeePositionDto();

                        employeePosition.EmployeePositionId = reader["EmployeePositionID"].ToInt();
                        employeePosition.Name = reader["EmployeePositionName"].ToString();

                        employeePositions.Add(employeePosition);
                    }
                    reader.Close();

                    return employeePositions.ToArray();
                }
            }
        }

        public TechnologyOperationDto[] GetTechnologyOperations()
        {
            var cmdText = "select distinct EmployeePositionID, TechnologyOperationID, TechnologyOperationUserCode, TechnologyOperationName from dbo.GetTechnologyOperations(default)";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var technologyOperations = new List<TechnologyOperationDto>();
                    while (reader.Read())
                    {
                        var technologyOperationDto = new TechnologyOperationDto();

                        technologyOperationDto.EmployeePositionId = reader["EmployeePositionId"].ToInt();
                        technologyOperationDto.TechnologyOperationId = reader["TechnologyOperationID"].ToInt();
                        technologyOperationDto.UserCode = reader["TechnologyOperationUserCode"].ToString();
                        technologyOperationDto.Name = reader["TechnologyOperationName"].ToString();

                        technologyOperations.Add(technologyOperationDto);
                    }
                    reader.Close();

                    return technologyOperations.ToArray();
                }
            }
        }

        public DeleteResult DeleteTechnologyOperation(int technologyOperationId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DeleteTechnologyOperation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@technologyOperationId", SqlDbType.Int)).Value = technologyOperationId;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new DeleteResult()
                    {
                        Id = technologyOperationId,
                        Count = count
                    };
                }
            }
        }
    }
}
