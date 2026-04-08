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

        public async Task<User> GetByIdAsync(int id, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Role, Location, Email, PasswordHash FROM Users WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapReaderToUser(reader);
                }
            }
            return null;
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Role, Location, Email, PasswordHash FROM Users WHERE Email = @Email";
                command.Parameters.AddWithValue("@Email", email);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapReaderToUser(reader);
                }
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllAsync(string searchQuery, CancellationToken token)
        {
            var users = new List<User>();
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Role, Location, Email, PasswordHash FROM Users";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(MapReaderToUser(reader));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                users = users.Where(u =>
                    u.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.Location.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return users;
        }

        public async Task UpsertAsync(User entity, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    IF EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
                    BEGIN
                        UPDATE Users
                        SET Name = @Name, Role = @Role, Location = @Location, Email = @Email, PasswordHash = @PasswordHash
                        WHERE Id = @Id
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Users (Name, Role, Location, Email, PasswordHash)
                        VALUES (@Name, @Role, @Location, @Email, @PasswordHash)
                    END";
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Role",  entity.Role.ToString());
                command.Parameters.AddWithValue("@Location", entity.Location);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);

                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task DeleteAsync(int id, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
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
            var email = reader["Email"].ToString();
            var passwordHash = reader["PasswordHash"].ToString();

            // Parse Role enum safely
            if (!Enum.TryParse<RoleEnum>(roleString, out var role))
            {
                throw new InvalidOperationException($"Invalid role value: {roleString}");
            }

            return new User(id, name, role, location, email, passwordHash);
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken token)
        {
            return GetAllAsync(null, token);
        }
    }
}