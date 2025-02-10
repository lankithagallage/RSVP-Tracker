namespace Rsvp.Domain.Contexts.Users;

using System.Text.RegularExpressions;

public partial class User
{
  protected User() { }

  private User(Guid id, string firstName, string lastName, string email, UserRole role)
  {
    if (string.IsNullOrWhiteSpace(firstName))
    {
      throw new ArgumentException("First name cannot be empty.", nameof(firstName));
    }

    if (string.IsNullOrWhiteSpace(lastName))
    {
      throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
    }

    if (string.IsNullOrWhiteSpace(email))
    {
      throw new ArgumentException("Email cannot be empty.", nameof(email));
    }

    if (!IsValidEmail(email))
    {
      throw new ArgumentException("Invalid email format.", nameof(email));
    }

    if (!Enum.IsDefined(typeof(UserRole), role))
    {
      throw new ArgumentException("Invalid user role.", nameof(role));
    }

    this.Id = id;
    this.FirstName = firstName;
    this.LastName = lastName;
    this.Email = email;
    this.Role = role;
  }

  public Guid Id { get; private set; }
  public string FirstName { get; private set; }
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

  private static bool IsValidEmail(string email)
  {
    var emailRegex = EmailRegex();
    return emailRegex.IsMatch(email);
  }

  [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
  private static partial Regex EmailRegex();

  public void UpdateFirstName(string firstName)
  {
    if (string.IsNullOrWhiteSpace(firstName))
    {
      throw new ArgumentException("First name cannot be empty.", nameof(firstName));
    }

    this.FirstName = firstName;
  }
}
