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
            var userDevice = await _userDeviceRepository.GetByIdAsync(id);
            return await _mapper.PrepareItemAsync(userDevice, token);
        }

        public async Task<List<UserDeviceDto>> GetAllAsync(CancellationToken token)
        {
            var userDevices = await _userDeviceRepository.GetAllAsync();
            return (await _mapper.PrepareItemsAsync(userDevices, token)).ToList();
        }

        public async Task AssignDeviceToUserAsync(int userId, int deviceId)
        {
            var userDevice = new UserDevice(userId, deviceId);
            await _userDeviceRepository.AddAsync(userDevice);
        }

        public async Task UnassignDeviceFromUserAsync(int id)
        {
            var userDevice = await _userDeviceRepository.GetByIdAsync(id);
            if (userDevice == null) throw new Exception("User-Device association not found");
            await _userDeviceRepository.DeleteAsync(userDevice.Id);
        }
        
    }
}