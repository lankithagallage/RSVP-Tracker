namespace Rsvp.Domain.Interfaces;

public interface IJsonFileReader
{
  List<T> LoadData<T>(string fileName);
}
