namespace DeviceManagementSystem.Controllers
{
    using DeviceManagementSystem.Application.Contracts;
    using DeviceManagementSystem.Application.Features.UserDevices;
    using DeviceManagementSystem.Application.Features.UserDevices.Commands;
    using DeviceManagementSystem.Application.Features.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserDevicesController : ControllerBase
    {
        private readonly UserDeviceService _userDeviceService;

        public UserDevicesController(UserDeviceService userDeviceService)
        {
            _userDeviceService = userDeviceService;
        }

        [HttpGet]
        public async Task<List<UserDeviceDto>> GetAllUserDevices(CancellationToken token)
        {
            return await _userDeviceService.GetAllAsync(token);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDeviceDto>> GetUserDeviceById(int id, CancellationToken token)
        {
            var userDevice = await _userDeviceService.GetByIdAsync(id, token);
            if (userDevice == null) return NotFound();
            return Ok(userDevice);
        }
        [HttpPost]
        public async Task<IActionResult> AssignDeviceToUser([FromBody] AssignDeviceToUserCommand command, CancellationToken token)
        {
            await _userDeviceService.AssignDeviceToUserAsync(command.UserId, command.DeviceId, token);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> UnassignDeviceFromUser(int id, CancellationToken token)
        {
            await _userDeviceService.UnassignDeviceFromUserAsync(id, token);
            return Ok();
        }


    }
}