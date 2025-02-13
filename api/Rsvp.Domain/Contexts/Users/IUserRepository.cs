namespace Rsvp.Domain.Contexts.Users;

public interface IUserRepository : IRepository<User>
{
  Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
