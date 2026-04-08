namespace DeviceManagementSystem.Controllers
{
    using DeviceManagementSystem.Application.Contracts;
    using DeviceManagementSystem.Application.Features.Devices;
    using DeviceManagementSystem.Application.Features.Devices.Commands;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly DeviceService _deviceService;
        public DevicesController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<List<DeviceDto>> GetAllDevices([FromQuery] string? searchQuery, CancellationToken token)
        {
            return await _deviceService.GetAllDevicesAsync(searchQuery, token);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetDeviceById(int id, CancellationToken token)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id, token);
            if (device == null) return NotFound();
            return Ok(device);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id, CancellationToken token)
        {
            await _deviceService.DeleteDeviceAsync(id, token);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpsertDevice([FromBody] UpsertDeviceCommand command, CancellationToken token   )
        {
            await _deviceService.UpsertDeviceAsync(command, token);
            return Ok();
        }

        [HttpGet("unassigned")]
        public async Task<List<DeviceDto>> GetUnassignedDevices([FromQuery] string? searchQuery, CancellationToken token)
        {
            return await _deviceService.GetUnassignedDevicesAsync(searchQuery, token);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<DeviceDto>>> GetDevicesForUser(int userId, [FromQuery] string? searchQuery, CancellationToken token)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));

            var devices = await _deviceService.GetDevicesForUserAsync(userId, searchQuery, token);
            return Ok(devices);
        }

        [HttpPost("generateDescription")]
        public async Task<ActionResult<string>> GenerateDeviceDescription([FromBody] GenerateDeviceDescriptionCommand command, CancellationToken token)
        {
            return await _deviceService.GenerateDeviceDescriptionAsync(command, token);
        }
    }
}