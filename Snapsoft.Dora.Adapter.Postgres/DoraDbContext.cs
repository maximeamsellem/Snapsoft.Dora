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

    public DbSet<Component> Components { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new ComponentEntityTypeConfiguration().Configure(modelBuilder.Entity<Component>());
    }
}