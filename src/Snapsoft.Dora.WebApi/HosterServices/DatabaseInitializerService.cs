using Microsoft.EntityFrameworkCore;
using Snapsoft.Dora.Adapter.Postgres;
using Snapsoft.Dora.Adapter.Postgres.Extensions;

namespace Snapsoft.Dora.WebApi.HosterServices
{
    public class DatabaseInitializerService : IHostedService
    {
        private readonly ILogger<DatabaseInitializerService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DatabaseInitializerService(ILogger<DatabaseInitializerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<DoraDbContext>();

#if DEBUG
            var differences = context.GetDifferencesBetweenModelAndSnapshot();
            if (differences.Count > 0)
            {
                var error = "Inconsistency between Snapshot and you C# models has been found. Please check the differences by debugging";
                logger.LogWarning(error);
                throw new Exception(error);
            }
#endif
            await context.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
