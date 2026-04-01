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

        public async Task UpsertDeviceAsync(DeviceDto deviceDto)
        {
            // Validate DTO
            if (deviceDto == null)
                throw new ArgumentNullException(nameof(deviceDto), "Device data cannot be null");

            // Validate required fields
            if (string.IsNullOrWhiteSpace(deviceDto.Name))
                throw new ArgumentException("Device name is required", nameof(deviceDto.Name));

            if (string.IsNullOrWhiteSpace(deviceDto.Manufacturer))
                throw new ArgumentException("Manufacturer is required", nameof(deviceDto.Manufacturer));

            if (string.IsNullOrWhiteSpace(deviceDto.DeviceType))
                throw new ArgumentException("Device type is required", nameof(deviceDto.DeviceType));

            if (string.IsNullOrWhiteSpace(deviceDto.OS))
                throw new ArgumentException("Operating system is required", nameof(deviceDto.OS));

            if (string.IsNullOrWhiteSpace(deviceDto.OSVersion))
                throw new ArgumentException("OS version is required", nameof(deviceDto.OSVersion));

            if (string.IsNullOrWhiteSpace(deviceDto.Processor))
                throw new ArgumentException("Processor is required", nameof(deviceDto.Processor));

            if (string.IsNullOrWhiteSpace(deviceDto.RAM))
                throw new ArgumentException("RAM is required", nameof(deviceDto.RAM));

            var device = new Device(
                deviceDto.Name,
                deviceDto.Manufacturer,
                deviceDto.DeviceType,
                deviceDto.OS,
                deviceDto.OSVersion,
                deviceDto.Processor,
                deviceDto.RAM,
                deviceDto.Description ?? string.Empty
            );
            await _deviceRepository.UpsertAsync(device);
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