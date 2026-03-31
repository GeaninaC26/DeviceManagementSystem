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
                UserName = entity.Name,
                Role = entity.Role,
                UserLocation = entity.Location
            };
            return Task.FromResult(dto);
        }


    }
}