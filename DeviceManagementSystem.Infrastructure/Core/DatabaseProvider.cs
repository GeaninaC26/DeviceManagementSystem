using DeviceManagementSystem.Application.Abstractions;
using Microsoft.Data.SqlClient;

namespace DeviceManagementSystem.Infrastructure.Data
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly string _connectionString;

        public DatabaseProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}