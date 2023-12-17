namespace Snapsoft.Dora.WebApi.Dtos;

public record ComponentDeploymentDto : BaseEntityDto
{
    public required string Version { get; init; }

    public required string CommitId { get; init; }

    public required long ComponentId { get; init; }

    public required string ComponentName { get; init; }
}
