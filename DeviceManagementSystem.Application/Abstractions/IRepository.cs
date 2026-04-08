namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken token);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
        Task UpsertAsync(T entity, CancellationToken token);
        Task DeleteAsync(int id, CancellationToken token);
    }
}