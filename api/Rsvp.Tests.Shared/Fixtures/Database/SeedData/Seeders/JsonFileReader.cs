namespace Rsvp.Tests.Shared.Fixtures.Database.SeedData.Seeders;

using System.Text.Json;

using Rsvp.Domain.Interfaces;

public class JsonFileReader(string baseDirectory) : IJsonFileReader
{
  public List<T> LoadData<T>(string fileName)
  {
    var jsonFilePath = Path.Combine(baseDirectory, fileName);

    if (!File.Exists(jsonFilePath))
    {
      throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
    }

    var json = File.ReadAllText(jsonFilePath);
    return JsonSerializer.Deserialize<List<T>>(json) ?? [];
  }
}
