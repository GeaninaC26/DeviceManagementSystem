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
                command.CommandText = "SELECT Id, Name, Role, Location FROM Users WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapReaderToUser(reader);
                    }
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
                command.CommandText = "SELECT Id, Name, Role, Location FROM Users";

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
                command.CommandText = "INSERT INTO Users (Name, Role, Location) VALUES (@Name, @Role, @Location)";
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Role", entity.Role.ToString());
                command.Parameters.AddWithValue("@Location", entity.Location);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(User entity)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET Name = @Name, Role = @Role, Location = @Location WHERE Id = @Id";
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Role", entity.Role.ToString());
                command.Parameters.AddWithValue("@Location", entity.Location);
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
            var name = reader["Name"].ToString();
            var roleString = reader["Role"].ToString();
            var location = reader["Location"].ToString();

            // Parse Role enum safely
            if (!Enum.TryParse<RoleEnum>(roleString, out var role))
            {
                throw new InvalidOperationException($"Invalid role value: {roleString}");
            }

            var user = new User(name, role, location);
            
            // Set Id through reflection since it's protected
            typeof(User).BaseType?.GetProperty("Id")?.SetValue(user, id);

            return user;
        }
    }
}