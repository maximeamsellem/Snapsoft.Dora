using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Snapsoft.Dora.Domain.Contracts.Entities;
using Snapsoft.Dora.Domain.Contracts.Constants;
using Snapsoft.Dora.Adapter.Postgres.Extensions;

namespace Snapsoft.Dora.Adapter.Postgres.EntityTypeConfigurations;

internal class ComponentDeploymentEntityTypeConfiguration : IEntityTypeConfiguration<ComponentDeployment>
{
    public void Configure(EntityTypeBuilder<ComponentDeployment> builder)
    {
        builder.IsEntity();

        builder.HasIndex(u => new { u.ComponentId, u.CommitId }).IsUnique();
        builder.HasIndex(u => new { u.ComponentId, u.Version }).IsUnique();

        builder.Property(u => u.CommitId)
            .HasMaxLength(ComponentDeploymentConstants.MaxCommitIdLength)
            .IsRequired();

        builder.Property(u => u.Version)
            .HasMaxLength(ComponentDeploymentConstants.MaxVersionLength)
            .IsRequired();
    }
}
