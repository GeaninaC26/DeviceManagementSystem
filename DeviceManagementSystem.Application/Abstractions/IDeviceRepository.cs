using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IDeviceRepository: IRepository<Device>
    {
        Task<Device> GetByIdAsync(int id);
        Task<IEnumerable<Device>> GetAllAsync();
        Task UpsertAsync(Device entity);
        Task DeleteAsync(int id);
        Task<List<Device>> GetUnassignedDevicesAsync();
        Task<List<Device>> GetDevicesForUserAsync(int userId);
    }
}