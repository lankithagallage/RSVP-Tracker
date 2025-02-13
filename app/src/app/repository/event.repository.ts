import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import {
  Client,
  EventDto,
  EventDtoListPagedResult,
} from '../services/api-client';

@Injectable({
  providedIn: 'root',
})
export class EventRepository {
  constructor(private apiClient: Client) {}

  fetchEvents(page: number = 1, size: number = 10): Observable<EventDto[]> {
    return this.apiClient
      .search(page, size, undefined, undefined, undefined)
      .pipe(map((response: EventDtoListPagedResult) => response.value || []));
  }

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
}
