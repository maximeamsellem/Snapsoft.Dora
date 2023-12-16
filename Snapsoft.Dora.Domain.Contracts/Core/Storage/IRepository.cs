namespace Snapsoft.Dora.Domain.Contracts.Core.Storage;

public interface IRepository<TEntity> where TEntity : IEntity, new()
{
    Task SaveAsync();

    Task AddAsync(TEntity entity);

    Task<TEntity?> GetByIdAsync(long id);
}
