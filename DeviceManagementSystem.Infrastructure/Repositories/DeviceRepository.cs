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

        public async Task<Device> GetByIdAsync(int id, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
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

        public async Task<IEnumerable<Device>> GetAllAsync(string searchQuery = null, CancellationToken token = default)
        {
            var devices = new List<Device>();

            using (var connection = _databaseProvider.GetConnection(token))
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
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    devices = FilterAndScoreDevices(devices, searchQuery);
                }
            }

            return devices;
        }



        public async Task UpsertAsync(Device entity, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
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

        public async Task DeleteAsync(int id, CancellationToken token)
        {
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Devices WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Device>> GetUnassignedDevicesAsync(string searchQuery, CancellationToken token)
        {
            var devices = new List<Device>();
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT d.Id, d.Name, d.Manufacturer, d.Type, d.OS, d.OSVersion, d.Processor, d.RAM, d.Description
                    FROM Devices d
                    LEFT JOIN UserDevices ud ON d.Id = ud.DeviceId
                    WHERE ud.UserId IS NULL";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        devices.Add(MapReaderToDevice(reader));
                    }
                }

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    devices = FilterAndScoreDevices(devices, searchQuery);
                }   
            }
            return devices;
        }

        public async Task<List<Device>> GetDevicesForUserAsync(int userId, string? searchQuery, CancellationToken token)
        {
            var devices = new List<Device>();
            using (var connection = _databaseProvider.GetConnection(token))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT d.Id, d.Name, d.Manufacturer, d.Type, d.OS, d.OSVersion, d.Processor, d.RAM, d.Description
                    FROM Devices d
                    INNER JOIN UserDevices ud ON d.Id = ud.DeviceId
                    WHERE ud.UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        devices.Add(MapReaderToDevice(reader));
                    }
                }

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    devices = FilterAndScoreDevices(devices, searchQuery);
                }
            }
            return devices;
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

                private int CalculateScore(Device device, string[] tokens)
        {
            int score = 0;
            foreach (var token in tokens)
            {
                if (device.Name?.Contains(token, StringComparison.OrdinalIgnoreCase) == true) score += 10;
                if (device.Manufacturer?.Contains(token, StringComparison.OrdinalIgnoreCase) == true) score += 5;
                if (device.Processor?.Contains(token, StringComparison.OrdinalIgnoreCase) == true) score += 3;
                if (device.RAM?.Contains(token, StringComparison.OrdinalIgnoreCase) == true) score += 1;
            }
            return score;
        }

        private List<Device> FilterAndScoreDevices(List<Device> devices, string searchQuery)
        {
            var tokens = searchQuery.ToLowerInvariant()
                                    .Split(new[] { ' ', ',', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            return devices
                .Select(device => new
                {
                    Device = device,
                    Score = CalculateScore(device, tokens)
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Device.Name)
                .Select(x => x.Device)
                .ToList();
        }

        public Task<IEnumerable<Device>> GetAllAsync(CancellationToken token)
        {
            return GetAllAsync(null, token);
        }
    }
}