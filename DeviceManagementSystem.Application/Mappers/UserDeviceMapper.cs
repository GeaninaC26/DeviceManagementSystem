using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Mappers
{
    public class UserDeviceMapper : DtoMapper<UserDevice, UserDeviceDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeviceRepository _deviceRepository;
        public override Task<UserDeviceDto> PrepareItemAsync(UserDevice entity, CancellationToken token)
        {
            if (entity == null) return Task.FromResult<UserDeviceDto>(null);
            var userDeviceDto = new UserDeviceDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                DeviceId = entity.DeviceId,
                UserName =  _userRepository.GetByIdAsync(entity.UserId).Result.Name,
                DeviceName =  _deviceRepository.GetByIdAsync(entity.DeviceId).Result.Name
            };
            return Task.FromResult(userDeviceDto);
        }

    }
}