using Flurl.Http;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using Snapsoft.Dora.WebApi.Integration.Test.Core;
using System.Net;

namespace Snapsoft.Dora.WebApi.Integration.Test.HttpRequestBuilders;

internal class ComponentDeploymentRequestExecutor
{
    private readonly ServiceFactory _factory;

    public ComponentDeploymentRequestExecutor(ServiceFactory factory)
    {
        _factory = factory;
    }
        
    internal async Task<ComponentDeploymentDto> CreateComponentDeploymentWithChecksAsync(
        CreateComponentDeploymentCommand createComponentDeploymentCommand)
    {
        var createResponse = await CreateComponentDeploymentAsync(createComponentDeploymentCommand);

        Assert.Equal(createResponse?.StatusCode, (int)HttpStatusCode.Created);

        var successDto = await createResponse!
            .GetJsonAsync<SuccessResponseDto<ComponentDeploymentDto>>();

        Assert.NotNull(successDto?.Data);

        return successDto.Data;

    }

    internal async Task<IFlurlResponse> CreateComponentDeploymentAsync(
        CreateComponentDeploymentCommand createComponentDeploymentCommand)
    {
        var createHttpRequest = BuildComponentHttpRequest();
        return await createHttpRequest.PostJsonAsync(createComponentDeploymentCommand);

    }

    internal async Task<IFlurlResponse> GetComponentDeploymentAsync(long componentDeploymentId)
    {
        return await BuildComponentHttpRequest()
            .AppendPathSegment(componentDeploymentId)
            .GetAsync();
    }
        
    private IFlurlRequest BuildComponentHttpRequest()
    {
        return new FlurlClient(_factory.CreateClient())
            .Request("ComponentDeployment")
            .AllowAnyHttpStatus();
    }
}
