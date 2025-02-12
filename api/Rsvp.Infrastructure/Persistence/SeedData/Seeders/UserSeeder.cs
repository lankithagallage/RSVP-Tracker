namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.SeedData.Json;

public class UserSeeder(RsvpContext context, IJsonFileReader jsonReader, ILogger<UserSeeder> logger) : ISeeder
{
  public void Seed()
  {
    if (context.Users.Any())
    {
      logger.LogInformation("Users already exist. Skipping seeding.");
      return;
    }

    var users = jsonReader.LoadData<UserJson>("users.json");

      // @formatter:off
      foreach (var newUser in users.Select(u =>
                User.CreateNew(u.Id, u.FirstName, u.LastName, u.Email, Enum.Parse<UserRole>(u.Role))))
      {
        context.Users.Add(newUser);
      }
    // @formatter:on

    context.SaveChanges();
    logger.LogInformation("Seeded users.");
  }
}
