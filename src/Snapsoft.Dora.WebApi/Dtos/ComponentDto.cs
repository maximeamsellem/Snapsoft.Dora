namespace Snapsoft.Dora.WebApi.Dtos;

public record ComponentDto : BaseEntityDto
{
    public required string Name { get; init; }
}
