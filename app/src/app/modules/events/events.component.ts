import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventRepository } from '../../repository/event.repository';
import { EventDtoListPagedResult } from '../../services/api-client';

@Component({
  selector: 'app-events',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
})
export class EventsComponent implements OnInit {
  pagedObject: EventDtoListPagedResult = new EventDtoListPagedResult();
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 6;
  sortBy: string = 'date';
  sortOrder: string = 'desc';

  constructor(private repository: EventRepository) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
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
}
