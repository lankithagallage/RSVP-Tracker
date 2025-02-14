import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventRepository } from '../../../core/repositories/event.repository';
import { EventDtoListPagedResult } from '../../../core/api/api-client';
import { RsvpModalComponent } from '../rsvp-modal/rsvp-model.component';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule, RsvpModalComponent],
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss'],
})
export class EventsComponent implements OnInit {
  pagedObject: EventDtoListPagedResult = new EventDtoListPagedResult();
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 6;
  sortBy: string = 'date';
  sortOrder: string = 'desc';
  showModal = false;
  selectedEventId: string | null = null;

  constructor(
    private repository: EventRepository,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.searchTerm = params['search'] || '';
      this.currentPage = params['page'] ? parseInt(params['page'], 10) : 1;
      this.sortBy = params['sort'] || 'date';
      this.sortOrder = params['order'] || 'desc';

      this.loadEvents();
    });
  }

  loadEvents(): void {
    this.updateUrl();

    this.repository
      .searchEvents(
        this.searchTerm,
        this.currentPage,
        this.pageSize,
        this.sortBy,
        this.sortOrder
      )
      .subscribe((data) => {
        this.pagedObject = data || new EventDtoListPagedResult();
      });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadEvents();
  }

  changePage(newPage: number): void {
    if (
      newPage > 0 &&
      newPage <= (this.pagedObject.pagedInfo?.totalPages || 1)
    ) {
      this.currentPage = newPage;
      this.loadEvents();
    }
  }

  updateUrl(): void {
    this.router.navigate([], {
      queryParams: {
        search: this.searchTerm || null,
        page: this.currentPage > 1 ? this.currentPage : null,
        sort: this.sortBy !== 'date' ? this.sortBy : null,
        order: this.sortOrder !== 'desc' ? this.sortOrder : null,
      },
      queryParamsHandling: 'merge',
    });
  }

  openModal(eventId: string): void {
    this.selectedEventId = eventId;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.selectedEventId = null;
  }

  onRsvpSuccess(): void {
    alert('RSVP Successful!');
    this.closeModal();
  }
}
