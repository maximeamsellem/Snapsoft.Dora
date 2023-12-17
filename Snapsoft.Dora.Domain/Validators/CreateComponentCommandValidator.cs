using FluentValidation;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Constants;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;

namespace Snapsoft.Dora.Domain.Validators;

internal class CreateComponentCommandValidator : AbstractValidator<CreateComponentCommand>
{
    public CreateComponentCommandValidator(IRepository<Component> repository)
    {
        RuleFor(x => x.Name)
            .Length(ComponentConstants.MinNameLength, ComponentConstants.MaxNameLength)
            .MustAsync(async (name, cancellation) =>
            {
                var nameAlreadyUsed = await repository.AnyAsync(c => c.Name == name);

                return !nameAlreadyUsed;
            })
            .WithMessage(cmd => $"{nameof(CreateComponentCommand.Name)} '{cmd.Name}' is already used")
            .WithErrorCode(Constants.VALIDATION_UNICITY_ERROR);
    }
}
