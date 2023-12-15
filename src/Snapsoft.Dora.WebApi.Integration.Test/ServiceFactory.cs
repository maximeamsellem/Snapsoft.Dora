using Microsoft.AspNetCore.Mvc.Testing;

namespace Snapsoft.Dora.WebApi.Integration.Test
{
    internal class ServiceFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private ServiceFactory() { }

        private static Lazy<ServiceFactory> _instance = new Lazy<ServiceFactory>(() => new ServiceFactory());

        public static ServiceFactory Instance => _instance.Value;

        public Task InitializeAsync()
        {
            // nothing for now but it could be for starting containers
            return Task.CompletedTask;
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            // nothing for now  but it could be for disposing containers
            return Task.CompletedTask;
        }
    }
}
