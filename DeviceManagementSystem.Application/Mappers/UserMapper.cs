using System.Security.Cryptography;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Domain.Core;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Mappers
{
    public class UserMapper : DtoMapper<User, UserDto>
    {
        public override Task<UserDto> PrepareItemAsync(User entity, CancellationToken token)
        {
            if (entity == null) return Task.FromResult<UserDto>(null);
            var dto = new UserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Role = RoleEnum.GetName(entity.Role),
                UserLocation = entity.UserLocation
            };
            return Task.FromResult(dto);
        }


    }
}