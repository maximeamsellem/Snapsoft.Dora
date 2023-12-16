namespace Snapsoft.Dora.WebApi.Dtos;

public record UnprocessableEntityResponseDto
{
    public Dictionary<string, IEnumerable<string>>? PropertyErrors { get; init; }
}
