using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.WebApi.Entities;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        public void UnlockWorkOrder(int workOrderId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "AddOrDeleteOccupancyWO";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@workOrderID", SqlDbType.Int)).Value = workOrderId;
                    command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int)).Value = DBNull.Value;

                    command.ExecuteNonQuery();
                }
            }
        }

        private LockWorkOrderInfoEntity[] GetWorkOrdersLockInfo(int? workOrderId = null)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                string idText = workOrderId == null ? "NULL" : workOrderId.Value.ToString();
                var cmdText = string.Format("select * from GetOccupancyWO({0})", idText);
                var lockers = new List<LockWorkOrderInfoEntity>();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new LockWorkOrderInfoEntity();
                            entity.WorkOrderId = reader[nameof(entity.WorkOrderId)].ToInt();
                            entity.UserId = reader[nameof(entity.UserId)].ToInt();
                            entity.OccupancyDateTime = reader[nameof(entity.OccupancyDateTime)].ToDateTime();
                            lockers.Add(entity);
                        }
                    }
                }

                return lockers.ToArray();
            }
        }

        private void LockWorkOrder(int workOrderId, int userId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "AddOrDeleteOccupancyWO";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@workOrderID", SqlDbType.Int)).Value = workOrderId;
                    command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int)).Value = userId;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
