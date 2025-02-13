import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { of } from 'rxjs';
import { EventsComponent } from './events.component';
import { EventRepository } from '../../repository/event.repository';
import { EventDtoListPagedResult, PagedInfo } from '../../services/api-client';

describe('EventsComponent', () => {
  let component: EventsComponent;
  let fixture: ComponentFixture<EventsComponent>;
  let repositorySpy: jasmine.SpyObj<EventRepository>;

  beforeEach(async () => {
    repositorySpy = jasmine.createSpyObj('EventRepository', ['searchEvents']);

    await TestBed.configureTestingModule({
      imports: [RouterModule, CommonModule, FormsModule, EventsComponent],
      providers: [{ provide: EventRepository, useValue: repositorySpy }],
    }).compileComponents();

    fixture = TestBed.createComponent(EventsComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadEvents on ngOnInit', () => {
    spyOn(component, 'loadEvents');
    component.ngOnInit();
    expect(component.loadEvents).toHaveBeenCalled();
  });

  it('should update pagedObject when loadEvents is called', () => {
    const mockResponse: EventDtoListPagedResult = new EventDtoListPagedResult();
    mockResponse.value = [{ id: '1', title: 'Event 1' }] as any;
    mockResponse.pagedInfo = { totalPages: 3 } as PagedInfo;

    repositorySpy.searchEvents.and.returnValue(of(mockResponse));

    component.loadEvents();
    expect(repositorySpy.searchEvents).toHaveBeenCalled();
    expect(component.pagedObject).toEqual(mockResponse);
  });

  it('should call loadEvents when onSearch is triggered', () => {
    spyOn(component, 'loadEvents');
    component.onSearch();
    expect(component.loadEvents).toHaveBeenCalled();
  });

  it('should update page number when changePage is called with a valid page', () => {
    component.pagedObject.pagedInfo = { totalPages: 3 } as PagedInfo;
    spyOn(component, 'loadEvents');

    component.changePage(2);
    expect(component.currentPage).toBe(2);
    expect(component.loadEvents).toHaveBeenCalled();
  });

  it('should not change page if new page is out of range', () => {
    component.pagedObject.pagedInfo = { totalPages: 3 } as PagedInfo;
    spyOn(component, 'loadEvents');

    component.changePage(0); // Invalid page
    expect(component.currentPage).toBe(1);
    expect(component.loadEvents).not.toHaveBeenCalled();
  });
});
