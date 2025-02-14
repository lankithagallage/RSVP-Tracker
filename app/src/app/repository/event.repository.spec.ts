import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { EventRepository } from './event.repository';
import {
  Client,
  EventDto,
  EventDtoListPagedResult,
  EventItemDto,
  OrganizerDto,
} from '../services/api-client';

describe('EventRepository', () => {
  let repository: EventRepository;
  let clientSpy: jasmine.SpyObj<Client>;

  beforeEach(() => {
    clientSpy = jasmine.createSpyObj('Client', ['search', 'events']);

    TestBed.configureTestingModule({
      providers: [EventRepository, { provide: Client, useValue: clientSpy }],
    });

    repository = TestBed.inject(EventRepository);
  });

  it('should be created', () => {
    expect(repository).toBeTruthy();
  });

  it('should call Client.search with correct parameters for fetchEvents', () => {
    const mockResponse: EventDtoListPagedResult = new EventDtoListPagedResult();
    clientSpy.search.and.returnValue(of(mockResponse));

    repository.fetchEvents(1, 10).subscribe();

    expect(clientSpy.search).toHaveBeenCalledWith(
      1,
      10,
      undefined,
      undefined,
      undefined
    );
  });

  it('should return events when fetchEvents is called', (done) => {
    const mockEvents: EventDto[] = [
      { id: '1', title: 'Test Event' } as EventDto,
    ];
    const mockResponse: EventDtoListPagedResult = {
      value: mockEvents,
      pagedInfo: undefined,
    } as EventDtoListPagedResult;

    clientSpy.search.and.returnValue(of(mockResponse));

    repository.fetchEvents(1, 10).subscribe((events) => {
      expect(events).toEqual(mockEvents);
      done();
    });
  });

  it('should return an empty array when fetchEvents gets no events', (done) => {
    const mockResponse: EventDtoListPagedResult = new EventDtoListPagedResult();
    clientSpy.search.and.returnValue(of(mockResponse));

    repository.fetchEvents(1, 10).subscribe((events) => {
      expect(events).toEqual([]);
      done();
    });
  });

  it('should call Client.search with correct parameters for searchEvents', () => {
    const mockResponse: EventDtoListPagedResult = new EventDtoListPagedResult();
    clientSpy.search.and.returnValue(of(mockResponse));

    repository.searchEvents('AI', 1, 10, 'title', 'asc').subscribe();

    expect(clientSpy.search).toHaveBeenCalledWith(1, 10, 'AI', 'title', 'asc');
  });

  it('should return paged results when searchEvents is called', (done) => {
    const mockResponse: EventDtoListPagedResult = {
      value: [{ id: '1', title: 'AI Conference' } as EventDto],
      pagedInfo: {
        pageNumber: 1,
        pageSize: 10,
        totalPages: 1,
        totalRecords: 1,
      },
    } as EventDtoListPagedResult;

    clientSpy.search.and.returnValue(of(mockResponse));

    repository.searchEvents('AI', 1, 10, 'title', 'asc').subscribe((result) => {
      expect(result).toEqual(mockResponse);
      done();
    });
  });

  it('should return an empty paged result when searchEvents gets no data', (done) => {
    const mockResponse: EventDtoListPagedResult = new EventDtoListPagedResult();
    clientSpy.search.and.returnValue(of(mockResponse));

    repository.searchEvents('AI', 1, 10, 'title', 'asc').subscribe((result) => {
      expect(result).toEqual(mockResponse);
      done();
    });
  });

  it('should call Client.events with correct eventId', () => {
    const eventId = 'event-123';
    const mockResponse = new EventItemDto({
      id: eventId,
      title: 'Event Test',
      description: 'Sample event description',
      location: 'New York',
      startTime: new Date(),
      attendees: [],
      orgnizer: new OrganizerDto({ fullName: 'John Doe' }),
    });

    clientSpy.events.and.returnValue(of(mockResponse));

    repository.events(eventId).subscribe();

    expect(clientSpy.events).toHaveBeenCalledWith(eventId);
  });

  it('should return event details when events is called', (done) => {
    const eventId = 'event-123';
    const mockResponse = new EventItemDto({
      id: eventId,
      title: 'Event Test',
      description: 'Sample event description',
      location: 'New York',
      startTime: new Date(),
      attendees: [],
      orgnizer: new OrganizerDto({ fullName: 'John Doe' }),
    });

    clientSpy.events.and.returnValue(of(mockResponse));

    repository.events(eventId).subscribe((event) => {
      expect(event).toEqual(mockResponse);
      done();
    });
  });

  it('should return an empty EventItemDto when events gets null response', (done) => {
    const eventId = 'event-123';

    clientSpy.events.and.returnValue(of(null as any));

    repository.events(eventId).subscribe((event) => {
      expect(event).toEqual(new EventItemDto()); // Should return empty EventItemDto
      done();
    });
  });

  it('should return an empty EventItemDto when events gets undefined response', (done) => {
    const eventId = 'event-123';

    clientSpy.events.and.returnValue(of(undefined as any));

    repository.events(eventId).subscribe((event) => {
      expect(event).toEqual(new EventItemDto()); // Should return empty EventItemDto
      done();
    });
  });
});
