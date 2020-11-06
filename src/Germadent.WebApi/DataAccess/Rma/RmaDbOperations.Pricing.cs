using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
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
            throw new NotImplementedException();
        }
    }
}
