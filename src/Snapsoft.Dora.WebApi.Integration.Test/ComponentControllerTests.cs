using Flurl.Http;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using Snapsoft.Dora.WebApi.Integration.Test.Core;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            var command = BuildCreateComponentCommand();

            // Act
            var actual = await createHttpRequest.PostJsonAsync(command);
            
            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.Created);

            var body = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();
            Assert.Equal(body.Data?.Name, command.Name);
            Assert.True(body.Data?.Id > 0);
        }

        [Fact]
        public async Task CreateComponent_Returns_Always_Increment_Id()
        {
            // Arrange
            var command1 = BuildCreateComponentCommand();
            var command2 = BuildCreateComponentCommand();

            // Act            
            var createdComponent1 = await CreateComponentWithChecks(command1);
            var createdComponent2 = await CreateComponentWithChecks(command2);

            // Assert
            Assert.True(createdComponent1.Id < createdComponent2.Id);
        }

        [Fact]
        public async Task GetComponent_Returns_Request_Component()
        {
            // Arrange
            var command = BuildCreateComponentCommand();
            var createdComponent = await CreateComponentWithChecks(command);

            // Act
            var actual = await BuildComponentHttpRequest()
                .AppendPathSegment(createdComponent.Id)
                .GetAsync();

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.OK);

            var actualComponent = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();
            Assert.Equal(createdComponent.Name, command.Name);            
            Assert.Equal(createdComponent.Id, actualComponent?.Data?.Id);
        }

        [Fact]
        public async Task GetComponent_Returns_Not_Found_When_Component_Does_Not_Exists()
        {
            // Act
            var actual = await GetComponent(Random.Shared.NextInt64());

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.NotFound);
        }
                
        private static CreateComponentCommand BuildCreateComponentCommand()
        {
            return new CreateComponentCommand
            {
                Name = Guid.NewGuid().ToString(),
            };
        }

        private async Task<ComponentDto> CreateComponentWithChecks(CreateComponentCommand createComponentCommand)
        {
            var createHttpRequest = BuildComponentHttpRequest();
            var createResponse = await createHttpRequest.PostJsonAsync(createComponentCommand);

            Assert.Equal(createResponse?.StatusCode, (int)HttpStatusCode.Created);
            
            var successDto = await createResponse!
                .GetJsonAsync<SuccessResponseDto<ComponentDto>>();

            Assert.NotNull(successDto?.Data);

            return successDto.Data;

        }

        private async Task<IFlurlResponse> GetComponent(long componentId)
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
}