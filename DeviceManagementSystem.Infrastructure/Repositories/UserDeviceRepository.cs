using Microsoft.Data.SqlClient;
using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Domain.Entities;
using DeviceManagementSystem.Infrastructure.Data;

namespace DeviceManagementSystem.Infrastructure.Repositories
{
    public class UserDeviceRepository : IUserDeviceRepository
    {
        private readonly IDatabaseProvider _databaseProvider;

        public UserDeviceRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<UserDevice> GetByIdAsync(int id)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, UserId, DeviceId FROM UserDevices WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapReaderToUserDevice(reader);
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<UserDevice>> GetAllAsync()
        {
            var userDevices = new List<UserDevice>();
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, UserId, DeviceId FROM UserDevices";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userDevices.Add(MapReaderToUserDevice(reader));
                    }
                }
            }
            return userDevices;
        }

        public async Task<IEnumerable<UserDevice>> GetByUserIdAsync(int userId)
        {
            var userDevices = new List<UserDevice>();
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, UserId, DeviceId FROM UserDevices WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userDevices.Add(MapReaderToUserDevice(reader));
                    }
                }
            }
            return userDevices;
        }

        public async Task AddAsync(UserDevice entity)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO UserDevices (UserId, DeviceId) VALUES (@UserId, @DeviceId)";
                command.Parameters.AddWithValue("@UserId", entity.UserId);
                command.Parameters.AddWithValue("@DeviceId", entity.DeviceId);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(UserDevice entity)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE UserDevices SET UserId = @UserId, DeviceId = @DeviceId WHERE Id = @Id";
                command.Parameters.AddWithValue("@UserId", entity.UserId);
                command.Parameters.AddWithValue("@DeviceId", entity.DeviceId);
                command.Parameters.AddWithValue("@Id", entity.Id);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM UserDevices WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                await command.ExecuteNonQueryAsync();
            }
        }

        private UserDevice MapReaderToUserDevice(SqlDataReader reader)
        {
            var id = (int)reader["Id"];
            var userId = (int)reader["UserId"];
            var deviceId = (int)reader["DeviceId"];

            return new UserDevice(id, userId, deviceId);
        }
    }
}