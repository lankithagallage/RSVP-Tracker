import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Event {
  id: number;
  name: string;
  date: string;
  location: string;
  description: string;
}

@Injectable({
  providedIn: 'root',
})
export class EventStateService {
  private eventsSubject = new BehaviorSubject<Event[]>([]); // Holds event data
  public events$ = this.eventsSubject.asObservable(); // Exposes as an observable

  constructor() {}

  // Set new event data
  setEvents(events: Event[]): void {
    this.eventsSubject.next(events);
  }

  // Get current event data
  getEvents(): Observable<Event[]> {
    return this.events$;
  }
}
