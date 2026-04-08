using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IUserDeviceRepository : IRepository<UserDevice>
    {
        Task<UserDevice> GetByIdAsync(int id, CancellationToken token);
        Task<IEnumerable<UserDevice>> GetAllAsync(CancellationToken token);
        Task<IEnumerable<UserDevice>> GetByUserIdAsync(int userId, CancellationToken token);
        Task UpsertAsync(UserDevice entity, CancellationToken token);
        Task DeleteAsync(int id, CancellationToken token);
    }
}