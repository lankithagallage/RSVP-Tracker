namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using System.Text.Json;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Users;
using Rsvp.Infrastructure.Persistence.SeedData.Json;

public class UserSeeder(RsvpContext context, ILogger<UserSeeder> logger) : ISeeder
{
  public void Seed()
  {
    if (context.Users.Any())
    {
      logger.LogInformation("Users already exist. Skipping seeding.");
      return;
    }

    var json = JsonFileReader.LoadJsonFile("users.json");
    var users = JsonSerializer.Deserialize<List<UserJson>>(json);

    if (users != null)
    {
      // @formatter:off
      foreach (var newUser in users.Select(u =>
                User.CreateNew(u.Id, u.FirstName, u.LastName, u.Email, Enum.Parse<UserRole>(u.Role))))
      {
        context.Users.Add(newUser);
      }
      // @formatter:on
    }

    context.SaveChanges();
    logger.LogInformation("Seeded users.");
  }
}
