namespace DeviceManagementSystem.Application.Features.UserDevices.Commands{
    public class AssignDeviceToUserCommand
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
    }
}