using Snapsoft.Dora.Domain.Contracts.Commands;

namespace Snapsoft.Dora.WebApi.Integration.Test.CommandBuilders;

internal static class DefaultComponentCommands
{
    internal static CreateComponentCommand Create => new CreateComponentCommand
    { 
        Name = Guid.NewGuid().ToString() 
    };
}
