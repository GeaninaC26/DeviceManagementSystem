using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IUserDeviceRepository : IRepository<UserDevice>
    {
        Task<UserDevice> GetByIdAsync(int id);
        Task<IEnumerable<UserDevice>> GetAllAsync();
        Task<IEnumerable<UserDevice>> GetByUserIdAsync(int userId);
        Task UpsertAsync(UserDevice entity);
        Task DeleteAsync(int id);
    }
}