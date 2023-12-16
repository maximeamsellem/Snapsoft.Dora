using Snapsoft.Dora.Adapter.Postgres.Extensions;
using Snapsoft.Dora.Domain.Extensions;
using Snapsoft.Dora.WebApi;
using Snapsoft.Dora.WebApi.HosterServices;

var builder = WebApplication.CreateBuilder(args);

var appConfiguration = AddConfiguration(builder);
ConfigureServices(builder.Services, appConfiguration);

var app = builder.Build();
ConfigureHttpPipeline(app);
app.Run();

static void ConfigureServices(
    IServiceCollection services, 
    AppConfiguration appConfiguration)
{
    services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddPostgresAdapter(appConfiguration.PostgresConnectionString)
        .AddHostedService<DatabaseInitializerService>()
        .AddDomainValidators()
        .AddDomainConcreteCommandHandlers()
        .AddControllers();
}

static void ConfigureHttpPipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger().UseSwaggerUI();
    }

    app
        .UseHttpsRedirection()
        .UseAuthorization();

    app.MapControllers();
}

static AppConfiguration AddConfiguration(WebApplicationBuilder builder)
{
    return builder.Configuration
        .Get<AppConfiguration>() ?? throw new Exception("Could not build AppConfiguration");
}

public partial class Program { }