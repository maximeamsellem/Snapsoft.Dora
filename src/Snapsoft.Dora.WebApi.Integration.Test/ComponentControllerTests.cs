using Snapsoft.Dora.Domain.Contracts.Commands;
using Snapsoft.Dora.WebApi.Dtos;
using Snapsoft.Dora.WebApi.Integration.Test.CommandBuilders;
using Snapsoft.Dora.WebApi.Integration.Test.Core;
using Snapsoft.Dora.WebApi.Integration.Test.HttpRequestBuilders;
using System.Net;

namespace Snapsoft.Dora.WebApi.Integration.Test
{
    [Collection(nameof(ServiceFactory))]
    public class ComponentControllerTests
    {
        private readonly ComponentRequestExecutor _sutRequestExecutor;

        public ComponentControllerTests(ServiceFactory factory)
        {
            _sutRequestExecutor = new ComponentRequestExecutor(factory);
        }

        [Fact]
        public async Task CreateComponent_Returns_Ok()
        {
            // Arrange
            var command = DefaultComponentCommands.Create with 
            { 
                Name = Generator.RandomString(length: 100),
            };

            // Act
            var actual = await _sutRequestExecutor.CreateComponentAsync(command);
            
            // Assert
            Assert.Equal((int)HttpStatusCode.Created, actual?.StatusCode);

            var actualResponseDto = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();
            Assert.Equal(command.Name, actualResponseDto.Data?.Name);
            Assert.True(actualResponseDto.Data?.Id > 0);
        }

        [Fact]
        public async Task CreateComponent_Returns_Always_Increment_Id()
        {
            // Act            
            var createdComponent1 = await _sutRequestExecutor
                .CreateComponentWithChecksAsync(DefaultComponentCommands.Create);

            var createdComponent2 = await _sutRequestExecutor
                .CreateComponentWithChecksAsync(DefaultComponentCommands.Create);

            // Assert
            Assert.True(createdComponent1.Id < createdComponent2.Id);
        }

        [Fact]
        public async Task CreateComponent_Returns_Conflict_When_Duplicate_Name()
        {
            // Arrange
            var command = DefaultComponentCommands.Create;

            await _sutRequestExecutor.CreateComponentWithChecksAsync(command);

            command = command with { Name = command.Name.ToLower() };

            // Act                        
            var actual = await _sutRequestExecutor.CreateComponentAsync(command);

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.Conflict);

            var actualResponseDto = await actual!.GetJsonAsync<UnprocessableEntityResponseDto>();

            Assert.NotNull(actualResponseDto?.PropertyErrors);
            Assert.Contains("Name", actualResponseDto.PropertyErrors.Keys);

            Assert.Equal(
                $"'Name' '{command.Name}' is already used",
                actualResponseDto.PropertyErrors["Name"].Single());
        }

        [Theory]
        [InlineData("a")]
        [InlineData("1")]
        [InlineData("a1")]
        [InlineData("ab")]
        public async Task CreateComponent_Returns_Unprocessable_When_Name_Is_Less_Than_3_Character(string name)
        {
            await CreateComponent_Returns_Unprocessable_When_Wrong_Name_Length(name);
        }

        [Fact]
        public async Task CreateComponent_Returns_Unprocessable_When_Name_Is_More_Than_100_Character()
        {
            var longName = Generator.RandomString(length: 101);

            await CreateComponent_Returns_Unprocessable_When_Wrong_Name_Length(longName);
        }
        
        [Fact]
        public async Task GetComponent_Returns_Request_Component()
        {
            // Arrange
            var command = DefaultComponentCommands.Create;
            var createdComponent = await _sutRequestExecutor.CreateComponentWithChecksAsync(command);

            // Act
            var actual = await _sutRequestExecutor.GetComponentAsync(createdComponent.Id);

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.OK);

            var actualComponent = await actual!.GetJsonAsync<SuccessResponseDto<ComponentDto>>();
            Assert.Equal(command.Name, createdComponent.Name);
            Assert.Equal(createdComponent.Id, actualComponent?.Data?.Id);
        }

        [Fact]
        public async Task GetComponent_Returns_Not_Found_When_Component_Does_Not_Exists()
        {
            // Act
            var actual = await _sutRequestExecutor.GetComponentAsync(Random.Shared.NextInt64());

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, actual?.StatusCode);
        }

        private async Task CreateComponent_Returns_Unprocessable_When_Wrong_Name_Length(string name)
        {
            // Arrange            
            var command = DefaultComponentCommands.Create with { Name = name};

            // Act                        
            var actual = await _sutRequestExecutor.CreateComponentAsync(command);

            // Assert
            Assert.Equal(actual?.StatusCode, (int)HttpStatusCode.UnprocessableEntity);

            var actualResponseDto = await actual!.GetJsonAsync<UnprocessableEntityResponseDto>();

            Assert.NotNull(actualResponseDto?.PropertyErrors);
            Assert.Contains(
                nameof(CreateComponentCommand.Name),
                actualResponseDto.PropertyErrors.Keys);

            Assert.Equal(
                $"'Name' must be between 3 and 100 characters. You entered {name.Length} characters.",
                actualResponseDto.PropertyErrors[nameof(CreateComponentCommand.Name)].Single());
        }        
    }
}