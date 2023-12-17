using Microsoft.EntityFrameworkCore;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using System.Linq.Expressions;

namespace Snapsoft.Dora.Adapter.Postgres.Repositories;

internal class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected DoraDbContext DbContext { get; }

    public BaseRepository(DoraDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity) => await DbContext.AddAsync(entity);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    public async Task<TEntity?> GetByIdAsync(long id)
    {
        return await DbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task SaveAsync() => await DbContext.SaveChangesAsync();
}
