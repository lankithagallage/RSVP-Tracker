import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Client, GuidResult, SaveRsvpRequest } from '../services/api-client';

@Injectable({
  providedIn: 'root',
})
export class RsvpRepository {
  constructor(private apiClient: Client) {}

  /**
   * Submits an RSVP for a given event.
   * @param eventId The unique identifier of the event.
   * @param request RSVP request containing attendee details.
   * @returns An Observable containing the GUID of the saved RSVP.
   */
  submitRsvp(
    eventId: string,
    request: SaveRsvpRequest
  ): Observable<GuidResult> {
    return this.apiClient
      .rsvps(eventId, request)
      .pipe(map((response: GuidResult) => response));
  }
}
