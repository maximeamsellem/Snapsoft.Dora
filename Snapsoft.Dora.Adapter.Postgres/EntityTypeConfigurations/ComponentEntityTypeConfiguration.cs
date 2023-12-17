using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.Domain.Contracts.Constants;
using Snapsoft.Dora.Adapter.Postgres.Extensions;

namespace Snapsoft.Dora.Adapter.Postgres.EntityTypeConfigurations;

internal class ComponentEntityTypeConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {        
        builder.IsEntity();
        builder.HasIndex(u => u.Name).IsUnique();        
        builder.Property(u => u.Name)
            .HasMaxLength(ComponentConstants.MaxNameLength)
            .IsRequired();
    }
}
