namespace Snapsoft.Dora.WebApi;

public record AppConfiguration
{
    public string PostgresConnectionString { get; init; } = string.Empty;
}
