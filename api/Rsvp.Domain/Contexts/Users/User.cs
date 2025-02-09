namespace Rsvp.Domain.Contexts.Users;

public class User
{
  public Guid Id { get; private set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string FullName => $"{this.FirstName} {this.LastName}";
  public string Email { get; private set; }
  public UserRole Role { get; private set; }

  protected User() { }

  public static User CreateNew(string firstName, string lastName, string email, UserRole role)
  {
    return new User(firstName, lastName, email, role);
  }

  private User(string firstName, string lastName, string email, UserRole role)
  {
    this.Id = Guid.NewGuid();
    this.FirstName = firstName;
    this.LastName = lastName;
    this.Email = email;
    this.Role = role;
  }
}
