namespace Rsvp.Domain.Contexts.Rsvps;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Users;

public class Attendee
{
  private readonly TimeProvider timeProvider = TimeProvider.System;

  private Attendee() { }

  private Attendee(Guid id, Event @event, User user, DateTimeOffset createdAt, DateTimeOffset modifiedAt)
  {
    this.Id = id;
    this.Event = @event;
    this.User = user;
    this.Status = RsvpStatus.Pending;
    this.CreatedAt = createdAt;
    this.ModifiedAt = modifiedAt;
  }

  public Guid Id { get; private set; }
  public Event Event { get; private set; }
  public User User { get; private set; }
  public RsvpStatus Status { get; private set; }
  public DateTimeOffset CreatedAt { get; private set; }
  public DateTimeOffset ModifiedAt { get; private set; }

  public static Attendee CreateNew(Guid id, Event @event, User user, DateTimeOffset createdAt,
    DateTimeOffset modifiedAt)
  {
    return new Attendee(id, @event, user, createdAt, modifiedAt);
  }

  public Attendee CreateNew(Event @event, User user)
  {
    return new Attendee(Guid.NewGuid(), @event, user, this.timeProvider.GetUtcNow(), this.timeProvider.GetUtcNow());
  }

  public void Confirm()
  {
    this.Status = RsvpStatus.Confirmed;
    this.ModifiedAt = this.timeProvider.GetUtcNow();
  }

  public void Cancel()
  {
    this.Status = RsvpStatus.Cancelled;
    this.ModifiedAt = this.timeProvider.GetUtcNow();
  }
}
