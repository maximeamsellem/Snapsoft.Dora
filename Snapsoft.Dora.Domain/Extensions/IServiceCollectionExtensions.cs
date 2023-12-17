using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.Domain.Validators;

namespace Snapsoft.Dora.Domain.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDomainValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<CreateComponentCommand>, CreateComponentCommandValidator>();
    }
}
