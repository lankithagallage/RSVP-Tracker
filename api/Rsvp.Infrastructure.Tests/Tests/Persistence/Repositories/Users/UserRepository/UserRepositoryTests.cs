namespace Rsvp.Infrastructure.Tests.Tests.Persistence.Repositories.Users.UserRepository;

using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.Repositories.Users;
using Rsvp.Infrastructure.Tests.Fixtures;
using Rsvp.Tests.Shared.JsonObjects;

[Collection("Database collection")]
public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
  private static readonly IJsonFileReader JsonFileReader = new InfrastructureJsonFileReader(Path.Combine("Tests",
    "Persistence", "Repositories", "Users", "UserRepository"));

  private readonly UserRepository userRepository;

  public UserRepositoryTests(DatabaseFixture fixture)
  {
    var context = fixture.Context;
    this.userRepository = new UserRepository(context);
  }

  public static IEnumerable<object[]> GetUserTestData()
  {
    var users = JsonFileReader.LoadData<UserJson>("valid_users.json");
    return users.Select(u => new object[] { u });
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public async Task UserRepository_CanAddUser(UserJson userJson)
  {
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    await this.userRepository.AddAsync(newUser, CancellationToken.None);

    var fetchedUser = await this.userRepository.GetByIdAsync(newUser.Id, CancellationToken.None);
    Assert.NotNull(fetchedUser);
    Assert.Equal(newUser.Email, fetchedUser.Email);
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public async Task UserRepository_CanUpdateUser(UserJson userJson)
  {
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    await this.userRepository.AddAsync(newUser, CancellationToken.None);

    newUser.UpdateFirstName("UpdatedFirstName");
    await this.userRepository.UpdateAsync(newUser, CancellationToken.None);

    var updatedUser = await this.userRepository.GetByIdAsync(newUser.Id, CancellationToken.None);
    Assert.NotNull(updatedUser);
    Assert.Equal("UpdatedFirstName", updatedUser.FirstName);
  }

  [Theory]
  [MemberData(nameof(GetUserTestData))]
  public async Task UserRepository_CanDeleteUser(UserJson userJson)
  {
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    await this.userRepository.AddAsync(newUser, CancellationToken.None);

    await this.userRepository.DeleteAsync(newUser.Id, CancellationToken.None);

    var deletedUser = await this.userRepository.GetByIdAsync(newUser.Id, CancellationToken.None);
    Assert.Null(deletedUser);
  }
}
