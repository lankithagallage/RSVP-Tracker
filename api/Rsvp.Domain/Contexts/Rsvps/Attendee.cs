namespace Rsvp.Domain.Contexts.Rsvps;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Users;

public class Attendee
{
  public Guid Id { get; private set; }
  public Event Event { get; private set; }
  public User User { get; private set; }
  public RsvpStatus Status { get; private set; }
  public DateTimeOffset CreatedAt { get; private set; }
  public DateTimeOffset ModifiedAt { get; private set; }

  private readonly TimeProvider timeProvider = TimeProvider.System;

  private Attendee() { }

  public static Attendee CreateNew(Event @event, User user)
  {
    return new Attendee(@event, user);
  }

  private Attendee(Event @event, User user)
  {
    this.Id = Guid.NewGuid();
    this.Event = @event;
    this.User = user;
    this.Status = RsvpStatus.Pending;
    this.CreatedAt = timeProvider.GetUtcNow();
  }

  public void Confirm()
  {
    this.Status = RsvpStatus.Confirmed;
    this.ModifiedAt = timeProvider.GetUtcNow();
  }

  public void Cancel()
  {
    this.Status = RsvpStatus.Cancelled;
  }
}
