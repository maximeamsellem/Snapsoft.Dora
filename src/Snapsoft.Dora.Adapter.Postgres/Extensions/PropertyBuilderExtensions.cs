using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Snapsoft.Dora.Domain.Contracts.Core.Storage;

namespace Snapsoft.Dora.Adapter.Postgres.Extensions;

public static class PropertyBuilderExtensions
{    
    public static PropertyBuilder<DateTime> IsDateTime(this PropertyBuilder<DateTime> propertyBuilder)
    {
        return propertyBuilder
            .HasColumnType(nameof(NpgsqlDbType.TimestampTz))
            .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => v.ToUniversalTime());
    }
}
