using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IDeviceRepository: IRepository<Device>
    {
        Task<Device> GetByIdAsync(int id);
        Task<IEnumerable<Device>> GetAllAsync();
        Task AddAsync(Device entity);
        Task UpdateAsync(Device entity);
        Task DeleteAsync(int id);
    }
}