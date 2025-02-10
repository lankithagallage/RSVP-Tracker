namespace Rsvp.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Infrastructure.Persistence.Configurations;

public class RsvpContext : DbContext
{
  public RsvpContext(DbContextOptions<RsvpContext> options) : base(options) { }

  public DbSet<Event> Events { get; init; }
  public DbSet<Attendee> Attendees { get; init; }
  public DbSet<User> Users { get; init; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfigurationAssemblyMarker).Assembly);
  }
}
