using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Application.Features.Devices.Commands;
using DeviceManagementSystem.Application.Mappers;
using DeviceManagementSystem.Domain.Entities;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration;
namespace DeviceManagementSystem.Application.Features.Devices
{
    public class DeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly DeviceMapper _deviceMapper;
        private readonly IConfiguration _configuration;
        public DeviceService(IDeviceRepository deviceRepository, DeviceMapper deviceMapper, IConfiguration configuration)
        {
            _deviceRepository = deviceRepository;
            _deviceMapper = deviceMapper;
            _configuration = configuration;
        }

        public async Task<DeviceDto> GetDeviceByIdAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("Device ID must be greater than 0", nameof(id));

            var device = await _deviceRepository.GetByIdAsync(id, token);

            // Check if device exists
            if (device == null)
                throw new Exception($"Device with ID {id} not found");

            return await _deviceMapper.PrepareItemAsync(device, token);
        }

        public async Task<List<DeviceDto>> GetAllDevicesAsync(string searchQuery, CancellationToken token)
        {
            var devices = await _deviceRepository.GetAllAsync(token);
            if(!string.IsNullOrWhiteSpace(searchQuery))
            {
                devices = FilterAndScoreDevices(devices.ToList(), searchQuery);
            }
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task<List<DeviceDto>> GetUnassignedDevicesAsync(string searchQuery, CancellationToken token)
        {
            var devices = await _deviceRepository.GetUnassignedDevicesAsync(token);
            if(!string.IsNullOrWhiteSpace(searchQuery))
            {
                devices = FilterAndScoreDevices(devices.ToList(), searchQuery);
            }
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task UpsertDeviceAsync(UpsertDeviceCommand command, CancellationToken token)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "Device data cannot be null");

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("Device name is required", nameof(command.Name));

            if (string.IsNullOrWhiteSpace(command.Manufacturer))
                throw new ArgumentException("Manufacturer is required", nameof(command.Manufacturer));

            if (string.IsNullOrWhiteSpace(command.Type))
                throw new ArgumentException("Device type is required", nameof(command.Type));

            if (string.IsNullOrWhiteSpace(command.OS))
                throw new ArgumentException("Operating system is required", nameof(command.OS));

            if (string.IsNullOrWhiteSpace(command.OSVersion))
                throw new ArgumentException("OS version is required", nameof(command.OSVersion));

            if (string.IsNullOrWhiteSpace(command.Processor))
                throw new ArgumentException("Processor is required", nameof(command.Processor));

            if (string.IsNullOrWhiteSpace(command.RAM))
                throw new ArgumentException("RAM is required", nameof(command.RAM));

            Device device;
            if (command.Id > 0)
            {
                device = new Device(
                    command.Id,
                    command.SerialNumber,
                    command.Name,
                    command.Manufacturer,
                    command.Type,
                    command.OS,
                    command.OSVersion,
                    command.Processor,
                    command.RAM,
                    command.Description ?? string.Empty
                );
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(command.SerialNumber))
                {
                    var existingDevice = await _deviceRepository.GetBySerialNumberAsync(command.SerialNumber, token);
                    if (existingDevice != null)
                        throw new InvalidOperationException($"A device with this serial number already exists. Please register a unique device number.");
                }

                device = new Device(
                    command.SerialNumber,
                    command.Name,
                    command.Manufacturer,
                    command.Type,
                    command.OS,
                    command.OSVersion,
                    command.Processor,
                    command.RAM,
                    command.Description ?? string.Empty
                );
            }
            await _deviceRepository.UpsertAsync(device, token);
        }

        public async Task<List<DeviceDto>> GetDevicesForUserAsync(int userId, string? searchQuery, CancellationToken token)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));

            var devices = await _deviceRepository.GetDevicesForUserAsync(userId, token);
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                devices = FilterAndScoreDevices(devices.ToList(), searchQuery);
            }
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task DeleteDeviceAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("Device ID must be greater than 0", nameof(id));

            var device = await _deviceRepository.GetByIdAsync(id, token);

            // Check if device exists
            if (device == null)
                throw new Exception($"Device with ID {id} not found");

            await _deviceRepository.DeleteAsync(device.Id, token);
        }

        public async Task<string> GenerateDeviceDescriptionAsync(GenerateDeviceDescriptionCommand command, CancellationToken token)
        {
            var client = new Client(apiKey: _configuration["GEMINI_API_KEY"]);

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview", contents:
                $@"
                Act as a professional IT Asset Manager. Generate a concise, human-readable summary for a device.
    
                ### Instructions:
                1. Analyze the specs (RAM, Processor) to determine the performance tier (e.g., 'high-performance', 'entry-level', 'powerful').
                2. Identify the primary use case (e.g., 'daily business use', 'intensive multitasking', 'mobile productivity').
                3. Output a single, elegant sentence that combines the Manufacturer, Type, and OS.
                4. **CRITICAL**: Do not list technical numbers (like '{command.RAM}' or '{command.Processor}') in the final string. Use descriptive adjectives instead.

                ### Input Data:
                - Name: {command.Name}
                - Manufacturer: {command.Manufacturer}
                - Type: {command.Type}
                - OS: {command.OS} {command.OSVersion}
                - Processor: {command.Processor}
                - RAM: {command.RAM}

                ### Expected Format:
                ""A [performance adjective] [Manufacturer] [Type] running [OS], suitable for [use case].""
                ");

            return response.Candidates[0].Content.Parts[0].Text.Trim();
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

    }
}

