using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Snapsoft.Dora.WebApi.Integration.Test.Core;

public class ServiceFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static Lazy<IContainer> _postgresContainer = new Lazy<IContainer>(GetPostgresContainer);

    private const string POSTGRES_PASSWORD = "postgres123456";
    private const int POSTGRES_HOST_PORT = 7000;

    private readonly static string DORA_DB_CONNECTION_STRING =
        $"Host=localhost;Database=doratest;Username=postgres;Password={POSTGRES_PASSWORD};Port={POSTGRES_HOST_PORT}";

    private readonly static string POSTGRES_DB_CONNECTION_STRING =
        $"Host=localhost;Database=postgres;Username=postgres;Password={POSTGRES_PASSWORD};Port={POSTGRES_HOST_PORT}";

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _postgresContainer.Value.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((webBuilder, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
            {
                new KeyValuePair<string, string?>(
                    nameof(AppConfiguration.PostgresConnectionString),
                    DORA_DB_CONNECTION_STRING)
            });
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await _postgresContainer.Value.StopAsync();
        await base.DisposeAsync();
    }

    async Task IAsyncLifetime.DisposeAsync() => await DisposeAsync();

    private static IContainer GetPostgresContainer()
    {
        const int POSTGRES_CONTAINER_PORT = 5432;

        return new ContainerBuilder()
            .WithName(Guid.NewGuid().ToString("D"))
            .WithImage("postgres:16-alpine3.19")
            .WithPortBinding(POSTGRES_HOST_PORT, POSTGRES_CONTAINER_PORT)
            .WithEnvironment("POSTGRES_PASSWORD", POSTGRES_PASSWORD)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .AddCustomWaitStrategy(new WaitDatabaseStrategy(POSTGRES_DB_CONNECTION_STRING)))
            .WithAutoRemove(true)
            .Build();
    }
}
