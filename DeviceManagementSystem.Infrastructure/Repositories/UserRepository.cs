// Infrastructure/Repositories/UserRepository.cs
using Microsoft.Data.SqlClient;
using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Domain.Entities;
using DeviceManagementSystem.Domain.Core;
using DeviceManagementSystem.Infrastructure.Data;

namespace DeviceManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseProvider _databaseProvider;

        public UserRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, UserName, Role, UserLocation FROM Users WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapReaderToUser(reader);
                }
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, UserName, Role, UserLocation FROM Users";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(MapReaderToUser(reader));
                    }
                }
            }
            return users;
        }

        public async Task AddAsync(User entity)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (UserName, Role, UserLocation) VALUES (@UserName, @Role, @UserLocation)";
                command.Parameters.AddWithValue("@UserName", entity.UserName);
                command.Parameters.AddWithValue("@Role", entity.Role.ToString());
                command.Parameters.AddWithValue("@UserLocation", entity.UserLocation);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(User entity)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET UserName = @UserName, Role = @Role, UserLocation = @UserLocation WHERE Id = @Id";
                command.Parameters.AddWithValue("@UserName", entity.UserName);
                command.Parameters.AddWithValue("@Role", entity.Role.ToString());
                command.Parameters.AddWithValue("@UserLocation", entity.UserLocation);
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
                command.CommandText = "DELETE FROM Users WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                
                await command.ExecuteNonQueryAsync();
            }
        }

        private User MapReaderToUser(SqlDataReader reader)
        {
            var id = (int)reader["Id"];
            var name = reader["UserName"].ToString();
            var roleString = reader["Role"].ToString();
            var location = reader["UserLocation"].ToString();

            // Parse Role enum safely
            if (!Enum.TryParse<RoleEnum>(roleString, out var role))
            {
                throw new InvalidOperationException($"Invalid role value: {roleString}");
            }

            return new User(id, name, role, location);
        }
    }
}