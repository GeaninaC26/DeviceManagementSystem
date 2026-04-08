using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Application.Features.Users.Commands;
using DeviceManagementSystem.Application.Mappers;
using DeviceManagementSystem.Domain.Core;
using DeviceManagementSystem.Domain.Entities;
using Scrypt;

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

        public async Task<UserDto> AuthenticateAsync(string email, string password, CancellationToken token)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required", nameof(password));

            var user = await _userRepository.GetByEmailAsync(email, token);
            
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} not found");
            
            if (!VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid password");

            return await _userMapper.PrepareItemAsync(user, token);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            if (password == passwordHash)
            {
                return true;
            }

            try
            {
                ScryptEncoder encoder = new ScryptEncoder();
                return encoder.Compare(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            var encoder = new ScryptEncoder();
            return encoder.Encode(password);
        }

        public async Task<UserDto> GetUserByIdAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(id));

            var user = await _userRepository.GetByIdAsync(id, token);
            
            // Check if user exists
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");
            
            return await _userMapper.PrepareItemAsync(user, token);
        }

        public async Task<List<UserDto>> GetAllUsersAsync(string? searchQuery, CancellationToken token)
        {
            var users = await _userRepository.GetAllAsync(token);
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                users = users.Where(u =>
                    u.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.Location.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return (await _userMapper.PrepareItemsAsync(users, token)).ToList();
        }

        public async Task UpsertUserAsync(UpsertUserCommand userCommand, CancellationToken token)
        {
            // Validate command
            if (userCommand == null)
                throw new ArgumentNullException(nameof(userCommand), "User data cannot be null");

            // Validate required fields
            if (string.IsNullOrWhiteSpace(userCommand.Name))
                throw new ArgumentException("User name is required", nameof(userCommand.Name));

            if (string.IsNullOrWhiteSpace(userCommand.Location))
                throw new ArgumentException("User location is required", nameof(userCommand.Location));

            if (string.IsNullOrWhiteSpace(userCommand.Email))
                throw new ArgumentException("User email is required", nameof(userCommand.Email));

            if (string.IsNullOrWhiteSpace(userCommand.Password))
                throw new ArgumentException("User password is required", nameof(userCommand.Password));
            
            var existingUser = await _userRepository.GetByEmailAsync(userCommand.Email, token);
            if (existingUser != null && existingUser.Id != userCommand.Id)
                throw new InvalidOperationException($"Account with email {userCommand.Email} already exists");

            var passwordHash = HashPassword(userCommand.Password);

            var role = RoleEnum.User;

            User user;
            if (userCommand.Id > 0)
            {
                user = new User(userCommand.Id, userCommand.Name, role, userCommand.Location, userCommand.Email, passwordHash);
            }
            else
            {
                user = new User(userCommand.Name, role, userCommand.Location, userCommand.Email, passwordHash);
            }
            await _userRepository.UpsertAsync(user, token);
        }

        public async Task DeleteUserAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(id));

            var user = await _userRepository.GetByIdAsync(id, token);
            
            // Check if user exists
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");
            
            await _userRepository.DeleteAsync(user.Id, token);
        }

    }
}