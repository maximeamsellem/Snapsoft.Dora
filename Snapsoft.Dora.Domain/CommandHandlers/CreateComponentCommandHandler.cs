using FluentValidation;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;

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
        var validationResult = _validator.Validate(command);

        if(!validationResult.IsValid)
        {
            return BuildUnprocessableCommandResult(validationResult);
        }

        var component = new Component
        {
            Name = command.Name,
        };

        await _repository.AddAsync(component);

        await _repository.SaveAsync();

        return new SuccessCommandResult(component);
    }

    private static ICommandResult BuildUnprocessableCommandResult(FluentValidation.Results.ValidationResult validationResult)
    {
        return new UnprocessableCommandResult
        {
            PropertyErrors = validationResult
                .Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(e => e.Key, e => e.Select(e => e.ErrorMessage))
        };
    }
}