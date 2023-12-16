namespace Snapsoft.Dora.WebApi.Dtos;

public record SuccessResponseDto<T>
{
    public T? Data { get; init; }
}
