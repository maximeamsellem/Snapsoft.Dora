using FluentValidation.Results;
using Snapsoft.Dora.Domain.Contracts.Core.Commands;

namespace Snapsoft.Dora.Domain.Extensions;

internal static class ValidationResultExtensions
{
    internal static ICommandResult ToErrorCommandResult(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            throw new Exception($"{nameof(ToErrorCommandResult)} should not be called with IsValid = true");

        var hasUnicityErrors = validationResult
            .Errors
            .Any(e => e.ErrorCode == Constants.VALIDATION_UNICITY_ERROR);

        return new UnprocessableCommandResult
        {
            HasUnicityError = hasUnicityErrors,
            PropertyErrors = validationResult
                .Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(e => e.Key, e => e.Select(e => e.ErrorMessage))
        };
    }
}
