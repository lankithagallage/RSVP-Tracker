namespace Rsvp.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public class RsvpContext : DbContext
{
  public RsvpContext(DbContextOptions<RsvpContext> options) : base(options) { }
}
