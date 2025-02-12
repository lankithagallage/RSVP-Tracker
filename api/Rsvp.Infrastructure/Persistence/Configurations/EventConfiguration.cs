namespace Rsvp.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Rsvp.Domain.Contexts.Events;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
  public void Configure(EntityTypeBuilder<Event> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Title)
      .IsRequired()
      .HasColumnType("varchar(200)")
      .HasMaxLength(200);

    builder.Property(e => e.Description)
      .HasColumnType("varchar(500)")
      .HasMaxLength(500);

    builder.Property(e => e.StartTime)
      .IsRequired();

    builder.Property(e => e.EndTime)
      .IsRequired();

    builder.HasMany(e => e.Attendees)
      .WithOne(attendee => attendee.Event)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
