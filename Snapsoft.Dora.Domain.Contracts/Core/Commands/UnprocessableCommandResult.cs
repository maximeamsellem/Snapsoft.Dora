namespace Snapsoft.Dora.Domain.Contracts.Core.Commands;

public class UnprocessableCommandResult : ICommandResult
{
    public Dictionary<string, IEnumerable<string>>? PropertyErrors { get; init; }
}