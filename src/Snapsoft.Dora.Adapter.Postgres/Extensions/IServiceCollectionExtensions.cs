using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Snapsoft.Dora.Adapter.Postgres.Repositories;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;
using Snapsoft.Dora.Domain.Contracts.Entities;

namespace Snapsoft.Dora.Adapter.Postgres.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresAdapter(
        this IServiceCollection services,
        string connectionString,
        string migrationsDefaultSchema = "public")
    {
        services.AddScoped<IRepository<Component>, BaseRepository<Component>>();

        return services.AddDbContextPool<DoraDbContext>(optionsBuilder =>
        {
            var source = new NpgsqlDataSourceBuilder(connectionString);
            optionsBuilder.UseNpgsql(
                source.Build(), 
                o => o.MigrationsHistoryTable("__EFMigrationsHistory", migrationsDefaultSchema));
        });
    }
}
