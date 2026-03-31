namespace DeviceManagementSystem.Application.Contracts
{
    public class UserDto : Dto<int>
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string UserLocation { get; set; }
    }
}