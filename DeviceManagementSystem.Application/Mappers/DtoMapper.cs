using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Application.Mappers
{
    public abstract class DtoMapper<TEntity, TDto>
        where TEntity : Entity<int>
        where TDto : class
    {
        protected DtoMapper() { }
        public abstract Task<TDto> PrepareItemAsync(TEntity entity, CancellationToken token);
        public async Task<IEnumerable<TDto>> PrepareItemsAsync(IEnumerable<TEntity> entities, CancellationToken token)
        {
           var res = new List<TDto>();
            foreach (var entity in entities)
            {
                var item = await PrepareItemAsync(entity, token);
                if (item == null) continue;
                res.Add(item);
            }
            return res;
        }
    }

}