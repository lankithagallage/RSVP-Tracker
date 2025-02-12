namespace Rsvp.Integration.Tests.Tests.Integration.v1.Events.SearchEvents;

using System.Net;
using System.Text.Json;

using Ardalis.Result;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Infrastructure.Persistence;
using Rsvp.Tests.Shared.Fixtures.Database;

[Collection("Database collection")]
public class EventsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>,
  IClassFixture<DatabaseFixture>
{
  private const string BaseUrl = "/api/v1/events/search";
  private readonly HttpClient client;

  private readonly JsonSerializerOptions jsonOptions = new()
    { PropertyNameCaseInsensitive = true };

  public EventsControllerIntegrationTests(WebApplicationFactory<Program> factory, DatabaseFixture fixture)
  {
    this.client = factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.RemoveAll(typeof(DbContextOptions<RsvpContext>));
        services.AddDbContext<RsvpContext>(options =>
          options.UseInMemoryDatabase(fixture.Context.Database.GetDbConnection().Database));
        services.AddSingleton(fixture.Context);
      });
    }).CreateClient();
  }

  [Fact]
  public async Task SearchEvents_ReturnsPaginatedResults()
  {
    var response = await this.client.GetAsync($"{BaseUrl}?page=1&size=10&sort=date&order=asc");
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<PagedResult<List<EventDto>>>(content, this.jsonOptions);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.NotEmpty(result.Value);
  }

  [Fact]
  public async Task SearchEvents_ReturnsBadRequest_ForInvalidPageNumber()
  {
    var response = await this.client.GetAsync($"{BaseUrl}?page=1000&size=10&sort=date&order=asc");
    var content = await response.Content.ReadAsStringAsync();

    Assert.NotNull(content);
    Assert.False(response.IsSuccessStatusCode);
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Theory]
  [InlineData("Health & Wellness")]
  [InlineData("Next-Gen Gaming")]
  [InlineData("NonExistingEvent")]
  public async Task SearchEvents_FiltersResultsBySearchTerm(string searchTerm)
  {
    var encodedSearchTerm = Uri.EscapeDataString(searchTerm);
    var response =
      await this.client.GetAsync($"{BaseUrl}?page=1&size=10&search={encodedSearchTerm}&sort=title&order=asc");
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<PagedResult<List<EventDto>>>(content, this.jsonOptions);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    if (result.Value.Any())
    {
      Assert.All(result.Value, e => Assert.Contains(searchTerm, e.Title, StringComparison.OrdinalIgnoreCase));
    }
    else
    {
      Assert.Empty(result.Value);
    }
  }

  [Theory]
  [InlineData("title", "asc")]
  [InlineData("title", "desc")]
  [InlineData("date", "asc")]
  [InlineData("date", "desc")]
  public async Task SearchEvents_ReturnsSortedResults(string sort, string order)
  {
    var response = await this.client.GetAsync($"{BaseUrl}?page=1&size=10&sort={sort}&order={order}");
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<PagedResult<List<EventDto>>>(content, this.jsonOptions);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.NotEmpty(result.Value);
  }
}
