using FluentValidation;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.Domain.Extensions;

namespace Snapsoft.Dora.Domain.CommandsHandlers;

public class CreateComponentDeploymentCommandHandler : ICommandHandler<CreateComponentDeploymentCommand>
{
    private readonly IValidator<CreateComponentDeploymentCommand> _validator;
    private readonly IRepository<ComponentDeployment> _repository;

    public CreateComponentDeploymentCommandHandler(
        IValidator<CreateComponentDeploymentCommand> validator, 
        IRepository<ComponentDeployment> repository)
    {
        _validator = validator;
        _repository = repository;
    }

    public async Task<ICommandResult> HandleAsync(CreateComponentDeploymentCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if(!validationResult.IsValid)
        {
            return validationResult.ToErrorCommandResult();
        }
                
        var componentDeployment = new ComponentDeployment
        {
            CommitId = command.CommitId,
            ComponentId = command.ComponentId,
            Version = command.Version
        };

        await _repository.AddAsync(componentDeployment);

        await _repository.SaveAsync();

        return new CreationSuccessCommandResult(componentDeployment);
    }
}