using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations : IRmaDbOperations
    {
        public PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddPriceGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = (int)priceGroupDto.BranchType;
                    command.Parameters.Add(new SqlParameter("@priceGroupName", SqlDbType.NVarChar)).Value = priceGroupDto.Name;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    priceGroupDto.Id = command.Parameters["@priceGroupId"].Value.ToInt();

                    return priceGroupDto;
                }
            }
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UpdatePriceGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = (int)priceGroupDto.Id;
                    command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = priceGroupDto.BranchType;
                    command.Parameters.Add(new SqlParameter("@priceGroupName", SqlDbType.NVarChar)).Value = priceGroupDto.Name;

                    command.ExecuteNonQuery();

                    return priceGroupDto;
                }
            }
        }

        public PriceGroupDeleteResult DeletePriceGroup(int priceGroupId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UnionPriceGroups", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@oldPriceGroupId", SqlDbType.NVarChar)).Value = priceGroupId;
                    command.Parameters.Add(new SqlParameter("@newPriceGroupId", SqlDbType.NVarChar)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new PriceGroupDeleteResult()
                    {
                        PriceGroupId = priceGroupId,
                        Count = count
                    };
                }
            }
        }

        public PricePositionDto AddPricePosition(PricePositionDto pricePositionDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddPricePosition", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@pricePositionCode", SqlDbType.NVarChar)).Value = pricePositionDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = pricePositionDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@pricePositionName", SqlDbType.Int)).Value = pricePositionDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@materialId", SqlDbType.Int)).Value = pricePositionDto.MaterialId;
                    command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    pricePositionDto.PricePositionId = command.Parameters["@pricePositionId"].Value.ToInt();

                    return pricePositionDto;
                }
            }
        }
    }
}
