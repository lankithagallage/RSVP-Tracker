namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

public interface ISeeder
{
  int Order { get; }
  void Seed();
}
