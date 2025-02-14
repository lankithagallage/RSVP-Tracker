import { Component, OnInit } from '@angular/core';
import { EventsService } from '../services/events.service';
import { Event } from '../models/event.model';

@Component({
  selector: 'app-events-page',
  templateUrl: './events-page.component.html',
  styleUrls: ['./events-page.component.css'], // You can leave this empty if you're using Tailwind
})
export class EventsPageComponent implements OnInit {
  // State properties for search, sorting, pagination, and modal
  searchTerm: string = '';
  sortBy: string = 'date';
  sortOrder: string = 'asc';
  currentPage: number = 1;
  showModal: boolean = false;
  selectedEventId?: number;

  // This holds the paged result from your API
  pagedObject: { value: Event[]; pagedInfo?: { totalPages: number } } = {
    value: [],
    pagedInfo: { totalPages: 1 },
  };

  constructor(private eventsService: EventsService) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  // Loads events from the service based on current filters and pagination
  loadEvents(): void {
    this.eventsService
      .getEvents(this.currentPage, this.searchTerm, this.sortBy, this.sortOrder)
      .subscribe((data) => {
        this.pagedObject = data;
      });
  }

  // Called when the search term is submitted from the search bar component
  onSearch(searchTerm: string): void {
    this.searchTerm = searchTerm;
    this.currentPage = 1; // Reset to first page on a new search
    this.loadEvents();
  }

  // Called when sorting options are updated in the sort controls component
  onSort(sortOptions: { sortBy: string; sortOrder: string }): void {
    this.sortBy = sortOptions.sortBy;
    this.sortOrder = sortOptions.sortOrder;
    this.loadEvents();
  }

  // Called when the user changes pages using the pagination component
  changePage(page: number): void {
    if (page < 1 || page > (this.pagedObject.pagedInfo?.totalPages || 1)) {
      return;
    }
    this.currentPage = page;
    this.loadEvents();
  }

  // Opens the RSVP modal for a selected event
  openModal(eventId: number): void {
    this.selectedEventId = eventId;
    this.showModal = true;
  }

  // Closes the RSVP modal
  closeModal(): void {
    this.showModal = false;
  }

  // Optionally handle additional actions when an RSVP is successful
  onRsvpSuccess(): void {
    this.closeModal();
    // You might want to reload the events or show a success message here.
  }
}
