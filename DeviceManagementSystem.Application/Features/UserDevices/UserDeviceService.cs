using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Application.Mappers;
using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Features.UserDevices
{
    public class UserDeviceService
    {
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly UserDeviceMapper _mapper;
        public UserDeviceService(IUserDeviceRepository userDeviceRepository, UserDeviceMapper mapper)
        {
            _userDeviceRepository = userDeviceRepository;
            _mapper = mapper;
        }
        public async Task<UserDeviceDto> GetByIdAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("User-Device association ID must be greater than 0", nameof(id));

            var userDevice = await _userDeviceRepository.GetByIdAsync(id, token);

            // Check if association exists
            if (userDevice == null)
                throw new Exception($"User-Device association with ID {id} not found");

            return await _mapper.PrepareItemAsync(userDevice, token);
        }

        public async Task<List<UserDeviceDto>> GetAllAsync(CancellationToken token)
        {
            var userDevices = await _userDeviceRepository.GetAllAsync(token);
            return (await _mapper.PrepareItemsAsync(userDevices, token)).ToList();
        }

        public async Task AssignDeviceToUserAsync(int userId, int deviceId, CancellationToken token)
        {
            // Validate IDs
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));

            if (deviceId <= 0)
                throw new ArgumentException("Device ID must be greater than 0", nameof(deviceId));

            var userDevice = new UserDevice(userId, deviceId);
            await _userDeviceRepository.UpsertAsync(userDevice, token);
        }

        public async Task UnassignDeviceFromUserAsync(int id, CancellationToken token)
        {
            // Validate ID
            if (id <= 0)
                throw new ArgumentException("User-Device association ID must be greater than 0", nameof(id));

            var userDevice = await _userDeviceRepository.GetByIdAsync(id, token);

            // Check if association exists
            if (userDevice == null)
                throw new Exception($"User-Device association with ID {id} not found");

            await _userDeviceRepository.DeleteAsync(userDevice.Id, token);
        }
    }
}