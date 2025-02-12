namespace Rsvp.Infrastructure.Tests.Fixtures;

using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence;
using Rsvp.Infrastructure.Persistence.SeedData;
using Rsvp.Infrastructure.Persistence.SeedData.Seeders;

public class DatabaseFixture : IDisposable
{
  private readonly IServiceScope scope;

  public DatabaseFixture()
  {
    var services = new ServiceCollection();
    services.AddLogging();

    var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
    services.AddSingleton<IJsonFileReader>(
      new JsonFileReader(Path.Combine(assemblyPath, "Persistence", "SeedData", "Json")));

    services.AddDbContext<RsvpContext>(options =>
      options.UseInMemoryDatabase($"TestDatabase:{Guid.NewGuid()}"));

    services.AddScoped<ISeeder, EventSeeder>();
    services.AddScoped<ISeeder, UserSeeder>();
    services.AddScoped<ISeeder, AttendeeSeeder>();
    services.AddScoped<DatabaseInitializer>();

    var provider = services.BuildServiceProvider();
    this.scope = provider.CreateScope();
    this.Context = this.scope.ServiceProvider.GetRequiredService<RsvpContext>();

    // Ensure database is created and seed data is loaded
    this.Context.Database.EnsureCreated();
    SeedDatabase(provider);
  }

  public RsvpContext Context { get; }

  public void Dispose()
  {
    this.Context.Database.EnsureDeleted();
    this.Context.Dispose();
    this.scope.Dispose();
  }

  private static void SeedDatabase(IServiceProvider serviceProvider)
  {
    var initializer = serviceProvider.GetRequiredService<DatabaseInitializer>();
    initializer.Initialize();
  }
}
