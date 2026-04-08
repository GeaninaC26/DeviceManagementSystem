using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(int id, CancellationToken token);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);
        Task UpsertAsync(User entity, CancellationToken token);
        Task DeleteAsync(int id, CancellationToken token);
        Task<User> GetByEmailAsync(string email, CancellationToken token);
    }
}