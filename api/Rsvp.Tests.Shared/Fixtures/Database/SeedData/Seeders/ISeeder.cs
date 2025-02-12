namespace Rsvp.Tests.Shared.Fixtures.Database.SeedData.Seeders;

public interface ISeeder
{
  int Order { get; }
  void Seed();
}
