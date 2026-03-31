using DeviceManagementSystem.Domain.Entities;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);
    }
}