namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpsertAsync(T entity);
        Task DeleteAsync(int id);
    }
}