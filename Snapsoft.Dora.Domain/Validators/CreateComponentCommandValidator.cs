using FluentValidation;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Contracts.Constants;

namespace Snapsoft.Dora.Domain.Validators;

internal class CreateComponentCommandValidator : AbstractValidator<CreateComponentCommand>
{
    public CreateComponentCommandValidator()
    {
        RuleFor(x => x.Name).Length(
            ComponentConstants.MinNameLength, 
            ComponentConstants.MaxNameLength);
    }
}
