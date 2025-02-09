namespace Rsvp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Rsvp.Infrastructure.Persistence;

public static class DependencyInjection
{
  public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<RsvpContext>(options =>
      options.UseSqlServer(configuration.GetConnectionString("RsvpSqlDbConnection")));
  }
}
