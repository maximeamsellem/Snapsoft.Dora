using Flurl.Http;
using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using Snapsoft.Dora.WebApi.Integration.Test.CommandBuilders;
using Snapsoft.Dora.WebApi.Integration.Test.Core;
using Snapsoft.Dora.WebApi.Integration.Test.HttpRequestBuilders;
using System.Net;

namespace Snapsoft.Dora.WebApi.Integration.Test
{
    [Collection(nameof(ServiceFactory))]
    public class ComponentDeploymentControllerTests
    {
        private readonly ComponentRequestExecutor _componentRequestExecutor;
        private readonly ComponentDeploymentRequestExecutor _sutRequestExecutor;

        public ComponentDeploymentControllerTests(ServiceFactory factory)
        {
            _componentRequestExecutor = new ComponentRequestExecutor(factory);
            _sutRequestExecutor = new ComponentDeploymentRequestExecutor(factory);
        }

        [Fact]
        public async Task CreateComponentDeployment_Returns_Ok()
        {
            // Arrange
            var component = await _componentRequestExecutor.CreateComponentWithChecksAsync();

            var command = DefaultComponentDeploymentCommands.Create with
            {
                ComponentId = component.Id,
                CommitId = Generator.RandomString(length: 200),
                Version = Generator.RandomString(length: 100),
            };

            // Act
            var actual = await _sutRequestExecutor.CreateComponentDeploymentAsync(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, actual?.StatusCode);

            var actualResponseDto = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDeploymentDto>>();
            CheckComponentDtoValues(command, actualResponseDto);
            Assert.True(actualResponseDto.Data?.Id > 0);
        }

        [Fact]
        public async Task CreateComponentDeployment_Returns_Conflict_When_Duplicate_Version()
        {
            // Arrange
            var component = await _componentRequestExecutor.CreateComponentWithChecksAsync();

            var command = DefaultComponentDeploymentCommands.Create with
            {
                ComponentId = component.Id,
            };

            await _sutRequestExecutor.CreateComponentDeploymentAsync(command);

            command = command with { CommitId = Generator.RandomString() };

            // Act                        
            var actual = await _sutRequestExecutor.CreateComponentDeploymentAsync(command);

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.Conflict);

            var actualResponseDto = await actual!.GetJsonAsync<UnprocessableEntityResponseDto>();

            Assert.NotNull(actualResponseDto?.PropertyErrors);
            Assert.Contains("Version", actualResponseDto.PropertyErrors.Keys);

            Assert.Equal(
                $"'Version' '{command.Version}' is already used",
                actualResponseDto.PropertyErrors["Version"].Single());
        }

        [Fact]
        public async Task CreateComponentDeployment_Returns_Conflict_When_Duplicate_CommitId()
        {
            // Arrange
            var component = await _componentRequestExecutor.CreateComponentWithChecksAsync();

            var command = DefaultComponentDeploymentCommands.Create with
            {
                ComponentId = component.Id,
            };

            await _sutRequestExecutor.CreateComponentDeploymentAsync(command);

            command = command with { Version = Generator.RandomString() };

            // Act                        
            var actual = await _sutRequestExecutor.CreateComponentDeploymentAsync(command);

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.Conflict);

            var actualResponseDto = await actual!.GetJsonAsync<UnprocessableEntityResponseDto>();

            Assert.NotNull(actualResponseDto?.PropertyErrors);
            Assert.Contains("CommitId", actualResponseDto.PropertyErrors.Keys);

            Assert.Equal(
                $"'CommitId' '{command.CommitId}' is already used",
                actualResponseDto.PropertyErrors["CommitId"].Single());
        }

        [Fact]
        public async Task CreateComponentDeployment_Returns_Ok_When_Creating_Several_Deployments()
        {
            // Arrange
            var component = await _componentRequestExecutor.CreateComponentWithChecksAsync();
                        
            // Act                        
            var createTasks = Enumerable.Range(0, 10).Select(_ =>
            {
                return _sutRequestExecutor.CreateComponentDeploymentAsync(DefaultComponentDeploymentCommands.Create with
                {
                    ComponentId = component.Id,
                });
            }).ToArray();

            await Task.WhenAll(createTasks);
                        
            // Assert
            Assert.True(createTasks.All(t => t.Result.StatusCode == (int)HttpStatusCode.Created));

            var componentDeploymentIds = createTasks.Select(t => t.Id).Distinct();

            Assert.Equal(createTasks.Count(), componentDeploymentIds.Count());
        }

        [Fact]
        public async Task GetComponentDeployment_Returns_Request_Component()
        {
            // Arrange
            var component = await _componentRequestExecutor.CreateComponentWithChecksAsync();

            var command = DefaultComponentDeploymentCommands.Create with
            {
                ComponentId = component.Id,
            };

            var componentDeployment = await _sutRequestExecutor.CreateComponentDeploymentWithChecksAsync(command);

            // Act
            var actual = await _sutRequestExecutor.GetComponentDeploymentAsync(componentDeployment.Id);

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.OK);

            var actualResponseDto = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDeploymentDto>>();
            CheckComponentDtoValues(command, actualResponseDto);
            Assert.Equal(componentDeployment.Id, actualResponseDto?.Data?.Id);
        }

        private static void CheckComponentDtoValues(Domain.Contracts.Commands.CreateComponentDeploymentCommand command, SuccessResponseDto<ComponentDeploymentDto> actualResponseDto)
        {
            Assert.Equal(command.CommitId, actualResponseDto.Data?.CommitId);
            Assert.Equal(command.Version, actualResponseDto.Data?.Version);
            Assert.Equal(command.ComponentId, actualResponseDto.Data?.ComponentId);
        }
    }
}