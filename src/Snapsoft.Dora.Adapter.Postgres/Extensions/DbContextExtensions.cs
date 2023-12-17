using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Snapsoft.Dora.Adapter.Postgres.Extensions;

internal static class DbContextExtensions
{
    public static IReadOnlyCollection<MigrationOperation> GetDifferencesBetweenModelAndSnapshot(this DbContext context)
    {
        var differences = new List<MigrationOperation>();
        var migrationsAssembly = context.GetService<IMigrationsAssembly>();

        var snapshotModel = migrationsAssembly.ModelSnapshot?.Model;
        if (snapshotModel is IMutableModel mutableModel)
            snapshotModel = mutableModel.FinalizeModel();

        if (snapshotModel != null)
        {
            snapshotModel = context.GetService<IModelRuntimeInitializer>().Initialize(snapshotModel);
            differences = context.GetService<IMigrationsModelDiffer>().GetDifferences(
                snapshotModel.GetRelationalModel(),
                context.GetService<IDesignTimeModel>().Model.GetRelationalModel())
                .ToList();
        }
        return differences;
    }
}
