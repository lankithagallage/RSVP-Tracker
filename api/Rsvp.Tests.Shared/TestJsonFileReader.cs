namespace Rsvp.Tests.Shared;

using System.Reflection;
using System.Text.Json;

using Rsvp.Domain.Interfaces;

public abstract class TestJsonFileReader : IJsonFileReader
{
  private readonly string baseDirectory;

  protected TestJsonFileReader(string relativePath)
  {
    var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
    this.baseDirectory = Path.Combine(assemblyPath, relativePath);
  }

  public List<T> LoadData<T>(string fileName)
  {
    var jsonFilePath = Path.Combine(this.baseDirectory, fileName);

    if (!File.Exists(jsonFilePath))
    {
      throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
    }

    var json = File.ReadAllText(jsonFilePath);
    return JsonSerializer.Deserialize<List<T>>(json) ?? [];
  }
}
