using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IDeviceRepository: IRepository<Device>
    {
        Task<Device> GetByIdAsync(int id, CancellationToken token);
        Task<IEnumerable<Device>> GetAllAsync(string searchQuery = null, CancellationToken token = default);
        Task UpsertAsync(Device entity, CancellationToken token);
        Task DeleteAsync(int id, CancellationToken token);
        Task<List<Device>> GetUnassignedDevicesAsync(string searchQuery = null, CancellationToken token = default);
        Task<List<Device>> GetDevicesForUserAsync(int userId, string? searchQuery, CancellationToken token);
    }
}