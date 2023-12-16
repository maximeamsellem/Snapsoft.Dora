using Snapsoft.Dora.Domain.Contracts.Core.Storage;

namespace Snapsoft.Dora.Adapter.Postgres.Repositories;

internal class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
{
    private readonly DoraDbContext _dbContext;

    public BaseRepository(DoraDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity) => await _dbContext.AddAsync(entity);

    public async Task<TEntity?> GetByIdAsync(long id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
}
