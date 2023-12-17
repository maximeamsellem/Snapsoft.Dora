using Snapsoft.Dora.Domain.Contracts.Core.Commands;

namespace Snapsoft.Dora.Domain.Contracts.Commands;

public record CreateComponentCommand : ICommand
{
    public required string Name { get; init; } = string.Empty;
}
