namespace Rsvp.Domain.Contexts.Rsvps;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Users;

public class Attendee
{
  private static readonly TimeProvider TimeProvider = TimeProvider.System;

  private Attendee() { }

  private Attendee(Guid id, Event @event, User user)
  {
    if (@event == null)
    {
      throw new ArgumentNullException(nameof(@event), "Event cannot be null.");
    }

    if (user == null)
    {
      throw new ArgumentNullException(nameof(user), "User cannot be null.");
    }

    this.Id = id;
    this.Event = @event;
    this.User = user;
    this.Status = RsvpStatus.Pending;
    this.CreatedAt = TimeProvider.GetUtcNow();
    this.ModifiedAt = TimeProvider.GetUtcNow();
  }

  public Guid Id { get; private set; }
  public Event Event { get; private set; }
  public User User { get; private set; }
  public RsvpStatus Status { get; private set; }
  public DateTimeOffset CreatedAt { get; private set; }
  public DateTimeOffset ModifiedAt { get; private set; }

  public static Attendee CreateNew(Guid id, Event @event, User user)
  {
    return new Attendee(id, @event, user);
  }

  public static Attendee CreateNew(Event @event, User user)
  {
    return new Attendee(Guid.NewGuid(), @event, user);
  }

  public void Confirm()
  {
    this.Status = RsvpStatus.Confirmed;
    this.ModifiedAt = TimeProvider.GetUtcNow();
  }

  public void Cancel()
  {
    this.Status = RsvpStatus.Cancelled;
    this.ModifiedAt = TimeProvider.GetUtcNow();
  }
}
