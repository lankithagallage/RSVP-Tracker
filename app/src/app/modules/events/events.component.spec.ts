import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { EventRepository } from '../../repository/event.repository';
import { EventsComponent } from './events.component';
import { EventDtoListPagedResult, PagedInfo } from '../../services/api-client';

describe('EventsComponent', () => {
  let component: EventsComponent;
  let fixture: ComponentFixture<EventsComponent>;
  let repositoryMock: jasmine.SpyObj<EventRepository>;
  let routerMock: jasmine.SpyObj<Router>;
  let activatedRouteMock: Partial<ActivatedRoute>;

  beforeEach(async () => {
    repositoryMock = jasmine.createSpyObj('EventRepository', ['searchEvents']);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);

    activatedRouteMock = {
      queryParams: of({
        search: 'tech',
        page: '1',
        sort: 'title',
        order: 'asc',
      }),
    };

    await TestBed.configureTestingModule({
      imports: [EventsComponent, HttpClientTestingModule, FormsModule],
      providers: [
        { provide: EventRepository, useValue: repositoryMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: activatedRouteMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(EventsComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadEvents when onSearch is triggered', () => {
    spyOn(component, 'loadEvents');
    component.onSearch();
    expect(component.loadEvents).toHaveBeenCalled();
  });

  it('should update pagedObject when loadEvents is called', () => {
    const mockData = new EventDtoListPagedResult();
    mockData.value = [];
    mockData.pagedInfo = new PagedInfo();
    mockData.pagedInfo.totalPages = 1;

    repositoryMock.searchEvents.and.returnValue(of(mockData));

    component.loadEvents();
    expect(component.pagedObject).toEqual(mockData);
  });

  it('should call loadEvents on ngOnInit', () => {
    spyOn(component, 'loadEvents');
    component.ngOnInit();
    expect(component.loadEvents).toHaveBeenCalled();
  });

  it('should not change page if new page is out of range', () => {
    component.currentPage = 1;
    const mockData = new EventDtoListPagedResult();
    mockData.value = [];
    mockData.pagedInfo = new PagedInfo();
    mockData.pagedInfo.totalPages = 1;
    spyOn(component, 'loadEvents');

    component.changePage(2);
    expect(component.loadEvents).not.toHaveBeenCalled();
  });
});
