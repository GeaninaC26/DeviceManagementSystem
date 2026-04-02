namespace DeviceManagementSystem.Controllers
{
    using DeviceManagementSystem.Application.Contracts;
    using DeviceManagementSystem.Application.Features.Devices;
    using DeviceManagementSystem.Application.Features.Devices.Commands;
    using DeviceManagementSystem.Application.Features.UserDevices.Commands;
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
        public async Task<List<DeviceDto>> GetAllDevices(CancellationToken token)
        {
            return await _deviceService.GetAllDevicesAsync(token);
    
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetDeviceById(int id, CancellationToken token)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id, token);
            if (device == null) return NotFound();
            return Ok(device);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            await _deviceService.DeleteDeviceAsync(id);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpsertDevice([FromBody] UpsertDeviceCommand command)
        {
            await _deviceService.UpsertDeviceAsync(command);
            return Ok();
        }
        
    }
}