using FluentValidation;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Constants;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;

namespace Snapsoft.Dora.Domain.Validators;

internal class CreateComponentDeploymentCommandValidator : AbstractValidator<CreateComponentDeploymentCommand>
{
    public CreateComponentDeploymentCommandValidator(
        IRepository<ComponentDeployment> repository,
        IRepository<Component> componentRepository)
    {
        RuleFor(x => x.Version)
            .Length(ComponentDeploymentConstants.MinVersionLength, ComponentDeploymentConstants.MaxVersionLength)
            .MustAsync(async (cmd ,version, cancellation) =>
            {
                var versionAlreadyUsed = await repository.AnyAsync(c => c.Version == version && c.ComponentId == cmd.ComponentId);
                return !versionAlreadyUsed;
            })
            .WithMessage(cmd => $"'{nameof(CreateComponentDeploymentCommand.Version)}' '{cmd.Version}' is already used")
            .WithErrorCode(Constants.VALIDATION_UNICITY_ERROR);

        RuleFor(x => x.CommitId)
            .Length(ComponentDeploymentConstants.MinCommitIdLength, ComponentDeploymentConstants.MaxCommitIdLength)
            .MustAsync(async (cmd, commitId, cancellation) =>
            {
                var commitAlreadyUsed = await repository.AnyAsync(c => c.CommitId == commitId && c.ComponentId == cmd.ComponentId);
                return !commitAlreadyUsed;
            })
            .WithMessage(cmd => $"'{nameof(CreateComponentDeploymentCommand.CommitId)}' '{cmd.CommitId}' is already used")
            .WithErrorCode(Constants.VALIDATION_UNICITY_ERROR);

        RuleFor(x => x.ComponentId)
            .MustAsync(async (componentId, cancellation) => 
            {
                var componentIdExists = await componentRepository.AnyAsync(c => c.Id == componentId);
                return componentIdExists;
            });
    }
}
