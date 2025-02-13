namespace Rsvp.Infrastructure.Persistence.Repositories.Users;

using Microsoft.EntityFrameworkCore;

using Rsvp.Domain.Contexts.Users;

public class UserRepository(RsvpContext context) : Repository<User>(context), IUserRepository
{
  public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
  {
    return context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
  }
}
