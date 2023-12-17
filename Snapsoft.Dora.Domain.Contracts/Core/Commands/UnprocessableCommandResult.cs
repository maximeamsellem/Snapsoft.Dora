namespace Snapsoft.Dora.Domain.Contracts.Core.Commands;

public record UnprocessableCommandResult : ICommandResult
{
    public IReadOnlyDictionary<string, IEnumerable<string>>? PropertyErrors { get; init; } = new Dictionary<string, IEnumerable<string>>();

    public bool HasUnicityError { get; init; } = false;
}