namespace Rsvp.Api;

using Rsvp.Application;
using Rsvp.Infrastructure;

using Serilog;

public static class DependencyInjection
{
  public static void AddServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddApplicationServices();
    services.AddInfrastructureServices(configuration);
  }

  public static void AddLoggingServices(this WebApplicationBuilder builder)
  {
    // Add Serilog
    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .CreateLogger();

    builder.Host.UseSerilog();
  }
}
