using Flurl.Http;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using Snapsoft.Dora.WebApi.Integration.Test.Core;
using System.Net;

namespace Snapsoft.Dora.WebApi.Integration.Test
{
    [Collection(nameof(ServiceFactory))]
    public class ComponentControllerTests
    {
        private readonly ServiceFactory _factory;

        public ComponentControllerTests(ServiceFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateComponent_Returns_Ok()
        {
            // Arrange
            var createHttpRequest = BuildComponentHttpRequest();
            var command = new CreateComponentCommand
            {
                Name = Guid.NewGuid().ToString(),
            };

            // Act
            var actual = await createHttpRequest.PostJsonAsync(command);
            
            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.Created);

            var body = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();
            Assert.Equal(body.Data?.Name, command.Name);
            Assert.True(body.Data?.Id > 0);
        }
                
        [Fact]
        public async Task GetComponent_Returns_It()
        {
            // Arrange
            var createHttpRequest = BuildComponentHttpRequest();
            var command = new CreateComponentCommand
            {
                Name = Guid.NewGuid().ToString(),
            };

            var createResponse = await createHttpRequest.PostJsonAsync(command);
            Assert.Equal(createResponse?.StatusCode, (int)HttpStatusCode.Created);
            var createdComponent = await createResponse!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();

            // Act
            var actual = await createHttpRequest
                .AppendPathSegment(createdComponent.Data?.Id)
                .GetAsync();

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.OK);

            var actualComponent = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();
            Assert.Equal(createdComponent.Data?.Name, command.Name);
            Assert.NotNull(actualComponent?.Data?.Id);
            Assert.Equal(createdComponent.Data?.Id, actualComponent?.Data?.Id);
        }

        private IFlurlRequest BuildComponentHttpRequest()
        {
            return new FlurlClient(_factory.CreateClient())
                .Request("Component")
                .AllowAnyHttpStatus();
        }
    }
}