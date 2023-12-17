namespace Snapsoft.Dora.WebApi.Dtos;

public record UnprocessableEntityResponseDto
{
    public IReadOnlyDictionary<string, IEnumerable<string>>? PropertyErrors { get; init; }
}
