namespace Rsvp.Domain.Contexts.Users;

public class User
{
  protected User() { }

  private User(Guid id, string firstName, string lastName, string email, UserRole role)
  {
    this.Id = id;
    this.FirstName = firstName;
    this.LastName = lastName;
    this.Email = email;
    this.Role = role;
  }

  public Guid Id { get; private set; }
  public string FirstName { get; }
  public string LastName { get; }
  public string FullName => $"{this.FirstName} {this.LastName}";
  public string Email { get; private set; }
  public UserRole Role { get; private set; }

  public static User CreateNew(string firstName, string lastName, string email, UserRole role)
  {
    return new User(Guid.NewGuid(), firstName, lastName, email, role);
  }

  public static User CreateNew(Guid id, string firstName, string lastName, string email, UserRole role)
  {
    return new User(id, firstName, lastName, email, role);
  }
}
