using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Application.Contracts
{
    public class UserDto : Dto<int>
    {
        public string Name { get; set; }
        public RoleEnum Role { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
    }
}