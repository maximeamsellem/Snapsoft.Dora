using Snapsoft.Dora.Domain.Contracts.Core.Storage;

namespace Snapsoft.Dora.Domain.Contracts.Entities;

public record ComponentDeployment : IEntity
{
    public long Id { get; init; }

    public Component? Component { get; init; }

    public long ComponentId { get; init; }

    public string Version { get; init; } = string.Empty;

    public string CommitId { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
