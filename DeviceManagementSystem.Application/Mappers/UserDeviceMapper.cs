using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Mappers
{
    public class UserDeviceMapper : DtoMapper<UserDevice, UserDeviceDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeviceRepository _deviceRepository;

        public UserDeviceMapper(IUserRepository userRepository, IDeviceRepository deviceRepository)
        {
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
        }

        public override async Task<UserDeviceDto> PrepareItemAsync(UserDevice entity, CancellationToken token)
        {
            if (entity == null) return null;

            var user = await _userRepository.GetByIdAsync(entity.UserId);
            var device = await _deviceRepository.GetByIdAsync(entity.DeviceId);

            var userDeviceDto = new UserDeviceDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                DeviceId = entity.DeviceId,
                UserName = user?.Name ?? $"User {entity.UserId}",
                DeviceName = device?.Name ?? $"Device {entity.DeviceId}"
            };
            return userDeviceDto;
        }
    }
}