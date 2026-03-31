using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Mappers
{
    public class DeviceMapper : DtoMapper<Device, DeviceDto>
    {
        public override Task<DeviceDto> PrepareItemAsync(Device entity, CancellationToken token)
        {
            if (entity == null) return Task.FromResult<DeviceDto>(null);
            var device = new DeviceDto
            {
                Id = entity.Id,
                DeviceName = entity.DeviceName,
                Manufacturer = entity.Manufacturer,
                DeviceType = entity.DeviceType,
                OS = entity.OS,
                OSVersion = entity.OSVersion,
                Processor = entity.Processor,
                RAM = entity.RAM,
                Description = entity.Description
            };
            return Task.FromResult(device);
        }
    }
}