using Microsoft.EntityFrameworkCore;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.Domain.Read.Contracts.Repositories;

namespace Snapsoft.Dora.Adapter.Postgres.Repositories
{
    internal class ComponentDeploymentRepository : BaseRepository<ComponentDeployment>, IComponentDeploymentRepository
    {
        public ComponentDeploymentRepository(DoraDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ComponentDeployment?> GetByIdWithComponentAsync(long id)
        {
            return await DbContext
                .Set<ComponentDeployment>()
                .Include(cd => cd.Component)
                .SingleOrDefaultAsync(cd => cd.Id == id);
        }
    }
}
