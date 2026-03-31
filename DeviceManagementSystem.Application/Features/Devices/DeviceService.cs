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
            var device = await _deviceRepository.GetByIdAsync(id);
            return await _deviceMapper.PrepareItemAsync(device, token);
        }

        public async Task<List<DeviceDto>> GetAllDevicesAsync(CancellationToken token)
        {
            var devices = await _deviceRepository.GetAllAsync();
            return (await _deviceMapper.PrepareItemsAsync(devices, token)).ToList();
        }

        public async Task AddDeviceAsync(CreateDeviceCommand createDeviceCommand)
        {
            var device = new Device(
                createDeviceCommand.DeviceName,
                createDeviceCommand.Manufacturer,
                createDeviceCommand.DeviceType,
                createDeviceCommand.OS,
                createDeviceCommand.OSVersion,
                createDeviceCommand.Processor,
                createDeviceCommand.RAM,
                createDeviceCommand.Description
            );
            await _deviceRepository.AddAsync(device);
        }

        public async Task UpdateDeviceAsync(int id, UpdateDeviceCommand updateDeviceCommand)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) throw new Exception("Device not found");

            device.ChangeDeviceName(updateDeviceCommand.DeviceName);
            device.ChangeManufacturer(updateDeviceCommand.Manufacturer);
            device.ChangeDeviceType(updateDeviceCommand.DeviceType);
            device.ChangeOS(updateDeviceCommand.OS);
            device.ChangeOSVersion(updateDeviceCommand.OSVersion);
            device.ChangeProcessor(updateDeviceCommand.Processor);
            device.ChangeRAM(updateDeviceCommand.RAM);
            device.ChangeDescription(updateDeviceCommand.Description);

            await _deviceRepository.UpdateAsync(device);
        }

        public async Task DeleteDeviceAsync(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) throw new Exception("Device not found");
            await _deviceRepository.DeleteAsync(device.Id);
        }

    }
}