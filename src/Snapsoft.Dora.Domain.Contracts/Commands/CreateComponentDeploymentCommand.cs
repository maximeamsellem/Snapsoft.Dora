using Snapsoft.Dora.Domain.Contracts.Core.Commands;

namespace Snapsoft.Dora.Domain.Contracts.Commands;

public record CreateComponentDeploymentCommand : ICommand
{
    public required long ComponentId { get; init; }

    public required string Version { get; init; } = string.Empty;

    public required string CommitId { get; init; } = string.Empty;
}
