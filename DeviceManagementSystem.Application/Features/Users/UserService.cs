using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Application.Mappers;
using DeviceManagementSystem.Domain.Core;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Features.Users
{
    public class UserService 
    {
        private readonly IUserRepository _userRepository;
        private readonly UserMapper _userMapper = new UserMapper();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(id));

            var user = await _userRepository.GetByIdAsync(id);
            
            // Check if user exists
            if (user == null)
                throw new Exception($"User with ID {id} not found");
            
            return _userMapper.PrepareItemAsync(user, CancellationToken.None).Result;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _userMapper.PrepareItemsAsync(users, CancellationToken.None).Result.ToList();
        }

        public async Task UpsertUserAsync(UserDto userDto)
        {
            // Validate DTO
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto), "User data cannot be null");

            // Validate required fields
            if (string.IsNullOrWhiteSpace(userDto.UserName))
                throw new ArgumentException("User name is required", nameof(userDto.UserName));

            if (userDto.Role == null)
                throw new ArgumentException("User role is required", nameof(userDto.Role));

            if (string.IsNullOrWhiteSpace(userDto.UserLocation))
                throw new ArgumentException("User location is required", nameof(userDto.UserLocation));

            var user = new User(userDto.UserName, userDto.Role, userDto.UserLocation);
            await _userRepository.UpsertAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(id));

            var user = await _userRepository.GetByIdAsync(id);
            
            // Check if user exists
            if (user == null)
                throw new Exception($"User with ID {id} not found");
            
            await _userRepository.DeleteAsync(user.Id);
        }

    }
}