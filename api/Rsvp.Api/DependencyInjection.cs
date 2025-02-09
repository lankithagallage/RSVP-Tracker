namespace Rsvp.Api;

using Rsvp.Api.Extensions;
using Rsvp.Api.Middleware;
using Rsvp.Application;
using Rsvp.Infrastructure;

using Serilog;

public static class DependencyInjection
{
  public static WebApplication Configure(this WebApplication application)
  {
    application.UseRequestLogging();

    if (application.Environment.IsDevelopment())
    {
      application.UseSwagger();
      application.UseSwaggerUI();
    }

    application.UseExceptionHandler();

    application.UseHttpsRedirection();
    application.MapControllers();

    return application;
  }

  public static void AddServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    services.AddSwaggerServices(configuration);

    services.AddApplicationServices();
    services.AddInfrastructureServices(configuration, isDevelopment);
  }

  public static void AddLoggingServices(this WebApplicationBuilder builder)
  {
    // Add Serilog
    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .CreateLogger();

    builder.Host.UseSerilog();
  }

  private static void UseRequestLogging(this IApplicationBuilder builder)
  {
    builder.UseMiddleware<RequestLoggingMiddleware>();
  }
}
