namespace DeviceManagementSystem.Controllers
{
    using DeviceManagementSystem.Application.Contracts;
    using DeviceManagementSystem.Application.Features.UserDevices.Commands;
    using DeviceManagementSystem.Application.Features.Users;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAllUsers(CancellationToken token)
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken token)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertUser([FromBody] UserDto userDto)
        {
            await _userService.UpsertUserAsync(userDto);
            return Ok();
        }

        

    }
}