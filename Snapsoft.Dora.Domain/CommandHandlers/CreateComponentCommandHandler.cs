using FluentValidation;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.Domain.Extensions;

namespace Snapsoft.Dora.Domain.CommandsHandlers;

public class CreateComponentCommandHandler : ICommandHandler<CreateComponentCommand>
{
    private readonly IValidator<CreateComponentCommand> _validator;
    private readonly IRepository<Component> _repository;

    public CreateComponentCommandHandler(
        IValidator<CreateComponentCommand> validator, 
        IRepository<Component> repository)
    {
        _validator = validator;
        _repository = repository;
    }

    public async Task<ICommandResult> HandleAsync(CreateComponentCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if(!validationResult.IsValid)
        {
            return validationResult.ToErrorCommandResult();
        }
                
        var component = new Component
        {
            Name = command.Name,
        };

        await _repository.AddAsync(component);

        await _repository.SaveAsync();

        return new SuccessCommandResult(component);
    }
}