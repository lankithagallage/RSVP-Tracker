namespace Rsvp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Infrastructure.Persistence;
using Rsvp.Infrastructure.Persistence.Repositories.Events;
using Rsvp.Infrastructure.Persistence.Repositories.Rsvps;
using Rsvp.Infrastructure.Persistence.Repositories.Users;
using Rsvp.Infrastructure.Persistence.SeedData;
using Rsvp.Infrastructure.Persistence.SeedData.Seeders;

public static class DependencyInjection
{
  public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration,
    bool isDevelopment)
  {
    services.AddScoped<ISeeder, EventSeeder>();
    services.AddScoped<ISeeder, UserSeeder>();
    services.AddScoped<ISeeder, AttendeeSeeder>();
    services.AddScoped<DatabaseInitializer>();

    services.AddScoped<IEventRepository, EventRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAttendeeRepository, AttendeeRepository>();

    services.AddDbContext<RsvpContext>(options =>
    {
      options.UseSqlServer(configuration.GetConnectionString("RsvpSqlDbConnection"),
        sqlOptions =>
        {
          sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
        });

      if (isDevelopment)
      {
        options.EnableSensitiveDataLogging();
      }
    });
  }

  public static void AddSeeding(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    initializer.Initialize();
  }
}
