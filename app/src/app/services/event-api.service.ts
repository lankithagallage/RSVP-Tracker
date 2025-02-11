import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Event } from './event-state.service';

@Injectable({
  providedIn: 'root',
})

// Get top 10 event data
export class EventApiService {
  private apiUrl = 'http://localhost:3000/events';

  constructor(private http: HttpClient) {}

  fetchEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(this.apiUrl);
  }
}
