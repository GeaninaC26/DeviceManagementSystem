using Microsoft.Data.SqlClient;
using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IDatabaseProvider _databaseProvider;

        public DeviceRepository(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<Device> GetByIdAsync(int id)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description FROM Devices WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapReaderToDevice(reader);
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            var devices = new List<Device>();
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description FROM Devices";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        devices.Add(MapReaderToDevice(reader));
                    }
                }
            }
            return devices;
        }

        public async Task UpsertAsync(Device entity)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    IF EXISTS (SELECT 1 FROM Devices WHERE Id = @Id)
                    BEGIN
                        UPDATE Devices
                        SET Name = @Name,
                            Manufacturer = @Manufacturer,
                            Type = @Type,
                            OS = @OS,
                            OSVersion = @OSVersion,
                            Processor = @Processor,
                            RAM = @RAM,
                            Description = @Description
                        WHERE Id = @Id
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description)
                        VALUES (@Name, @Manufacturer, @Type, @OS, @OSVersion, @Processor, @RAM, @Description)
                    END";

                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer);
                command.Parameters.AddWithValue("@Type", entity.Type);
                command.Parameters.AddWithValue("@OS", entity.OS);
                command.Parameters.AddWithValue("@OSVersion", entity.OSVersion);
                command.Parameters.AddWithValue("@Processor", entity.Processor);
                command.Parameters.AddWithValue("@RAM", entity.RAM);
                command.Parameters.AddWithValue("@Description", entity.Description);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _databaseProvider.GetConnection())
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Devices WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                await command.ExecuteNonQueryAsync();
            }
        }

        private Device MapReaderToDevice(SqlDataReader reader)
        {
            var id = (int)reader["Id"];
            var name = reader["Name"].ToString();
            var manufacturer = reader["Manufacturer"].ToString();
            var deviceType = reader["Type"].ToString();
            var os = reader["OS"].ToString();
            var osVersion = reader["OSVersion"].ToString();
            var processor = reader["Processor"].ToString();
            var ram = reader["RAM"].ToString();
            var description = reader["Description"].ToString();

            return new Device(id, name, manufacturer, deviceType, os, osVersion, processor, ram, description);
        }
    }
}