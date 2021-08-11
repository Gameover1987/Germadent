using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.WebApi.Entities;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        public CustomerDto[] GetCustomers(string name)
        {
            var cmdText = string.Format("select * from dbo.GetCustomers(default, '{0}')", name);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var customerEntities = new List<CustomerEntity>();
                        while (reader.Read())
                        {
                            var customerEntity = new CustomerEntity();
                            customerEntity.CustomerId = int.Parse(reader[nameof(customerEntity.CustomerId)].ToString());
                            customerEntity.CustomerName = reader[nameof(customerEntity.CustomerName)].ToString();
                            customerEntity.CustomerPhone = reader[nameof(customerEntity.CustomerPhone)].ToString();
                            customerEntity.CustomerEmail = reader[nameof(customerEntity.CustomerEmail)].ToString();
                            customerEntity.CustomerWebSite = reader[nameof(customerEntity.CustomerWebSite)].ToString();
                            customerEntity.CustomerDescription =
                                reader[nameof(customerEntity.CustomerDescription)].ToString();

                            customerEntities.Add(customerEntity);
                        }

                        var customers = customerEntities.Select(x => _converter.ConvertToCustomer(x)).ToArray();
                        return customers;
                    }
                }
            }
        }

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            var cmdText = "select * from dbo.GetResponsiblePersons(default, default)";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var responsiblePersonEntities = new List<ResponsiblePersonEntity>();
                        while (reader.Read())
                        {
                            var rpEntity = new ResponsiblePersonEntity();
                            rpEntity.ResponsiblePersonId = int.Parse(reader[nameof(rpEntity.ResponsiblePersonId)].ToString());
                            rpEntity.ResponsiblePerson = reader[nameof(rpEntity.ResponsiblePerson)].ToString();
                            rpEntity.RP_Position = reader[nameof(rpEntity.RP_Position)].ToString();
                            rpEntity.RP_Phone = reader[nameof(rpEntity.RP_Phone)].ToString();
                            rpEntity.RP_Email = reader[nameof(rpEntity.RP_Email)].ToString();
                            rpEntity.RP_Description = reader[nameof(rpEntity.RP_Description)].ToString();

                            responsiblePersonEntities.Add(rpEntity);
                        }

                        var responsiblePersons = responsiblePersonEntities.Select(x => _converter.ConvertToResponsiblePerson(x)).ToArray();
                        return responsiblePersons;
                    }
                }
            }
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.AddRespPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@rp_position", SqlDbType.NVarChar)).Value = responsiblePerson.Position;
                    command.Parameters.Add(new SqlParameter("@responsiblePerson", SqlDbType.NVarChar)).Value = responsiblePerson.FullName;
                    command.Parameters.Add(new SqlParameter("@rp_phone", SqlDbType.NVarChar)).Value = responsiblePerson.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_email", SqlDbType.NVarChar)).Value = responsiblePerson.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_description", SqlDbType.NVarChar)).Value = responsiblePerson.Description.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    responsiblePerson.Id = command.Parameters["@responsiblePersonId"].Value.ToInt();

                    return responsiblePerson;
                }
            }
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UpdateResponsiblePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = responsiblePerson.Id;
                    command.Parameters.Add(new SqlParameter("@rp_Position", SqlDbType.NVarChar)).Value = responsiblePerson.Position;
                    command.Parameters.Add(new SqlParameter("@responsiblePerson", SqlDbType.NVarChar)).Value = responsiblePerson.FullName;
                    command.Parameters.Add(new SqlParameter("@rp_phone", SqlDbType.NVarChar)).Value = responsiblePerson.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_email", SqlDbType.NVarChar)).Value = responsiblePerson.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_description", SqlDbType.NVarChar)).Value = responsiblePerson.Description.GetValueOrDbNull();

                    command.ExecuteNonQuery();

                    return responsiblePerson;
                }
            }
        }

        public DeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UnionResponsiblePersons", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@oldResponsiblePersonId", SqlDbType.NVarChar)).Value = responsiblePersonId;
                    command.Parameters.Add(new SqlParameter("@newResponsiblePersonId", SqlDbType.NVarChar)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new DeleteResult()
                    {
                        Id = responsiblePersonId,
                        Count = count
                    };
                }
            }
        }

        public CustomerDto AddCustomer(CustomerDto customer)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.AddCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = customer.Name;
                    command.Parameters.Add(new SqlParameter("@customerPhone", SqlDbType.NVarChar)).Value = customer.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerEmail", SqlDbType.NVarChar)).Value = customer.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerWebsite", SqlDbType.NVarChar)).Value = customer.WebSite.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerDescription", SqlDbType.NVarChar)).Value = customer.Description.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    customer.Id = command.Parameters["@customerId"].Value.ToInt();

                    return customer;
                }
            }
        }

        public CustomerDto UpdateCustomer(CustomerDto customer)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UpdateCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customer.Id;
                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = customer.Name;
                    command.Parameters.Add(new SqlParameter("@customerPhone", SqlDbType.NVarChar)).Value = customer.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerEmail", SqlDbType.NVarChar)).Value = customer.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerWebsite", SqlDbType.NVarChar)).Value = customer.WebSite.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerDescription", SqlDbType.NVarChar)).Value = customer.Description.GetValueOrDbNull();

                    command.ExecuteNonQuery();

                    return customer;
                }
            }
        }

        public DeleteResult DeleteCustomer(int customerId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UnionCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@oldCustomerId", SqlDbType.NVarChar)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@newCustomerId", SqlDbType.NVarChar)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new DeleteResult
                    {
                        Id = customerId,
                        Count = count
                    };
                }
            }
        }
    }
}
