using Snapsoft.Dora.Domain.CommandsHandlers;

namespace Snapsoft.Dora.WebApi;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDomainConcreteCommandHandlers(
        this IServiceCollection services)
    {
        return services
            .AddScoped<CreateComponentDeploymentCommandHandler>()
            .AddScoped<CreateComponentCommandHandler>();
    }
}
