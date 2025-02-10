namespace Rsvp.Domain.Contexts.Events;

using Rsvp.Domain.Contexts.Rsvps;

public class Event
{
  private readonly List<Attendee> attendees = [];

  protected Event() { }

  private Event(Guid id, string title, string description, DateTime startTime, DateTime endTime)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Title cannot be empty.", nameof(title));
    }

    if (string.IsNullOrWhiteSpace(description))
    {
      throw new ArgumentException("Description cannot be empty.", nameof(description));
    }

    if (startTime >= endTime)
    {
      throw new ArgumentException("Start time must be before end time.", nameof(startTime));
    }

    if (startTime < DateTime.UtcNow)
    {
      throw new ArgumentException("Start time cannot be in the past.", nameof(startTime));
    }

    this.Id = id;
    this.Title = title;
    this.Description = description;
    this.StartTime = startTime;
    this.EndTime = endTime;
  }

  public Guid Id { get; private set; }
  public string Title { get; private set; }
  public string Description { get; private set; }
  public DateTime StartTime { get; private set; }
  public DateTime EndTime { get; private set; }

  public IReadOnlyCollection<Attendee> Attendees => this.attendees.AsReadOnly();

  public static Event CreateNew(Guid id, string title, string description, DateTime startTime, DateTime endTime)
  {
    return new Event(id, title, description, startTime, endTime);
  }

  public static Event CreateNew(string title, string description, DateTime startTime, DateTime endTime)
  {
    return new Event(Guid.NewGuid(), title, description, startTime, endTime);
  }

  public void AddAttendee(Attendee attendee)
  {
    if (attendee == null)
    {
      throw new ArgumentNullException(nameof(attendee), "Attendee cannot be null.");
    }

    if (this.attendees.Any(a => a.User.Email.Equals(attendee.User.Email, StringComparison.OrdinalIgnoreCase)))
    {
      throw new InvalidOperationException("Attendee with this email is already registered for this event.");
    }

    this.attendees.Add(attendee);
  }

  public void UpdateTitle(string title)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Title cannot be empty.", nameof(title));
    }

    this.Title = title;
  }
}
