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
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return _userMapper.PrepareItemAsync(user, CancellationToken.None).Result;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _userMapper.PrepareItemsAsync(users, CancellationToken.None).Result.ToList();
        }

        public async Task AddUserAsync(string userName, RoleEnum role, string location)
        {
            var user = new User(userName, role, location);
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(int id, string newUserName, RoleEnum newRole, string newLocation)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            user.ChangeName(newUserName);
            user.ChangeRole(newRole);
            user.ChangeLocation(newLocation);

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            await _userRepository.DeleteAsync(user.Id);
        }

    }
}