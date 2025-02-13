import { Injectable } from '@angular/core';
import { Observable, catchError, throwError, map } from 'rxjs';
import {
  Client,
  GuidResult,
  SaveRsvpRequest,
  ProblemDetails,
} from '../services/api-client';

@Injectable({
  providedIn: 'root',
})
export class RsvpRepository {
  constructor(private apiClient: Client) {}

  /**
   * Submits an RSVP for a given event.
   * Handles conflicts (409) and extracts the error message.
   * @param eventId The unique identifier of the event.
   * @param request RSVP request containing attendee details.
   * @returns An Observable containing the GUID of the saved RSVP or an error message.
   */
  submitRsvp(
    eventId: string,
    request: SaveRsvpRequest
  ): Observable<GuidResult | string> {
    return this.apiClient.rsvps(eventId, request).pipe(
      map((response: GuidResult) => response),
      catchError((error) => {
        if (error.status === 409) {
          console.log(error);
          const problemDetails: ProblemDetails = error;
          return throwError(
            () => problemDetails.detail || 'An RSVP conflict occurred.'
          );
        }
        return throwError(() => error);
      })
    );
  }
}
