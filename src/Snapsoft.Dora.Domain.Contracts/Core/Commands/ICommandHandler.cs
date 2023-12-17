namespace Snapsoft.Dora.Domain.Contracts.Core.Commands;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<ICommandResult> HandleAsync(TCommand command);
}
