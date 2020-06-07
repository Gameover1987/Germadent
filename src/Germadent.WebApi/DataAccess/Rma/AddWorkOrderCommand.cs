using System;
using System.Data;
using System.Data.SqlClient;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.WebApi.Configuration;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.Rma
{
    public interface IAddWorOrderCommand
    {
        OrderDto Execute(OrderDto order);
    }

    public class AddWorkOrderCommand : IAddWorOrderCommand
    {
        private readonly IServiceConfiguration _configuration;

        public AddWorkOrderCommand(IServiceConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public OrderDto Execute(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                OrderDto outputOrder;
                connection.Open();
                outputOrder = AddWorkOrder(order, connection);

                order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
                AddOrUpdateToothCard(order.ToothCard, connection);

                return outputOrder;
            }
        }

        private static OrderDto AddWorkOrder(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("AddNewWorkOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = (int)order.BranchType;
                command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = order.CustomerId;
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = order.ResponsiblePersonId == 0 ? (object)DBNull.Value : order.ResponsiblePersonId;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientGender", SqlDbType.TinyInt)).Value = (int)order.Gender;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.TinyInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.DateOfCompletion;
                command.Parameters.Add(new SqlParameter("@additionalInfo", SqlDbType.NVarChar)).Value = order.AdditionalInfo;
                command.Parameters.Add(new SqlParameter("@carcassColor", SqlDbType.NVarChar)).Value = order.CarcassColor;
                command.Parameters.Add(new SqlParameter("@implantSystem", SqlDbType.NVarChar)).Value = order.ImplantSystem;
                command.Parameters.Add(new SqlParameter("@individualAbutmentProcessing", SqlDbType.NVarChar)).Value = order.IndividualAbutmentProcessing;
                command.Parameters.Add(new SqlParameter("@understaff", SqlDbType.NVarChar)).Value = order.Understaff;
                command.Parameters.Add(new SqlParameter("@transparenceId", SqlDbType.Int)).Value = order.Transparency;
                command.Parameters.Add(new SqlParameter("@colorAndFeatures", SqlDbType.NVarChar)).Value = order.ColorAndFeatures;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar) { Direction = ParameterDirection.Output, Size = 10 });

                command.ExecuteNonQuery();

                order.WorkOrderId = command.Parameters["@workOrderId"].Value.ToInt();
                order.DocNumber = command.Parameters["@docNumber"].Value.ToString();
            }
            order.AdditionalEquipment.ForEach(x => x.WorkOrderId = order.WorkOrderId);
            AddOrUpdateAdditionalEquipmentInWO(order, connection);

            return order;
        }

        private void AddOrUpdateToothCard(ToothDto[] toothCard, SqlConnection connection)
        {
            var toothCardJson = toothCard.SerializeToJson(Formatting.Indented);

            var cmdText = "AddOrUpdateToothCardInWO";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@jsonString", SqlDbType.NVarChar)).Value = toothCardJson;

                command.ExecuteNonQuery();
            }
        }

        private static void AddOrUpdateAdditionalEquipmentInWO(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("AddOrUpdateAdditionalEquipmentInWO", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                var jsonEquipments = order.AdditionalEquipment.SerializeToJson();

                command.Parameters.Add(new SqlParameter("@jsonEquipments", SqlDbType.NVarChar)).Value = jsonEquipments;

                command.ExecuteNonQuery();
            }
        }
    }
}