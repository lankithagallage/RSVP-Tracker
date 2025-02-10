namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using System.Reflection;

public static class JsonFileReader
{
  public static string LoadJsonFile(string fileName)
  {
    // Get the full path of the build directory
    var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    var jsonFilePath = Path.Combine(baseDirectory ?? throw new InvalidOperationException(), "Persistence", "SeedData",
      "Json", fileName);

    if (!File.Exists(jsonFilePath))
    {
      throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
    }

    return File.ReadAllText(jsonFilePath);
  }
}
