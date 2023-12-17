namespace Snapsoft.Dora.WebApi.Dtos;

public record ComponentDto : BaseEntityDto
{
    public string Name { get; init; } = string.Empty;
}
