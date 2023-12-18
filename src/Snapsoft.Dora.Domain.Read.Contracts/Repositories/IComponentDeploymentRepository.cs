using Snapsoft.Dora.Domain.Contracts.Entities;

namespace Snapsoft.Dora.Domain.Read.Contracts.Repositories;

public interface IComponentDeploymentRepository
{
    Task<ComponentDeployment?> GetByIdWithComponentAsync(long id);
}
