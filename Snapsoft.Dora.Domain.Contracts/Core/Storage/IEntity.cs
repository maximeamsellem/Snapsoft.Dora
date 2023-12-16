namespace Snapsoft.Dora.Domain.Contracts.Core.Storage;

public interface IEntity
{
    long Id { get; init; }

    DateTime CreatedAt { get; init; }
}
