import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { RsvpRepository } from './rsvp.repository';
import { Client, GuidResult, SaveRsvpRequest } from '../api/api-client';
import { of, throwError } from 'rxjs';

describe('RsvpRepository', () => {
  let repository: RsvpRepository;
  let httpMock: HttpTestingController;
  let mockApiClient: jasmine.SpyObj<Client>;

  beforeEach(() => {
    const apiClientSpy = jasmine.createSpyObj('Client', ['rsvps']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RsvpRepository, { provide: Client, useValue: apiClientSpy }],
    });

    repository = TestBed.inject(RsvpRepository);
    httpMock = TestBed.inject(HttpTestingController);
    mockApiClient = TestBed.inject(Client) as jasmine.SpyObj<Client>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(repository).toBeTruthy();
  });

  it('should call rsvps API and return GuidResult on submitRsvp()', () => {
    const eventId = '123e4567-e89b-12d3-a456-426614174000';
    const request = new SaveRsvpRequest({
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
    });
    const expectedResponse = new GuidResult({ value: 'abcd-efgh-ijkl' });

    mockApiClient.rsvps.and.returnValue(of(expectedResponse)); // Simulate API response

    repository.submitRsvp(eventId, request).subscribe((result) => {
      expect(result).toEqual(expectedResponse);
      expect(mockApiClient.rsvps).toHaveBeenCalledOnceWith(eventId, request);
    });
  });

  it('should handle errors gracefully', () => {
    const eventId = '123e4567-e89b-12d3-a456-426614174000';
    const request = new SaveRsvpRequest({
      firstName: 'Jane',
      lastName: 'Doe',
      email: 'jane.doe@example.com',
    });
    const errorMessage = 'RSVP submission failed';

    mockApiClient.rsvps.and.returnValue(
      throwError(() => new Error(errorMessage))
    );

    repository.submitRsvp(eventId, request).subscribe({
      next: () => fail('Expected error, but got success'),
      error: (error) => {
        expect(error.message).toContain(errorMessage);
        expect(mockApiClient.rsvps).toHaveBeenCalledOnceWith(eventId, request);
      },
    });
  });
});
