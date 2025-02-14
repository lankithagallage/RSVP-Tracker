import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EventComponent } from './event.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { EventRepository } from '../../../repository/event.repository';
import { EventItemDto, OrganizerDto } from '../../../services/api-client';
import { RouterTestingModule } from '@angular/router/testing';
import { CommonModule } from '@angular/common';
import { RsvpModalComponent } from '../../rsvp-modal/rsvp-model.component';

describe('EventComponent', () => {
  let component: EventComponent;
  let fixture: ComponentFixture<EventComponent>;
  let mockRepository: jasmine.SpyObj<EventRepository>;

  beforeEach(async () => {
    mockRepository = jasmine.createSpyObj('EventRepository', ['events']);

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        CommonModule,
        RsvpModalComponent,
        EventComponent,
      ], // ✅ Import the standalone component
      providers: [
        { provide: EventRepository, useValue: mockRepository },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => '12345' } } },
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should get eventId from route and load event on init', () => {
    const mockEvent = new EventItemDto({
      id: '12345',
      title: 'Sample Event',
      description: 'Sample description',
      location: 'New York',
      startTime: new Date(),
      attendees: [],
      orgnizer: new OrganizerDto({ fullName: 'John Doe' }),
    });

    mockRepository.events.and.returnValue(of(mockEvent));

    component.ngOnInit();
    expect(component.eventId).toBe('12345');
    expect(mockRepository.events).toHaveBeenCalledWith('12345');
  });

  it('should open the modal', () => {
    component.openModal('12345');
    expect(component.showModal).toBeTrue();
  });

  it('should close the modal', () => {
    component.closeModal();
    expect(component.showModal).toBeFalse();
  });

  it('should refresh event details on RSVP success', () => {
    spyOn(component, 'loadEvent');
    component.eventId = '12345';

    component.onRsvpSuccess();

    expect(component.loadEvent).toHaveBeenCalledWith('12345');
    expect(component.showModal).toBeFalse();
  });

  it('should not refresh event details if eventId is null', () => {
    spyOn(component, 'loadEvent');
    component.eventId = null;

    component.onRsvpSuccess();

    expect(component.loadEvent).not.toHaveBeenCalled();
  });
});
