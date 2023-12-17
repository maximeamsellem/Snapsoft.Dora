using Flurl.Http;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using Snapsoft.Dora.WebApi.Integration.Test.CommandBuilders;
using Snapsoft.Dora.WebApi.Integration.Test.Core;
using System.Net;

namespace Snapsoft.Dora.WebApi.Integration.Test.HttpRequestBuilders;

internal class ComponentRequestExecutor
{
    private readonly ServiceFactory _factory;

    public ComponentRequestExecutor(ServiceFactory factory)
    {
        _factory = factory;
    }

    internal async Task<ComponentDto> CreateComponentWithChecksAsync()
    {
        return await CreateComponentWithChecksAsync(DefaultComponentCommands.Create);
    }

    internal async Task<ComponentDto> CreateComponentWithChecksAsync(CreateComponentCommand createComponentCommand)
    {
        var createResponse = await CreateComponentAsync(createComponentCommand);

        Assert.Equal(createResponse?.StatusCode, (int)HttpStatusCode.Created);

        var successDto = await createResponse!
            .GetJsonAsync<SuccessResponseDto<ComponentDto>>();

        Assert.NotNull(successDto?.Data);

        return successDto.Data;

    }

    internal async Task<IFlurlResponse> CreateComponentAsync(CreateComponentCommand createComponentCommand)
    {
        var createHttpRequest = BuildComponentHttpRequest();
        return await createHttpRequest.PostJsonAsync(createComponentCommand);

    }

    internal async Task<IFlurlResponse> GetComponentAsync(long componentId)
    {
        return await BuildComponentHttpRequest()
            .AppendPathSegment(componentId)
            .GetAsync();
    }
        
    private IFlurlRequest BuildComponentHttpRequest()
    {
        return new FlurlClient(_factory.CreateClient())
            .Request("Component")
            .AllowAnyHttpStatus();
    }
}
