namespace Snapsoft.Dora.Domain.Contracts.Core.Commands;

public record SuccessCommandResult(object? Value) : ICommandResult { }