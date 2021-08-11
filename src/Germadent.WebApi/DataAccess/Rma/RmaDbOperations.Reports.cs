using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                var cmdText = $"select * from dbo.GetReportVariousSalary(@userId, @dateStatusFrom, @dateStatusTo)";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = salaryFilter.UserId.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@dateStatusFrom", SqlDbType.DateTime)).Value = salaryFilter.DateFrom;
                    command.Parameters.Add(new SqlParameter("@dateStatusTo", SqlDbType.DateTime)).Value = salaryFilter.DateTo;

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
                                UserIdStarted = reader["UserId"].ToInt(),
                                WorkOrderId = reader["WorkOrderId"].ToInt(),
                                TechnologyOperationId = reader["TechnologyOperationId"].ToInt(),
                                TechnologyOperationName = reader["TechnologyOperationName"].ToString(),
                                TechnologyOperationUserCode = reader["TechnologyOperationUserCode"].ToString(),
                                CustomerName = reader["CustomerName"].ToString(),
                                PatientFullName = reader["PatientFullName"].ToString(),
                                UrgencyRatio = reader["UrgencyRatio"].ToFloat(),
                                UserFullNameStarted = reader["UserFullName"].ToString(),
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

        public ReportListDto[] GetOrdersByProducts(int workOrderId)
        {
            using (var connection = CreateAndOpenSqlConnection())
            {
                var cmdText = $"select * from dbo.GetReportWorkOrders(default, default, default, @workOrderId)";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.NVarChar)).Value = workOrderId;

                    using (var reader = command.ExecuteReader())
                    {
                        var workOrders = new List<ReportListDto>();
                        while (reader.Read())
                        {
                            var workOrder = new ReportListDto()
                            {
                                Created = reader["Created"].ToDateTime(),
                                DocNumber = reader["DocNumber"].ToString(),
                                Customer = reader["CustomerName"].ToString(),
                                EquipmentSubstring = reader["EquipmentName"].ToString(),
                                Patient = reader["PatientFullName"].ToString(),
                                ProstheticSubstring = reader["ProductName"].ToString(),
                                PricePositionCode = reader["PricePositionCode"].ToString(),
                                MaterialsStr = reader["MaterialName"].ToString(),
                                ConstructionColor = reader["Color"].ToString(),
                                Quantity = reader["Quantity"].ToInt(),
                                ImplantSystem = reader["ImplantSystem"].ToString(),
                                TotalPriceCashless = reader["Cashless"].ToDecimal(),
                                TotalPrice = reader["Cash"].ToDecimal(),
                                ResponsiblePerson = reader["ResponsiblePerson"].ToString()
                            };
                            workOrders.Add(workOrder);
                        }
                        return workOrders.ToArray();
                    }
                }                    
            }
        }
    }
}
