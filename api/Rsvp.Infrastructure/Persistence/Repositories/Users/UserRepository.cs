namespace Rsvp.Infrastructure.Persistence.Repositories.Users;

using Rsvp.Domain.Contexts.Users;

public class UserRepository(RsvpContext context) : Repository<User>(context), IUserRepository { }
