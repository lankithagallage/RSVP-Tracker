namespace Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;

using Ardalis.Result;

using MediatR;

using Rsvp.Application.Features.Events.Dtos;

public class GetPaginatedEventsQuery(
  int page,
  int size,
  string? search = null,
  string? sort = "date",
  string? order = "asc")
  : IRequest<Result<PagedResult<List<EventDto>>>>
{
  public int Page { get; } = page;
  public int Size { get; } = size;
  public string? Search { get; } = search;

  public string Sort { get; } = IsValidSortField(sort) ? sort!.ToLower() : "date";

  public string Order { get; } = IsValidOrder(order) ? order!.ToLower() : "asc";

  private static bool IsValidSortField(string? value) =>
    value?.ToLower() is "date" or "title";

  private static bool IsValidOrder(string? value) =>
    value?.ToLower() is "asc" or "desc";
}
