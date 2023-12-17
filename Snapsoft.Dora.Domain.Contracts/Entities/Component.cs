using Snapsoft.Dora.Domain.Contracts.Core.Storage;

namespace Snapsoft.Dora.Domain.Contracts.Entities;

public record Component : IEntity
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
