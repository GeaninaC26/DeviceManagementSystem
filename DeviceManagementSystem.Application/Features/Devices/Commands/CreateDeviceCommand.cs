namespace DeviceManagementSystem.Application.Features.Devices.Commands
{
    public class CreateDeviceCommand 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string Processor { get; set; }
        public string RAM { get; set; }
        public string Description { get; set; }
    }
}