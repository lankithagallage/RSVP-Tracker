namespace Rsvp.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;

public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
  public void Configure(EntityTypeBuilder<Attendee> builder)
  {
    builder.HasKey(a => a.Id);

    builder.Property(attendee => attendee.Status)
      .HasDefaultValue(RsvpStatus.Pending)
      .HasColumnType("varchar(20)")
      .HasConversion<string>()
      .IsRequired();

    builder.HasOne<Event>(attendee => attendee.Event)
      .WithMany(@event => @event.Attendees)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne<User>(attendee => attendee.User)
      .WithMany()
      .OnDelete(DeleteBehavior.Cascade);
  }
}
