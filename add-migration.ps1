param($migrationName)

dotnet ef migrations add $migrationName `
    --context DoraDbContext `
    --output-dir Migrations `
    --project Snapsoft.Dora.Adapter.Postgres/Snapsoft.Dora.Adapter.Postgres.csproj `
    --startup-project src/Snapsoft.Dora.WebApi/Snapsoft.Dora.WebApi.csproj