namespace Rsvp.Api;

using Rsvp.Application;
using Rsvp.Infrastructure;

public static class DependencyInjection
{
  public static void AddServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddApplicationServices();
    services.AddInfrastructureServices(configuration);
  }
}
