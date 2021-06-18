using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        private T ExecuteInTransactionScope<T>(Func<SqlTransaction, T> func)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction("StartWorks"))
                {
                    var result = func(transaction);

                    transaction.Commit();

                    return result;
                }
            }
        }

        private SqlConnection CreateAndOpenSqlConnection()
        {
            var connection = new SqlConnection(_configuration.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
