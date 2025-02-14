import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import {
  Client,
  EventDto,
  EventDtoListPagedResult,
  EventItemDto,
} from '../services/api-client';

@Injectable({
  providedIn: 'root',
})
export class EventRepository {
  constructor(private apiClient: Client) {}

  /**
   * Fetches a paginated list of events without search filters.
   * @param page The page number to retrieve (default: 1).
   * @param size The number of events per page (default: 10).
   * @returns An observable emitting an array of `EventDto` objects.
   */
  fetchEvents(page: number = 1, size: number = 10): Observable<EventDto[]> {
    return this.apiClient
      .search(page, size, undefined, undefined, undefined)
      .pipe(map((response: EventDtoListPagedResult) => response.value || []));
  }

  /**
   * Searches for events based on a query string with pagination and sorting.
   * @param query The search term to filter events by title or description.
   * @param page The page number to retrieve (default: 1).
   * @param size The number of events per page (default: 10).
   * @param sort The sorting field (default: "title").
   * @param order The sorting order, either "asc" or "desc" (default: "asc").
   * @returns An observable emitting a paginated list of events (`EventDtoListPagedResult`).
   */
  searchEvents(
    query: string,
    page: number = 1,
    size: number = 10,
    sort: string = 'title',
    order: string = 'asc'
  ): Observable<EventDtoListPagedResult> {
    return this.apiClient
      .search(page, size, query, sort, order)
      .pipe(
        map(
          (response: EventDtoListPagedResult) =>
            response || new EventDtoListPagedResult()
        )
      );
  }

  /**
   * Fetches a specific event by its unique identifier.
   * @param eventId - The unique identifier of the event.
   * @returns An observable emitting an `EventItemDto` object containing event details.
   */
  events(eventId: string): Observable<EventItemDto> {
    return this.apiClient
      .events(eventId)
      .pipe(map((response: EventItemDto) => response ?? new EventItemDto()));
  }
}
