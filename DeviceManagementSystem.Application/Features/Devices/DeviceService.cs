using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Application.Features.Devices.Commands;
using DeviceManagementSystem.Application.Mappers;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Features.Devices
{
    public class DeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly DeviceMapper _deviceMapper;
        public DeviceService(IDeviceRepository deviceRepository, DeviceMapper deviceMapper)
        {
            _deviceRepository = deviceRepository;
            _deviceMapper = deviceMapper;
        }

        public async Task<DeviceDto> GetDeviceByIdAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("Device ID must be greater than 0", nameof(id));

            var device = await _deviceRepository.GetByIdAsync(id);
            
            // Check if device exists
            if (device == null)
                throw new Exception($"Device with ID {id} not found");
            
            return await _deviceMapper.PrepareItemAsync(device, token);
        }

        public async Task<List<DeviceDto>> GetAllDevicesAsync(CancellationToken token)
        {
            var devices = await _deviceRepository.GetAllAsync();
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task<List<DeviceDto>> GetUnassignedDevicesAsync(CancellationToken token)
        {
            var devices = await _deviceRepository.GetUnassignedDevicesAsync();
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task UpsertDeviceAsync(UpsertDeviceCommand command)
        {
            // Validate command
            if (command == null)
                throw new ArgumentNullException(nameof(command), "Device data cannot be null");

            // Validate required fields
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
                // Update existing device
                device = new Device(
                    command.Id,
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
                // Create new device
                device = new Device(
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
            await _deviceRepository.UpsertAsync(device);
        }

        public async Task<List<DeviceDto>> GetDevicesForUserAsync(int userId, CancellationToken token)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));

            var devices = await _deviceRepository.GetDevicesForUserAsync(userId);
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task DeleteDeviceAsync(int id)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("Device ID must be greater than 0", nameof(id));

            var device = await _deviceRepository.GetByIdAsync(id);
            
            // Check if device exists
            if (device == null)
                throw new Exception($"Device with ID {id} not found");
            
            await _deviceRepository.DeleteAsync(device.Id);
        }

    }
}