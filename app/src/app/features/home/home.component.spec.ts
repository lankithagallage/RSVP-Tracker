import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HomeComponent } from './home.component';
import { EventRepository } from '../../core/repositories/event.repository';
import { EventDto } from '../../core/api/api-client';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let repositorySpy: jasmine.SpyObj<EventRepository>;

  beforeEach(async () => {
    repositorySpy = jasmine.createSpyObj('EventRepository', ['fetchEvents']);

    await TestBed.configureTestingModule({
      imports: [HomeComponent],
      providers: [
        { provide: EventRepository, useValue: repositorySpy },
        {
          provide: ActivatedRoute,
          useValue: { params: of({}) },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call fetchEvents with page 1 and size 2 on ngOnInit', () => {
    spyOn(component, 'ngOnInit').and.callThrough();
    repositorySpy.fetchEvents.and.returnValue(of([])); // Mock empty response
    component.ngOnInit();
    expect(repositorySpy.fetchEvents).toHaveBeenCalledWith(1, 2);
  });

  it('should update upcomingEvents when fetchEvents returns data', () => {
    const mockEvents: EventDto[] = [
      { id: '1', title: 'Event 1' } as EventDto,
      { id: '2', title: 'Event 2' } as EventDto,
    ];

    repositorySpy.fetchEvents.and.returnValue(of(mockEvents));

    component.ngOnInit();
    expect(component.upcomingEvents).toEqual(mockEvents);
  });

  it('should set upcomingEvents to an empty array when fetchEvents returns no data', () => {
    repositorySpy.fetchEvents.and.returnValue(of([]));
    component.ngOnInit();
    expect(component.upcomingEvents).toEqual([]);
  });
});
