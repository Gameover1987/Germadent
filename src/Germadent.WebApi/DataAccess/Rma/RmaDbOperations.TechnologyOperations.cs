using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Model.Pricing;
using Germadent.Model.Production;
using Newtonsoft.Json;

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
                        employeePosition.EmployeePositionName = reader["EmployeePositionName"].ToString();

                        employeePositions.Add(employeePosition);
                    }
                    reader.Close();

                    return employeePositions.ToArray();
                }
            }
        }

        public TechnologyOperationDto[] GetTechnologyOperations()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                // Получили технологические операции без рейтов
                var technologyOperations = new List<TechnologyOperationDto>();
                var getTechCommandText = "select distinct EmployeePositionID, TechnologyOperationID, TechnologyOperationUserCode, IsObsoleteTechnologyOperation, TechnologyOperationName from dbo.GetTechnologyOperations(default)";
                using (var command = new SqlCommand(getTechCommandText, connection))
                {
                    var reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        var technologyOperationDto = new TechnologyOperationDto();

                        technologyOperationDto.EmployeePositionId = reader["EmployeePositionId"].ToInt();
                        technologyOperationDto.TechnologyOperationId = reader["TechnologyOperationID"].ToInt();
                        technologyOperationDto.UserCode = reader["TechnologyOperationUserCode"].ToString();
                        technologyOperationDto.Name = reader["TechnologyOperationName"].ToString();
                        technologyOperationDto.IsObsolete = reader["IsObsoleteTechnologyOperation"].ToBool();

                        technologyOperations.Add(technologyOperationDto);
                    }
                    reader.Close();
                }

                // Получили рейты
                var rates = new List<RateDto>();
                var getRatesCommandText = "select * from dbo.Rates";
                using (var command = new SqlCommand(getRatesCommandText, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var rateDto = new RateDto();
                        rateDto.TechnologyOperationId = reader["TechnologyOperationID"].ToInt();
                        rateDto.QualifyingRank = reader["QualifyingRank"].ToInt();
                        rateDto.Rate = reader["Rate"].ToDecimal();
                        rateDto.DateBeginning = reader["DateBeginning"].ToDateTime();
                        rates.Add(rateDto);
                    }
                }

                // Соединили рейты и тех. операции
                foreach (var technologyOperationDto in technologyOperations)
                {
                    technologyOperationDto.Rates = rates.Where(x => x.TechnologyOperationId == technologyOperationDto.TechnologyOperationId).ToArray();
                }

                return technologyOperations.ToArray();
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

        public TechnologyOperationDto AddTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            var jsonStringRates = technologyOperationDto.Rates.SerializeToJson(Formatting.Indented);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddTechnologyOperation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@technologyOperationUserCode", SqlDbType.NVarChar)).Value = technologyOperationDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@technologyOperationName", SqlDbType.NVarChar)).Value = technologyOperationDto.Name;
                    command.Parameters.Add(new SqlParameter("@employeePositionID", SqlDbType.Int)).Value = technologyOperationDto.EmployeePositionId;
                    command.Parameters.Add(new SqlParameter("@isObsoleteTechnologyOperation", SqlDbType.Bit)).Value = technologyOperationDto.IsObsolete;
                    command.Parameters.Add(new SqlParameter("@jsonStringRates", SqlDbType.NVarChar)).Value = jsonStringRates;
                    command.Parameters.Add(new SqlParameter("@technologyOperationId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.ExecuteNonQuery();

                    technologyOperationDto.TechnologyOperationId = command.Parameters["@technologyOperationId"].Value.ToInt();

                    return technologyOperationDto;
                }
            }
        }

        public TechnologyOperationDto UpdateTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            var jsonStringRates = technologyOperationDto.Rates.SerializeToJson(Formatting.Indented);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UpdateTechnologyOperation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@technologyOperationId", SqlDbType.Int)).Value = technologyOperationDto.TechnologyOperationId;
                    command.Parameters.Add(new SqlParameter("@technologyOperationUserCode", SqlDbType.NVarChar)).Value = technologyOperationDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@technologyOperationName", SqlDbType.NVarChar)).Value = technologyOperationDto.Name;
                    command.Parameters.Add(new SqlParameter("@employeePositionID", SqlDbType.Int)).Value = technologyOperationDto.EmployeePositionId;
                    command.Parameters.Add(new SqlParameter("@isObsoleteTechnologyOperation", SqlDbType.Bit)).Value = technologyOperationDto.IsObsolete;
                    command.Parameters.Add(new SqlParameter("@jsonStringRates", SqlDbType.NVarChar)).Value = jsonStringRates;
                    command.ExecuteNonQuery();

                    return technologyOperationDto;
                }
            }
        }
    }
}
