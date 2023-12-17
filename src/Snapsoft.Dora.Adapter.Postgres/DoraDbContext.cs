using Microsoft.EntityFrameworkCore;
using Snapsoft.Dora.Adapter.Postgres.EntityTypeConfigurations;
using Snapsoft.Dora.Domain.Contracts.Entities;

namespace Snapsoft.Dora.Adapter.Postgres;

internal class DoraDbContext : DbContext
{
    public DoraDbContext(DbContextOptions<DoraDbContext> options) : base(options)
    {
    }

    public DoraDbContext() : base() { }

    public required DbSet<Component> Components { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new ComponentEntityTypeConfiguration().Configure(modelBuilder.Entity<Component>());
    }
}