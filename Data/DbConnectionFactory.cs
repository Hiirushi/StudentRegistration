using System;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace StudentRegistration.Data
{
    public class DbConnectionFactory
    {
        private readonly string connectionString;

        public DbConnectionFactory()
        {
            connectionString = ConfigurationManager.ConnectionStrings["STUREGConnection"]?.ConnectionString ?? throw new InvalidOperationException(
                    "Database connection string not found."
                );
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
