using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagementSystem.Application.Features.Devices.Commands
{
    public class UpdateDeviceCommand : CreateDeviceCommand
    {
        public int Id { get; set; }
    }
}