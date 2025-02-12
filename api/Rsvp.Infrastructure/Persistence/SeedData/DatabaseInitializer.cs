namespace Rsvp.Infrastructure.Persistence.SeedData;

using Microsoft.Extensions.Logging;

using Rsvp.Infrastructure.Persistence.SeedData.Seeders;

public class DatabaseInitializer(IEnumerable<ISeeder> seeders, ILogger<DatabaseInitializer> logger)
{
  public void Initialize()
  {
    logger.LogInformation("Starting database seeding...");

    foreach (var seeder in seeders.OrderBy(s => s.Order))
    {
      seeder.Seed();
    }

    logger.LogInformation("Database seeding complete.");
  }
}
