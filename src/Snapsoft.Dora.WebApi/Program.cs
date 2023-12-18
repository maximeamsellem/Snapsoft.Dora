using DotNetEnv;
using Snapsoft.Dora.Adapter.Postgres.Extensions;
using Snapsoft.Dora.Domain.Extensions;
using Snapsoft.Dora.WebApi;
using Snapsoft.Dora.WebApi.HosterServices;

#if DEBUG
    Env.Load("../../.env");
#endif

var host = Host.CreateDefaultBuilder(args);

host
    .ConfigureWebHostDefaults(x =>
    {
        x.ConfigureAppConfiguration(ConfigureAppConfiguration);
        x.ConfigureServices(ConfigureServices);
        x.Configure(ConfigureHttpPipeline);
    });

var app = host.Build();

using (app)
{
    await app.StartAsync();
    await app.WaitForShutdownAsync();
}

static void ConfigureServices(
    WebHostBuilderContext webHostBuilderContext,
    IServiceCollection services)
{
    var appConfiguration = webHostBuilderContext
        .Configuration
        .Get<AppConfiguration>() ?? throw new Exception("Could not build AppConfiguration");

    services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddPostgresAdapter(appConfiguration.PostgresConnectionString)
        .AddHostedService<DatabaseInitializerService>()
        .AddDomainValidators()
        .AddDomainConcreteCommandHandlers();

    services
        .AddControllers()
        .AddJsonOptions(jsonOption =>
        {
            jsonOption.JsonSerializerOptions.WriteIndented = 
                !webHostBuilderContext.HostingEnvironment.IsProduction();
        });
}

static void ConfigureHttpPipeline(
    WebHostBuilderContext webHostBuilderContext,
    IApplicationBuilder app)
{
    app.UseRouting();

    if (webHostBuilderContext.HostingEnvironment.IsDevelopment())
    {
        app.UseSwagger().UseSwaggerUI();
    }

    app
        .UseHttpsRedirection()
        .UseAuthorization();

    app.UseEndpoints(endpoints => endpoints.MapControllers());
}

static void ConfigureAppConfiguration(
    WebHostBuilderContext _,
    IConfigurationBuilder configurationBuilder)
{
    configurationBuilder.AddEnvironmentVariables();
}

public partial class Program { }