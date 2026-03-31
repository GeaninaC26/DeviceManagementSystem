using System.Runtime.CompilerServices;

namespace DeviceManagementSystem.Application.Contracts
{
    public class UserDeviceDto : Dto<int>
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public string UserName { get; set; }
        public string DeviceName { get; set; }
    }
}