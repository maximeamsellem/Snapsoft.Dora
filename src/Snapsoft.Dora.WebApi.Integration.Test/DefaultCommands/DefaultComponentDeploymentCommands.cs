using Snapsoft.Dora.Domain.Contracts.Commands;

namespace Snapsoft.Dora.WebApi.Integration.Test.CommandBuilders;

internal static class DefaultComponentDeploymentCommands
{
    internal static CreateComponentDeploymentCommand Create => new CreateComponentDeploymentCommand
    { 
        CommitId = Guid.NewGuid().ToString(),
        Version = Guid.NewGuid().ToString(),
        ComponentId = Random.Shared.Next()
    };
}
