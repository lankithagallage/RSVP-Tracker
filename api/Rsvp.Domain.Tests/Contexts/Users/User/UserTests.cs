namespace Rsvp.Domain.Tests.Contexts.Users.User;

using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Tests.Shared.JsonObjects;

public class UserTests
{
  private static readonly IJsonFileReader JsonFileReader =
    new DomainJsonFileReader(Path.Combine("Contexts", "Users", "User"));

  public static IEnumerable<object[]> GetUserTestData()
  {
    var users = JsonFileReader.LoadData<UserJson>("valid_users.json");
    return users.Select(u => new object[] { u });
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public void User_CreateNew_CanCreateNewUser(UserJson user)
  {
    var newUser = User.CreateNew(user.FirstName, user.LastName, user.Email, Enum.Parse<UserRole>(user.Role));
    Assert.NotNull(newUser);
    Assert.Equal(user.FirstName, newUser.FirstName);
    Assert.Equal(user.LastName, newUser.LastName);
    Assert.Equal(user.Email, newUser.Email);
    Assert.Equal(Enum.Parse<UserRole>(user.Role), newUser.Role);
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public void User_CreateNew_ShouldThrowIfFirstNameIsEmpty(UserJson user)
  {
    Assert.Throws<ArgumentException>(() =>
      User.CreateNew("", user.LastName, user.Email, Enum.Parse<UserRole>(user.Role)));
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public void User_CreateNew_ShouldThrowIfLastNameIsEmpty(UserJson user)
  {
    Assert.Throws<ArgumentException>(() =>
      User.CreateNew(user.FirstName, "", user.Email, Enum.Parse<UserRole>(user.Role)));
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public void User_CreateNew_ShouldThrowIfEmailIsEmpty(UserJson user)
  {
    Assert.Throws<ArgumentException>(() =>
      User.CreateNew(user.FirstName, user.LastName, "", Enum.Parse<UserRole>(user.Role)));
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public void User_CreateNew_ShouldThrowIfEmailIsInvalid(UserJson user)
  {
    Assert.Throws<ArgumentException>(() =>
      User.CreateNew(user.FirstName, user.LastName, "invalid-email", Enum.Parse<UserRole>(user.Role)));
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public void User_CreateNew_ShouldThrowIfRoleIsInvalid(UserJson user)
  {
    Assert.Throws<ArgumentException>(() => User.CreateNew(user.FirstName, user.LastName, user.Email, (UserRole)999));
  }
}
