using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        public WorkDto[] GetSalaryReport(SalaryFilter salaryFilter)
        {
            using (var connection = CreateAndOpenSqlConnection())
            {
                var cmdText = "select * from dbo.GetReportSalary(@userId, null, null, @dateCompletedFrom, @dateCompletedTo)";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = salaryFilter.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@dateCompletedFrom", SqlDbType.DateTime)).Value = salaryFilter.DateFrom;
                    command.Parameters.Add(new SqlParameter("@dateCompletedTo", SqlDbType.DateTime)).Value = salaryFilter.DateTo;

                    using (var reader = command.ExecuteReader())
                    {
                        var works = new List<WorkDto>();
                        while (reader.Read())
                        {
                            var work = new WorkDto
                            {
                                DocNumber = reader["DocNumber"].ToString(),
                                OperationCost = reader["OperationCost"].ToDecimal(),
                                LastEditorId = 0,
                                ProductId = reader["ProductId"].ToIntOrNull(),
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = reader["Quantity"].ToInt(),
                                Rate = reader["Rate"].ToDecimal(),
                                UserId = reader["UserId"].ToInt(),
                                WorkOrderId = reader["WorkOrderId"].ToInt(),
                                TechnologyOperationId = reader["TechnologyOperationId"].ToInt(),
                                TechnologyOperationName = reader["TechnologyOperationName"].ToString(),
                                TechnologyOperationUserCode = reader["TechnologyOperationUserCode"].ToString(),
                                UrgencyRatio = reader["UrgencyRatio"].ToFloat(),
                                UserFullName = reader["UserFullName"].ToString(),
                                WorkCompleted = reader["WorkCompleted"].ToDateTime(),
                                WorkStarted = reader["WorkStarted"].ToDateTime(),
                                WorkId = reader["WorkId"].ToInt(),
                            };
                            
                            works.Add(work);
                        }

                        return works.ToArray();
                    }
                }
            }
        }
    }
}
