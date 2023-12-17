using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;

namespace Snapsoft.Dora.Adapter.Postgres.Extensions;

public static class EntityBuilderExtensions
{
    public static EntityTypeBuilder<T> IsEntity<T>(this EntityTypeBuilder<T> entityTypeBuilder) where T : class, IEntity
    {
        entityTypeBuilder.HasKey(e => e.Id);
        entityTypeBuilder.Property(e => e.Id).UseIdentityAlwaysColumn();
        entityTypeBuilder.Property(e => e.CreatedAt).IsDateTime();

        return entityTypeBuilder;
    }
}
