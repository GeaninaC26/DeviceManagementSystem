using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Application.Contracts
{
    public class UserDto : Dto<int>
    {
        public string UserName { get; set; }
        public RoleEnum Role { get; set; }
        public string UserLocation { get; set; }
    }
}