﻿namespace Rsvp.Domain.Contexts.Events;

using Rsvp.Domain.Contexts.Rsvps;

public class Event
{
  public Guid Id { get; private set; }
  public string Title { get; private set; }
  public string Description { get; private set; }
  public DateTime StartTime { get; private set; }
  public DateTime EndTime { get; private set; }

  private readonly List<Attendee> attendees = [];

  public IReadOnlyCollection<Attendee> Attendees => this.attendees.AsReadOnly();

  protected Event() { }

  public static Event CreateNew(string title, string description, DateTime startTime, DateTime endTime)
  {
    return new Event(title, description, startTime, endTime);
  }

  private Event(string title, string description, DateTime startTime, DateTime endTime)
  {
    this.Id = Guid.NewGuid();
    this.Title = title;
    this.Description = description;
    this.StartTime = startTime;
    this.EndTime = endTime;
  }

  public void AddAttendee(Attendee attendee)
  {
    if (this.attendees.Any(a => a.User.Email == attendee.User.Email))
      throw new InvalidOperationException("Attendee is already registered for this event.");

    this.attendees.Add(attendee);
  }
}
