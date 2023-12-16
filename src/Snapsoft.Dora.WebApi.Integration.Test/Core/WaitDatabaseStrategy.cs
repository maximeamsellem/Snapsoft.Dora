using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;

namespace Snapsoft.Dora.WebApi.Integration.Test.Core;

internal class WaitDatabaseStrategy : IWaitUntil
{
    private readonly string _connectionString;

    public WaitDatabaseStrategy(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> UntilAsync(IContainer container)
    {
        var options = new DbContextOptionsBuilder<DbContext>();
        options.UseNpgsql(_connectionString);

        var dbContext = new DbContext(options.Options);
        var connected = await dbContext.Database.CanConnectAsync();

        return connected;
    }
}
