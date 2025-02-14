import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RsvpRepository } from '../repositories/rsvp.repository';
import { GuidResult, SaveRsvpRequest } from '../api/api-client';

@Injectable({
  providedIn: 'root',
})
export class RsvpService {
  constructor(private RsvpRepository: RsvpRepository) {}

  submitRsvp(
    eventId: string,
    request: SaveRsvpRequest
  ): Observable<GuidResult | string> {
    return this.RsvpRepository.submitRsvp(eventId, request);
  }
}
