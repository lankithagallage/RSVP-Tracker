import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventRepository } from '../repositories/event.repository';
import {
  EventDto,
  EventDtoListPagedResult,
  EventItemDto,
} from '../api/api-client';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  constructor(private eventRepository: EventRepository) {}

  fetchEvents(page: number = 1, size: number = 10): Observable<EventDto[]> {
    return this.eventRepository.fetchEvents(page, size);
  }

  searchEvents(
    query: string,
    page: number,
    size: number,
    sort: string,
    order: string
  ): Observable<EventDtoListPagedResult> {
    return this.eventRepository.searchEvents(query, page, size, sort, order);
  }

  events(eventId: string): Observable<EventItemDto> {
    return this.eventRepository.events(eventId);
  }
}
